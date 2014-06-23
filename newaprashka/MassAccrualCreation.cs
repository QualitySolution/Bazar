using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;

namespace bazar
{
	public partial class MassAccrualCreation : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		Gtk.ListStore ContractsListStore;
		Gtk.TreeModelFilter ContractsFilter;

		bool ItemsSelected;
		int SelectedItems;

		private enum ContractsCol{
			selected,
			id,
			number,
			place,
			lessee,
			end_date,
			sum_text,
			sum,
			accrual_exist
		};

		public MassAccrualCreation ()
		{
			this.Build ();

			MainClass.ComboAccrualYearsFill (comboYear);

			//Создаем таблицу "Договора"
			ContractsListStore = new Gtk.ListStore (typeof (bool), typeof(int), typeof (string), typeof (string), typeof (string), typeof (string),
			                                       typeof (string), typeof (decimal), typeof (bool));

			CellRendererToggle CellSelect = new CellRendererToggle();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;
			TreeViewColumn SelectColumn = new TreeViewColumn("Выбор", CellSelect, "active", (int)ContractsCol.selected);
			SelectColumn.SetCellDataFunc (CellSelect, new Gtk.TreeCellDataFunc (RenderSelectColumn));

			treeviewContracts.AppendColumn(SelectColumn);
			treeviewContracts.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)ContractsCol.number);
			treeviewContracts.AppendColumn("Место", new Gtk.CellRendererText (), "text", (int)ContractsCol.place);
			treeviewContracts.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", (int)ContractsCol.lessee);
			treeviewContracts.AppendColumn("Дата окончания", new Gtk.CellRendererText (), "text", (int)ContractsCol.end_date);
			treeviewContracts.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", (int)ContractsCol.sum_text);
			
			ContractsFilter = new Gtk.TreeModelFilter (ContractsListStore, null);
			ContractsFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeContracts);
			treeviewContracts.Model = ContractsFilter;
			treeviewContracts.ShowAll();

			comboMonth.Active = DateTime.Now.Month;
			CalculateSelected ();
		}

		private void RenderSelectColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if((bool) model.GetValue (iter, (int)ContractsCol.accrual_exist))
				(cell as Gtk.CellRendererToggle).Visible = false;
			else
				(cell as Gtk.CellRendererToggle).Visible = true;
			(cell as Gtk.CellRendererToggle).Active = (bool) model.GetValue (iter, (int)ContractsCol.selected);
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			TreeIter iter, filteriter;

			if (ContractsFilter.GetIter (out filteriter, new TreePath(args.Path))) 
			{
				iter = ContractsFilter.ConvertIterToChildIter (filteriter);
				bool old = (bool) ContractsListStore.GetValue(iter, (int)ContractsCol.selected);
				ContractsListStore.SetValue(iter, (int)ContractsCol.selected, !old);
			}
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

			if(model.GetValue (iter, (int)ContractsCol.id) == null)
				return false;

			if (model.GetValue (iter, (int)ContractsCol.lessee) != null)
			{
				cellvalue  = model.GetValue (iter, (int)ContractsCol.lessee).ToString();
				filterLessee = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			if (model.GetValue (iter, (int)ContractsCol.number) != null)
			{
				cellvalue  = model.GetValue (iter, (int)ContractsCol.number).ToString();
				filterNumber = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			if (model.GetValue (iter, (int)ContractsCol.place) != null)
			{
				cellvalue  = model.GetValue (iter, (int)ContractsCol.place).ToString();
				filterPlaceN = cellvalue.IndexOf (entrySearch.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
			}
			return (filterLessee || filterNumber || filterPlaceN);
		}

		void CalculateSelected()
		{
			int Count = 0;
			decimal Sum = 0;
			ItemsSelected = false;

			foreach(object[] row in ContractsListStore)
			{
				if((bool) row[(int)ContractsCol.selected] == true)
				{
					Count++;
					Sum += Convert.ToDecimal(row[(int)ContractsCol.sum]);
					ItemsSelected = true;
				}
			}

			labelSelected.LabelProp = String.Format ( "Выбрано {0} договоров на сумму {1:C} ", Count, Sum);
			SelectedItems = Count;
			TestCanSave ();
		}

		protected void OnCheckAllClicked (object sender, EventArgs e)
		{
			TreeIter iter, filteriter;
			if(ContractsFilter.GetIterFirst(out filteriter))
			{
				iter = ContractsFilter.ConvertIterToChildIter (filteriter);
				ContractsListStore.SetValue (iter, (int)ContractsCol.selected, checkAll.Active && !(bool)ContractsListStore.GetValue(iter, (int)ContractsCol.accrual_exist));
				while (ContractsFilter.IterNext(ref filteriter)) 
				{
					iter = ContractsFilter.ConvertIterToChildIter (filteriter);
					ContractsListStore.SetValue (iter, (int)ContractsCol.selected, checkAll.Active && !(bool)ContractsListStore.GetValue(iter, (int)ContractsCol.accrual_exist));
				}
			}
			CalculateSelected ();
		}

		void UpdateContracts()
		{
			if(ContractsListStore == null)
				return;
			if(comboMonth.Active == 0)
			{
				ContractsListStore.Clear ();
				labelTotal.LabelProp = "";
				return;
			}
			MainClass.StatusMessage("Получаем таблицу договоров...");
			
			string sql = "SELECT contracts.*, place_types.name as type, lessees.name as lessee, pays.sum as sum, accrual.id as exist_accrual FROM contracts " +
				"LEFT JOIN place_types ON contracts.place_type_id = place_types.id " +
					"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
					"LEFT JOIN (SELECT contract_id as contract, SUM(count * price) as sum FROM contract_pays GROUP BY contract_id) as pays " +
					"ON pays.contract = contracts.id " +
					"LEFT JOIN accrual ON contracts.id = accrual.contract_id AND accrual.month = @month AND accrual.year = @year " +
					"WHERE !(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date) ";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			int Month = comboMonth.Active;
			int Year = Convert.ToInt32(comboYear.ActiveText);
			DateTime BeginOfMonth = new DateTime(Year, Month, 1);
			DateTime EndOfMonth = new DateTime(Year, Month, DateTime.DaysInMonth (Year,Month));
			cmd.Parameters.AddWithValue("@start", BeginOfMonth);
			cmd.Parameters.AddWithValue("@end", EndOfMonth);
			cmd.Parameters.AddWithValue("@month", Month);
			cmd.Parameters.AddWithValue("@year", Year);
			MySqlDataReader rdr = cmd.ExecuteReader();

			bool cancaled;
			String End_date;
			decimal sum;
			decimal Total = 0m;
			int Count = 0;
			bool AccrualExist;

			ContractsListStore.Clear();
			while (rdr.Read())
			{
				cancaled = (rdr["cancel_date"] != DBNull.Value);
				if(cancaled)
				{
					End_date = DateTime.Parse(rdr["cancel_date"].ToString()).ToShortDateString();
				}
				else
				{
					End_date = DateTime.Parse( rdr["end_date"].ToString ()).ToShortDateString();
				}
				if(rdr["sum"] != DBNull.Value)
					sum = rdr.GetDecimal ("sum");
				else
					sum = 0m;
				Total += sum;
				Count++;
				AccrualExist = (rdr["exist_accrual"] != DBNull.Value);
				ContractsListStore.AppendValues(false,
				                                rdr.GetInt32 ("id"),
				                               rdr["number"].ToString(),
				                               rdr["type"].ToString() + " - " + rdr["place_no"].ToString(),
				                               rdr["lessee"].ToString(),
				                               End_date,
				                               String.Format ("{0:C}", sum),
				                                sum,
				                                AccrualExist);
			}
			rdr.Close();

			labelTotal.LabelProp = String.Format ("Всего {0} договоров на {1:C} ", Count, Total);
			CalculateSelected ();
			MainClass.StatusMessage("Ok");
		}

		protected void OnComboMonthChanged (object sender, EventArgs e)
		{
			UpdateContracts ();
		}		

		protected void OnComboYearChanged (object sender, EventArgs e)
		{
			UpdateContracts ();
		}

		void TestCanSave()
		{
			buttonOk.Sensitive = ItemsSelected;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			MainClass.StatusMessage("Запись начислений...");
			try 
			{
				int Month = comboMonth.Active;
				int Year = Convert.ToInt32(comboYear.ActiveText);
				progressOperation.Adjustment.Upper = SelectedItems;

				foreach (object[] row in ContractsListStore)
				{
					if( !(bool) row[(int)ContractsCol.selected])
						continue;
					string sql = "SELECT MIN(count * price) FROM contract_pays WHERE contract_id = @id";
					MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", row[(int)ContractsCol.id]);
					object Result = cmd.ExecuteScalar();
					bool NotComplete = true;
					if (Result != DBNull.Value)
						NotComplete = (Convert.ToDecimal(Result) == 0);

					sql = "INSERT INTO accrual (contract_id, month, year, user_id, no_complete) " +
						"VALUES (@contract_id, @month, @year, @user_id, @no_complete)";

					cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
					cmd.Parameters.AddWithValue("@contract_id", row[(int)ContractsCol.id]);
					cmd.Parameters.AddWithValue("@month", Month);
					cmd.Parameters.AddWithValue("@year", Year);
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.id);
					cmd.Parameters.AddWithValue("@no_complete", NotComplete);
					cmd.ExecuteNonQuery();

					long NewAccrual_id = cmd.LastInsertedId;

					sql = "INSERT INTO accrual_pays (accrual_id, service_id, cash_id, count, price) " +
						"SELECT @accrual_id, service_id, cash_id, count, price FROM contract_pays WHERE contract_id = @contract";

					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@contract", row[(int)ContractsCol.id]);
					cmd.Parameters.AddWithValue("@accrual_id", NewAccrual_id);
					cmd.ExecuteNonQuery ();

					progressOperation.Adjustment.Value++;
					while (GLib.MainContext.Pending())
					{
		   				Gtk.Main.Iteration();
					}
				}
				MainClass.StatusMessage("Ok");
			} 
			catch (Exception ex) 
			{
				logger.ErrorException ("Ошибка записи начисления!", ex);
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnEntrySearchChanged(object sender, EventArgs e)
		{
			ContractsFilter.Refilter ();
		}
	}
}

