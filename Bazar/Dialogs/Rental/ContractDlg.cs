using System;
using System.Collections.Generic;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Bazar.Domain.Estate;
using Bazar.Domain.Payments;
using Bazar.Domain.Rental;
using Bazar.JournalViewModels.Estate;
using Bazar.Repositories.Payments;
using Bazar.Repositories.Rental;
using Gamma.GtkWidgets;
using Gtk;
using MySql.Data.MySqlClient;
using NLog;
using QS.Dialog.GtkUI;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace Bazar.Dialogs.Rental
{
	public partial class ContractDlg : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public bool NewContract;

		int LesseeId; 
		int ContractId = -1;
		int OrigLesseeId = -1;
		bool LesseeisNull = true;

		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();
		List<ContractItem> ContractItems = new List<ContractItem> ();
		GenericObservableList<ContractItem> ObservableContractItems;
		List<ContractItem> deletedRows = new List<ContractItem>();
		Place DefaultPlace;

		public ContractDlg ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithNo, OrderBy: "name");

			treeviewServices.ColumnsConfig = ColumnsConfigFactory.Create<ContractItem>()
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
				.AddColumn("Мин. платеж")
					.AddNumericRenderer(x => x.MinimalSum).Editing(new Adjustment(0, 0, 10000000, 100, 1000, 1000))
				.RowCells ().AddSetter<Gtk.CellRendererText> ((c, x) => c.Background = x.Cash != null ? x.Cash.Color : null)
				.Finish ();

			treeviewServices.Selection.Mode = SelectionMode.Multiple;
			treeviewServices.Selection.Changed += Selection_Changed;
			RecreateObservable();
			treeviewServices.ShowAll();
		}

		#region Загрука\сохранение

		// Так как мы заполняем лист без Observable, нам нужно его пересоздавать, чтобы он корректно подписался все объекты списка.
		private void RecreateObservable()
		{
			ObservableContractItems = new GenericObservableList<ContractItem>(ContractItems);
			ObservableContractItems.ListContentChanged += ObservableContractItems_ListContentChanged;
			treeviewServices.SetItemsSource<ContractItem>(ObservableContractItems);
		}

		public void ContractFill(int Id)
		{
			NewContract = false;
			ContractId = Id;
			TreeIter iter;
			
			logger.Info("Запрос договора ID:" + Id +"...");
			string sql = "SELECT contracts.*, lessees.name as lessee FROM contracts " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"WHERE contracts.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Id);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
				
				entryNumber.Text = rdr["number"].ToString();
				if(rdr["lessee_id"] != DBNull.Value)
				{
					LesseeId = Convert.ToInt32(rdr["lessee_id"].ToString());
					OrigLesseeId = LesseeId;
					entryLessee.Text = rdr["lessee"].ToString();
					entryLessee.TooltipText = rdr["lessee"].ToString();
					LesseeisNull = false;
				}
				if(rdr["sign_date"] != DBNull.Value)
					datepickerSign.Date = DateTime.Parse( rdr["sign_date"].ToString());
				datepickerStart.Date = DateTime.Parse(rdr["start_date"].ToString());
				datepickerEnd.Date = DateTime.Parse(rdr["end_date"].ToString());
				if(rdr["cancel_date"] != DBNull.Value)
					datepickerCancel.Date = DateTime.Parse(rdr["cancel_date"].ToString());
				if(rdr["org_id"] != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
				comboOrg.SetActiveIter (iter);

				if(rdr["pay_day"] != DBNull.Value)
					comboPayDay.Active = Convert.ToInt32(rdr["pay_day"].ToString());
				else
					comboPayDay.Active = 0;

				textComments.Buffer.Text = rdr["comments"].ToString();
				rdr.Close();

				this.Title = "Договор №" + entryNumber.Text;

				ContractItems.AddRange(ContractRepository.GetContractItems(UoW, ContractId));
				RecreateObservable();

				CalculateServiceSum();

				logger.Info("Ok");
			} catch(Exception ex) {
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о договоре!", logger, ex);
			}

			TestCanSave();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			TreeIter iter;

			logger.Info("Запись договора...");
			try {
				// Проверка номера договора на дубликат
				string sql = "SELECT COUNT(*) AS cnt FROM contracts WHERE number = @number AND sign_date = @sign_date AND id <> @id ";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				cmd.Parameters.AddWithValue("@id", ContractId);
				if(datepickerSign.IsEmpty)
					cmd.Parameters.AddWithValue("@sign_date", datepickerStart.Date);
				else
					cmd.Parameters.AddWithValue("@sign_date", datepickerSign.Date);
				long Count = (long)cmd.ExecuteScalar();

				if(Count > 0) {
					logger.Warn("Договор уже существует!");
					MessageDialog md = new MessageDialog(this, DialogFlags.Modal,
						  MessageType.Error,
						  ButtonsType.Ok, "ошибка");
					md.UseMarkup = false;
					md.Text = String.Format("Договор с номером {0} от {1:d}, уже существует в базе данных!", entryNumber.Text, datepickerSign.Date);
					md.Run();
					md.Destroy();
					return;
				}

				var allPlaces = ContractItems
					.Where(x => x.Service?.PlaceOccupy == true)
					.Select(x => x.Place)
					.Where(x => x != null)
					.Distinct().ToList();

				foreach(var place in allPlaces) {
					var conflicted = ContractRepository.GetContractItemsOnPlace(UoW, place.Id, datepickerStart.Date, (datepickerCancel.IsEmpty ? datepickerEnd.Date : datepickerCancel.Date))
						.Where(x => x.Service.PlaceOccupy)
						.Select(x => x.Contract).Distinct().Where(x => x.Id != ContractId).ToList();
					if(conflicted.Count > 0) {
						var validity = String.Join(", ", conflicted.Select(x => $"№{x.Number} ({x.ValidityText})"));
						MessageDialogHelper.RunErrorDialog($"Для места {place.Title}, период действия договора пересекается с другими договорами, со следующими датами:\n" +
							$"{validity}\n Вы должны, либо изменить даты " +
							"аренды в текущем договоре, либо досрочно расторгнуть предыдущий договор на это место.", "Место уже занято!");
						return;
					}
				}

				//Проверяем таблицу услуг
				var results = Contract.Validate(ContractItems);
				if(results.Any()) {
					var text = String.Join("\n", results.Select(x => "* " + x.ErrorMessage));
					MessageDialogHelper.RunErrorDialog("Список услуг договора не может быть сохранен. Исправьте:\n" + text, "Не верные данные");
					return;
				}

				// записываем
				if(NewContract) {
					sql = "INSERT INTO contracts (number, lessee_id, org_id, sign_date, " +
						"start_date, end_date, pay_day, cancel_date, comments) " +
							"VALUES (@number, @lessee_id, @org_id, @sign_date, " +
							"@start_date, @end_date, @pay_day, @cancel_date, @comments)";
				} else {
					sql = "UPDATE contracts SET number = @number, lessee_id = @lessee_id, org_id = @org_id, " +
						"sign_date = @sign_date, start_date = @start_date, " +
						"end_date = @end_date, pay_day = @pay_day, cancel_date = @cancel_date, comments = @comments " +
						"WHERE id = @id";
				}

				cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", ContractId);
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				cmd.Parameters.AddWithValue("@lessee_id", LesseeId);
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter, 1) != -1)
					cmd.Parameters.AddWithValue("@org_id", comboOrg.Model.GetValue(iter, 1));
				else
					cmd.Parameters.AddWithValue("@org_id", DBNull.Value);

				if(!datepickerSign.IsEmpty)
					cmd.Parameters.AddWithValue("@sign_date", datepickerSign.Date);
				else
					cmd.Parameters.AddWithValue("@sign_date", datepickerStart.Date);
				if(!datepickerStart.IsEmpty)
					cmd.Parameters.AddWithValue("@start_date", datepickerStart.Date);
				if(!datepickerEnd.IsEmpty)
					cmd.Parameters.AddWithValue("@end_date", datepickerEnd.Date);
				if(!datepickerCancel.IsEmpty)
					cmd.Parameters.AddWithValue("@cancel_date", datepickerCancel.Date);
				else
					cmd.Parameters.AddWithValue("@cancel_date", DBNull.Value);

				if(comboPayDay.Active > 0)
					cmd.Parameters.AddWithValue("@pay_day", comboPayDay.Active);
				else
					cmd.Parameters.AddWithValue("@pay_day", DBNull.Value);

				if(textComments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@comments", textComments.Buffer.Text);

				cmd.ExecuteNonQuery();
				if(NewContract)
					ContractId = (int)cmd.LastInsertedId;
				//записываем таблицу услуг
				var contract = UoW.GetById<Contract>(ContractId);
				foreach(var item in ContractItems) {
					item.Contract = contract;
					UoW.Save(item);
				}

				//Удаляем удаленные строки из базы данных
				foreach(var item in deletedRows) {
					UoW.Delete(item);
				}
				UoW.Commit();

				//Корректная смена арендатора
				if(!NewContract && OrigLesseeId != LesseeId && !LesseeisNull) {
					logger.Info("Арендатор изменился...");
					sql = "SELECT COUNT(*) FROM credit_slips WHERE contract_id = @contract AND lessee_id = @old_lessee";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@contract", ContractId);
					cmd.Parameters.AddWithValue("@old_lessee", OrigLesseeId);
					long rowcount = (long)cmd.ExecuteScalar();
					if(rowcount > 0) {
						MessageDialog md = new MessageDialog(this, DialogFlags.Modal,
															 MessageType.Warning,
															 ButtonsType.YesNo, "Предупреждение");
						md.UseMarkup = false;
						md.Text = String.Format("У договора изменился арендатор, но поэтому договору уже " +
							"было создано {0} приходных ордеров. Заменить арендатора в приходных ордерах?", rowcount);
						int result = md.Run();
						md.Destroy();

						if(result == (int)ResponseType.Yes) {
							logger.Info("Меняем арендатора в приходных ордерах...");
							sql = "UPDATE credit_slips SET lessee_id = @lessee_id " +
								"WHERE contract_id = @contract AND lessee_id = @old_lessee ";
							cmd = new MySqlCommand(sql, QSMain.connectionDB);
							cmd.Parameters.AddWithValue("@contract", ContractId);
							cmd.Parameters.AddWithValue("@old_lessee", OrigLesseeId);
							cmd.Parameters.AddWithValue("@lessee_id", LesseeId);
							cmd.ExecuteNonQuery();
						}
					}
				}

				logger.Info("Ok");
				Respond(ResponseType.Ok);
			} catch(Exception ex) {
				QSMain.ErrorMessageWithLog(this, "Ошибка записи договора!", logger, ex);
			}

		}

		public void SetPlace(int place_id)
		{
			DefaultPlace = UoW.GetById<Place>(place_id);
		}

		#endregion

		#region Проверки

		protected void TestCanSave ()
		{
			bool Numberok = (entryNumber.Text != "");
			bool Orgok = comboOrg.Active > 0;
			bool Lesseeok = !LesseeisNull;
			bool DatesCorrectok = TestCorrectDates (false);
			bool ServiceOk = ContractItems.All(x => x.Service != null && x.Cash != null);

			buttonLesseeOpen.Sensitive = Lesseeok;
			buttonOk.Sensitive = Numberok && Orgok && Lesseeok && DatesCorrectok && ServiceOk;
		}

		protected bool TestCorrectDates(bool DisplayMessage)
		{
			bool DateCorrectok = false;
			bool DateCancelok = false;
			bool DatesIsEmpty = datepickerStart.IsEmpty || datepickerEnd.IsEmpty;
			if( !DatesIsEmpty)
				DateCorrectok = datepickerEnd.Date.CompareTo(datepickerStart.Date) > 0;
			if(datepickerCancel.IsEmpty)
				DateCancelok = true;
			else
				DateCancelok = datepickerCancel.Date > datepickerStart.Date && datepickerCancel.Date < datepickerEnd.Date;
			if(DisplayMessage && !DateCorrectok && !DatesIsEmpty)
			{
				MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				                                      MessageType.Warning, 
				                                      ButtonsType.Ok, 
				                                      "Дата окончания аренды должна быть больше даты начала аренды.");
				md.Run ();
				md.Destroy();
			}
			if(DisplayMessage && !DateCancelok)
			{
				MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				                                      MessageType.Warning, 
				                                      ButtonsType.Ok, 
				                                      "Дата досрочного расторжения должна входить в период между датой начала аренды и датой ее окончания.");
				md.Run ();
				md.Destroy();
			}
			return DateCorrectok && DateCancelok;
		}

		#endregion

		#region События виджетов

		protected void OnEntryNumberChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnButtonLesseeEditClicked (object sender, EventArgs e)
		{
			Reference LesseeSelect = new Reference(orderBy: "name");
			LesseeSelect.SetMode(false,true,true,true,false);
			LesseeSelect.FillList("lessees","Арендатор", "Арендаторы");
			LesseeSelect.Show();
			int result = LesseeSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				LesseeId = LesseeSelect.SelectedID;
				LesseeisNull = false;
				entryLessee.Text = LesseeSelect.SelectedName;
				entryLessee.TooltipText = LesseeSelect.SelectedName;
			}
			LesseeSelect.Destroy();
			TestCanSave();
		}

		protected void OnDatepickerStartDateChanged(object sender, EventArgs e)
		{
			TestCorrectDates(true);
			TestCanSave();
		}

		protected void OnDatepickerEndDateChanged(object sender, EventArgs e)
		{
			TestCorrectDates(true);
			TestCanSave();
		}

		protected void OnButtonLesseeOpenClicked(object sender, EventArgs e)
		{
			LesseeDlg winLessee = new LesseeDlg();
			winLessee.LesseeFill(LesseeId);
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
		}

		protected void OnDatepickerCancelDateChanged(object sender, EventArgs e)
		{
			TestCorrectDates(true);
			TestCanSave();
		}

		protected void OnEntryActivated(object sender, EventArgs e)
		{
			this.ChildFocus(DirectionType.TabForward);
		}

		#endregion

		#region Работа с таблицей услуг.

		void Selection_Changed(object sender, EventArgs e)
		{
			TableButtonsSensetive();
		}

		private void TableButtonsSensetive()
		{
			bool isSelect = treeviewServices.Selection.CountSelectedRows() >= 1;
			buttonDelService.Sensitive = isSelect;

			var selected = treeviewServices.GetSelectedObjects<ContractItem>();
			buttonPlaceClean.Sensitive = selected.Any(x => x.Place != null);
			buttonPlaceSet.Sensitive = isSelect && selected.Any(x => x.Service != null && x.Service.PlaceSet != PlaceSetForService.Prohibited);
		}

		void ObservableContractItems_ListContentChanged(object sender, EventArgs e)
		{
			TestCanSave();
			CalculateServiceSum();
			TableButtonsSensetive();
		}

		protected void OnButtonAddServiceClicked(object sender, EventArgs e)
		{
			var newitem = new ContractItem();
			newitem.Place = DefaultPlace;
			newitem.Amount = 1;
			var cashes = CashRepository.GetActiveCashes(UoW);
			if(cashes.Count == 1)
				newitem.Cash = cashes.First();

			ObservableContractItems.Add(newitem);
		}

		protected void CalculateServiceSum()
		{
			Dictionary<Cash, decimal> cashSums = new Dictionary<Cash, decimal>();
			decimal TotalSum = 0;

			foreach(var item in ContractItems) {
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
			foreach(var item in treeviewServices.GetSelectedObjects<ContractItem>()) {
				if(item.Id > 0)
					deletedRows.Add(item);
				ObservableContractItems.Remove(item);
			}
		}

		protected void OnButtonPlaceCleanClicked(object sender, EventArgs e)
		{
			foreach(var item in treeviewServices.GetSelectedObjects<ContractItem>()) {
				item.Place = null;
			}
		}

		ContractItem[] SetPlaceItems;

		protected void OnButtonPlaceSetClicked(object sender, EventArgs e)
		{
			SetPlaceItems = treeviewServices.GetSelectedObjects<ContractItem>();
			if(!SetPlaceItems.Any(x => x.Service?.PlaceSet == PlaceSetForService.Allowed || x.Service?.PlaceSet == PlaceSetForService.Required))
				return;

			var viewModel = MainClass.MainWin.NavigationManager.OpenViewModel<PlacesJournalViewModel>(null).ViewModel;
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

		protected void OnTreeviewServicesRowActivated(object o, RowActivatedArgs args)
		{
			if(treeviewServices.ColumnsConfig.GetColumnsByTag("IsPlaceColumn").First() == args.Column) {
				buttonPlaceSet.Click();
			}
		}

		#endregion
	}
}