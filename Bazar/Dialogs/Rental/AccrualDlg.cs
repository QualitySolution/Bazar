using System;
using System.Collections.Generic;
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
using Bazar.Services;
using Gamma.Binding.Converters;
using Gamma.GtkWidgets;
using Gtk;
using MySql.Data.MySqlClient;
using NLog;
using QS.Dialog.GtkUI;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Journal.GtkUI;
using QS.Project.Repositories;
using QS.Project.Services.GtkUI;
using QS.Validation.GtkUI;
using QSProjectsLib;
using QSWidgetLib;

namespace Bazar.Dialogs.Rental
{
	public partial class AccrualDlg : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		Gtk.ListStore IncomeListStore;
		CachedMetersRepository CachedMeters;
		Dictionary<AccrualItem,List<PendingMeterReading>> allPendingMeterReadings = new Dictionary<AccrualItem, List<PendingMeterReading>>();

		IUnitOfWorkGeneric<Accrual> UoW;
		Accrual Entity => UoW.Root;
		IList<PaymentItem> paids = new List<PaymentItem>();

		#region Внутренние свойства

		decimal IncomeTotal => paids.Sum(x => x.Sum);

		#endregion

		public AccrualDlg ()
		{
			UoW = UnitOfWorkFactory.CreateWithNewRoot<Accrual>();
			Entity.User = UserRepository.GetCurrentUser(UoW);
			ConfigureDlg();
			ycheckInvoiceAuto.Active = true;
		}

		public AccrualDlg(int id)
		{
			UoW = UnitOfWorkFactory.CreateForRoot<Accrual>(id);

			ConfigureDlg();
			AccrualFill(id);
		}

		void ConfigureDlg()
		{
			this.Build();
			MainClass.ComboAccrualYearsFill(comboAccuralYear);

			CachedMeters = new CachedMetersRepository(UoW);

			dateAccrual.Binding.AddBinding(Entity, e => e.Date, w => w.DateOrNull).InitializeFromSource();
			yentryInvoceNumber.Binding.AddBinding(Entity, e => e.InvoiceNumber, w => w.Text, new UintToStringConverter()).InitializeFromSource();
			ycheckInvoiceAuto.Sensitive = Entity.InvoiceNumber == null;

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
			Entity.ObservableItems.ListContentChanged += ObservableAccrualItems_ListContentChanged;
			treeviewServices.SetItemsSource<AccrualItem>(Entity.ObservableItems);
			treeviewServices.ShowAll();

			//Создаем таблицу оплат
			IncomeListStore = new Gtk.ListStore(typeof(int), typeof(string), typeof(string), typeof(string),
												 typeof(string), typeof(string), typeof(decimal));

			//ID -0
			treeviewIncomes.AppendColumn("Документ", new Gtk.CellRendererText(), "text", 1);
			treeviewIncomes.AppendColumn("Дата", new Gtk.CellRendererText(), "text", 2);
			treeviewIncomes.AppendColumn("Касса", new Gtk.CellRendererText(), "text", 3);
			// пусто
			treeviewIncomes.AppendColumn("Сумма", new Gtk.CellRendererText(), "text", 5);
			//Сумма цифровое -6

			treeviewIncomes.Model = IncomeListStore;
			treeviewIncomes.ShowAll();

			MakePrintMenu();
		}

		#region Загрука\сохранение

		private void AccrualFill(int accrualId)
		{
			logger.Info("Запрос начисления №" + accrualId + "...");

			entryUser.Text = Entity.User.Name;
			textviewComments.Buffer.Text = Entity.Comments;

			comboAccrualMonth.Active = (int)Entity.Month;
			ListStoreWorks.SearchListStore ((ListStore)comboAccuralYear.Model, Entity.Year.ToString(), out TreeIter iter);
			comboAccuralYear.SetActiveIter (iter);
			if(Entity.Contract != null)
			{
				if(ListStoreWorks.SearchListStore((ListStore)comboContract.Model, Entity.Contract.Id, out iter))
				{
					comboContract.SetActiveIter (iter);
					comboContract.Sensitive = false;
				}
			}
			this.Title = "Начисление №" + Entity.Id;

			//Получаем таблицу услуг
			UpdatePaid();

			logger.Info("Ok");

			UpdateIncomes ();
			ShowOldDebts ();

			TestCanSave();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			if(SaveAccountable())
				Respond(ResponseType.Ok);
		}

