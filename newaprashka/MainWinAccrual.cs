using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;
using System.Collections.Generic;
using Gamma.GtkWidgets;
using System.Diagnostics;
using System.Linq;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore AccrualListStore;

	List<AccrualListEntryDTO> AccrualList;

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
		ComboWorks.ComboFillReference(comboAccrualOrg, "organizations", ComboWorks.ListMode.WithAll, false);
		ComboWorks.ComboFillReference(comboAccrualCash,"cash", ComboWorks.ListMode.WithAll, false);
		ComboWorks.ComboFillReference (comboAccrualItem, "income_items", ComboWorks.ListMode.WithAll, false);
		MainClass.ComboAccrualYearsFill (comboAccuralYear, NameOfAllOption);
		comboAccrualMonth.Active = DateTime.Now.Month;
		
		//Создаем таблицу "Начислений"
		AccrualListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (int), typeof (string), 
		                                      typeof (int), typeof (string), typeof (string), typeof (decimal),
		                                      typeof (string), typeof (decimal),typeof (string), typeof (decimal),
		                                      typeof (bool));
		/*
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
		*/
		treeviewAccrual.ColumnsConfig = ColumnsConfigFactory.Create<AccrualListEntryDTO> ()
			.AddColumn ("Номер").AddTextRenderer (node => node.Id.ToString())
			.AddColumn ("Месяц").AddTextRenderer (node => node.MonthText)
			.AddColumn ("Договор").AddTextRenderer (node => node.ContractNumber)
			.AddColumn ("Арендатор").AddTextRenderer (node => node.Lessee)
			.AddColumn ("Начислено").AddTextRenderer (node => node.SumText)
			.AddColumn ("Оплачено").AddTextRenderer (node => node.PaidSumText)
			.AddColumn ("Долг").AddTextRenderer (node => node.DebtText)
			.AddSetter ((cell, node) => cell.Foreground = node.Debt > 0 ? "red" : "darkgreen")
			.AddColumn ("Незаполнено").AddToggleRenderer (node => node.NotComplete).Editing (false)
			.AddColumn ("")
			.Finish ();
	}

	void UpdateAccrual()
	{
		if(AccrualListStore == null)
			return;
		logger.Info("Получаем таблицу начислений...");
		
		TreeIter iter;
		string cashClause = "";
		if(comboAccrualCash.GetActiveIter(out iter) && comboAccrualCash.Active > 0)
		{
			cashClause = String.Format ("cash_id = '{0}'", ComboWorks.GetActiveId (comboAccrualCash));
		}
		bool legacyPayments = true;
		string incomeItemClause = "";
		if (comboAccrualItem.GetActiveIter (out iter) && comboAccrualItem.Active > 0) 
		{
			incomeItemClause = String.Format ("income_id = '{0}'", ComboWorks.GetActiveId (comboAccrualItem));
			legacyPayments = false;
		}
			
		string clause = ((cashClause.Length > 0) && (incomeItemClause.Length > 0)) ? cashClause +" AND "+ incomeItemClause : cashClause + incomeItemClause ;
		string whereClause = (clause.Length > 0) ? "WHERE " + clause : "";

		string paymentsTableName = legacyPayments ? "credit_slips ": "accrual_pays LEFT JOIN payment_details ON accrual_pays.id=payment_details.accrual_pay_id ";

		DBWorks.SQLHelper sql = new DBWorks.SQLHelper(
			"SELECT accrual.id as id, month, year, contracts.number as contract_no, no_complete, contracts.lessee_id as lessee_id, " +
			"lessees.name as lessee, sumtable.sum as sum, paidtable.sum as paidsum FROM accrual " +
			"LEFT JOIN contracts ON contracts.id = accrual.contract_id " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
			"LEFT JOIN (SELECT accrual_id, SUM(count * price) as sum FROM accrual_pays "+
			"LEFT JOIN services ON accrual_pays.service_id = services.id " +
			whereClause + " GROUP BY accrual_id) as sumtable " +
				"ON sumtable.accrual_id = accrual.id " +
			"LEFT JOIN ("+
			"SELECT accrual_id, SUM(sum) as sum FROM " + paymentsTableName +
			whereClause + " GROUP BY accrual_id) as paidtable " +
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

		var Column = new {
			Sum = rdr.GetOrdinal ("sum"),
			Paidsum = rdr.GetOrdinal ("paidsum"),
			Id = rdr.GetOrdinal ("id"),
			Month = rdr.GetOrdinal ("month"),
			Year = rdr.GetOrdinal("year"),
			ContractNumber = rdr.GetOrdinal ("contract_no"),
			NotComplete = rdr.GetOrdinal ("no_complete"),
			LesseeId = rdr.GetOrdinal ("lessee_id"),
			LesseeName = rdr.GetOrdinal("lessee")
		};
		AccrualList = new List<AccrualListEntryDTO> ();

		while (rdr.Read())
		{
			decimal rowsum, rowpaidsum;
			rowsum = DBWorks.GetDecimal (rdr, Column.Sum, 0m);
			rowpaidsum = DBWorks.GetDecimal (rdr, Column.Paidsum, 0m);
			AccrualList.Add (new AccrualListEntryDTO {
				Id = rdr.GetInt32 (Column.Id),
				MonthText = String.Format ("{0:MMMM}", new DateTime (1, rdr.GetInt32 (Column.Month), 1)),
				Month = rdr.GetInt32 (Column.Month),
				ContractNumber = rdr [Column.ContractNumber].ToString (),
				LesseeId = rdr.GetInt32 (Column.LesseeId),
				Lessee = rdr [Column.LesseeName].ToString (),
				SumText = String.Format ("{0:C}", rowsum),
				Sum = rowsum,
				PaidSumText = String.Format ("{0:C}", rowpaidsum),
				PaidSum = rowpaidsum,
				DebtText = String.Format ("{0:C}", rowsum - rowpaidsum),
				Debt = rowsum - rowpaidsum,
				NotComplete = rdr.GetBoolean (Column.NotComplete)
			});
		}
		rdr.Close ();
		treeviewAccrual.ItemsDataSource = AccrualList;	
		Refilter ();
		logger.Info("Ok");
		CalculateAccrualSum();		
		OnTreeviewAccrualCursorChanged (null, EventArgs.Empty);
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

	protected void Refilter(){		
		List<AccrualListEntryDTO> AccrualsFiltered = AccrualList
			.Where (accrual => accrual.Lessee.IndexOf (entryAccrualLessee.Text, StringComparison.InvariantCultureIgnoreCase) >= 0)
			.Where(accrual=>accrual.ContractNumber.IndexOf(entryAccrualContract.Text,StringComparison.InvariantCultureIgnoreCase)>=0)
			.ToList();
		treeviewAccrual.ItemsDataSource = AccrualsFiltered;
	}

	protected void OnEntryAccrualLesseeChanged (object sender, EventArgs e)
	{
		Refilter ();
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
		int itemid = Convert.ToInt32(treeviewAccrual.GetSelectedObject<AccrualListEntryDTO>().Id);

		PayAccrual winPay = new PayAccrual();
		winPay.FillPayTable (itemid);
		winPay.ShowAll ();
		if((ResponseType)winPay.Run () == ResponseType.Ok)
			UpdateAccrual ();
		winPay.Destroy ();
	}

	protected void CalculateAccrualSum ()
	{		
		var AccrualsFiltered = treeviewAccrual.ItemsDataSource as List<AccrualListEntryDTO>;
		decimal Sum = AccrualsFiltered.Sum (accrual => accrual.Sum);
		decimal PaidSum = AccrualsFiltered.Sum (accrual => accrual.PaidSum);
		decimal DebtSum = AccrualsFiltered.Sum (accrual => accrual.Debt);
		labelSum.LabelProp = String.Format("Всего начислено: {0:C} Оплачено: {1:C} Долг: {2:C}", Sum, PaidSum, DebtSum);
	}

	protected void OnButtonAccrualContractClearClicked (object sender, EventArgs e)
	{
		entryAccrualContract.Text = "";
	}
	
	protected void OnEntryAccrualContractChanged (object sender, EventArgs e)
	{
		Refilter ();
		CalculateAccrualSum();
	}
/*
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
*/
	protected void OnComboAccrualCashChanged(object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
	protected void OnComboAccrualItemChanged(object sender, EventArgs e)
	{
		UpdateAccrual ();
	}
}
public class AccrualListEntryDTO{
	public int Id{ get; set;}
	public int Month{get;set;}
	public string MonthText{ get; set;}
	public int Year{get;set;}
	public bool Paid{get;set;}
	public string ContractNumber{get;set;}
	public bool NotComplete{get;set;}
	public int LesseeId{get;set;}
	public string Lessee{get;set;}
	public decimal Sum{get;set;}
	public string SumText{ get; set;}
	public decimal PaidSum{ get; set;}
	public string PaidSumText{get;set;}
	public decimal Debt{get;set;}
	public string DebtText{get;set;}
}