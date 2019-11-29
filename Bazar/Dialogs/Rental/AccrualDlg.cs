using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using bazar;
using Bazar.Dialogs.Payments;
using Bazar.Domain.Estate;
using Bazar.Domain.Payments;
using Bazar.Domain.Rental;
using Bazar.JournalViewModels.Estate;
using Bazar.Repositories.Estate;
using Bazar.Repositories.Payments;
using Bazar.Repositories.Rental;
using Gamma.GtkWidgets;
using Gtk;
using MySql.Data.MySqlClient;
using NLog;
using QS.Dialog.GtkUI;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Journal.GtkUI;
using QS.Project.Services.GtkUI;
using QSProjectsLib;
using QSWidgetLib;

namespace Bazar.Dialogs.Rental
{
	public partial class AccrualDlg : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public bool NewAccrual;
		private int AccrualId;

		Gtk.ListStore IncomeListStore;
		CachedMetersRepository CachedMeters;
		Dictionary<AccrualItem,List<PendingMeterReading>> allPendingMeterReadings = new Dictionary<AccrualItem, List<PendingMeterReading>>();

		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		List<AccrualItem> AccrualItems = new List<AccrualItem>();
		GenericObservableList<AccrualItem> ObservableAccrualItems;
		List<AccrualItem> deletedRows = new List<AccrualItem>();
		IList<PaymentItem> paids = new List<PaymentItem>();

		#region Внутренние свойства

		decimal AccrualTotal => AccrualItems.Sum(x => x.Total);
		decimal IncomeTotal => paids.Sum(x => x.Sum);

		#endregion

		public AccrualDlg ()
		{
			this.Build ();
			MainClass.ComboAccrualYearsFill (comboAccuralYear);

			CachedMeters = new CachedMetersRepository(UoW);

			treeviewServices.ColumnsConfig = ColumnsConfigFactory.Create<AccrualItem>()
				.AddColumn("Наименованиe").MinWidth(180)
					.AddComboRenderer(x => x.Service).Editing()
						.SetDisplayFunc(x => x.Name)
						.FillItems(ServiceRepository.GetActiveServices(UoW))
				.AddColumn("Место").Tag("IsPlaceColumn")
					.AddTextRenderer(x => x.Place != null ? x.Place.Title : String.Empty)
				.AddColumn("Касса").MinWidth(130)
					.AddComboRenderer(x => x.Cash).Editing()
						.SetDisplayFunc(x => x.Name)
						.FillItems(CashRepository.GetActiveCashes(UoW))
				.AddColumn("Количество")
					.AddNumericRenderer(x => x.Amount).Editing(new Adjustment(1, 0, 100000, 1, 10, 10)).Digits(2).WidthChars(9)
					.AddTextRenderer(x => x.Service != null && x.Service.Units != null ? x.Service.Units.Name : String.Empty)
				.AddColumn("Цена").MinWidth(90)
					.AddNumericRenderer(x => x.Price).Editing(new Adjustment(0, 0, 10000000, 100, 1000, 1000)).Digits(2)
				.AddColumn("Сумма")
					.AddTextRenderer(x => x.Total.ToShortCurrencyString())
				.AddColumn("Оплачено")
					.AddTextRenderer(x => PayColumnRender(x), useMarkup: true)
				.RowCells().AddSetter<Gtk.CellRendererText>((c, x) => c.Background = x.Cash != null ? x.Cash.Color : null)
				.Finish();

			treeviewServices.Selection.Mode = SelectionMode.Multiple;
			treeviewServices.Selection.Changed += Selection_Changed;
			RecreateObservable();
			treeviewServices.ShowAll();

			//Создаем таблицу оплат
			IncomeListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (string), typeof (string),
			                                     typeof (string), typeof (string), typeof (decimal));

			//ID -0
			treeviewIncomes.AppendColumn("Документ", new Gtk.CellRendererText (), "text", 1);
			treeviewIncomes.AppendColumn("Дата", new Gtk.CellRendererText (), "text", 2);
			treeviewIncomes.AppendColumn("Касса", new Gtk.CellRendererText (), "text", 3);
			// пусто
			treeviewIncomes.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", 5);
			//Сумма цифровое -6
			
			treeviewIncomes.Model = IncomeListStore;
			treeviewIncomes.ShowAll();

			var menu = new Menu ();
			var itemAllCashPrint = new MenuItem ("По всем кассам");
			itemAllCashPrint.Activated += ItemAllCashPrint_Activated;;
			menu.Add (itemAllCashPrint);
			var separator = new SeparatorMenuItem ();
			menu.Add (separator);