		bool SaveAccountable()
		{
			logger.Info("Запись начисления...");

			Entity.Month = (uint)comboAccrualMonth.Active;
			Entity.Year = uint.Parse(comboAccuralYear.ActiveText);
			Entity.Comments = textviewComments.Buffer.Text;

			Entity.Paid = Entity.AccrualTotal - IncomeTotal <= 0;

			var valid = new QSValidator<Accrual>(Entity);
			if(valid.RunDlgIfNotValid((Gtk.Window)this.Toplevel))
				return false;

			//Проверка на существование номера счета
			if(Entity.InvoiceNumber != null && !UoW.IsNew) {
				var exist = AccrualRepository.GetAcctualByInvoice(UoW, Entity.InvoiceNumber.Value, Entity.Year, Entity.Id);
				if(exist != null) {
					MessageDialogHelper.RunErrorDialog($"Счет с номером {Entity.InvoiceNumber} на {Entity.Year} год, " +
						$"уже указан в начислении {exist.Id} от {exist.Date}");
					return false;
				}
			}

			if(ycheckInvoiceAuto.Active) {
				Entity.InvoiceNumber = new AutoincrementDocNumberService()
					.GetNewNumber(UoW, Domain.Application.DocumentType.Invoice, Entity.Year);
			}

			UoW.Save();

			foreach(var pair in allPendingMeterReadings) {
				foreach(PendingMeterReading unsavedReading in pair.Value) {
					unsavedReading.accrualPayId = pair.Key.Id;
					unsavedReading.Save();
				}
			}
			allPendingMeterReadings.Clear();

			logger.Info("Ok");
			return true;
		}

		#endregion

		#region Приходные ордера

		protected void UpdateIncomes()
		{
			if(UoW.IsNew)
				return;

			logger.Info("Получаем таблицу приходных ордеров...");
			
			string sql = "SELECT credit_slips.*, cash.name as cash " +
					"FROM credit_slips " +
					"LEFT JOIN cash ON credit_slips.cash_id = cash.id " +
					"WHERE credit_slips.accrual_id = @id";

			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Entity.Id);
			
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

			comboContract.GetActiveIter(out TreeIter iter);
			Entity.Contract = UoW.GetById<Contract>((int)comboContract.Model.GetValue(iter, 1));

			labelLessee.LabelProp = Entity.Contract.Lessee.Name;
			labelOrg.LabelProp = Entity.Contract.Organization.Name;

