using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore AccrualListStore;
	Gtk.TreeModelFilter Accrualfilter;
	Gtk.TreeModelSort AccrualSort;

	private string NameOfAllOption = "Все";
	private enum AccrualCol{
		id,
		month_text,
		month,
		contract,
		lessee_id,
		lessee,
		sum_text,
		sum,
		paidsum_text,
		paidsum,
		debt_text,
		debt,
		not_complete
	};
	
	void PrepareAccrual()
	{
		//Заполняем комбобокс
		ComboWorks.ComboFillReference(comboAccrualOrg, "organizations", ComboWorks.ListMode.WithAll);
		ComboWorks.ComboFillReference(comboAccrualCash,"cash", ComboWorks.ListMode.WithAll);
		MainClass.ComboAccrualYearsFill (comboAccuralYear, NameOfAllOption);
		comboAccrualMonth.Active = DateTime.Now.Month;
		
		//Создаем таблицу "Начислений"
		AccrualListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (int), typeof (string), 
		                                      typeof (int), typeof (string), typeof (string), typeof (decimal),
		                                      typeof (string), typeof (decimal),typeof (string), typeof (decimal),
		                                      typeof (bool));
				
		treeviewAccrual.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)AccrualCol.id);
		treeviewAccrual.AppendColumn("Месяц", new Gtk.CellRendererText (), "text", (int)AccrualCol.month_text);
		treeviewAccrual.AppendColumn("Договор", new Gtk.CellRendererText (), "text", (int)AccrualCol.contract);
		treeviewAccrual.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", (int)AccrualCol.lessee);
		treeviewAccrual.AppendColumn("Начислено", new Gtk.CellRendererText (), "text", (int)AccrualCol.sum_text);
		treeviewAccrual.AppendColumn("Оплачено", new Gtk.CellRendererText (), "text", (int)AccrualCol.paidsum_text);
		treeviewAccrual.AppendColumn("Долг", new Gtk.CellRendererText (), new Gtk.TreeCellDataFunc (RenderDebtColumn));
		treeviewAccrual.AppendColumn("Незаполнено", new Gtk.CellRendererToggle (), "active", (int)AccrualCol.not_complete);

		Accrualfilter = new Gtk.TreeModelFilter (AccrualListStore, null);
		Accrualfilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeAccrual);
		AccrualSort = new TreeModelSort (Accrualfilter);
		AccrualSort.SetSortFunc ((int)AccrualCol.sum , SumSortFunction);
		AccrualSort.SetSortFunc ((int)AccrualCol.paidsum , PaidSumSortFunction);
		AccrualSort.SetSortFunc ((int)AccrualCol.debt , DebtSortFunction);
		treeviewAccrual.Model = AccrualSort;
		treeviewAccrual.Columns [0].SortColumnId = (int)AccrualCol.id;
		treeviewAccrual.Columns [1].SortColumnId = (int)AccrualCol.month;
		treeviewAccrual.Columns [2].SortColumnId = (int)AccrualCol.contract;
		treeviewAccrual.Columns [3].SortColumnId = (int)AccrualCol.lessee;
		treeviewAccrual.Columns [4].SortColumnId = (int)AccrualCol.sum;
		treeviewAccrual.Columns [5].SortColumnId = (int)AccrualCol.paidsum;
		treeviewAccrual.Columns [6].SortColumnId = (int)AccrualCol.debt;
		treeviewAccrual.ShowAll();
	}

	void UpdateAccrual()
	{
		if(AccrualListStore == null)
			return;
		MainClass.StatusMessage("Получаем таблицу начислений...");
		
		TreeIter iter;
		string WhereCash = "";
		if(comboAccrualCash.GetActiveIter(out iter) && comboAccrualCash.Active > 0)
		{
			WhereCash = String.Format ("WHERE cash_id = '{0}'", ComboWorks.GetActiveId (comboAccrualCash));
		}
		DBWorks.SQLHelper sql = new DBWorks.SQLHelper(
			"SELECT accrual.id as id, month, year, contracts.number as contract_no, no_complete, contracts.lessee_id as lessee_id, " +
			"lessees.name as lessee, sumtable.sum as sum, paidtable.sum as paidsum FROM accrual " +
			"LEFT JOIN contracts ON contracts.id = accrual.contract_id " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
			"LEFT JOIN (SELECT accrual_id, SUM(count * price) as sum FROM accrual_pays " + WhereCash + " GROUP BY accrual_id) as sumtable " +
				"ON sumtable.accrual_id = accrual.id " +
			"LEFT JOIN (SELECT accrual_id, SUM(sum) as sum FROM credit_slips " + WhereCash + " GROUP BY accrual_id) as paidtable " +
			"ON paidtable.accrual_id = accrual.id");
		sql.StartNewList (" WHERE ", " AND ");

		if (comboAccuralYear.ActiveText != NameOfAllOption)
			sql.AddAsList (String.Format("year = '{0}'", comboAccuralYear.ActiveText));

		if(comboAccrualMonth.Active > 0)
		{
			sql.AddAsList("accrual.month = '" + comboAccrualMonth.Active + "' ");
		}
		if(comboAccrualOrg.GetActiveIter(out iter) && comboAccrualOrg.Active > 0)
		{
			sql.AddAsList ("contracts.org_id = '" + comboAccrualOrg.Model.GetValue(iter,1) + "' ");
		}
		if(checkNotComplete.Active)
		{
			sql.AddAsList("no_complete = TRUE");
		}
		if(checkOnlyNotPaid.Active)
		{
			sql.AddAsList("IFNULL(paidtable.sum, 0) < IFNULL(sumtable.sum,0)");
		}
		MySqlCommand cmd = new MySqlCommand(sql.Text, QSMain.connectionDB);
		MySqlDataReader rdr = cmd.ExecuteReader();

		decimal rowsum, rowpaidsum;
		AccrualListStore.Clear();
		while (rdr.Read())
		{
			rowsum = DBWorks.GetDecimal (rdr, "sum", 0m);
			rowpaidsum = DBWorks.GetDecimal (rdr, "paidsum", 0m);
			AccrualListStore.AppendValues(rdr.GetInt32("id"),
			                              String.Format("{0:MMMM}", new DateTime(1, rdr.GetInt32("month"), 1)),
			                              rdr.GetInt32("month"),
			                              rdr["contract_no"].ToString (),
			                              rdr.GetInt32("lessee_id"),
			                              rdr["lessee"].ToString(),
			                              String.Format ("{0:C}", rowsum),
			               				  rowsum,
			                              String.Format ("{0:C}", rowpaidsum),
			                              rowpaidsum,
			                              String.Format ("{0:C}", rowsum - rowpaidsum),
			                              rowsum - rowpaidsum,
			               				  rdr.GetBoolean("no_complete"));
		}
		rdr.Close();
		
		MainClass.StatusMessage("Ok");
		CalculateAccrualSum();		
		OnTreeviewAccrualCursorChanged (null, EventArgs.Empty);
	}
	
	private bool FilterTreeAccrual (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryAccrualLessee.Text == "" && entryAccrualContract.Text == "")
			return true;
		bool filterLessee = true;
		bool filterContract = true;
		string cellvalue;
		
		if(model.GetValue (iter, (int)AccrualCol.lessee) == null)
			return false;
		
		if (entryAccrualLessee.Text != "" && model.GetValue (iter, (int)AccrualCol.lessee) != null)
		{
			cellvalue  = model.GetValue (iter, (int)AccrualCol.lessee).ToString();
			filterLessee = cellvalue.IndexOf (entryAccrualLessee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}

		if (entryAccrualContract.Text != "" && model.GetValue (iter, (int)AccrualCol.contract) != null)
		{
			cellvalue  = model.GetValue (iter, (int)AccrualCol.contract).ToString();
			filterContract = cellvalue.IndexOf (entryAccrualContract.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}

		return filterLessee && filterContract;
	}

	private int SumSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)AccrualCol.sum);
		object ob = model.GetValue(b, (int)AccrualCol.sum);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	private int PaidSumSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)AccrualCol.paidsum);
		object ob = model.GetValue(b, (int)AccrualCol.paidsum);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	private int DebtSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)AccrualCol.debt);
		object ob = model.GetValue(b, (int)AccrualCol.debt);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((decimal)oa).CompareTo ((decimal)ob);
	}

	protected void OnComboAccrualOrgChanged (object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
	
	protected void OnComboAccrualMonthChanged (object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
	
	protected void OnComboAccuralYearChanged (object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
	
	protected void OnCheckOnlyNotPaidClicked (object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
	
	protected void OnCheckNotCompleteClicked (object sender, EventArgs e)
	{
		UpdateAccrual ();
	}

	protected void OnTreeviewAccrualCursorChanged (object sender, EventArgs e)
	{
		bool isSelect = treeviewAccrual.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
		buttonMakePayment.Sensitive = isSelect;
	}
	
	protected void OnTreeviewAccrualRowActivated (object o, RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}

	protected void OnEntryAccrualLesseeChanged (object sender, EventArgs e)
	{
		Accrualfilter.Refilter ();
		CalculateAccrualSum();
	}
	
	protected void OnButtonAccrualLesseeClearClicked (object sender, EventArgs e)
	{
		entryAccrualLessee.Text = "";
	}

	protected void OnButtonAccrualMassClicked (object sender, EventArgs e)
	{
		MassAccrualCreation winMass = new MassAccrualCreation();
		winMass.Show();
		winMass.Run();
		winMass.Destroy();
		UpdateAccrual ();
	}

	protected void OnButtonMakePaymentClicked (object sender, EventArgs e)
	{
		TreeIter iter;
		treeviewAccrual.Selection.GetSelected(out iter);
		int itemid = Convert.ToInt32(AccrualSort.GetValue(iter, (int)AccrualCol.id));

		PayAccrual winPay = new PayAccrual();
		winPay.FillPayTable (itemid);
		winPay.ShowAll ();
		if((ResponseType)winPay.Run () == ResponseType.Ok)
			UpdateAccrual ();
		winPay.Destroy ();
	}

	protected void CalculateAccrualSum ()
	{
		decimal Sum = 0;
		decimal PaidSum = 0;
		decimal DebtSum = 0;
		TreeIter iter;
		TreeModelFilter Model;
		Model = Accrualfilter;

		if(Model.GetIterFirst(out iter))
		{
			Sum = (decimal)Model.GetValue(iter, (int)AccrualCol.sum);
			PaidSum = (decimal)Model.GetValue(iter, (int)AccrualCol.paidsum);
			DebtSum = (decimal)Model.GetValue(iter, (int)AccrualCol.debt);
			while (Model.IterNext(ref iter)) 
			{
				Sum += (decimal)Model.GetValue(iter, (int)AccrualCol.sum);
				PaidSum += (decimal)Model.GetValue(iter, (int)AccrualCol.paidsum);
				DebtSum += (decimal)Model.GetValue(iter, (int)AccrualCol.debt);
			}
		}
		labelSum.LabelProp = String.Format("Всего начислено: {0:C} Оплачено: {1:C} Долг: {2:C}", Sum, PaidSum, DebtSum);
	}

	protected void OnButtonAccrualContractClearClicked (object sender, EventArgs e)
	{
		entryAccrualContract.Text = "";
	}
	
	protected void OnEntryAccrualContractChanged (object sender, EventArgs e)
	{
		Accrualfilter.Refilter ();
		CalculateAccrualSum();
	}

	private void RenderDebtColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		decimal Debt = (decimal) model.GetValue (iter, (int)AccrualCol.debt);
		string DebtText = model.GetValue (iter, (int)AccrualCol.debt_text).ToString ();
		if (Debt > 0) 
		{
			(cell as Gtk.CellRendererText).Foreground = "red";
		} 
		else 
		{
			(cell as Gtk.CellRendererText).Foreground = "darkgreen";
		}
		(cell as Gtk.CellRendererText).Text = DebtText;
	}

	protected void OnComboAccrualCashChanged(object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
}