			foreach(var cash in CashRepository.GetActiveCashes(UoW)) {
				var itemSelectedCashPrint = new MenuItemId<Cash> (cash.Name);
				itemSelectedCashPrint.ID = cash;
				itemSelectedCashPrint.Activated += ItemSelectedCashPrint_Activated;;
				menu.Add (itemSelectedCashPrint);
			}

			menu.ShowAll ();
			buttonPrint.Menu = menu;
		}

		#region Загрука\сохранение

		// Так как мы заполняем лист без Observable, нам нужно его пересоздавать, чтобы он корректно подписался все объекты списка.
		private void RecreateObservable()
		{
			ObservableAccrualItems = new GenericObservableList<AccrualItem>(AccrualItems);
			ObservableAccrualItems.ListContentChanged += ObservableAccrualItems_ListContentChanged;
			treeviewServices.SetItemsSource<AccrualItem>(ObservableAccrualItems);
		}

		public void AccrualFill(int accrualId)
		{
			AccrualId = accrualId;
			NewAccrual = false;
			TreeIter iter;
			
			logger.Info("Запрос начисления №" + AccrualId +"...");
			string sql = "SELECT accrual.*, users.name as user FROM accrual " +
				"LEFT JOIN users ON users.id = accrual.user_id " +
				"WHERE accrual.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", AccrualId);
				MySqlDataReader rdr = cmd.ExecuteReader();
		
				if(!rdr.Read())
					return;
				
				entryNumber.Text = rdr["id"].ToString();
				entryUser.Text = rdr["user"].ToString ();
				textviewComments.Buffer.Text = rdr["comments"].ToString ();

				//запоминаем переменные что бы освободить соединение
				object DBContract_id = rdr["contract_id"];
				object DBMonth = rdr["month"];
				object DBYear = rdr["year"];

				rdr.Close();

				comboAccrualMonth.Active = Convert.ToInt32(DBMonth);
				ListStoreWorks.SearchListStore ((ListStore)comboAccuralYear.Model, DBYear.ToString (), out iter);
				comboAccuralYear.SetActiveIter (iter);
				if(DBContract_id != DBNull.Value)
				{
					if(ListStoreWorks.SearchListStore((ListStore)comboContract.Model, Convert.ToInt32(DBContract_id), out iter))
					{
						comboContract.SetActiveIter (iter);
						comboContract.Sensitive = false;
					}
				}
				this.Title = "Начисление №" + entryNumber.Text;

				//Получаем таблицу услуг
				AccrualItems.AddRange(AccrualRepository.GetAccrualItems(UoW, AccrualId));
				UpdatePaid();
				RecreateObservable();

				logger.Info("Ok");

				UpdateIncomes ();
				ShowOldDebts ();
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о начисление!", logger, ex);
			}
			
			TestCanSave();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			if(SaveAccountable())
				Respond(ResponseType.Ok);
		}

		bool SaveAccountable()
		{
			TreeIter iter;
			logger.Info("Запись начисления...");
			try {
				// Проверка нет ли уже начисления по этому договору
				string sql = "SELECT id FROM accrual WHERE contract_id = @contract AND month = @month AND year = @year";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				comboContract.GetActiveIter(out iter);
				cmd.Parameters.AddWithValue("@contract", comboContract.Model.GetValue(iter, 1));
				cmd.Parameters.AddWithValue("@month", comboAccrualMonth.Active);
				cmd.Parameters.AddWithValue("@year", comboAccuralYear.ActiveText);
				MySqlDataReader rdr = cmd.ExecuteReader();

				if(rdr.Read() && rdr["id"].ToString() != entryNumber.Text) {
					logger.Warn("Начисление уже существует!");
					MessageDialog md = new MessageDialog(this, DialogFlags.Modal,
														 MessageType.Error,
														 ButtonsType.Ok, "ошибка");
					md.UseMarkup = false;
					md.Text = "Начисление на указанный месяц по этому договору уже произведено. Начисление имеет номер " + rdr["id"].ToString();
					md.Run();
					md.Destroy();
					rdr.Close();
					return false;
				}
				rdr.Close();
				// записываем
				if(NewAccrual) {
					sql = "INSERT INTO accrual (contract_id, month, year, user_id, no_complete, comments) " +
						"VALUES (@contract_id, @month, @year, @user_id, @no_complete, @comments)";
				} else {
					sql = "UPDATE accrual SET contract_id = @contract_id, month = @month, year = @year, " +
						"no_complete = @no_complete, paid = @paid, comments = @comments " +
						"WHERE id = @id";
				}

				cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", entryNumber.Text);
				comboContract.GetActiveIter(out iter);
				cmd.Parameters.AddWithValue("@contract_id", comboContract.Model.GetValue(iter, 1));
				cmd.Parameters.AddWithValue("@month", comboAccrualMonth.Active);
				cmd.Parameters.AddWithValue("@year", comboAccuralYear.ActiveText);
				cmd.Parameters.AddWithValue("@user_id", QSMain.User.Id);
				if(textviewComments.Buffer.Text != "")
					cmd.Parameters.AddWithValue("@comments", textviewComments.Buffer.Text);
				else
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				cmd.Parameters.AddWithValue("@no_complete", AccrualItems.Any(x => x.Total == 0));
				if(AccrualTotal - IncomeTotal > 0)
					cmd.Parameters.AddWithValue("@paid", false);
				else
					cmd.Parameters.AddWithValue("@paid", true);
				cmd.ExecuteNonQuery();
				if(NewAccrual) {
					AccrualId = (int)cmd.LastInsertedId;
					entryNumber.Text = AccrualId.ToString();
					NewAccrual = false;
				}

				//записываем таблицу услуг
				var accrual = UoW.GetById<Accrual>(AccrualId);
				foreach(var item in AccrualItems) {

					item.Accrual = accrual;
					UoW.Save(item);
				}

				//Удаляем удаленные строки из базы данных
				foreach(var item in deletedRows) {
					UoW.Delete(item);
				}
				UoW.Commit();
				deletedRows.Clear();

				foreach(var pair in allPendingMeterReadings) {
					foreach(PendingMeterReading unsavedReading in pair.Value) {
						unsavedReading.accrualPayId = pair.Key.Id;
						unsavedReading.Save();
					}
				}
				allPendingMeterReadings.Clear();

			logger.Info("Ok");
				return true;
			} catch(Exception ex) {
				QSMain.ErrorMessageWithLog(this, "Ошибка записи начисления!", logger, ex);
				return false;
			}
		}

		#endregion

		#region Приходные ордера

		protected void UpdateIncomes()
		{
			if(NewAccrual)
				return;

			logger.Info("Получаем таблицу приходных ордеров...");
			
			string sql = "SELECT credit_slips.*, cash.name as cash " +
					"FROM credit_slips " +
					"LEFT JOIN cash ON credit_slips.cash_id = cash.id " +
					"WHERE credit_slips.accrual_id = @id";

			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", entryNumber.Text);
			
			MySqlDataReader rdr = cmd.ExecuteReader();
			
			IncomeListStore.Clear();
			while (rdr.Read())
			{
				IncomeListStore.AppendValues(rdr.GetInt32("id"),
				                             String.Format ("Приходный ордер № {0}", rdr["id"]),
				                             DateTime.Parse(rdr["date"].ToString()).ToShortDateString(),
				                             rdr["cash"].ToString (),
				                             null,
				                             String.Format ("{0:C}",rdr.GetDecimal ("sum")),
				                             rdr.GetDecimal ("sum"));
			}
			rdr.Close();
			
			logger.Info("Ok");
		}

		#endregion

		#region Обработка событий

		void ItemAllCashPrint_Activated(object sender, EventArgs e)
		{
			string param = $"id={entryNumber.Text}&cash_id=-1";
			ViewReportExt.Run("PayList", param);
		}

		void ItemSelectedCashPrint_Activated(object sender, EventArgs e)
		{
			var id = (sender as MenuItemId<int>).ID;
			string param = $"id={entryNumber.Text}&cash_id={id}";
			ViewReportExt.Run("PayList", param);
		}

		protected void OnComboAccrualMonthChanged(object sender, EventArgs e)
		{
			if(comboAccrualMonth.Active > 0 && comboAccuralYear.Active >= 0)
				MainClass.ComboContractFill(comboContract, comboAccrualMonth.Active, Convert.ToInt32(comboAccuralYear.ActiveText));
			TestCanSave();
		}

		protected void OnComboAccuralYearChanged(object sender, EventArgs e)
		{
			if(comboAccrualMonth.Active > 0 && comboAccuralYear.Active >= 0)
				MainClass.ComboContractFill(comboContract, comboAccrualMonth.Active, Convert.ToInt32(comboAccuralYear.ActiveText));
			TestCanSave();
		}

		protected void OnComboContractChanged (object sender, EventArgs e)
		{
			TestCanSave();
			if(comboContract.Active < 0)
			{
				labelLessee.LabelProp = "--";
				labelOrg.LabelProp = "--";
				buttonOpenContract.Sensitive = false;
				return;
			}

			TreeIter iter;
			comboContract.GetActiveIter(out iter);
			var contract = UoW.GetById<Contract>((int)comboContract.Model.GetValue(iter, 1));

			labelLessee.LabelProp = contract.Lessee.Name;
			labelOrg.LabelProp = contract.Organization.Name;

			buttonOpenContract.Sensitive = true;
		}

		protected void OnButtonMakePaymentClicked(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				PayAccrual winPay = new PayAccrual();
				winPay.FillPayTable(Convert.ToInt32(entryNumber.Text));
				winPay.ShowAll();
				if((ResponseType)winPay.Run() == ResponseType.Ok) {
					UpdateIncomes();
					UpdatePaid();
				}
				winPay.Destroy();
			}
		}

		protected void OnButtonOpenContractClicked(object sender, EventArgs e)
		{
			TreeIter iter;
			comboContract.GetActiveIter(out iter);
			int itemid = (int)comboContract.Model.GetValue(iter, 1);

			ContractDlg winContract = new ContractDlg();
			winContract.ContractFill(itemid);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
		}

		#endregion

		#region Проверки

		protected void TestCanSave ()
		{
			bool Contractok = comboContract.Active >= 0;
			bool Monthok = comboAccrualMonth.Active > 0 && comboAccuralYear.Active >= 0;
			bool SumOk = AccrualTotal > 0;
			bool ServicesOk = AccrualItems.All(x => x.Service != null && x.Cash != null);
			
			buttonMakePayment.Sensitive = Contractok && Monthok && SumOk && ServicesOk;
			buttonOk.Sensitive = Contractok && Monthok && ServicesOk;
		}

		#endregion

		#region Таблица услуг

		void Selection_Changed(object sender, EventArgs e)
		{
			bool isSelect = treeviewServices.Selection.CountSelectedRows() >= 1;
			buttonDelService.Sensitive = buttonPlaceSet.Sensitive = buttonPlaceClean.Sensitive = isSelect;
			var rows = treeviewServices.GetSelectedObjects<AccrualItem>();
			buttonFromMeter.Sensitive = rows.Count() == 1 
				&& rows[0].Service != null
				&& rows[0].Place != null
				&& CachedMeters.MeterCount(rows[0].Service.Id, rows[0].Place.Id) > 0;
		}

		void ObservableAccrualItems_ListContentChanged(object sender, EventArgs e)
		{
			TestCanSave();
			CalculateServiceSum();
		}

		protected void OnButtonAddServiceClicked(object sender, EventArgs e)
		{
			var newitem = new AccrualItem();
			newitem.Amount = 1;
			var cashes = CashRepository.GetActiveCashes(UoW);
			if(cashes.Count == 1)
				newitem.Cash = cashes.First();

			ObservableAccrualItems.Add(newitem);
		}

		protected void CalculateServiceSum()
		{
			Dictionary<Cash, decimal> cashSums = new Dictionary<Cash, decimal>();
			decimal TotalSum = 0;

			foreach(var item in AccrualItems) {
				if(item.Cash != null) {
					if(!cashSums.ContainsKey(item.Cash))
						cashSums.Add(item.Cash, 0);
					cashSums[item.Cash] += item.Total;
				}
				TotalSum += item.Total;
			}

			string Text = "";
			foreach(var pair in cashSums) {
				Text += string.Format("{0}: {1:C} \n", pair.Key.Name, pair.Value);
			}
			Text += string.Format("Всего: {0:C} ", TotalSum);
			labelSum.LabelProp = Text;
		}

		protected void OnButtonDelServiceClicked(object sender, EventArgs e)
		{
			var deletedItems = treeviewServices.GetSelectedObjects<AccrualItem>();
			var pays = PaymentRepository.GetPaymentItemsByAccrualItems(UoW, deletedItems.Select(x => x.Id).Where(x => x > 0).ToArray());
			if(pays.Count > 0) {
				var paydServices = String.Join(", ", pays.Select(x => x.AccrualItem.Service.Name));
				MessageDialogHelper.RunErrorDialog($"Нельзя удалить следующие услуги: {paydServices}, так как они уже оплачены.");
				return;
			}

			foreach(var item in deletedItems) {
				if(item.Id > 0)
					deletedRows.Add(item);
				ObservableAccrualItems.Remove(item);
				allPendingMeterReadings.Remove(item);
			}
		}

		protected void OnButtonPlaceCleanClicked(object sender, EventArgs e)
		{
			foreach(var item in treeviewServices.GetSelectedObjects<AccrualItem>()) {
				item.Place = null;
			}
		}

		AccrualItem[] SetPlaceItems;
		Dialog SelectWindow;

		protected void OnButtonPlaceSetClicked(object sender, EventArgs e)
		{
			SetPlaceItems = treeviewServices.GetSelectedObjects<AccrualItem>();

			var viewModel = new PlacesJournalViewModel(UnitOfWorkFactory.GetDefaultFactory, new GtkInteractiveService());
			viewModel.SelectionMode = QS.Project.Journal.JournalSelectionMode.Single;
			viewModel.OnSelectResult += ViewModel_OnSelectResult;

			var view = new JournalView(viewModel);
			SelectWindow = new Gtk.Dialog("Выберите место", this, DialogFlags.Modal);
			SelectWindow.SetDefaultSize(800, 500);
			SelectWindow.VBox.Add(view);
			view.Show();
			SelectWindow.Show();
			SelectWindow.Run();
			SelectWindow.Destroy();
		}

		void ViewModel_OnSelectResult(object sender, QS.Project.Journal.JournalSelectedEventArgs e)
		{
			var place = UoW.GetById<Place>(DomainHelper.GetId(e.SelectedObjects.First()));
			foreach(var item in SetPlaceItems) {
				item.Place = place;
			}
			SelectWindow.Respond(ResponseType.Ok);
		}

		protected void OnButtonFromMeterClicked(object sender, EventArgs e)
		{
			var row = treeviewServices.GetSelectedObjects<AccrualItem>().First();

			PayFromMeter WinMeter = new PayFromMeter();
			WinMeter.Price = row.Price;
			WinMeter.Fill(row.Id,
						   row.Service.Id,
						   row.Place.PlaceType.Id,
						   row.Place.PlaceNumber,
						   row.Service.Units.Name);

			if(allPendingMeterReadings.ContainsKey(row)) {
				WinMeter.SetPendingReadings(allPendingMeterReadings[row]);
			}
			int result = WinMeter.Run();
			if(result == (int)ResponseType.Ok) {
				allPendingMeterReadings[row] = WinMeter.PendingReadings;
				row.Price = WinMeter.Price;
				row.Amount = WinMeter.TotalCount;
			}
			WinMeter.Destroy();
		}

		protected void OnTreeviewServicesRowActivated(object o, RowActivatedArgs args)
		{
			if(treeviewServices.ColumnsConfig.GetColumnsByTag("IsPlaceColumn").First() == args.Column) {
				buttonPlaceSet.Click();
			}
		}

		#endregion

		#region Вывод информации диалога

		protected void ShowStatus()
		{
			string CompleteStatus, BalanceStatus;
			decimal Balance = AccrualTotal - IncomeTotal;
			if(AccrualItems.Any(x => x.Total == 0))
				CompleteStatus = "<span background=\"Cyan\">Неполное</span>";
			else
				CompleteStatus = "";
			if(AccrualTotal == 0)
				CompleteStatus = "Незаполнено";
			BalanceStatus = "";
			if(Balance == 0)
				BalanceStatus = "<span background=\"green\">Оплачено</span>";
			if(Balance > 0 )
			{
				if(IncomeTotal == 0)
					BalanceStatus = String.Format ("<span background=\"orange\">Не оплачено</span>");
				else
					BalanceStatus = String.Format ("<span background=\"yellow\">Не оплачено {0:C}</span>", Balance);
			}
			if(Balance < 0)
				BalanceStatus = String.Format ("<span background=\"red\">Переплата {0:C}</span>", Math.Abs (Balance));

			if(AccrualTotal == 0 || (CompleteStatus != "" && Balance >= 0) )
				labelStatus.LabelProp = CompleteStatus;
			else if (Balance < 0)
			{
				labelStatus.LabelProp = CompleteStatus + "\n" + BalanceStatus;
			}
			else
				labelStatus.LabelProp = BalanceStatus;
		}

		private void ShowOldDebts()
		{
			string sql = "SELECT accrual.month, accrual.year, SUM(money) as debt FROM (" +
				"SELECT accrual_id, SUM(count * price) as money FROM accrual_pays WHERE accrual_id IN " +
				"(SELECT id FROM accrual WHERE contract_id = @contract)" +
				"GROUP BY accrual_id " +
				"UNION ALL SELECT accrual_id, -SUM(sum) as money FROM credit_slips WHERE accrual_id IN " +
				"(SELECT id FROM accrual WHERE contract_id = @contract) " +
				"GROUP BY accrual_id ) as sumtable " +
				"LEFT JOIN accrual ON accrual.id = sumtable.accrual_id " +
				"GROUP BY accrual_id";
			decimal TotalDebt = 0;
			int count = 0;
			string DebtsText = "";
			int year = Convert.ToInt32 (comboAccuralYear.ActiveText);
			try
			{
				TreeIter iter;
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				comboContract.GetActiveIter ( out iter);
				cmd.Parameters.AddWithValue("@contract", comboContract.Model.GetValue (iter, 1));
				MySqlDataReader rdr = cmd.ExecuteReader ();

				while(rdr.Read ())
				{
					if(rdr.GetDecimal ("debt") <= 0)
						continue;
					if( rdr.GetInt32 ("year") > year  || ( rdr.GetInt32 ("year") == year && rdr.GetInt32 ("month") >= comboAccrualMonth.Active))
						continue;
					if(DebtsText != "")
						DebtsText += "\n";
					DateTime Month = new DateTime(rdr.GetInt32 ("year"), rdr.GetInt32("month"), 1);
					if(rdr.GetInt32 ("year") == year)
					{
						DebtsText += String.Format ("{0:MMMM} = <span foreground=\"red\">{1:C}</span>", Month, rdr.GetDecimal ("debt"));
					}
					else
					{
						DebtsText += String.Format ("{0:Y} = <span foreground=\"red\">{1:C}</span>", Month, rdr.GetDecimal ("debt"));
					}
					TotalDebt += rdr.GetDecimal ("debt");
					count ++;
				}
				rdr.Close ();
				if (count == 0)
					return;
				if (count > 5)
				{
					labelDebts.LabelProp = String.Format ("за {0} месяцев - {1:C}", count, TotalDebt);
				}
				else
				{
					labelDebts.LabelProp = DebtsText;
				}
			}
			catch (Exception ex) 
			{
				logger.Warn(ex, "Ошибка вычисления прошлого долга!");
			}
		}

		#endregion

		#region Таблица оплаты

		private void UpdatePaid()
		{
			paids = PaymentRepository.GetPaymentItemsForAccrual(UoW, AccrualId);
			labelIncomeSum.Text = string.Format("Сумма: {0:C} ", IncomeTotal);
		}

		protected void OnTreeviewIncomesRowActivated (object o, RowActivatedArgs args)
		{
			TreeIter iter;
			treeviewIncomes.Selection.GetSelected(out iter);
			int itemid = Convert.ToInt32(IncomeListStore.GetValue(iter,0));
			IncomeSlipDlg winIncome = new IncomeSlipDlg();
			winIncome.SlipFill(itemid, false);
			winIncome.Show();
			ResponseType result = (ResponseType)winIncome.Run();
			winIncome.Destroy();
			if(result == ResponseType.Ok)
			{
				UpdateIncomes();
				UpdatePaid ();
			}
		}

		#endregion

		#region Внутренние методы

		private string PayColumnRender(AccrualItem item)
		{
			var paid = paids.Where(x => x.AccrualItem.Id == item.Id).Sum(x => x.Sum);
			decimal Debt = item.Total - paid;
			string color = (Debt <= 0 && item.Total != 0) ? "darkgreen" : "black";
			return String.Format("<span foreground=\"{1}\">{0:0.00}</span>", paid, color);
		}

		#endregion

		#region Статические методы

		public static decimal GetAccrualPaidBalance(int id)
		{
			string sql = "SELECT SUM(money) as balance FROM (" +
				"SELECT SUM(count * price) as money FROM accrual_pays WHERE accrual_id = @id " +
				"UNION ALL SELECT -(sum) as money FROM credit_slips WHERE accrual_id = @id ) as sumtable";
			decimal balance;
			try {
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", id);
				balance = (decimal)cmd.ExecuteScalar();

				sql = "UPDATE accrual SET paid = @paid WHERE id = @id";
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", id);
				if(balance <= 0)
					cmd.Parameters.AddWithValue("@paid", true);
				else
					cmd.Parameters.AddWithValue("@paid", false);
				cmd.ExecuteNonQuery();
				return balance;
			} catch(Exception ex) {
				logger.Warn(ex, "Ошибка вычисления баланса!");
			}
			return 0m;
		}

		#endregion
	}
}
	