			buttonOpenContract.Sensitive = buttonFillService.Sensitive = true;
		}

		protected void OnButtonMakePaymentClicked(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				PayAccrual winPay = new PayAccrual();
				winPay.FillPayTable(Entity.Id);
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
			ContractDlg winContract = new ContractDlg();
			winContract.ContractFill(Entity.Contract.Id);
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
			bool SumOk = Entity.AccrualTotal > 0;
			bool ServicesOk = Entity.Items.All(x => x.Service != null && x.Cash != null);
			
			buttonMakePayment.Sensitive = Contractok && Monthok && SumOk && ServicesOk;
			buttonOk.Sensitive = Contractok && Monthok && ServicesOk;
		}

		#endregion

		#region Таблица услуг

		void Selection_Changed(object sender, EventArgs e)
		{
			TableButtonsSensetive();
		}
		private void TableButtonsSensetive()
		{ 
			bool isSelect = treeviewServices.Selection.CountSelectedRows() >= 1;
			buttonDelService.Sensitive = isSelect;
			var rows = treeviewServices.GetSelectedObjects<AccrualItem>();
			buttonFromMeter.Sensitive = rows.Count() == 1 
				&& rows[0].Service != null
				&& rows[0].Place != null
				&& CachedMeters.MeterCount(rows[0].Service.Id, rows[0].Place.Id) > 0;

			buttonPlaceClean.Sensitive = rows.Any(x => x.Place != null);
			buttonPlaceSet.Sensitive = isSelect && rows.Any(x => x.Service != null && x.Service.PlaceSet != PlaceSetForService.Prohibited);
		}

		void ObservableAccrualItems_ListContentChanged(object sender, EventArgs e)
		{
			TestCanSave();
			CalculateServiceSum();
			MakePrintMenu();
			TableButtonsSensetive();
		}

		protected void OnButtonAddServiceClicked(object sender, EventArgs e)
		{
			var newitem = new AccrualItem(Entity);
			newitem.Amount = 1;
			var cashes = CashRepository.GetActiveCashes(UoW);
			if(cashes.Count == 1)
				newitem.Cash = cashes.First();

			Entity.ObservableItems.Add(newitem);
		}

		protected void CalculateServiceSum()
		{
			Dictionary<Cash, decimal> cashSums = new Dictionary<Cash, decimal>();
			decimal TotalSum = 0;

			foreach(var item in Entity.Items) {
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
					UoW.Delete(item);
				Entity.ObservableItems.Remove(item);
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

		protected void OnButtonPlaceSetClicked(object sender, EventArgs e)
		{
			SetPlaceItems = treeviewServices.GetSelectedObjects<AccrualItem>();

			var viewModel = MainClass.MainWin.NavigationManager.OpenViewModel<PlacesJournalViewModel> (null).ViewModel;
			viewModel.SelectionMode = QS.Project.Journal.JournalSelectionMode.Single;
			viewModel.OnSelectResult += ViewModel_OnSelectResult;
			viewModel.Title = "Выберите место";
		}

		void ViewModel_OnSelectResult(object sender, QS.Project.Journal.JournalSelectedEventArgs e)
		{
			var place = UoW.GetById<Place>(DomainHelper.GetId(e.SelectedObjects.First()));
			foreach(var item in SetPlaceItems) {
				if(item.Service != null && item.Service.PlaceSet != PlaceSetForService.Prohibited)
					item.Place = place;
			}
		}

		protected void OnButtonFromMeterClicked(object sender, EventArgs e)
		{
			var row = treeviewServices.GetSelectedObjects<AccrualItem>().First();

			PayFromMeter WinMeter = new PayFromMeter();
			WinMeter.Price = row.Price;
			WinMeter.Fill(row.Id,
						   row.Service.Id,
						   row.Place.Id,
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

		protected void OnButtonFillServiceClicked(object sender, EventArgs e)
		{
			var question = new GtkInteractiveService();
			Entity.FillItemsFromContract(UoW, question);
		}

		#endregion

		#region Вывод информации диалога

		protected void ShowStatus()
		{
			string CompleteStatus, BalanceStatus;
			decimal Balance = Entity.AccrualTotal - IncomeTotal;
			if(Entity.Items.Any(x => x.Total == 0))
				CompleteStatus = "<span background=\"Cyan\">Неполное</span>";
			else
				CompleteStatus = "";
			if(Entity.AccrualTotal == 0)
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

			if(Entity.AccrualTotal == 0 || (CompleteStatus != "" && Balance >= 0) )
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
			try {
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				comboContract.GetActiveIter ( out TreeIter iter);
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
			paids = PaymentRepository.GetPaymentItemsForAccrual(UoW, Entity.Id);
			labelIncomeSum.Text = string.Format("Сумма: {0:C} ", IncomeTotal);
		}

		protected void OnTreeviewIncomesRowActivated (object o, RowActivatedArgs args)
		{
			treeviewIncomes.Selection.GetSelected(out TreeIter iter);
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

		#region Печать

		private void MakePrintMenu()
		{
			var cashes = Entity.Items.Where(x => x.Cash != null).Select(x => x.Cash).Distinct().ToList();

			buttonPrint.Sensitive = cashes.Count > 0;

			if(cashes.Count == 0)
				return;

			var menu = new Menu();

			if(cashes.Count > 1) {
				var payListMenu = new Menu();
				var paylistItem = new MenuItem("Расчётный лист");
				paylistItem.Submenu = payListMenu;

				var itemAllCashPrint = new MenuItem("По всем кассам");
				itemAllCashPrint.Activated += ItemAllCashPrint_Activated;
				payListMenu.Add(itemAllCashPrint);
				var separator = new SeparatorMenuItem();
				payListMenu.Add(separator);

				foreach(var cash in cashes) {
					var itemSelectedCashPrint = new MenuItemId<Cash>(cash.Name);
					itemSelectedCashPrint.ID = cash;
					itemSelectedCashPrint.Activated += ItemSelectedCashPrint_Activated;
					payListMenu.Add(itemSelectedCashPrint);
				}

				menu.Add(paylistItem);

				var invoiceMenu = new Menu();
				var invoiceItem = new MenuItem("Счёт на оплату");
				invoiceItem.Submenu = invoiceMenu;

				var invoiceAllCashPrint = new MenuItem("По всем кассам");
				invoiceAllCashPrint.Activated += InvoiceAllCashPrint_Activated;
				invoiceMenu.Add(invoiceAllCashPrint);
				separator = new SeparatorMenuItem();
				invoiceMenu.Add(separator);

				foreach(var cash in cashes) {
					var itemSelectedCashPrint = new MenuItemId<Cash>(cash.Name);
					itemSelectedCashPrint.ID = cash;
					itemSelectedCashPrint.Activated += InvoiceSelectedCashPrint_Activated;
					invoiceMenu.Add(itemSelectedCashPrint);
				}

				menu.Add(invoiceItem);
			} else {
				var itemAllCashPrint = new MenuItem("Расчётный лист");
				itemAllCashPrint.Activated += ItemAllCashPrint_Activated;
				menu.Add(itemAllCashPrint);

				var invoiceAllCashPrint = new MenuItem("Счёт на оплату");
				invoiceAllCashPrint.Activated += InvoiceAllCashPrint_Activated;
				menu.Add(invoiceAllCashPrint);
			}

			menu.ShowAll();
			buttonPrint.Menu = menu;
		}

		void ItemAllCashPrint_Activated(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				string param = $"id={Entity.Id}&cash_id=-1";
				ViewReportExt.Run("PayList", param);
			}
		}

		void ItemSelectedCashPrint_Activated(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				var id = (sender as MenuItemId<Cash>).ID.Id;
				string param = $"id={Entity.Id}&cash_id={id}";
				ViewReportExt.Run("PayList", param);
			}
		}

		void InvoiceAllCashPrint_Activated(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				string param = $"accrual_id={Entity.Id}&cash_id=-1";
				ViewReportExt.Run("Invoice", param);
			}
		}

		void InvoiceSelectedCashPrint_Activated(object sender, EventArgs e)
		{
			if(SaveAccountable()) {
				var id = (sender as MenuItemId<Cash>).ID.Id;
				string param = $"accrual_id={Entity.Id}&cash_id={id}";
				ViewReportExt.Run("Invoice", param);
			}
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
	
