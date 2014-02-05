using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class AccountableDebts : Gtk.Dialog
	{
		TreeStore DebtsTreeStore;

		public AccountableDebts ()
		{
			this.Build ();

			DebtsTreeStore = new Gtk.TreeStore (typeof (int), typeof (string), typeof (string));
			
			treeviewDebts.AppendColumn ("Подотчетное лицо", new Gtk.CellRendererText (), "text", 1);
			treeviewDebts.AppendColumn ("Задолжность", new Gtk.CellRendererText (), "text", 2);

			treeviewDebts.Model = DebtsTreeStore;
			treeviewDebts.ShowAll();
			UpdateDebts ();
		}

		protected void UpdateDebts()
		{
			int countOrg, countCash;
			try
			{
				string sql = "SELECT COUNT(*) as count FROM organizations";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				object result = cmd.ExecuteScalar();
				if(result != DBNull.Value)
					countOrg = Convert.ToInt32(result);
				else 
					countOrg = 0;

				sql = "SELECT COUNT(*) as count FROM cash";
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				result = cmd.ExecuteScalar();
				if(result != DBNull.Value)
					countCash = Convert.ToInt32(result);
				else 
					countCash = 0;

				bool DisplayCash, DisplayOrg;
				DisplayOrg = (countOrg > 1);
				DisplayCash = (countCash > 1);
				string sqlgroup = "";
				if(DisplayOrg)
					sqlgroup += ", org_id";
				if(DisplayCash)
					sqlgroup += ", cash_id";
				sql = "SELECT employee_id, employees.name as employee, organizations.name as organization, cash.name as cash, debt " +
					"FROM( SELECT employee_id, org_id, cash_id, SUM(count) as debt FROM ( " +
						"SELECT employee_id, org_id, cash_id, SUM(debit_slips.sum) as count FROM debit_slips WHERE operation = 'advance' GROUP BY employee_id" + sqlgroup +
						" UNION ALL SELECT employee_id, org_id, cash_id, -SUM(credit_slips.sum) as count FROM credit_slips WHERE operation = 'advance' GROUP BY employee_id" + sqlgroup +
						" UNION ALL SELECT employee_id, org_id, cash_id, -SUM(advance.sum) as count FROM advance GROUP BY employee_id " + sqlgroup + " ) AS slips " +
						"GROUP BY employee_id" + sqlgroup + " WITH ROLLUP ) as result " +
						"LEFT JOIN employees ON result.employee_id = employees.id " +
						"LEFT JOIN organizations ON result.org_id = organizations.id " +
						"LEFT JOIN cash ON result.cash_id = cash.id " +
						"WHERE result.debt <> 0 " +
						"ORDER BY employee" + sqlgroup;
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader();
				decimal TotalDebt = 0m;
				if(rdr.Read () && rdr["debt"] != DBNull.Value && rdr["employee_id"] == DBNull.Value)
					TotalDebt = rdr.GetDecimal("debt");
				labelTotal.LabelProp = String.Format("Общая задолжность всех подотчетных лиц: {0:C}", TotalDebt);
				TreeIter EmployeeIter, OrgIter, CurrentIter, TempIter;
				OrgIter = TreeIter.Zero;
				EmployeeIter = TreeIter.Zero;
				CurrentIter = TreeIter.Zero;
				string FirstColumn;
				while (rdr.Read())
				{
					bool FirstLevel = false;
					bool CashLevel = false;
					if(DisplayCash && rdr["cash"] != DBNull.Value)
					{
						FirstColumn = rdr["cash"].ToString ();
						CashLevel = true;
						if(DisplayOrg)
							CurrentIter = OrgIter;
						else
							CurrentIter = EmployeeIter;
					}
					else if(DisplayOrg && rdr["organization"] != DBNull.Value)
					{
						FirstColumn = rdr["organization"].ToString ();
						CurrentIter = EmployeeIter;
					}
					else
					{
						FirstColumn = rdr["employee"].ToString();
						FirstLevel = true;
					}

					if(FirstLevel)
						EmployeeIter = DebtsTreeStore.AppendValues(rdr.GetInt32 ("employee_id"),
						                            FirstColumn,
						                            string.Format("{0:C}",rdr.GetDecimal("debt")));
					else
					{
						TempIter = DebtsTreeStore.AppendValues(CurrentIter,
																rdr.GetInt32 ("employee_id"),
						                                       FirstColumn,
						                                       string.Format("{0:C}",rdr.GetDecimal("debt")));
						if(DisplayOrg && !CashLevel)
							OrgIter = TempIter;
					}
				}
				rdr.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Не удалось сформировать отчет!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnTreeviewDebtsRowActivated (object o, RowActivatedArgs args)
		{
			TreeIter iter;
			int itemid;
			treeviewDebts.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(DebtsTreeStore.GetValue(iter,0));
			AccountableSlips winSlips = new AccountableSlips();
			winSlips.FillByEmployee(itemid);
			winSlips.Show();
			winSlips.Run();
			winSlips.Destroy();
		}
	}
}

