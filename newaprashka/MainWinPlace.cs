using System;
using Gtk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using bazar;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore PlacesListStore;
	Gtk.TreeModelFilter Placefilter;

	void PreparePlaces()
	{
		//Заполняем комбобокс
		MainClass.ComboFillReference(comboPlaceType,"place_types",1);
		MainClass.ComboFillReference(comboPlaceOrg,"organizations",1);

		//Создаем таблицу "Места"
		PlacesListStore = new Gtk.ListStore (typeof (int), typeof (string),typeof (string), typeof (string),
		                                     typeof (string), typeof (int),typeof (string), typeof (int),
		                                     typeof (string), typeof (string), typeof (int), typeof (double));
		
		//ID тип места - 0
		treeviewPlaces.AppendColumn ("Тип", new Gtk.CellRendererText (), "text", 1);
		treeviewPlaces.AppendColumn ("Номер", new Gtk.CellRendererText (), "text", 2);
		treeviewPlaces.AppendColumn ("Площадь", new Gtk.CellRendererText (), "text", 3);
		treeviewPlaces.AppendColumn ("Арендатор", new Gtk.CellRendererText (), "text", 4);
		//ID Арендатора - 5
		treeviewPlaces.AppendColumn ("Контактное лицо", new Gtk.CellRendererText (), "text", 6);
		//ID Контактного лица - 7
		treeviewPlaces.AppendColumn ("Телефоны К.Л.", new Gtk.CellRendererText (), "text", 8);
		treeviewPlaces.AppendColumn ("Организация", new Gtk.CellRendererText (), "text", 9);
		//ID организации - 10
		
		Placefilter = new Gtk.TreeModelFilter (PlacesListStore, null);
		Placefilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreePlace);
		treeviewPlaces.Model = Placefilter;
		treeviewPlaces.ShowAll();
	}

	void UpdatePlaces()
	{
		MainClass.StatusMessage("Получаем таблицу c местами...");
		TreeIter iter;
		
		string sql = "SELECT places.*, place_types.name as type, contracts.lessee_id as lessee_id, lessees.name as lessee, contact_persons.name as contact," +
			"contact_persons.telephones as telephones, organizations.name as organization FROM places " +
				"LEFT JOIN place_types ON places.type_id = place_types.id " +
				"LEFT JOIN contact_persons ON places.contact_person_id = contact_persons.id " +
				"LEFT JOIN organizations ON places.org_id = organizations.id " +
				"LEFT JOIN contracts ON places.type_id = contracts.place_type_id AND places.place_no = contracts.place_no AND " +
				"((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
				"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id";
		bool WhereExist = false;
		if(comboPlaceType.GetActiveIter(out iter) && comboPlaceType.Active != 0)
		{
			sql += " WHERE places.type_id = '" + comboPlaceType.Model.GetValue(iter,1) + "' ";
			WhereExist = true;
		}
		if(comboPlaceOrg.GetActiveIter(out iter) && comboPlaceOrg.Active != 0)
		{
			if(!WhereExist) 
				sql += " WHERE";
			else
				sql += " AND";
			sql += " places.org_id = '" + comboPlaceOrg.Model.GetValue(iter,1) + "' ";
			WhereExist = true;
		}
		MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		int lessee_id;
		int contact_person_id;
		int org_id;
		double area;
		
		PlacesListStore.Clear();
		while (rdr.Read())
		{
			if(rdr["lessee_id"] != DBNull.Value)
				lessee_id =  rdr.GetInt32("lessee_id");
			else
				lessee_id = -1;
			if(rdr["contact_person_id"] != DBNull.Value)
				contact_person_id = rdr.GetInt32 ("contact_person_id");
			else
				contact_person_id = -1;
			if(rdr["org_id"] != DBNull.Value)
				org_id = rdr.GetInt32("org_id");
			else
				org_id = -1;
			if(rdr["area"] != DBNull.Value)
				area = Convert.ToDouble(rdr["area"].ToString());
			else
				area = 0;
			PlacesListStore.AppendValues(int.Parse(rdr["type_id"].ToString()),
			                             rdr["type"].ToString(),
			                             rdr["place_no"].ToString(),
			                             rdr["area"].ToString(),
			                             rdr["lessee"].ToString(),
			                             lessee_id,
			                             rdr["contact"].ToString(),
			                             contact_person_id,
			                             rdr["telephones"].ToString(),
			                             rdr["organization"].ToString(),
			                             org_id,
			                             area);
		}
		rdr.Close();
		
		MainClass.StatusMessage("Ok");
		
		bool isSelect = treeviewPlaces.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
		CalculateAreaSum();
	}

	protected virtual void OnComboPlaceTypeChanged (object sender, System.EventArgs e)
	{
		UpdatePlaces();
	}

	private bool FilterTreePlace (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryPlaceNum.Text == "" && entryPlaceLess.Text == "" && entryPlaceContact.Text == "")
			return true;
		bool filterNum = true;
		bool filterLes = true;
		bool filterCont = true;
		string cellvalue;
		
		if(model.GetValue (iter, 1) == null)
			return false;
		
		if (entryPlaceNum.Text != "" && model.GetValue (iter, 2) != null)
		{
			cellvalue  = model.GetValue (iter, 2).ToString();
			filterNum = cellvalue.IndexOf (entryPlaceNum.Text) > -1;
		}
		if (entryPlaceLess.Text != "" && model.GetValue (iter, 4) != null)
		{
			cellvalue  = model.GetValue (iter, 4).ToString();
			filterLes = cellvalue.IndexOf (entryPlaceLess.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryPlaceContact.Text != "" && model.GetValue (iter, 6) != null)
		{
			cellvalue  = model.GetValue (iter, 6).ToString();
			filterCont = cellvalue.IndexOf (entryPlaceContact.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		
		return (filterNum && filterLes && filterCont);
	}

	protected virtual void OnEntryPlaceNumChanged (object sender, System.EventArgs e)
	{
		Placefilter.Refilter ();
		CalculateAreaSum();
	}

	protected virtual void OnTreeviewPlacesCursorChanged (object sender, System.EventArgs e)
	{
		bool isSelect = treeviewPlaces.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	protected virtual void OnTreeviewPlacesRowActivated (object o, Gtk.RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}

	protected virtual void OnEntryPlaceLessChanged (object sender, System.EventArgs e)
	{
		Placefilter.Refilter ();
		CalculateAreaSum();
	}
	
	protected virtual void OnEntryPlaceContactChanged (object sender, System.EventArgs e)
	{
		Placefilter.Refilter ();
		CalculateAreaSum();
	}
	
	protected virtual void OnButton236Clicked (object sender, System.EventArgs e)
	{
		entryPlaceNum.Text = "";		
	}
	
	protected virtual void OnButton237Clicked (object sender, System.EventArgs e)
	{
		entryPlaceLess.Text = "";
	}
	
	protected virtual void OnButton238Clicked (object sender, System.EventArgs e)
	{
		entryPlaceContact.Text = "";
	}

	[GLib.ConnectBefore]
	protected void OnTreeviewPlacesPopupMenu (object o, Gtk.PopupMenuArgs args)
	{
		bool ItemSelected = treeviewPlaces.Selection.CountSelectedRows() == 1;
		TreeIter iter;
		bool setLessee = false;
		bool setContact = false;
		
		if(ItemSelected)
		{
			treeviewPlaces.Selection.GetSelected(out iter);
			setLessee = Convert.ToInt32(Placefilter.GetValue(iter,5)) > 0;
			setContact = Convert.ToInt32(Placefilter.GetValue(iter,7)) > 0;
		}
		Gtk.Menu popupBox = new Gtk.Menu();
		Gtk.MenuItem MenuItemOpenPlace = new MenuItem("Открыть торговое место");
		MenuItemOpenPlace.Activated += new EventHandler(OnPlaceOpenPlace);
		MenuItemOpenPlace.Sensitive = ItemSelected;
		popupBox.Add(MenuItemOpenPlace);           
		Gtk.MenuItem MenuItemOpenLessee = new MenuItem("Открыть арендатора");
		MenuItemOpenLessee.Activated += new EventHandler(OnPlaceOpenLessee);
		MenuItemOpenLessee.Sensitive = ItemSelected && setLessee;
		popupBox.Add(MenuItemOpenLessee);
		Gtk.MenuItem MenuItemOpenContact = new MenuItem("Открыть контактное лицо");
		MenuItemOpenContact.Activated += new EventHandler(OnPlaceOpenContact);
		MenuItemOpenContact.Sensitive = ItemSelected && setContact;
		popupBox.Add(MenuItemOpenContact);           
		popupBox.ShowAll();
		popupBox.Popup();
	}
	
	[GLib.ConnectBefore]
	protected void OnTreeviewPlacesButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
	{
		if((int)args.Event.Button == 3)
		{       
			OnTreeviewPlacesPopupMenu (o, null);
		}
	}
	
	protected virtual void OnPlaceOpenPlace (object o, EventArgs args)
	{
		int result, type;
		string place;
		TreeIter iter;
		
		treeviewPlaces.Selection.GetSelected(out iter);
		place = Placefilter.GetValue(iter,2).ToString ();
		type = Convert.ToInt32(Placefilter.GetValue(iter,0));
		Place winPlace = new Place();
		winPlace.PlaceFill(type,place);
		winPlace.Show();
		result = winPlace.Run();
		winPlace.Destroy();
		if((ResponseType)result == ResponseType.Ok)
		{
			UpdatePlaces();
		}
	}

	protected virtual void OnPlaceOpenLessee (object o, EventArgs args)
	{
		int result, itemid;
		TreeIter iter;
		
		treeviewPlaces.Selection.GetSelected(out iter);
		itemid = Convert.ToInt32(Placefilter.GetValue(iter,5));
		lessee winLessee = new lessee();
		winLessee.LesseeFill(itemid);
		winLessee.Show();
		result = winLessee.Run();
		winLessee.Destroy();
		if((ResponseType)result == ResponseType.Ok)
		{
			UpdatePlaces();
		}
	}
	
	protected virtual void OnPlaceOpenContact (object o, EventArgs args)
	{
		int result, itemid;
		TreeIter iter;
		
		treeviewPlaces.Selection.GetSelected(out iter);
		itemid = Convert.ToInt32(Placefilter.GetValue(iter,7));
		Contact winContact = new Contact();
		winContact.ContactFill(itemid);
		winContact.Show();
		result = winContact.Run();
		winContact.Destroy();
		if((ResponseType)result == ResponseType.Ok)
		{
			UpdatePlaces();
		}
	}
	
	protected void OnComboPlaceOrgChanged (object sender, EventArgs e)
	{
		UpdatePlaces();
	}
	
	protected void CalculateAreaSum ()
	{
		double AreaSum = 0;
		TreeIter iter;
		
		if(Placefilter.GetIterFirst(out iter))
		{
			AreaSum = (double)Placefilter.GetValue(iter,11);
			while (Placefilter.IterNext(ref iter)) 
			{
				AreaSum += (double)Placefilter.GetValue(iter,11);
			}
		}
		labelSum.LabelProp = "Суммарная площадь: " + AreaSum.ToString() + " м<sup>2</sup>";
	}

}