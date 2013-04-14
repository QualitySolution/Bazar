using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class CashBalance : Gtk.Dialog
	{
		TreeStore BalanceTreeStore;

		public CashBalance ()
		{
			this.Build ();

			MainClass.ComboFillReference(comboCash,"cash",1);
			MainClass.ComboFillReference(comboOrg, "organizations", 1);

			BalanceTreeStore = new Gtk.TreeStore (typeof (string), typeof (string));
			
			treeviewBalance.AppendColumn ("Статья", new Gtk.CellRendererText (), "text", 0);
			treeviewBalance.AppendColumn ("Сумма", new Gtk.CellRendererText (), "text", 1);
			
			treeviewBalance.Model = BalanceTreeStore;
			treeviewBalance.ShowAll();
			radioMonth.Click ();
		}

		void UpdateBalance()
		{
			TreeIter iter;
			BalanceTreeStore.Clear ();
			decimal TotalExpense = 0m;
			decimal TotalIncome = 0m;
			try
			{
				// Расходы
				string sqlwhere = "";
				string sqlwhere2 = "";
				if(comboCash.GetActiveIter(out iter) && comboCash.Active != 0)
				{
					sqlwhere += " AND cash_id = '" + comboCash.Model.GetValue(iter,1) + "'";
					sqlwhere2 += " AND credit_slips.cash_id = '" + comboCash.Model.GetValue(iter,1) + "'";
				}
				if(comboOrg.GetActiveIter(out iter) && comboOrg.Active != 0)
				{
					sqlwhere += " AND org_id = '" + comboOrg.Model.GetValue(iter,1) + "'";
					sqlwhere2 += " AND credit_slips.org_id = '" + comboOrg.Model.GetValue(iter,1) + "'";
				}
				string sql = "SELECT expense_items.name as item, SUM(amount) as sum FROM ( " +
					"SELECT expense_id, SUM(sum) as amount FROM debit_slips " +
					"WHERE operation != 'advance' AND date BETWEEN @start AND @end "+ sqlwhere + " " +
					"GROUP BY expense_id " +
					"UNION ALL SELECT expense_id, SUM(sum) as amount FROM advance " +
					"WHERE date BETWEEN @start AND @end "+ sqlwhere + " " +
					"GROUP BY expense_id ) as result " +
					"LEFT JOIN expense_items ON result.expense_id = expense_items.id GROUP BY expense_id ";

				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
				cmd.Parameters.AddWithValue("@end", dateCashEnd.Date.AddDays (1));
				MySqlDataReader rdr = cmd.ExecuteReader();

				TreeIter FirstLevelIter;

				FirstLevelIter = BalanceTreeStore.AppendValues("Статьи расходов");

				while (rdr.Read())
				{
					TotalExpense += rdr.GetDecimal("sum");
					BalanceTreeStore.AppendValues(FirstLevelIter,
						                          rdr["item"].ToString (),
						                          string.Format("{0:C}",rdr.GetDecimal("sum")));
				}
				BalanceTreeStore.AppendValues("Итого:",
				                              string.Format("{0:C}",TotalExpense));
				rdr.Close();
				BalanceTreeStore.AppendNode();

				// Доходы
				sql = "SELECT income_items.name as item, SUM(sumtable.total) as sum FROM (" +
					"SELECT income_id, SUM(sum) as total FROM credit_slips " +
					"WHERE operation = 'common' AND date BETWEEN @start AND @end "+ sqlwhere + " " +
					"GROUP BY income_id " +
					"UNION ALL " +
					"SELECT payment_details.income_id as income_id, SUM(payment_details.sum) as total " +
					"FROM payment_details, payments, credit_slips " +
					"WHERE payment_details.payment_id = payments.id AND payments.credit_slip_id = credit_slips.id " +
					"AND credit_slips.operation = 'payment' AND credit_slips.date BETWEEN @start AND @end "+ sqlwhere2 + " " +
					"GROUP BY payment_details.income_id ) as sumtable " +
					"LEFT JOIN income_items ON income_id = income_items.id " +
					"GROUP BY sumtable.income_id";
				
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
				cmd.Parameters.AddWithValue("@end", dateCashEnd.Date.AddDays (1));
				rdr = cmd.ExecuteReader();
				
				FirstLevelIter = BalanceTreeStore.AppendValues("Статьи Доходов");
				
				while (rdr.Read())
				{
					TotalIncome += rdr.GetDecimal("sum");
					BalanceTreeStore.AppendValues(FirstLevelIter,
					                              rdr["item"].ToString (),
					                              string.Format("{0:C}",rdr.GetDecimal("sum")));
				}
				BalanceTreeStore.AppendValues("Итого:",
				                              string.Format("{0:C}",TotalIncome));
				rdr.Close();

				treeviewBalance.ExpandAll();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Не удалось сформировать отчет!");
				MainClass.ErrorMessage(this,ex);
			}
		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			UpdateBalance ();
		}

		protected void OnComboCashChanged (object sender, EventArgs e)
		{
			UpdateBalance ();
		}

		protected void OnDateCashStartDateChanged (object sender, EventArgs e)
		{
			UpdateBalance ();
		}

		protected void OnDateCashEndDateChanged (object sender, EventArgs e)
		{
			UpdateBalance ();
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
	}
}

