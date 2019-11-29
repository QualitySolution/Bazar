using System;
using bazar;
using Bazar.Dialogs.Payments;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore CashIncomeListStore;
	Gtk.TreeModelFilter CashIncomeFilter;
	Gtk.TreeModelSort CashIncomeSort;
	Gtk.ListStore CashExpenseListStore;
	Gtk.TreeModelFilter CashExpenseFilter;
	Gtk.TreeModelSort CashExpenseSort;
	Gtk.ListStore CashAdvanceListStore;
	Gtk.TreeModelFilter CashAdvanceFilter;
	Gtk.TreeModelSort CashAdvanceSort;

	private enum CashIncomeCol{
		id,
		date,
		cash_id,
		cash,
		org_id,
		org,
		lessee_id,
		lessee,
		employer_id,
		employer,
		income_id,
		income,
		sum_text,
		sum,
		income_sum,
		operation
	}
	private enum CashExpenseCol{
		id,
		date,
		cash_id,
		cash,
		org_id,
		org,
		contractor_id,
		contractor,
		employer_id,
		employer,
		expense_id,
		expense,
		sum_text,
		sum
	}
	private enum CashAdvanceCol{
		id,
		date,
		cash_id,
		cash,
		org_id,
		org,
		contractor_id,
		contractor,
		employer_id,
		employer,
		expense_id,
		expense,
		sum_text,
		sum
	}

	void PrepareCash()
	{
		//Заполняем комбобокс
		ComboWorks.ComboFillReference(comboCashOrg, "organizations", ComboWorks.ListMode.WithAll, false, OrderBy: "name");
		ComboWorks.ComboFillReference(comboCashCash,"cash", ComboWorks.ListMode.WithAll, false);
		
		//Создаем таблицу "Приходных ордеров"
		CashIncomeListStore = new Gtk.ListStore (typeof (int), // 0-id
		                                         typeof (DateTime), // 1-date
		                                         typeof (int), // 2 - id cash
		                                         typeof (string), // 3 - cash
		                                         typeof (int), // 4 - id org
		                                         typeof (string), // 5 - org
		                                         typeof (int), // 6 - lessee id
		                                         typeof (string), // 7 - lessee
		                                         typeof (int), // 8 - id employer
		                                         typeof (string), // 9 - employer 
		                                         typeof (int), // 10 - id income
		                                         typeof (string), // 11 - income
		                                         typeof (string), // 12 - text sum
		                                         typeof (decimal), // 13 - sum
		                                         typeof (decimal),  // 14 - income sum
		                                         typeof (string)   //15 - operation
		                                        );
		
		treeviewIncome.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.id);
		treeviewIncome.AppendColumn("Дата", new Gtk.CellRendererText (), RenderDateColumn);
		treeviewIncome.AppendColumn("Касса", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.cash);
		treeviewIncome.AppendColumn("Организация", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.org);
		treeviewIncome.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.lessee);
		treeviewIncome.AppendColumn("Сотрудник", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.employer);
		treeviewIncome.AppendColumn("Статья дохода", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.income);
		treeviewIncome.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", (int)CashIncomeCol.sum_text);

		CashIncomeFilter = new Gtk.TreeModelFilter (CashIncomeListStore, null);
		CashIncomeFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeIncome);
		CashIncomeSort = new TreeModelSort (CashIncomeFilter);
		CashIncomeSort.SetSortFunc ((int)CashIncomeCol.sum, CashIncomeSumSortFunction);
		CashIncomeSort.SetSortFunc ((int)CashIncomeCol.date, CashIncomeDateSortFunction);
		treeviewIncome.Model = CashIncomeSort;
		treeviewIncome.Columns [0].SortColumnId = (int)CashIncomeCol.id;
		treeviewIncome.Columns [1].SortColumnId = (int)CashIncomeCol.date;
		treeviewIncome.Columns [2].SortColumnId = (int)CashIncomeCol.cash;
		treeviewIncome.Columns [3].SortColumnId = (int)CashIncomeCol.org;
		treeviewIncome.Columns [4].SortColumnId = (int)CashIncomeCol.lessee;
		treeviewIncome.Columns [5].SortColumnId = (int)CashIncomeCol.employer;
		treeviewIncome.Columns [6].SortColumnId = (int)CashIncomeCol.income;
		treeviewIncome.Columns [7].SortColumnId = (int)CashIncomeCol.sum;
		treeviewIncome.ShowAll();

		//Создаем таблицу "Расходных ордеров"
		CashExpenseListStore = new Gtk.ListStore (typeof (int), typeof (DateTime), typeof (int), typeof (string), typeof (int), typeof (string),
		                                          typeof (int), typeof (string), typeof (int), typeof (string), typeof (int), typeof (string), typeof (string), typeof (decimal));
		
		treeviewExpense.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.id);
		treeviewExpense.AppendColumn("Дата", new Gtk.CellRendererText (), RenderDateColumn);
		treeviewExpense.AppendColumn("Касса", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.cash);
		treeviewExpense.AppendColumn("Организация", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.org);
		treeviewExpense.AppendColumn("Контрагент", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.contractor);
		treeviewExpense.AppendColumn("Сотрудник", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.employer);
		treeviewExpense.AppendColumn("Статья расхода", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.expense);
		treeviewExpense.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", (int)CashExpenseCol.sum_text);
		
		CashExpenseFilter = new Gtk.TreeModelFilter (CashExpenseListStore, null);
		CashExpenseFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeExpense);
		CashExpenseSort = new TreeModelSort (CashExpenseFilter);
		CashExpenseSort.SetSortFunc ((int)CashExpenseCol.sum, CashExpenseSumSortFunction);
		CashExpenseSort.SetSortFunc ((int)CashExpenseCol.date, CashExpenseDateSortFunction);
		treeviewExpense.Model = CashExpenseSort;
		treeviewExpense.Columns [0].SortColumnId = (int)CashExpenseCol.id;
		treeviewExpense.Columns [1].SortColumnId = (int)CashExpenseCol.date;
		treeviewExpense.Columns [2].SortColumnId = (int)CashExpenseCol.cash;
		treeviewExpense.Columns [3].SortColumnId = (int)CashExpenseCol.org;
		treeviewExpense.Columns [4].SortColumnId = (int)CashExpenseCol.contractor;
		treeviewExpense.Columns [5].SortColumnId = (int)CashExpenseCol.employer;
		treeviewExpense.Columns [6].SortColumnId = (int)CashExpenseCol.expense;
		treeviewExpense.Columns [7].SortColumnId = (int)CashExpenseCol.sum;
		treeviewExpense.ShowAll();

		//Создаем таблицу "Авансовых отчетов"
		CashAdvanceListStore = new Gtk.ListStore (typeof (int), typeof (DateTime), typeof (int), typeof (string), typeof (int), typeof (string),
		                                          typeof (int), typeof (string), typeof (int), typeof (string), typeof (int), typeof (string), typeof (string), typeof (decimal));
		
		treeviewAdvance.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.id);
		treeviewAdvance.AppendColumn("Дата", new Gtk.CellRendererText (), RenderDateColumn);
		treeviewAdvance.AppendColumn("Касса", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.cash);
		treeviewAdvance.AppendColumn("Организация", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.org);
		treeviewAdvance.AppendColumn("Контрагент", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.contractor);
		treeviewAdvance.AppendColumn("Сотрудник", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.employer);
		treeviewAdvance.AppendColumn("Статья расхода", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.expense);
		treeviewAdvance.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", (int)CashAdvanceCol.sum_text);
		
		CashAdvanceFilter = new Gtk.TreeModelFilter (CashAdvanceListStore, null);
		CashAdvanceFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeAdvance);
		CashAdvanceSort = new TreeModelSort (CashAdvanceFilter);
		CashAdvanceSort.SetSortFunc ((int)CashAdvanceCol.sum, CashAdvanceSumSortFunction);
		CashAdvanceSort.SetSortFunc ((int)CashAdvanceCol.date, CashAdvanceDateSortFunction);
		treeviewAdvance.Model = CashAdvanceSort;
		treeviewAdvance.Columns [0].SortColumnId = (int)CashAdvanceCol.id;
		treeviewAdvance.Columns [1].SortColumnId = (int)CashAdvanceCol.date;
		treeviewAdvance.Columns [2].SortColumnId = (int)CashAdvanceCol.cash;
		treeviewAdvance.Columns [3].SortColumnId = (int)CashAdvanceCol.org;
		treeviewAdvance.Columns [4].SortColumnId = (int)CashAdvanceCol.contractor;
		treeviewAdvance.Columns [5].SortColumnId = (int)CashAdvanceCol.employer;
		treeviewAdvance.Columns [6].SortColumnId = (int)CashAdvanceCol.expense;
		treeviewAdvance.Columns [7].SortColumnId = (int)CashAdvanceCol.sum;
		treeviewAdvance.ShowAll();

		radioCashWeek.Click();
		OnNotebook1SwitchPage (null, null);
	}

	protected void OnRadioCashTodayClicked (object sender, EventArgs e)
	{
		dateCashStart.Date = DateTime.Now.Date;
		dateCashEnd.Date = DateTime.Now.Date;
	}
	
	protected void OnRadioCashWeekClicked (object sender, EventArgs e)
	{
		dateCashStart.Date = DateTime.Now.AddDays(-7);
		dateCashEnd.Date = DateTime.Now.Date;
	}
	
	protected void OnRadioMonthClicked (object sender, EventArgs e)
	{
		dateCashStart.Date = DateTime.Now.AddDays(-30);
		dateCashEnd.Date = DateTime.Now.Date;
	}
	
	protected void OnRadioCash6MonthClicked (object sender, EventArgs e)
	{
		dateCashStart.Date = DateTime.Now.AddDays(-183);
		dateCashEnd.Date = DateTime.Now.Date;
	}

	private bool FilterTreeIncome (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryCashContractor.Text == "" && entryCashEmployee.Text == "")
			return true;
		bool filterContractor = true;
		bool filterEmployee = true;		
		string cellvalue;
		
		if(model.GetValue (iter, (int)CashIncomeCol.lessee) == null)
			return false;
		
		if (entryCashContractor.Text != "" && model.GetValue (iter, (int)CashIncomeCol.lessee) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashIncomeCol.lessee).ToString();
			filterContractor = cellvalue.IndexOf (entryCashContractor.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryCashEmployee.Text != "" && model.GetValue (iter, (int)CashIncomeCol.employer) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashIncomeCol.employer).ToString();
			filterEmployee = cellvalue.IndexOf (entryCashEmployee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		
		return (filterContractor && filterEmployee);
	}

	private bool FilterTreeExpense (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryCashContractor.Text == "" && entryCashEmployee.Text == "")
			return true;
		bool filterContractor = true;
		bool filterEmployee = true;		
		string cellvalue;
		
		if(model.GetValue (iter, (int)CashExpenseCol.contractor) == null)
			return false;
		
		if (entryCashContractor.Text != "" && model.GetValue (iter, (int)CashExpenseCol.contractor) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashExpenseCol.contractor).ToString();
			filterContractor = cellvalue.IndexOf (entryCashContractor.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryCashEmployee.Text != "" && model.GetValue (iter, (int)CashExpenseCol.employer) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashExpenseCol.employer).ToString();
			filterEmployee = cellvalue.IndexOf (entryCashEmployee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		
		return (filterContractor && filterEmployee);
	}

	private bool FilterTreeAdvance (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryCashContractor.Text == "" && entryCashEmployee.Text == "")
			return true;
		bool filterContractor = true;
		bool filterEmployee = true;		
		string cellvalue;
		
		if(model.GetValue (iter, (int)CashAdvanceCol.contractor) == null)
			return false;

		if (entryCashContractor.Text != "" && model.GetValue (iter, (int)CashAdvanceCol.contractor) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashAdvanceCol.contractor).ToString();
			filterContractor = cellvalue.IndexOf (entryCashContractor.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryCashEmployee.Text != "" && model.GetValue (iter, (int)CashAdvanceCol.employer) != null)
		{
			cellvalue  = model.GetValue (iter, (int)CashAdvanceCol.employer).ToString();
			filterEmployee = cellvalue.IndexOf (entryCashEmployee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}

		return (filterContractor && filterEmployee);
	}

	private int CashIncomeSumSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashIncomeCol.sum);
		object ob = model.GetValue(b, (int)CashIncomeCol.sum);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	private int CashIncomeDateSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashIncomeCol.date);
		object ob = model.GetValue(b, (int)CashIncomeCol.date);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((DateTime)oa).CompareTo((DateTime)ob);
	}

	private int CashExpenseSumSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashExpenseCol.sum);
		object ob = model.GetValue(b, (int)CashExpenseCol.sum);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	private int CashExpenseDateSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashExpenseCol.date);
		object ob = model.GetValue(b, (int)CashExpenseCol.date);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((DateTime)oa).CompareTo((DateTime)ob);
	}

	private int CashAdvanceSumSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashAdvanceCol.sum);
		object ob = model.GetValue(b, (int)CashAdvanceCol.sum);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	private int CashAdvanceDateSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)CashAdvanceCol.date);
		object ob = model.GetValue(b, (int)CashAdvanceCol.date);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((DateTime)oa).CompareTo((DateTime)ob);
	}

	private void RenderDateColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		int dateColumn;
		if(model == CashIncomeSort)
			dateColumn = (int)CashIncomeCol.date;
		else if(model == CashExpenseSort)
			dateColumn = (int)CashExpenseCol.date;
		else if(model == CashAdvanceSort)
			dateColumn = (int)CashAdvanceCol.date;
		else
			return;

		DateTime date = (DateTime) model.GetValue (iter, dateColumn);
		(cell as Gtk.CellRendererText).Text = date.ToShortDateString ();
	}

	protected void OnNotebook1SwitchPage (object o, SwitchPageArgs args)
	{
		comboCashItem.Clear ();
		switch (notebookCash.CurrentPage) {
		case 0:
			ComboWorks.ComboFillReference(comboCashItem,"income_items", ComboWorks.ListMode.WithAll, OrderBy:"name");
			break;
		case 1:
			ComboWorks.ComboFillReference(comboCashItem,"expense_items", ComboWorks.ListMode.WithAll, OrderBy: "name");
			break;
		case 2:
			ComboWorks.ComboFillReference(comboCashItem,"expense_items", ComboWorks.ListMode.WithAll, OrderBy: "name");
			break;
		default:
			break;
		}
		UpdateCash ();
	}

	void UpdateCashIncome()
	{
		if (CashIncomeListStore == null)
			return; //Попали сюда пока окно еще не прогрузилось до конца. Ничего не делаем чтобы не упасть дальше.

		logger.Info("Получаем таблицу приходных ордеров...");

		TreeIter iter;

		string sqlpayments1 = " NULL as item_sum ";
		string sqlpayments2 = "";
		if(comboCashItem.GetActiveIter(out iter) && comboCashItem.Active != 0)
		{
			sqlpayments2 = "LEFT JOIN payments ON payments.credit_slip_id = credit_slips.id " +
				"LEFT JOIN (SELECT payment_id, SUM(sum) as income_sum FROM payment_details WHERE income_id = '" +
					comboCashItem.Model.GetValue(iter,1) + "' GROUP BY payment_id) as payment_sum ON " +
					"payment_sum.payment_id = payments.id ";
			sqlpayments1 = " payment_sum.income_sum as item_sum ";
		}

		string sql = "SELECT credit_slips.*, cash.name as cash, lessees.name as lessee, " +
			"income_items.name as income_item,  organizations.name as organization, employees.name as employee, " +
				sqlpayments1 +
			"FROM credit_slips " +
				sqlpayments2 +
			"LEFT JOIN cash ON credit_slips.cash_id = cash.id " +
			"LEFT JOIN lessees ON credit_slips.lessee_id = lessees.id " +
			"LEFT JOIN organizations ON credit_slips.org_id = organizations.id " +
			"LEFT JOIN income_items ON credit_slips.income_id = income_items.id " +
			"LEFT JOIN employees ON credit_slips.employee_id = employees.id " +
			"WHERE credit_slips.date BETWEEN @start AND @end";
		if(comboCashCash.GetActiveIter(out iter) && comboCashCash.Active != 0)
		{
			sql += " AND credit_slips.cash_id = '" + comboCashCash.Model.GetValue(iter,1) + "'";
		}
		if(comboCashOrg.GetActiveIter(out iter) && comboCashOrg.Active != 0)
		{
			sql += " AND credit_slips.org_id = '" + comboCashOrg.Model.GetValue(iter,1) + "'";
		}
		if(comboCashItem.GetActiveIter(out iter) && comboCashItem.Active != 0)
		{
			sql += " AND ( credit_slips.income_id = '" + comboCashItem.Model.GetValue(iter,1) + "' OR " +
				" payment_sum.income_sum IS NOT NULL)";
		}

		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
		cmd.Parameters.AddWithValue("@end", dateCashEnd.Date);
		
		MySqlDataReader rdr = cmd.ExecuteReader();

		CashIncomeListStore.Clear();
		while (rdr.Read())
		{
			decimal item_sum = 0;
			int lessee_id = 0;
			int employee_id = 0;
			string SumFormat = "{0:C}";
			if(rdr["lessee_id"] != DBNull.Value)
				lessee_id = int.Parse (rdr["lessee_id"].ToString ());
			if(rdr["employee_id"] != DBNull.Value)
				employee_id = rdr.GetInt32("employee_id");
			if(rdr["item_sum"] != DBNull.Value)
			{
				item_sum = rdr.GetDecimal("item_sum");
				SumFormat = "{0:C} ({1:C})";
			}
			CashIncomeListStore.AppendValues(rdr.GetInt32 ("id"),
			                                 rdr.GetDateTime ("date"),
			                                 rdr.GetInt32 ("cash_id"),
			                               rdr["cash"].ToString (),
			                                 rdr.GetInt32 ("org_id"),
			                               rdr["organization"].ToString(),
			                               lessee_id,
			                               rdr["lessee"].ToString(),
			                               employee_id,
			                               rdr["employee"].ToString (),
			                               0,
			                               rdr["income_item"].ToString(),
			                               String.Format (SumFormat, rdr.GetDecimal ("sum"), item_sum),
			                               rdr.GetDecimal ("sum"),
			                                 item_sum,
			                                 rdr["operation"].ToString()
			                                );
		}
		rdr.Close();
		
		logger.Info("Ok");
		
		bool isSelect = treeviewIncome.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
		CalculateCashSum ();
	}

	void UpdateCashExpense()
	{
		if (CashIncomeListStore == null)
			return; //Попали сюда пока окно еще не прогрузилось до конца. Ничего не делаем чтобы не упасть дальше.

		logger.Info("Получаем таблицу расходных ордеров...");
		
		TreeIter iter;
		
		string sql = "SELECT debit_slips.*, cash.name as cash, contractors.name as contractor, " +
			"expense_items.name as expense_item,  organizations.name as organization, " +
			"employees.name as employee FROM debit_slips " +
				"LEFT JOIN cash ON debit_slips.cash_id = cash.id " +
				"LEFT JOIN contractors ON debit_slips.contractor_id = contractors.id " +
				"LEFT JOIN organizations ON debit_slips.org_id = organizations.id " +
				"LEFT JOIN expense_items ON debit_slips.expense_id = expense_items.id " +
				"LEFT JOIN employees ON debit_slips.employee_id = employees.id " +
				"WHERE debit_slips.date BETWEEN @start AND @end";
		if(comboCashCash.GetActiveIter(out iter) && comboCashCash.Active != 0)
		{
			sql += " AND debit_slips.cash_id = '" + comboCashCash.Model.GetValue(iter,1) + "'";
		}
		if(comboCashOrg.GetActiveIter(out iter) && comboCashOrg.Active != 0)
		{
			sql += " AND debit_slips.org_id = '" + comboCashOrg.Model.GetValue(iter,1) + "'";
		}
		if(comboCashItem.GetActiveIter(out iter) && comboCashItem.Active != 0)
		{
			sql += " AND debit_slips.expense_id = '" + comboCashItem.Model.GetValue(iter,1) + "'";
		}
		
		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
		cmd.Parameters.AddWithValue("@end", dateCashEnd.Date);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		
		CashExpenseListStore.Clear();
		while (rdr.Read())
		{
			int contractor_id = 0;
			int employee_id = 0;
			if(rdr["contractor_id"] != DBNull.Value)
				contractor_id = int.Parse (rdr["contractor_id"].ToString ());
			if(rdr["employee_id"] != DBNull.Value)
				employee_id = rdr.GetInt32("employee_id");
			CashExpenseListStore.AppendValues(rdr.GetInt32 ("id"),
			                                  rdr.GetDateTime ("date"),
			                                  rdr.GetInt32 ("cash_id"),
			                                 rdr["cash"].ToString (),
			                                  rdr.GetInt32 ("org_id"),
			                                 rdr["organization"].ToString(),
			                                 contractor_id,
			                                 rdr["contractor"].ToString(),
			                                 employee_id,
			                                 rdr["employee"].ToString (),			                               
			                                 0,
			                                 rdr["expense_item"].ToString(),
			                                 String.Format ("{0:C}",rdr.GetDecimal ("sum")),
			                                 rdr.GetDecimal ("sum"));
		}
		rdr.Close();
		
		logger.Info("Ok");
		
		bool isSelect = treeviewExpense.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
		CalculateCashSum ();
	}

	void UpdateCashAdvance()
	{
		if (CashIncomeListStore == null)
			return; //Попали сюда пока окно еще не прогрузилось до конца. Ничего не делаем чтобы не упасть дальше.

		logger.Info("Получаем таблицу авансовых отчетов...");
		
		TreeIter iter;
		
		string sql = "SELECT advance.*, cash.name as cash, contractors.name as contractor, " +
			"expense_items.name as expense_item,  organizations.name as organization, " +
				"employees.name as employee FROM advance " +
				"LEFT JOIN cash ON advance.cash_id = cash.id " +
				"LEFT JOIN contractors ON advance.contractor_id = contractors.id " +
				"LEFT JOIN organizations ON advance.org_id = organizations.id " +
				"LEFT JOIN expense_items ON advance.expense_id = expense_items.id " +
				"LEFT JOIN employees ON advance.employee_id = employees.id " +
				"WHERE advance.date BETWEEN @start AND @end";
		if(comboCashCash.GetActiveIter(out iter) && comboCashCash.Active != 0)
		{
			sql += " AND advance.cash_id = '" + comboCashCash.Model.GetValue(iter,1) + "'";
		}
		if(comboCashOrg.GetActiveIter(out iter) && comboCashOrg.Active != 0)
		{
			sql += " AND advance.org_id = '" + comboCashOrg.Model.GetValue(iter,1) + "'";
		}
		if(comboCashItem.GetActiveIter(out iter) && comboCashItem.Active != 0)
		{
			sql += " AND advance.expense_id = '" + comboCashItem.Model.GetValue(iter,1) + "'";
		}
		
		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		cmd.Parameters.AddWithValue("@start", dateCashStart.Date);
		cmd.Parameters.AddWithValue("@end", dateCashEnd.Date);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		
		CashAdvanceListStore.Clear();
		while (rdr.Read())
		{
			int contractor_id = 0;
			int employee_id = 0;
			if(rdr["contractor_id"] != DBNull.Value)
				contractor_id = int.Parse (rdr["contractor_id"].ToString ());
			if(rdr["employee_id"] != DBNull.Value)
				employee_id = rdr.GetInt32("employee_id");
			CashAdvanceListStore.AppendValues(rdr.GetInt32 ("id"),
			                                  rdr.GetDateTime ("date"),
			                                  rdr.GetInt32 ("cash_id"),
			                                  rdr["cash"].ToString (),
			                                  rdr.GetInt32 ("org_id"),
			                                  rdr["organization"].ToString(),
			                                  contractor_id,
			                                  rdr["contractor"].ToString(),
			                                  employee_id,
			                                  rdr["employee"].ToString (),
			                                  0,
			                                  rdr["expense_item"].ToString(),
			                                  String.Format ("{0:C}",rdr.GetDecimal ("sum")),
			                                  rdr.GetDecimal ("sum"));
		}
		rdr.Close();
		
		logger.Info("Ok");
		
		bool isSelect = treeviewAdvance.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
		CalculateCashSum ();
	}

	public void UpdateCash()
	{
		if(notebookMain.CurrentPage != 4)
			return;
		switch (notebookCash.CurrentPage) {
		case 0:
			UpdateCashIncome();
			break;
		case 1:
			UpdateCashExpense();
			break;
		case 2:
			UpdateCashAdvance();
			break;
		}
	}

	protected void OnComboCashOrgChanged (object sender, EventArgs e)
	{
		UpdateCash ();
		CalculateTotalCash ();
	}
	
	protected void OnComboCashCashChanged (object sender, EventArgs e)
	{
		UpdateCash ();
		CalculateTotalCash();
	}
	
	protected void OnComboCashItemChanged (object sender, EventArgs e)
	{
		UpdateCash ();
	}
	
	protected void OnDateCashStartDateChanged (object sender, EventArgs e)
	{
		UpdateCash ();
	}
	
	protected void OnDateCashEndDateChanged (object sender, EventArgs e)
	{
		UpdateCash ();
	}

	protected void OnEntryCashContractorChanged (object sender, EventArgs e)
	{
		RefilterCashTables ();
	}

	protected void RefilterCashTables()
	{
		switch (notebookCash.CurrentPage) {
		case 0:
			CashIncomeFilter.Refilter ();
			break;
		case 1:
			CashExpenseFilter.Refilter();
			break;
		case 2:
			CashAdvanceFilter.Refilter ();
			break;
		}
		CalculateCashSum ();
	}

	protected void OnTreeviewIncomeCursorChanged (object sender, EventArgs e)
	{
		bool isSelect = treeviewIncome.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
	}

	protected void OnTreeviewExpenseCursorChanged (object sender, EventArgs e)
	{
		bool isSelect = treeviewExpense.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
	}

	protected void OnTreeviewAdvanceCursorChanged (object sender, EventArgs e)
	{
		bool isSelect = treeviewAdvance.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		if(QSMain.User.Permissions["edit_slips"])
			buttonDel.Sensitive = isSelect;
		else
			buttonDel.Sensitive = false;
	}
	
	protected void OnTreeviewIncomeRowActivated (object o, RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}

	protected void OnTreeviewExpenseRowActivated (object o, RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}

	protected void OnTreeviewAdvanceRowActivated (object o, RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}

	protected void CalculateCashSum ()
	{
		decimal Sum = 0;
		TreeIter iter;
		TreeModelFilter Model;
		int sumColumn;

		switch (notebookCash.CurrentPage)
		{
		case 0:
			Model = CashIncomeFilter;
			sumColumn = (int)CashIncomeCol.sum;
			break;
		case 1:
			Model = CashExpenseFilter;
			sumColumn = (int)CashExpenseCol.sum;
			break;
		case 2:
			Model = CashAdvanceFilter;
			sumColumn = (int)CashAdvanceCol.sum;
			break;
		default:
			return;
		}
		if(Model.GetIterFirst(out iter))
		{
			Sum = (decimal)Model.GetValue(iter, sumColumn);
			while (Model.IterNext(ref iter)) 
			{
				Sum += (decimal)Model.GetValue(iter, sumColumn);
			}
		}
		labelSum.LabelProp = String.Format("Сумма документов: {0:C} ", Sum);
	}

	protected void CalculateTotalCash()
	{
		TreeIter iter;
		decimal SumIncome, SumExpense;

		if(comboCashOrg.Active > 0)
			labelCashSumOrg.LabelProp = comboCashOrg.ActiveText;
		else
			labelCashSumOrg.LabelProp = "Все организации";
		if(comboCashCash.Active > 0)
			labelCashSumCash.LabelProp = comboCashCash.ActiveText;
		else
			labelCashSumCash.LabelProp = "Все кассы";

		string sql = "SELECT SUM(credit_slips.sum) as total FROM credit_slips ";
		string sqlorg = "org_id = @org_id";
		string sqlcash = "cash_id = @cash_id";
		string sqlwhere = "";

		if(comboCashOrg.Active > 0 || comboCashCash.Active > 0)
		{
			sqlwhere += "WHERE ";
			if(comboCashOrg.Active > 0)
			{
				sqlwhere += sqlorg;
				if(comboCashCash.Active > 0)
					sqlwhere += " AND " + sqlcash;
			}
			else
				sqlwhere += sqlcash;
		}

		sql += sqlwhere;

		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

		if(comboCashOrg.GetActiveIter (out iter) && comboCashOrg.Active > 0)
			cmd.Parameters.AddWithValue("@org_id", comboCashOrg.Model.GetValue(iter,1));
		if(comboCashCash.GetActiveIter (out iter) && comboCashCash.Active > 0)
			cmd.Parameters.AddWithValue("@cash_id", comboCashCash.Model.GetValue(iter,1));

		MySqlDataReader rdr = cmd.ExecuteReader();
		if(rdr.Read () && rdr["total"] != DBNull.Value)
			SumIncome = rdr.GetDecimal("total");
		else
			SumIncome = 0m;
		rdr.Close ();

		sql = "SELECT SUM(debit_slips.sum) as total FROM debit_slips " + sqlwhere;
		cmd = new MySqlCommand(sql, QSMain.connectionDB);
		if(comboCashOrg.GetActiveIter (out iter) && comboCashOrg.Active > 0)
			cmd.Parameters.AddWithValue("@org_id", comboCashOrg.Model.GetValue(iter,1));
		if(comboCashCash.GetActiveIter (out iter) && comboCashCash.Active > 0)
			cmd.Parameters.AddWithValue("@cash_id", comboCashCash.Model.GetValue(iter,1));
		rdr = cmd.ExecuteReader();
		if(rdr.Read () && rdr["total"] != DBNull.Value)
			SumExpense = rdr.GetDecimal("total");
		else 
			SumExpense = 0m;
		rdr.Close ();

		decimal TotalCash = SumIncome - SumExpense;
		labelCashSum.LabelProp = String.Format("<big>{0:C}</big>", TotalCash);
	}

	protected void OnEntryEmployeeChanged (object sender, EventArgs e)
	{
		RefilterCashTables ();
	}

	protected void OnButtonCashDebtsClicked (object sender, EventArgs e)
	{
		AccountableDebts WinDebts = new AccountableDebts();
		WinDebts.ShowAll ();
		WinDebts.Run ();
		WinDebts.Destroy ();
	}
	
	protected void OnButtonCashBalanceClicked (object sender, EventArgs e)
	{
		CashBalance winBalance = new CashBalance();
		winBalance.Show();
		winBalance.Run();
		winBalance.Destroy();
	}

	protected void OnButtonCashEmployeeClearClicked (object sender, EventArgs e)
	{
		entryCashEmployee.Text = "";
	}
	
	protected void OnButtonCashContractorClearClicked (object sender, EventArgs e)
	{
		entryCashContractor.Text = "";
	}

	protected virtual void ExpenseCopy (object o, EventArgs args)
	{
		TreeIter iter;
		treeviewExpense.Selection.GetSelected(out iter);
		int itemid = Convert.ToInt32(CashExpenseSort.GetValue(iter, (int)CashExpenseCol.id));

		ExpenseSlipDlg winExpenseSlip = new ExpenseSlipDlg();
		winExpenseSlip.SlipFill(itemid, true);
		winExpenseSlip.Show();
		winExpenseSlip.Run();
		winExpenseSlip.Destroy();
		UpdateCashExpense();
		CalculateTotalCash();
	}

	[GLib.ConnectBefore]
	protected void OnTreeviewExpenseButtonReleaseEvent (object o, ButtonReleaseEventArgs args)
	{
		bool ItemSelected = treeviewExpense.Selection.CountSelectedRows() == 1;

		if((int)args.Event.Button == 3)
		{
			Gtk.Menu popupBox = new Gtk.Menu();
			Gtk.MenuItem MenuItem = new MenuItem("Создать копированием");
			MenuItem.Activated += new EventHandler(ExpenseCopy);
			MenuItem.Sensitive = ItemSelected;
			popupBox.Add(MenuItem);                     
			popupBox.ShowAll();
			popupBox.Popup();
		}
	}

	[GLib.ConnectBefore]
	protected void OnTreeviewIncomeButtonReleaseEvent (object o, ButtonReleaseEventArgs args)
	{
		bool ItemSelected = treeviewIncome.Selection.CountSelectedRows() == 1;

		TreeIter iter;
		if(treeviewIncome.Selection.GetSelected(out iter) && CashIncomeSort.GetValue(iter, (int)CashIncomeCol.operation).ToString() == "payment")
			ItemSelected=false;

		if((int)args.Event.Button == 3)
		{
			Gtk.Menu popupBox = new Gtk.Menu();
			Gtk.MenuItem MenuItem = new MenuItem("Создать копированием");
			MenuItem.Activated += new EventHandler(IncomeCopy);
			MenuItem.Sensitive = ItemSelected;
			popupBox.Add(MenuItem);                     
			popupBox.ShowAll();
			popupBox.Popup();
		}
	}

	protected virtual void IncomeCopy (object o, EventArgs args)
	{
		TreeIter iter;
		treeviewIncome.Selection.GetSelected(out iter);
		int itemid = Convert.ToInt32(CashIncomeSort.GetValue(iter, (int)CashIncomeCol.id));
		IncomeSlipDlg winIncomeSlip = new IncomeSlipDlg();
		winIncomeSlip.SlipFill(itemid, true);
		winIncomeSlip.Show();
		winIncomeSlip.Run();
		winIncomeSlip.Destroy();
		UpdateCashIncome();
		CalculateTotalCash();
	}	

	[GLib.ConnectBefore]
	protected void OnTreeviewAdvanceButtonReleaseEvent (object o, ButtonReleaseEventArgs args)
	{
		bool ItemSelected = treeviewAdvance.Selection.CountSelectedRows() == 1;

		if((int)args.Event.Button == 3)
		{
			Gtk.Menu popupBox = new Gtk.Menu();
			Gtk.MenuItem MenuItem = new MenuItem("Создать копированием");
			MenuItem.Activated += new EventHandler(AdvanceCopy);
			MenuItem.Sensitive = ItemSelected;
			popupBox.Add(MenuItem);                     
			popupBox.ShowAll();
			popupBox.Popup();
		}
	}

	protected virtual void AdvanceCopy (object o, EventArgs args)
	{
		TreeIter iter;
		treeviewAdvance.Selection.GetSelected(out iter);
		int itemid = Convert.ToInt32(CashAdvanceSort.GetValue(iter, (int)CashAdvanceCol.id));

		AdvanceStatement winAdvance = new AdvanceStatement();
		winAdvance.StatementFill(itemid, true);
		winAdvance.Show();
		winAdvance.Run();
		winAdvance.Destroy();
		UpdateCashAdvance();
	}
}
