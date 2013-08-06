using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class MassAccrualCreation : Gtk.Dialog
	{
		Gtk.ListStore ContractsListStore;
		bool ItemsSelected;
		int SelectedItems;

		public MassAccrualCreation ()
		{
			this.Build ();

			MainClass.ComboAccrualYearsFill (comboYear);

			//Создаем таблицу "Договора"
			ContractsListStore = new Gtk.ListStore (typeof (bool), typeof (string), typeof (string), typeof (string), typeof (string),
			                                       typeof (string), typeof (decimal), typeof (bool));

			CellRendererToggle CellSelect = new CellRendererToggle();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;
			TreeViewColumn SelectColumn = new TreeViewColumn("Выбор", CellSelect, "active", 0);
			SelectColumn.SetCellDataFunc (CellSelect, new Gtk.TreeCellDataFunc (RenderSelectColumn));

			treeviewContracts.AppendColumn(SelectColumn);
			treeviewContracts.AppendColumn("Номер", new Gtk.CellRendererText (), "text", 1);
			treeviewContracts.AppendColumn("Место", new Gtk.CellRendererText (), "text", 2);
			treeviewContracts.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", 3);
			treeviewContracts.AppendColumn("Дата окончания", new Gtk.CellRendererText (), "text", 4);
			treeviewContracts.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", 5);
			// Сумма цифровое -6
			// Существует ли уже начисление -7
			
			treeviewContracts.Model = ContractsListStore;
			treeviewContracts.ShowAll();

			comboMonth.Active = DateTime.Now.Month;
			CalculateSelected ();
		}

		private void RenderSelectColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if((bool) model.GetValue (iter, 7))
				(cell as Gtk.CellRendererToggle).Visible = false;
			else
				(cell as Gtk.CellRendererToggle).Visible = true;
			(cell as Gtk.CellRendererToggle).Active = (bool) model.GetValue (iter, 0);
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;
			
			if (ContractsListStore.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) ContractsListStore.GetValue(iter,0);
				if(!(bool) ContractsListStore.GetValue(iter,7))
					ContractsListStore.SetValue(iter, 0, !old);
			}
			CalculateSelected ();
		}

		void CalculateSelected()
		{
			int Count = 0;
			decimal Sum = 0;
			ItemsSelected = false;

			foreach(object[] row in ContractsListStore)
			{
				if((bool) row[0] == true)
				{
					Count++;
					Sum += Convert.ToDecimal(row[6]);
					ItemsSelected = true;
				}
			}

			labelSelected.LabelProp = String.Format ( "Выбрано {0} договоров на сумму {1:C} ", Count, Sum);
			SelectedItems = Count;
			TestCanSave ();
		}

		protected void OnCheckAllClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			if(ContractsListStore.GetIterFirst(out iter))
			{
				if(!(bool) ContractsListStore.GetValue(iter, 7))
					ContractsListStore.SetValue (iter, 0, checkAll.Active);
				while (ContractsListStore.IterNext(ref iter)) 
				{
					if(!(bool) ContractsListStore.GetValue(iter, 7))
						ContractsListStore.SetValue (iter, 0, checkAll.Active);
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
					"LEFT JOIN (SELECT contract_no as contract, SUM(count * price) as sum FROM contract_pays GROUP BY contract_no) as pays " +
					"ON pays.contract = number " +
					"LEFT JOIN accrual ON contracts.number = accrual.contract_no AND accrual.month = @month AND accrual.year = @year " +
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
				int Count = 0;

				foreach (object[] row in ContractsListStore)
				{
					if( !(bool) row[0])
						continue;
					string sql = "SELECT MIN(count * price) FROM contract_pays WHERE contract_no = @number";
					MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@number", row[1]);
					object Result = cmd.ExecuteScalar();
					bool NotComplete = true;
					if (Result != DBNull.Value)
						NotComplete = (Convert.ToDecimal(Result) == 0);

					sql = "INSERT INTO accrual (contract_no, month, year, user_id, no_complete) " +
						"VALUES (@contract_no, @month, @year, @user_id, @no_complete)";

					cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
					cmd.Parameters.AddWithValue("@contract_no", row[1]);
					cmd.Parameters.AddWithValue("@month", Month);
					cmd.Parameters.AddWithValue("@year", Year);
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.id);
					cmd.Parameters.AddWithValue("@no_complete", NotComplete);
					cmd.ExecuteNonQuery();

					long NewAccrual_id = cmd.LastInsertedId;

					sql = "INSERT INTO accrual_pays (accrual_id, service_id, cash_id, count, price) " +
						"SELECT @accrual_id, service_id, cash_id, count, price FROM contract_pays WHERE contract_no = @contract";

					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@contract", row[1]);
					cmd.Parameters.AddWithValue("@accrual_id", NewAccrual_id);
					cmd.ExecuteNonQuery ();

					Count++;
					progressOperation.Fraction = Count / SelectedItems;
					while (GLib.MainContext.Pending())
					{
		   				Gtk.Main.Iteration();
					}
				}
				MainClass.StatusMessage("Ok");
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи начисления!");
				QSMain.ErrorMessage(this,ex);
			}
		}

	}
}

