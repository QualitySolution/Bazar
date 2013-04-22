using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using bazar;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore AccrualListStore;
	Gtk.TreeModelFilter Accrualfilter;
	
	void PrepareAccrual()
	{
		//Заполняем комбобокс
		MainClass.ComboFillReference(comboAccrualOrg, "organizations", 1);
		MainClass.ComboAccrualYearsFill (comboAccuralYear);
		comboAccrualMonth.Active = DateTime.Now.Month;
		
		//Создаем таблицу "Договора"
		AccrualListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (int), typeof (string), 
		                                      typeof (int), typeof (string), typeof (string), typeof (decimal),
		                                      typeof (string), typeof (decimal),typeof (string), typeof (decimal),
		                                      typeof (bool));
				
		treeviewAccrual.AppendColumn("Номер", new Gtk.CellRendererText (), "text", 0);
		treeviewAccrual.AppendColumn("Месяц", new Gtk.CellRendererText (), "text", 1);
		treeviewAccrual.AppendColumn("Договор", new Gtk.CellRendererText (), "text", 3);
		treeviewAccrual.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", 5);
		treeviewAccrual.AppendColumn("Начислено", new Gtk.CellRendererText (), "text", 6);
		// 7 - сумма decimal
		treeviewAccrual.AppendColumn("Оплачено", new Gtk.CellRendererText (), "text", 8);
		// 9 - оплачено decimal
		treeviewAccrual.AppendColumn("Долг", new Gtk.CellRendererText (), new Gtk.TreeCellDataFunc (RenderDebtColumn));
		// 11 - долг decimal
		treeviewAccrual.AppendColumn("Незаполнено", new Gtk.CellRendererToggle (), "active", 12);

		Accrualfilter = new Gtk.TreeModelFilter (AccrualListStore, null);
		Accrualfilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeAccrual);
		treeviewAccrual.Model = Accrualfilter;
		treeviewAccrual.ShowAll();
	}

	void UpdateAccrual()
	{
		if(AccrualListStore == null)
			return;
		MainClass.StatusMessage("Получаем таблицу начислений...");
		
		TreeIter iter;
		
		string sql = "SELECT accrual.id as id, month, year, contract_no, paid, no_complete, contracts.lessee_id as lessee_id, " +
			"lessees.name as lessee, sumtable.sum as sum, paidtable.sum as paidsum FROM accrual " +
			"LEFT JOIN contracts ON contracts.number = contract_no " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"LEFT JOIN (SELECT accrual_id, SUM(count * price) as sum FROM accrual_pays GROUP BY accrual_id) as sumtable " +
				"ON sumtable.accrual_id = accrual.id " +
				"LEFT JOIN (SELECT accrual_id, SUM(sum) as sum FROM credit_slips GROUP BY accrual_id) as paidtable " +
				"ON paidtable.accrual_id = accrual.id  WHERE year = @year ";
	
		if(comboAccrualMonth.Active > 0)
		{
			sql += "AND accrual.month = '" + comboAccrualMonth.Active + "' ";
		}
		if(comboAccrualOrg.GetActiveIter(out iter) && comboAccrualOrg.Active > 0)
		{
			sql += " AND contracts.org_id = '" + comboAccrualOrg.Model.GetValue(iter,1) + "' ";
		}
		if(checkNotComplete.Active)
		{
			sql += " AND no_complete = TRUE ";
		}
		if(checkOnlyNotPaid.Active)
		{
			sql += " AND paid = FALSE ";
		}
		MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
		cmd.Parameters.AddWithValue("@year", comboAccuralYear.ActiveText);
		MySqlDataReader rdr = cmd.ExecuteReader();

		decimal rowsum, rowpaidsum;
		AccrualListStore.Clear();
		while (rdr.Read())
		{
			if(rdr["sum"] == DBNull.Value)
				rowsum = 0m;
			else
				rowsum = rdr.GetDecimal("sum");
			if(rdr["paidsum"] == DBNull.Value)
				rowpaidsum = 0m;
			else
				rowpaidsum = rdr.GetDecimal("paidsum");
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
		
		bool isSelect = treeviewAccrual.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}
	
	private bool FilterTreeAccrual (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryAccrualLessee.Text == "" && entryAccrualContract.Text == "")
			return true;
		bool filterLessee = true;
		bool filterContract = true;
		string cellvalue;
		
		if(model.GetValue (iter, 5) == null)
			return false;
		
		if (entryAccrualLessee.Text != "" && model.GetValue (iter, 5) != null)
		{
			cellvalue  = model.GetValue (iter, 5).ToString();
			filterLessee = cellvalue.IndexOf (entryAccrualLessee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}

		if (entryAccrualContract.Text != "" && model.GetValue (iter, 3) != null)
		{
			cellvalue  = model.GetValue (iter, 3).ToString();
			filterContract = cellvalue.IndexOf (entryAccrualContract.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}

		return filterLessee && filterContract;
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
		int itemid = Convert.ToInt32(Accrualfilter.GetValue(iter,0));

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
			Sum = (decimal)Model.GetValue(iter, 7);
			PaidSum = (decimal)Model.GetValue(iter, 9);
			DebtSum = (decimal)Model.GetValue(iter, 11);
			while (Model.IterNext(ref iter)) 
			{
				Sum += (decimal)Model.GetValue(iter, 7);
				PaidSum += (decimal)Model.GetValue(iter, 9);
				DebtSum += (decimal)Model.GetValue(iter, 11);
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
		decimal Debt = (decimal) model.GetValue (iter, 11);
		string DebtText = model.GetValue (iter, 10).ToString ();
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
}