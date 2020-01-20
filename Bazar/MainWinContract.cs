using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;
using QS.Utilities.Text;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore ContractListStore;
	Gtk.TreeModelFilter Contractfilter;
	Gtk.TreeModelSort ContractSort;

	private enum ContractCol{
		id,
		active,
		number,
		org_id,
		org,
		places_text,
		lessee_id,
		lessee,
		end_date
	}

	void PrepareContract()
	{
		//Заполняем комбобокс
		ComboWorks.ComboFillReference(comboContractOrg, "organizations", ComboWorks.ListMode.WithAll, false, OrderBy: "name");
		ComboWorks.ComboFillReference(comboContractPlaceT,"place_types", ComboWorks.ListMode.WithAll, false, OrderBy: "name");
		
		//Создаем таблицу "Договора"
		ContractListStore = new Gtk.ListStore (typeof (int), 	//0 - ID
		                                       typeof (bool),	//1 - active
		                                       typeof (string),	//2 - number
		                                       typeof (int),	//3 - Id org
		                                       typeof (string), //4 - org
		                                       typeof (string), //5 - places
		                                       typeof (int), 	//6 - id leesse
		                                       typeof (string),	//7 - lesse
		                                       typeof (DateTime)//8 - end date
		                                       );
		
		treeviewContract.AppendColumn("Актив.", new Gtk.CellRendererToggle (), "active", (int)ContractCol.active);
		treeviewContract.AppendColumn("Номер", new Gtk.CellRendererText (), "text", (int)ContractCol.number);
		treeviewContract.AppendColumn("Организация", new Gtk.CellRendererText (), "text", (int)ContractCol.org);
		treeviewContract.AppendColumn("Места", new Gtk.CellRendererText (), "text", (int)ContractCol.places_text);
		treeviewContract.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", (int)ContractCol.lessee);
		treeviewContract.AppendColumn("Дата окончания", new Gtk.CellRendererText (), RenderContractEndDateColumn);

		Contractfilter = new Gtk.TreeModelFilter (ContractListStore, null);
		Contractfilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeContract);
		ContractSort = new TreeModelSort (Contractfilter);
		ContractSort.SetSortFunc ((int)ContractCol.number, ContractNumberSortFunction);
		ContractSort.SetSortFunc ((int)ContractCol.end_date, ContractEndDateSortFunction);
		treeviewContract.Model = ContractSort;
		treeviewContract.Columns [1].SortColumnId = (int)ContractCol.number;
		treeviewContract.Columns [2].SortColumnId = (int)ContractCol.org;
		treeviewContract.Columns [4].SortColumnId = (int)ContractCol.lessee;
		treeviewContract.Columns [5].SortColumnId = (int)ContractCol.end_date;
		treeviewContract.ShowAll();
	}

	void UpdateContract()
	{
		logger.Info("Получаем таблицу договоров...");

		TreeIter iter;
		
		string sql = "SELECT contracts.*, GROUP_CONCAT(DISTINCT CONCAT(place_types.name, '-', places.place_no) SEPARATOR ', ') as places, lessees.name as lessee, organizations.name as organization " +
				"FROM contracts " +
				"LEFT JOIN contract_pays ON contract_pays.contract_id = contracts.id " +
				"LEFT JOIN places ON contract_pays.place_id = places.id " +
				"LEFT JOIN place_types ON places.type_id = place_types.id " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"LEFT JOIN organizations ON contracts.org_id = organizations.id";
		bool WhereExist = false;
		if(comboContractPlaceT.GetActiveIter(out iter) && comboContractPlaceT.Active != 0)
		{
			sql += " WHERE places.type_id = '" + comboContractPlaceT.Model.GetValue(iter,1) + "' ";
			WhereExist = true;
		}
		if(comboContractOrg.GetActiveIter(out iter) && comboContractOrg.Active != 0)
		{
			if(!WhereExist) 
				sql += " WHERE";
			else
				sql += " AND";
			sql += " contracts.org_id = '" + comboContractOrg.Model.GetValue(iter,1) + "' ";
			WhereExist = true;
		}
		if(checkActiveContracts.Active)
		{
			if(!WhereExist) 
				sql += " WHERE";
			else
				sql += " AND";
			sql += " ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
				"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date))";
			if(check30daysContracts.Active)
				sql += " AND contracts.end_date BETWEEN DATE_SUB(CURDATE(), INTERVAL 1 DAY) AND DATE_ADD(CURDATE(), INTERVAL 30 DAY)";
			WhereExist = true;
		}
		sql += " GROUP BY contracts.id";

		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		int lessee_id;
		int org_id;
		bool active, cancaled;
		DateTime endDate;
		
		ContractListStore.Clear();
		while (rdr.Read())
		{
			if (rdr ["lessee_id"] != DBNull.Value)
				lessee_id = rdr.GetInt32 ("lessee_id");
			else
				lessee_id = -1;
			if (rdr ["org_id"] != DBNull.Value)
				org_id = rdr.GetInt32 ("org_id");
			else
				org_id = -1;
			cancaled = (rdr["cancel_date"] != DBNull.Value);
			if(cancaled)
			{
				active = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["cancel_date"] >= DateTime.Now.Date);
				endDate = rdr.GetDateTime ("cancel_date");
			}
			else
			{
				active = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["end_date"] >= DateTime.Now.Date);
				endDate = rdr.GetDateTime ("end_date");
			}
			ContractListStore.AppendValues(rdr.GetInt32 ("id"),
											active,
			                             rdr["number"].ToString(),
			                             org_id,
			                             rdr["organization"].ToString(),
			                             rdr["places"].ToString(),
			                             lessee_id,
			                             rdr["lessee"].ToString(),
			                             endDate);
		}
		rdr.Close();
		
		logger.Info("Ok");
		
		bool isSelect = treeviewContract.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	private bool FilterTreeContract (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryContractLessee.Text == "" && entryContractNumber.Text == "" && entryContractPlaceN.Text == "")
			return true;
		bool filterLessee = true;
		bool filterNumber = true;
		bool filterPlaceN = true;
		string cellvalue;
		
		if(model.GetValue (iter, (int)ContractCol.id) == null)
			return false;
		
		if (entryContractLessee.Text != "" && model.GetValue (iter, (int)ContractCol.lessee) != null)
		{
			cellvalue  = model.GetValue (iter, (int)ContractCol.lessee).ToString();
			filterLessee = cellvalue.IndexOf (entryContractLessee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryContractNumber.Text != "" && model.GetValue (iter, (int)ContractCol.number) != null)
		{
			cellvalue  = model.GetValue (iter, (int)ContractCol.number).ToString();
			filterNumber = cellvalue.IndexOf (entryContractNumber.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (!String.IsNullOrWhiteSpace(entryContractPlaceN.Text) && model.GetValue (iter, (int)ContractCol.places_text) != null)
		{
			cellvalue  = model.GetValue (iter, (int)ContractCol.places_text).ToString();
			filterPlaceN = cellvalue.IndexOf ("-" + entryContractPlaceN.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		return (filterLessee && filterNumber && filterPlaceN);
	}

	private int ContractNumberSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		string oa = (string) model.GetValue(a, (int)ContractCol.number);
		string ob = (string) model.GetValue(b, (int)ContractCol.number);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return NaturalStringComparer.CompareStrings(oa, ob);
	}

	private int ContractEndDateSortFunction(TreeModel model, TreeIter a, TreeIter b) 
	{
		object oa = model.GetValue(a, (int)ContractCol.end_date);
		object ob = model.GetValue(b, (int)ContractCol.end_date);
		if (ob == null)
			return 1;
		if (oa == null)
			return -1;

		return ((DateTime)oa).CompareTo((DateTime)ob);
	}

	private void RenderContractEndDateColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		DateTime date = (DateTime) model.GetValue (iter, (int)ContractCol.end_date);
		(cell as Gtk.CellRendererText).Text = date.ToShortDateString ();
	}

	protected void OnComboContractOrgChanged (object sender, EventArgs e)
	{
		UpdateContract();
	}
	
	protected void OnComboContractPlaceTChanged (object sender, EventArgs e)
	{
		UpdateContract();
	}
	
	protected void OnCheckActiveContractsToggled (object sender, EventArgs e)
	{
		UpdateContract();
	}
	
	protected void OnEntryContractPlaceNChanged (object sender, EventArgs e)
	{
		Contractfilter.Refilter();
	}
	
	protected void OnEntryContractLesseeChanged (object sender, EventArgs e)
	{
		Contractfilter.Refilter();
	}
	
	protected void OnEntryContractNumberChanged (object sender, EventArgs e)
	{
		Contractfilter.Refilter();
	}

	protected void OnButtonContractClearPlaceClicked (object sender, EventArgs e)
	{
		entryContractPlaceN.Text = "";
	}

	protected void OnButtonContractClearLesseeClicked (object sender, EventArgs e)
	{
		entryContractLessee.Text = "";
	}

	protected void OnButtonContractClearNumberClicked (object sender, EventArgs e)
	{
		entryContractNumber.Text = "";
	}

	protected void OnTreeviewContractRowActivated (object o, RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}	

	protected void OnTreeviewContractCursorChanged (object sender, EventArgs e)
	{
		bool isSelect = treeviewContract.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	protected void OnCheck30daysContractsToggled (object sender, EventArgs e)
	{
		bool Active30days = check30daysContracts.Active;
		bool CheckActived = checkActiveContracts.Active;
		checkActiveContracts.Sensitive = !Active30days;
		if(Active30days)
			checkActiveContracts.Active = true;
		if(CheckActived == checkActiveContracts.Active) //Чтобы не запрашивать обновление дважды
			UpdateContract ();
	}

	protected void OnButtonProlongationClicked(object sender, EventArgs e)
	{
		ContractsProlongation winMass = new ContractsProlongation();
		winMass.Show();
		if (winMass.Run () == (int)ResponseType.Ok)
			UpdateContract ();
		winMass.Destroy();
	}
}