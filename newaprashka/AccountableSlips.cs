using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using NLog;
using QSProjectsLib;

namespace bazar
{
	public partial class AccountableSlips : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		ListStore SlipsListStore;

		int Accountable_id;
		bool AccountableNull = true;

		public AccountableSlips ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboCash,"cash", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithAll);

			SlipsListStore = new ListStore (typeof (string), typeof (string), typeof (string), typeof (string), typeof (string));
			
			treeviewSlips.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 0);
			treeviewSlips.AppendColumn ("Документа", new Gtk.CellRendererText (), "text", 1);
			treeviewSlips.AppendColumn ("Расход", new Gtk.CellRendererText (), "text", 2);
			treeviewSlips.AppendColumn ("Приход", new Gtk.CellRendererText (), "text", 3);
			treeviewSlips.AppendColumn ("Основание", new Gtk.CellRendererText (), "text", 4);

			treeviewSlips.Model = SlipsListStore;
			treeviewSlips.ShowAll();
			radio3month.Click ();
		}

		public void FillByEmployee(int id)
		{
			Accountable_id = id;
			AccountableNull = false;
			try
			{
				string sql = "SELECT name FROM employees WHERE id = @id";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", id);
				MySqlDataReader rdr = cmd.ExecuteReader();
				rdr.Read ();
				entryAccountable.Text = rdr["name"].ToString ();
				entryAccountable.TooltipText = rdr["name"].ToString ();
				rdr.Close ();
				UpdateSlips ();
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this,"Не удалось получить имя сотрудника!", logger, ex);
			}


		}
		protected void UpdateSlips()
		{
			if (SlipsListStore == null)
				return;
			TreeIter iter;
			SlipsListStore.Clear ();
			try
			{
				decimal TotalExpense = 0m;
				decimal TotalIncome = 0m;
				string sqlwhere = "";
				if(comboCash.GetActiveIter(out iter) && comboCash.Active != 0)
				{
					sqlwhere += " AND cash_id = '" + comboCash.Model.GetValue(iter,1) + "'";
				}
				if(comboOrg.GetActiveIter(out iter) && comboOrg.Active != 0)
				{
					sqlwhere += " AND org_id = '" + comboOrg.Model.GetValue(iter,1) + "'";
				}
				string sql = "SELECT (0) as doc, date, id, sum as expense, null as income, details FROM debit_slips " +
					"WHERE operation = 'advance' AND employee_id = @employee_id AND date BETWEEN @start AND @end " + sqlwhere +
					" UNION ALL SELECT (1) as doc, date, id, null as expense, sum as income, details FROM credit_slips " +
						"WHERE operation = 'advance' AND employee_id = @employee_id AND date BETWEEN @start AND @end" + sqlwhere +
						" UNION ALL SELECT (2) as doc, date, id, null as expence, sum as income, details FROM advance " +
						"WHERE employee_id = @employee_id AND date BETWEEN @start AND @end" + sqlwhere +
						" ORDER BY date";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				if(!AccountableNull)
					cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				else
					return;
				cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
				cmd.Parameters.AddWithValue("@end", dateCashEnd.Date.AddDays (1));
				MySqlDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					string DocName, expense, income;
					switch (rdr["doc"].ToString ()) 
					{
					case "0":
						DocName = String.Format ("Расходный ордер №{0}",rdr["id"].ToString ());
						break;
					case "1":
						DocName = String.Format ("Приходный ордер №{0}",rdr["id"].ToString ());
						break;
					case "2":
						DocName = String.Format ("Авансовый отчет №{0}",rdr["id"].ToString ());
						break;
					default:
						DocName = "";
					break;
					}
					if(rdr["expense"] != DBNull.Value)
					{
						expense = string.Format("{0:C}",rdr.GetDecimal("expense"));
						TotalExpense += rdr.GetDecimal("expense");
					}
					else
						expense = "";
					if(rdr["income"] != DBNull.Value)
					{
						income = string.Format("{0:C}",rdr.GetDecimal("income"));
						TotalIncome += rdr.GetDecimal("income");
					}
					else
						income = "";
					SlipsListStore.AppendValues(rdr.GetDateTime("date").ToShortDateString(),
						                        DocName,
						                        expense,
					                            income,
					                            rdr["details"].ToString ());
				}
				rdr.Close();
				//Итоги
				SlipsListStore.AppendValues("",
				                            "Итоги:",
				                            string.Format("{0:C}",TotalExpense),
				                            string.Format("{0:C}",TotalIncome),
				                            "");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Не удалось сформировать отчет!", logger, ex);
			}
		}

		protected void OnButtonAccountableEditClicked (object sender, EventArgs e)
		{
			Reference AccountableSelect = new Reference(orderBy: "name");
			AccountableSelect.SetMode(true,true,true,true,false);
			AccountableSelect.FillList("employees","Сотрудник", "Сотрудники");
			AccountableSelect.Show();
			int result = AccountableSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Accountable_id = AccountableSelect.SelectedID;
				AccountableNull = false;
				entryAccountable.Text = AccountableSelect.SelectedName;
				entryAccountable.TooltipText = AccountableSelect.SelectedName;
				UpdateSlips ();
			}
			AccountableSelect.Destroy();
		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			UpdateSlips ();
		}

		protected void OnComboCashChanged (object sender, EventArgs e)
		{
			UpdateSlips ();
		}

		protected void OnRadioMonthClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddMonths(-1);
			dateCashEnd.Date = DateTime.Now.Date;
		}

		protected void OnRadio3monthClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddMonths(-3);
			dateCashEnd.Date = DateTime.Now.Date;
		}

		protected void OnRadioYearClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddYears(-1);
			dateCashEnd.Date = DateTime.Now.Date;
		}

		protected void OnRadioAllClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Parse("1.1.2012");
			dateCashEnd.Date = DateTime.Now.Date;
		}

		protected void OnDateCashStartDateChanged (object sender, EventArgs e)
		{
			UpdateSlips ();
		}

		protected void OnDateCashEndDateChanged (object sender, EventArgs e)
		{
			UpdateSlips ();
		}
	}
}

