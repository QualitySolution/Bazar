using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore LesseesListStore;
	Gtk.TreeModelFilter Lesseesfilter;
	Gtk.TreeModelSort LesseesSort;

	private enum LesseesCol{
		id,
		name,
		boss,
		inn,
		goods_text,
		wholesaler,
		retail
	}

	void PrepareLessee()
	{
		//Создаем таблицу "Арендаторы"
		LesseesListStore = new Gtk.ListStore (typeof (int),typeof (string), typeof (string),
		                                      typeof (string), typeof (string), typeof (bool), typeof (bool));
		
		treeviewLessees.AppendColumn("Код", new Gtk.CellRendererText (), "text", (int)LesseesCol.id);
		treeviewLessees.AppendColumn("Название", new Gtk.CellRendererText (), "text", (int)LesseesCol.name);
		treeviewLessees.AppendColumn("Директор", new Gtk.CellRendererText (), "text", (int)LesseesCol.boss);
		treeviewLessees.AppendColumn("Товары", new Gtk.CellRendererText (), "text", (int)LesseesCol.goods_text);
		treeviewLessees.AppendColumn("Опт", new Gtk.CellRendererToggle (), "active", (int)LesseesCol.wholesaler);
		treeviewLessees.AppendColumn("Розница", new Gtk.CellRendererToggle (), "active", (int)LesseesCol.retail);
		
		Lesseesfilter = new Gtk.TreeModelFilter (LesseesListStore, null);
		Lesseesfilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeLessees);
		LesseesSort = new TreeModelSort (Lesseesfilter);
		treeviewLessees.Model = LesseesSort;
		treeviewLessees.Columns [0].SortColumnId = (int)LesseesCol.id;
		treeviewLessees.Columns [1].SortColumnId = (int)LesseesCol.name;
		treeviewLessees.Columns [2].SortColumnId = (int)LesseesCol.boss;
		treeviewLessees.Columns [3].SortColumnId = (int)LesseesCol.goods_text;
		treeviewLessees.ShowAll();
	}

	void UpdateLessees()
	{
		MainClass.StatusMessage("Получаем таблицу арендаторов...");

		string sql = "SELECT lessees.*, goods.name as goods FROM lessees";
		sql += " LEFT JOIN goods ON lessees.goods_id = goods.id ";
		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		
		using(MySqlDataReader rdr = cmd.ExecuteReader()) 
		{
			LesseesListStore.Clear ();
			while (rdr.Read ()) 
			{
				LesseesListStore.AppendValues (rdr.GetInt32 ("id"),
				                             rdr ["name"].ToString (),
				                             rdr ["FIO_dir"].ToString (),
				                             rdr ["INN"].ToString (),
				                             rdr ["goods"].ToString (),
				                             (bool)rdr ["wholesaler"],
				                             (bool)rdr ["retail"]);
			}
		}
		MainClass.StatusMessage("Ok");
		bool isSelect = treeviewLessees.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	private bool FilterTreeLessees (Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		if (entryFilterName.Text == "" && entryFilterFIO.Text == "" && entryFilterINN.Text == "")
			return true;
		bool filterName = true;
		bool filterFIO = true;
		bool filterINN = true;
		string cellvalue;
		
		if(model.GetValue (iter, 1) == null)
			return false;
		
		if (entryFilterName.Text != "" && model.GetValue (iter, (int)LesseesCol.name) != null)
		{
			cellvalue  = model.GetValue (iter, (int)LesseesCol.name).ToString();
			filterName = cellvalue.IndexOf (entryFilterName.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryFilterFIO.Text != "" && model.GetValue (iter, (int)LesseesCol.boss) != null)
		{
			cellvalue  = model.GetValue (iter, (int)LesseesCol.boss).ToString();
			filterFIO = cellvalue.IndexOf (entryFilterFIO.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		if (entryFilterINN.Text != "" && model.GetValue (iter, (int)LesseesCol.inn) != null)
		{
			cellvalue  = model.GetValue (iter, (int)LesseesCol.inn).ToString();
			filterINN = cellvalue.IndexOf (entryFilterINN.Text, StringComparison.CurrentCultureIgnoreCase) > -1;
		}
		return (filterName && filterINN && filterFIO);
	}
	
	protected virtual void OnButton233Clicked (object sender, System.EventArgs e)
	{
		entryFilterName.Text = "";
	}
	
	protected virtual void OnButton234Clicked (object sender, System.EventArgs e)
	{
		entryFilterFIO.Text = "";
	}
	
	protected virtual void OnButton235Clicked (object sender, System.EventArgs e)
	{
		entryFilterINN.Text = "";
	}
	
	protected virtual void OnEntryFilterNameChanged (object sender, System.EventArgs e)
	{
		Lesseesfilter.Refilter();
	}
	
	protected virtual void OnEntryFilterFIOChanged (object sender, System.EventArgs e)
	{
		Lesseesfilter.Refilter();
	}
	
	protected virtual void OnEntryFilterINNChanged (object sender, System.EventArgs e)
	{
		Lesseesfilter.Refilter();
	}
	
	protected virtual void OnTreeviewLesseesCursorChanged (object sender, System.EventArgs e)
	{
		bool isSelect = treeviewLessees.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	protected virtual void OnTreeviewLesseesRowActivated (object o, Gtk.RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}
}