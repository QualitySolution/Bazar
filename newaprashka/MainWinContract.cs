using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore ContractListStore;
	Gtk.TreeModelFilter Contractfilter;

	void PrepareContract()
	{
		//Заполняем комбобокс
		MainClass.ComboFillReference(comboContractOrg, "organizations", 1);
		MainClass.ComboFillReference(comboContractPlaceT,"place_types",1);
		
		//Создаем таблицу "Договора"
		ContractListStore = new Gtk.ListStore (typeof (bool), typeof (string), typeof (int), typeof (string), typeof (int), typeof (string), typeof (string),
		                                       typeof (int), typeof (string), typeof (string));
		
		treeviewContract.AppendColumn("Актив.", new Gtk.CellRendererToggle (), "active", 0);
		treeviewContract.AppendColumn("Номер", new Gtk.CellRendererText (), "text", 1);
		//ID организации - 2
		treeviewContract.AppendColumn("Организация", new Gtk.CellRendererText (), "text", 3);
		//ID Тип места -4
		//ID Номер мета - 5
		treeviewContract.AppendColumn("Место", new Gtk.CellRendererText (), "text", 6);
		//ID Арендатора - 7
		treeviewContract.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", 8);
		treeviewContract.AppendColumn("Дата окончания", new Gtk.CellRendererText (), "text", 9);


		Contractfilter = new Gtk.TreeModelFilter (ContractListStore, null);
		Contractfilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeContract);
		treeviewContract.Model = Contractfilter;
		treeviewContract.ShowAll();
	}

	void UpdateContract()
	{
		MainClass.StatusMessage("Получаем таблицу договоров...");

		TreeIter iter;
		
		string sql = "SELECT contracts.*, place_types.name as type, lessees.name as lessee, organizations.name as organization FROM contracts " +
				"LEFT JOIN place_types ON contracts.place_type_id = place_types.id " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"LEFT JOIN organizations ON contracts.org_id = organizations.id";
		bool WhereExist = false;
		if(comboContractPlaceT.GetActiveIter(out iter) && comboContractPlaceT.Active != 0)
		{
			sql += " WHERE contracts.place_type_id = '" + comboContractPlaceT.Model.GetValue(iter,1) + "' ";
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
		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		int lessee_id;
		int org_id;
		bool parsed, active, cancaled;
		String End_date;
		
		ContractListStore.Clear();
		while (rdr.Read())
		{
			parsed = int.TryParse (rdr["lessee_id"].ToString(), out lessee_id);
			parsed = int.TryParse (rdr["org_id"].ToString(), out org_id);
			cancaled = (rdr["cancel_date"] != DBNull.Value);
			if(cancaled)
			{
				active = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["cancel_date"] >= DateTime.Now.Date);
				End_date = DateTime.Parse(rdr["cancel_date"].ToString()).ToShortDateString();
			}
			else
			{
				active = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["end_date"] >= DateTime.Now.Date);
				End_date = DateTime.Parse( rdr["end_date"].ToString ()).ToShortDateString();
			}
			ContractListStore.AppendValues(active,
			                             rdr["number"].ToString(),
			                             org_id,
			                             rdr["organization"].ToString(),
			                             int.Parse(rdr["place_type_id"].ToString()),
			                             rdr["place_no"].ToString(),
			                             rdr["type"].ToString() + " - " + rdr["place_no"].ToString(),
			                             lessee_id,
			                             rdr["lessee"].ToString(),
			                             End_date);
		}
		rdr.Close();
		
		MainClass.StatusMessage("Ok");
		
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
		
		if(model.GetValue (iter, 1) == null)
			return false;
		
		if (entryContractLessee.Text != "" && model.GetValue (iter, 8) != null)
		{
			cellvalue  = model.GetValue (iter, 8).ToString();
			filterLessee = cellvalue.IndexOf (entryContractLessee.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryContractNumber.Text != "" && model.GetValue (iter, 1) != null)
		{
			cellvalue  = model.GetValue (iter, 1).ToString();
			filterNumber = cellvalue.IndexOf (entryContractNumber.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryContractPlaceN.Text != "" && model.GetValue (iter, 5) != null)
		{
			cellvalue  = model.GetValue (iter, 5).ToString();
			filterPlaceN = cellvalue.IndexOf (entryContractPlaceN.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		return (filterLessee && filterNumber && filterPlaceN);
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
}