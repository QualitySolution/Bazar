using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class ContractsProlongation : Gtk.Dialog
	{
		Gtk.ListStore ContractsListStore;
		Gtk.TreeModelFilter ContractsFilter;

		bool ItemsSelected;
		int SelectedItems, IgnoredItems;

		public ContractsProlongation ()
		{
			this.Build ();

			//Создаем таблицу "Договора"
			ContractsListStore = new Gtk.ListStore (typeof (bool), // 0 - select
				typeof(int), // 1 - id
				typeof (string), // 2 - number
				typeof (string), // 3 - place
				typeof (string), // 4 - lessee
				typeof (DateTime), // 5 - Start date
				typeof (DateTime), // 6 - End date
				typeof (BadContractChecks), //  7 - Conflict checks
				typeof (int), // 8 - place type id
				typeof (string), // 9 - place number
				typeof (DateTime) // 10 - sign date
			);

			CellRendererToggle CellSelect = new CellRendererToggle();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;
			TreeViewColumn SelectColumn = new TreeViewColumn("Выбор", CellSelect, "active", 0);
			SelectColumn.SetCellDataFunc (CellSelect, new Gtk.TreeCellDataFunc (RenderSelectColumn));

			treeviewContracts.AppendColumn(SelectColumn);
			treeviewContracts.AppendColumn("Номер", new Gtk.CellRendererText (), RenderNumberColumn);
			treeviewContracts.AppendColumn("Место", new Gtk.CellRendererText (), "text", 3);
			treeviewContracts.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", 4);
			treeviewContracts.AppendColumn("Начало аренды", new Gtk.CellRendererText (), RenderStartDateColumn);
			treeviewContracts.AppendColumn("Окончание", new Gtk.CellRendererText (), RenderEndDateColumn);

			ContractsFilter = new Gtk.TreeModelFilter (ContractsListStore, null);
			ContractsFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeContracts);
			treeviewContracts.Model = ContractsFilter;
			treeviewContracts.ShowAll();

			UpdateContracts ();
			radioChangeMode.Active = true;
		}

		private void RenderSelectColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (((BadContractChecks)model.GetValue (iter, 7)).Bad)
				(cell as Gtk.CellRendererToggle).CellBackground = "red";
			else
				(cell as Gtk.CellRendererToggle).CellBackground = "white"; //FIXME Возможно нужно исправить что бы корректо отображалась на других темах
			(cell as Gtk.CellRendererToggle).Active = (bool) model.GetValue (iter, 0);
		}

		private void RenderStartDateColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			(cell as Gtk.CellRendererText).Text =  String.Format("{0:d}", (DateTime) model.GetValue (iter, 5));
		}

		private void RenderNumberColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if ((DateTime)model.GetValue (iter, 10) == default(DateTime))
				(cell as Gtk.CellRendererText).Text = String.Format ("{0}", (string)model.GetValue (iter, 2));
			else
				(cell as Gtk.CellRendererText).Text = String.Format("{0} от {1:d}", (string)model.GetValue (iter, 2), (DateTime)model.GetValue (iter, 10));
		}

		private void RenderEndDateColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			(cell as Gtk.CellRendererText).Text =  String.Format("{0:d}", (DateTime) model.GetValue (iter, 6));
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			TreeIter iter, filteriter;

			if (ContractsFilter.GetIter (out filteriter, new TreePath(args.Path))) 
			{
				iter = ContractsFilter.ConvertIterToChildIter (filteriter);
				bool old = (bool) ContractsListStore.GetValue(iter,0);
				ContractsListStore.SetValue(iter, 0, !old);
			}
			CheckDateConflict ();
			CheckPlaceDubSelected ();
			CheckNumberDub ();
			CalculateSelected ();
		}

		private bool FilterTreeContracts (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if (entrySearch.Text == "")
				return true;
			bool filterLessee = false;
			bool filterNumber = false;
			bool filterPlaceN = false;
			string cellvalue;

			if(model.GetValue (iter, 1) == null)
				return false;

			if (model.GetValue (iter, 4) != null)
			{
				cellvalue  = model.GetValue (iter, 4).ToString();
				filterLessee = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			if (model.GetValue (iter, 2) != null)
			{
				cellvalue  = model.GetValue (iter, 2).ToString();
				filterNumber = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			if (model.GetValue (iter, 3) != null)
			{
				cellvalue  = model.GetValue (iter, 3).ToString();
				filterPlaceN = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			return (filterLessee || filterNumber || filterPlaceN);
		}

		void CalculateSelected()
		{
			int Count = 0;
			int IgnoreCount = 0;
			ItemsSelected = false;

			foreach(object[] row in ContractsListStore)
			{
				if((bool) row[0] == true)
				{
					Count++;
					if (((BadContractChecks)row[7]).Bad == true)
						IgnoreCount++;
					ItemsSelected = true;
				}
			}

			labelInfo.LabelProp = String.Format ( "Выбрано для обработки {0} договоров.", Count);
			if(IgnoreCount > 0)
				labelInfo.LabelProp += String.Format ( " Из них {0} невозможно обработать.", IgnoreCount);
			SelectedItems = Count;
			IgnoredItems = IgnoreCount;
			TestCanSave ();
		}

		void CheckDateConflict()
		{
			//Проверка будут ли пересекаться даты новых договоров с уже имеющимися если мы проведем операцию.
			TreeIter iter;
			List<object[]> Contracts = new List<object[]> ();
			bool FastCheck = (!checkStart.Active || dateStart.IsEmpty) && (!checkEnd.Active || dateEnd.IsEmpty);
			if(!FastCheck)
			{
				string sql = "SELECT id, start_date, (DATE(IFNULL(cancel_date,end_date))) as stop_date, place_type_id, place_no FROM contracts " +
				             "WHERE !(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date)";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				if (checkStart.Active && !dateStart.IsEmpty)
					cmd.Parameters.AddWithValue ("@start", dateStart.Date);
				else
					cmd.Parameters.AddWithValue ("@start", GetMinStartSelectedDate());
				if (checkEnd.Active && !dateEnd.IsEmpty)
					cmd.Parameters.AddWithValue ("@end", dateEnd.Date);
				else
					cmd.Parameters.AddWithValue ("@end", GetMaxEndSelectedDate());
				//Get data from server
				using (MySqlDataReader rdr = cmd.ExecuteReader ()) 
				{
					while (rdr.Read ()) 
					{
						object[] Contract = new object[5];
						rdr.GetValues (Contract);
						Contracts.Add (Contract);
					}
				}
			}
			//Check dates
			if (!ContractsListStore.GetIterFirst (out iter))
				return;
			do 
			{
				BadContractChecks ContrChk = (BadContractChecks) ContractsListStore.GetValue (iter, 7);
				ContrChk.RentDates = false;
				if((bool)ContractsListStore.GetValue(iter, 0))
				{
					int TypeId = (int) ContractsListStore.GetValue (iter, 8);
					string PlaceNo = (string) ContractsListStore.GetValue (iter, 9);
					DateTime Start = checkStart.Active ? dateStart.Date : (DateTime) ContractsListStore.GetValue (iter, 5);
					DateTime End = checkEnd.Active ? dateEnd.Date : (DateTime) ContractsListStore.GetValue (iter, 6);

					foreach(object[] row in Contracts)
					{
						if(radioChangeMode.Active && Convert.ToInt32 (row[0]) == (int)ContractsListStore.GetValue(iter, 1) )
							continue;
						if(TypeId == Convert.ToInt32 (row[3]) && PlaceNo == (string)row[4] && !(Start > (DateTime)row[2] || End <  (DateTime)row[1]))
						{
							ContrChk.RentDates = true;
							break;
						}
					}
				}
			} while (ContractsListStore.IterNext (ref iter));
		}

		private void CheckPlaceDubSelected()
		{
			TreeIter iter;
			if (!ContractsListStore.GetIterFirst (out iter))
				return;
			do 
			{
				BadContractChecks ContrChk = (BadContractChecks) ContractsListStore.GetValue (iter, 7);
				ContrChk.PlaceDub = false;

				if(!checkEnd.Active && !checkStart.Active)
					continue;

				if((bool)ContractsListStore.GetValue(iter, 0))
				{
					int TypeId = (int) ContractsListStore.GetValue (iter, 8);
					string PlaceNo = (string) ContractsListStore.GetValue (iter, 9);

					foreach(object[] row in ContractsListStore)
					{
						if(Convert.ToInt32 (row[1]) == (int)ContractsListStore.GetValue(iter, 1) )
							continue;
						if(TypeId == Convert.ToInt32 (row[8]) && PlaceNo == (string)row[9])
						{
							ContrChk.PlaceDub = true;
							break;
						}
					}
				}
			} while (ContractsListStore.IterNext (ref iter));
		}
	
		private void CheckNumberDub()
		{
			TreeIter iter;
			List<object[]> Contracts = new List<object[]> ();

			if(checkSign.Active)
			{
				string sql = "SELECT id, number, sign_date FROM contracts " +
				             "WHERE number IN (";
				if (!ContractsListStore.GetIterFirst (out iter))
					return;
				sql += String.Format ("'{0}'", ContractsListStore.GetValue(iter, 2));
				while(ContractsListStore.IterNext(ref iter))
				{
					sql += String.Format (", '{0}'", ContractsListStore.GetValue(iter, 2));
				}
				sql += ")";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				//Get data from server
				using (MySqlDataReader rdr = cmd.ExecuteReader ()) 
				{
					while (rdr.Read ()) 
					{
						object[] Contract = new object[3];
						rdr.GetValues (Contract);
						Contracts.Add (Contract);
					}
				}
			}
			//Check numbers
			if (!ContractsListStore.GetIterFirst (out iter))
				return;
			do 
			{
				BadContractChecks ContrChk = (BadContractChecks) ContractsListStore.GetValue (iter, 7);
				ContrChk.NumberDub = false;

				if(!checkSign.Active)
					continue;

				if((bool)ContractsListStore.GetValue(iter, 0))
				{
					DateTime SignDate = dateSign.Date;
					string Number = (string) ContractsListStore.GetValue (iter, 2);

					foreach(object[] row in Contracts)
					{
						if(radioChangeMode.Active && Convert.ToInt32 (row[0]) == (int)ContractsListStore.GetValue(iter, 1) )
							continue;
						if( Number == (string)row[1] && SignDate == ((DBNull.Value != row[2]) ? (DateTime)row[2] : new DateTime ()) )
						{
							ContrChk.NumberDub = true;
							break;
						}
					}
				}
			} while (ContractsListStore.IterNext (ref iter));
		}

		private DateTime GetMinStartSelectedDate()
		{
			DateTime MinDate = new DateTime();
			foreach(object[] row in ContractsListStore)
			{
				if((bool) row[0] == true)
				{
					if (MinDate == default(DateTime))
						MinDate = (DateTime) row [5];
					else if((DateTime) row [5] < MinDate )
						MinDate = (DateTime) row [5];
				}
			}
			return MinDate;
		}

		private DateTime GetMaxEndSelectedDate()
		{
			DateTime MaxDate = new DateTime();
			foreach(object[] row in ContractsListStore)
			{
				if((bool) row[0] == true)
				{
					if (MaxDate == default(DateTime))
						MaxDate = (DateTime) row [6];
					else if((DateTime) row [6] > MaxDate )
						MaxDate = (DateTime) row [6];
				}
			}
			return MaxDate;
		}

		protected void OnCheckAllClicked (object sender, EventArgs e)
		{
			TreeIter iter, filteriter;
			if(ContractsFilter.GetIterFirst(out filteriter))
			{
				iter = ContractsFilter.ConvertIterToChildIter (filteriter);
				ContractsListStore.SetValue (iter, 0, checkAll.Active);
				while (ContractsFilter.IterNext(ref filteriter)) 
				{
					iter = ContractsFilter.ConvertIterToChildIter (filteriter);
					ContractsListStore.SetValue (iter, 0, checkAll.Active);
				}
			}
			CheckDateConflict ();
			CheckPlaceDubSelected ();
			CheckNumberDub ();
			CalculateSelected ();
		}

		void UpdateContracts()
		{
			if(ContractsListStore == null)
				return;
			MainClass.StatusMessage("Получаем таблицу договоров...");

			string sql = "SELECT contracts.*, place_types.name as type, lessees.name as lessee FROM contracts " +
			             "LEFT JOIN place_types ON contracts.place_type_id = place_types.id " +
			             "LEFT JOIN lessees ON contracts.lessee_id = lessees.id ";
			if(radiobuttonActiveOnly.Active)
				sql += "WHERE ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date))";
			if(radiobuttonDates.Active)
				sql += "WHERE (DATE(IFNULL(cancel_date,end_date)) BETWEEN @start AND @end ) ";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@start", dateFrom.Date);
			cmd.Parameters.AddWithValue("@end", dateTo.Date);
			MySqlDataReader rdr = cmd.ExecuteReader();

			ContractsListStore.Clear();
			while (rdr.Read())
			{
				ContractsListStore.AppendValues(false,
					rdr.GetInt32 ("id"),
					rdr["number"].ToString(),
					rdr["type"].ToString() + " - " + rdr["place_no"].ToString(),
					rdr["lessee"].ToString(),
					rdr.GetDateTime("start_date"),
					DBWorks.GetDateTime(rdr, "cancel_date", rdr.GetDateTime ("end_date")),
					new BadContractChecks(),
					rdr.GetInt32("place_type_id"),
					rdr.GetString("place_no"),
					DBWorks.GetDateTime(rdr, "sign_date", new DateTime() )
				);
			}
			rdr.Close();
			ContractsFilter.Refilter ();

			CalculateSelected ();

			MainClass.StatusMessage("Ok");
		}

		protected void OnRadiobuttonActiveOnlyClicked(object sender, EventArgs e)
		{
			dateTo.Sensitive = dateFrom.Sensitive = false;
			UpdateContracts ();
		}

		protected void OnRadiobuttonDatesClicked(object sender, EventArgs e)
		{
			dateTo.Sensitive = dateFrom.Sensitive = true;
			UpdateContracts ();
		}

		protected void OnDateFromDateChanged(object sender, EventArgs e)
		{
			UpdateContracts ();
		}

		protected void OnDateToDateChanged(object sender, EventArgs e)
		{
			UpdateContracts ();
		}

		void TestCanSave()
		{
			bool SignDateOk = !(checkSign.Active && dateSign.IsEmpty);
			bool StartDateOk = !(checkStart.Active && dateStart.IsEmpty);
			bool EndDateOk = !(checkEnd.Active && dateEnd.IsEmpty);

			bool ActionsOk = SignDateOk && StartDateOk && EndDateOk;
			bool ModeOk = radioCopyMode.Active || radioChangeMode.Active;

			buttonOk.Sensitive = ItemsSelected && ModeOk && ActionsOk;
		}

		protected void OnEntrySearchChanged(object sender, EventArgs e)
		{
			ContractsFilter.Refilter ();
		}

		protected void OnCheckSignClicked(object sender, EventArgs e)
		{
			dateSign.Sensitive = checkSign.Active;
			CheckNumberDub ();
			CalculateSelected ();
		}

		protected void OnCheckStartClicked(object sender, EventArgs e)
		{
			dateStart.Sensitive = checkStart.Active;
			CheckDateConflict ();
			CheckPlaceDubSelected ();
			CalculateSelected ();
		}

		protected void OnCheckEndClicked(object sender, EventArgs e)
		{
			dateEnd.Sensitive = checkEnd.Active;
			CheckDateConflict ();
			CheckPlaceDubSelected ();
			CalculateSelected ();
		}

		protected void OnRadioCopyModeToggled(object sender, EventArgs e)
		{
			if(((RadioButton)sender).Active)
				checkEnd.Active = checkStart.Active = checkSign.Active = true;
			checkEnd.Sensitive = checkStart.Sensitive = checkSign.Sensitive = !((RadioButton)sender).Active;
		}

		protected void OnDateStartDateChanged(object sender, EventArgs e)
		{
			CheckDateConflict ();
			CalculateSelected ();
		}

		protected void OnDateEndDateChanged(object sender, EventArgs e)
		{
			CheckDateConflict ();
			CalculateSelected ();
		}

		protected void OnDateSignDateChanged(object sender, EventArgs e)
		{
			CheckNumberDub ();
			CalculateSelected ();
		}

		private void CopyContracts(MySqlTransaction trans)
		{
			int Count = 0;

			foreach (object[] row in ContractsListStore)
			{
				if( !(bool) row[0] || ((BadContractChecks)row[7]).Bad)
					continue;
				string sql = "INSERT INTO contracts (number, lessee_id, org_id, place_type_id, place_no, sign_date, " +
				             "start_date, end_date, pay_day, cancel_date, comments) " +
				             "SELECT number, lessee_id, org_id, place_type_id, place_no, @sign_date, @start_date, @end_date, pay_day, NULL, comments " +
				             "FROM contracts " +
				             "WHERE id = @id";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
				cmd.Parameters.AddWithValue("@id", row[1]);
				cmd.Parameters.AddWithValue("@sign_date", dateSign.Date);
				cmd.Parameters.AddWithValue("@start_date", dateStart.Date);
				cmd.Parameters.AddWithValue("@end_date", dateEnd.Date);
				cmd.ExecuteNonQuery();

				long NewContract_id = cmd.LastInsertedId;

				sql = "INSERT INTO contract_pays (contract_id, service_id, cash_id, count, price, min_sum) " +
				      "SELECT @newcontract_id, service_id, cash_id, count, price, min_sum FROM contract_pays WHERE contract_id = @oldcontract_id ";

				cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
				cmd.Parameters.AddWithValue("@oldcontract_id", row[1]);
				cmd.Parameters.AddWithValue("@newcontract_id", NewContract_id);
				cmd.ExecuteNonQuery ();

				Count++;
				progressbarMain.Fraction = Count / (SelectedItems - IgnoredItems);
				while (GLib.MainContext.Pending())
				{
					Gtk.Main.Iteration();
				}
			}
		}

		private void ChangeContracts(MySqlTransaction trans)
		{
			DBWorks.SQLHelper sql = new DBWorks.SQLHelper ("UPDATE contracts SET ");
			if (checkSign.Active)
				sql.AddAsList ("sign_date = @sign_date");
			if(checkStart.Active)
				sql.AddAsList ("start_date = @start_date");
			if(checkEnd.Active)
				sql.AddAsList ("end_date = @end_date");
			sql.Add (" WHERE id IN (");
			sql.StartNewList ();
			foreach(object[] row in ContractsListStore)
			{
				if((bool)row[0] && !((BadContractChecks)row[7]).Bad)
					sql.AddAsList (row [1].ToString ());
			}
			sql.Add (")");
			MySqlCommand cmd = new MySqlCommand(sql.Text, QSMain.connectionDB, trans);
			cmd.Parameters.AddWithValue("@sign_date", dateSign.Date);
			cmd.Parameters.AddWithValue("@start_date", dateStart.Date);
			cmd.Parameters.AddWithValue("@end_date", dateEnd.Date);
			cmd.ExecuteNonQuery ();
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			MainClass.StatusMessage("Обработка договоров...");
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction ();
			try 
			{
				if(radioCopyMode.Active)
					CopyContracts(trans);
				if(radioChangeMode.Active)
					ChangeContracts(trans);
				trans.Commit ();
				MainClass.StatusMessage("Ok");
			} 
			catch (Exception ex) 
			{
				trans.Rollback ();
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка в обработке договоров!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		class BadContractChecks
		{
			public bool NumberDub = false;
			public bool RentDates = false;
			public bool PlaceDub = false;

			public bool Bad {
				get{return NumberDub || RentDates || PlaceDub;}
			}
		}
	}
}

