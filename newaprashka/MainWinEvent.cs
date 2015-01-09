using System;
using Gtk;
using MySql.Data.MySqlClient;
using bazar;
using QSProjectsLib;

public partial class MainWindow : Gtk.Window
{
	Gtk.ListStore EventsListStore;
	int EventLessee_id;

	void PrepareEvent()
	{
		MainClass.ComboFillUsers(comboEventUser,"events");
		ComboWorks.ComboFillReference(comboEventType, "classes", ComboWorks.ListMode.WithAll);
		ComboWorks.ComboFillReference(comboEventPlaceT,"place_types", ComboWorks.ListMode.WithAll);
		
		//Создаем таблицу "События"
		EventsListStore = new Gtk.ListStore (typeof (int),typeof (string), typeof (int), typeof (string), typeof (string),
		                                     typeof (int), typeof (string), typeof (string), typeof (string), typeof (int), typeof (string), typeof (string));
		
		//Настройка колонки причины
		Gtk.TreeViewColumn CauseColumn = new Gtk.TreeViewColumn ();
		CauseColumn.Title = "Причина";
		Gtk.CellRendererText CauseCell = new Gtk.CellRendererText ();
		CauseCell.WrapMode = Pango.WrapMode.WordChar;
		CauseCell.WrapWidth = 500;
		CauseColumn.MaxWidth = 500;
		CauseColumn.PackStart (CauseCell, true);
		//Настройка колонки меры
		Gtk.TreeViewColumn ActivityColumn = new Gtk.TreeViewColumn ();
		ActivityColumn.Title = "Меры";
		Gtk.CellRendererText ActivityCell = new Gtk.CellRendererText ();
		ActivityCell.WrapMode = Pango.WrapMode.WordChar;
		ActivityCell.WrapWidth = 500;
		ActivityColumn.MaxWidth = 500;
		ActivityColumn.PackStart (ActivityCell, true);
		
		treeviewEvents.AppendColumn("Дата", new Gtk.CellRendererText (), "text", 1);
		treeviewEvents.AppendColumn("Место", new Gtk.CellRendererText (), "text", 4);
		treeviewEvents.AppendColumn("Событие", new Gtk.CellRendererText (), "text", 6);
		treeviewEvents.AppendColumn(CauseColumn);
		CauseColumn.AddAttribute(CauseCell, "text" ,7);
		treeviewEvents.AppendColumn(ActivityColumn);
		ActivityColumn.AddAttribute(ActivityCell, "text" ,8);
		treeviewEvents.AppendColumn("Арендатор", new Gtk.CellRendererText (), "text", 10);
		treeviewEvents.AppendColumn("Пользователь", new Gtk.CellRendererText (), "text", 11);
		
		treeviewEvents.Model = EventsListStore;
		treeviewEvents.ShowAll();
		
		radiobuttonToday.Click();

	}

	void UpdateEvents()
	{
		TreeIter iter;
		int class_id, place_type_id;
		class_id = 0;
		string user = "";
		place_type_id = 0;
		
		logger.Info("Получаем таблицу событий...");
		string sql = "SELECT events.*, place_types.name as type, lessees.name as lessee, classes.name as class FROM events"+
			" LEFT JOIN place_types ON events.place_type_id = place_types.id " +
				"LEFT JOIN lessees ON events.lessee_id = lessees.id " +
				"LEFT JOIN classes ON events.class_id = classes.id";
		sql += " WHERE events.date BETWEEN @dateBegin AND @dateEnd";
		
		if(comboEventType.Active > 0)
		{
			sql += " AND events.class_id = @class_id";
			comboEventType.GetActiveIter(out iter);
			class_id = Convert.ToInt32(comboEventType.Model.GetValue(iter, 1));
		}
		if(comboEventUser.Active > 0)
		{
			sql += " AND events.user = @user";
			user = comboEventUser.ActiveText;
			
		}
		if(comboEventPlaceT.Active > 0)
		{
			sql += " AND events.place_type_id = @place_type_id";
			comboEventPlaceT.GetActiveIter(out iter);
			place_type_id = Convert.ToInt32(comboEventPlaceT.Model.GetValue(iter, 1));
		}
		if(entryEventLessee.Text != "")
		{
			sql += " AND events.lessee_id = @lessee_id";
		}
		if(comboboxEventPlaceNo.Active >= 0)
		{
			sql += " AND events.place_no = @place_no";
		}
		if(entryEventCause.Text != "")
		{
			sql += " AND MATCH (cause) AGAINST (@cause) > 0";
		}
		if(entryEventActivity.Text != "")
		{
			sql += " AND MATCH (activity) AGAINST (@activity) > 0";
		}

		MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
		
		cmd.Parameters.AddWithValue("@dateBegin", datepickerEventBegin.Date );
		cmd.Parameters.AddWithValue("@dateEnd", datepickerEventEnd.Date.AddDays(1));
		cmd.Parameters.AddWithValue("@class_id", class_id);
		cmd.Parameters.AddWithValue("@user", user);
		cmd.Parameters.AddWithValue("@lessee_id", EventLessee_id);
		cmd.Parameters.AddWithValue("@place_type_id", place_type_id);
		cmd.Parameters.AddWithValue("@place_no", comboboxEventPlaceNo.ActiveText);
		cmd.Parameters.AddWithValue("@cause", entryEventCause.Text);
		cmd.Parameters.AddWithValue("@activity", entryEventActivity.Text);
		
		MySqlDataReader rdr = cmd.ExecuteReader();
		int Parse_place_id, lessee_id, Parse_class_id;
		bool parsed;
		string place_str;
		DateTime Parse_Date;
		
		EventsListStore.Clear();
		while (rdr.Read())
		{
			parsed = int.TryParse (rdr["place_type_id"].ToString(), out Parse_place_id);
			if(parsed)
				place_str = rdr["type"].ToString() + " - " + rdr["place_no"].ToString();
			else
				place_str = "";
			parsed = int.TryParse (rdr["lessee_id"].ToString(), out lessee_id);
			Parse_Date = (DateTime)rdr["date"];
			parsed = int.TryParse (rdr["class_id"].ToString(), out Parse_class_id);			
			EventsListStore.AppendValues(int.Parse(rdr["id"].ToString()),
			                             Parse_Date.ToString(),
			                             Parse_place_id,
			                             rdr["place_no"].ToString(),
			                             place_str,
			                             Parse_class_id,
			                             rdr["class"].ToString(),
			                             rdr["cause"].ToString(),
			                             rdr["activity"].ToString(),
			                             lessee_id,
			                             rdr["lessee"].ToString(),
			                             rdr["user"].ToString());
			
		}
		rdr.Close();
		
		logger.Info("Ok");
		
		bool isSelect = treeviewEvents.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}

	protected virtual void OnRadiobuttonTodayClicked (object sender, System.EventArgs e)
	{
		datepickerEventBegin.Date = DateTime.Now.Date;
		datepickerEventEnd.Date = DateTime.Now.Date;
	}
	
	protected virtual void OnRadiobuttonWeekClicked (object sender, System.EventArgs e)
	{
		datepickerEventBegin.Date = DateTime.Now.AddDays(-7);
		datepickerEventEnd.Date = DateTime.Now.Date;
	}
	
	protected virtual void OnRadiobuttonMonthClicked (object sender, System.EventArgs e)
	{
		datepickerEventBegin.Date = DateTime.Now.AddDays(-30);
		datepickerEventEnd.Date = DateTime.Now.Date;
	}
	
	protected virtual void OnRadiobutton6MonthClicked (object sender, System.EventArgs e)
	{
		datepickerEventBegin.Date = DateTime.Now.AddDays(-183);
		datepickerEventEnd.Date = DateTime.Now.Date;
	}
	
	protected virtual void OnButtonEventRefreshClicked (object sender, System.EventArgs e)
	{
		UpdateEvents();
	}
	
	protected virtual void OnTreeviewEventsCursorChanged (object sender, System.EventArgs e)
	{
		bool isSelect = treeviewEvents.Selection.CountSelectedRows() == 1;
		buttonOpen.Sensitive = isSelect;
		buttonDel.Sensitive = isSelect;
	}
	
	protected virtual void OnTreeviewEventsRowActivated (object o, Gtk.RowActivatedArgs args)
	{
		OnButtonViewClicked(o,EventArgs.Empty);
	}
	
	protected virtual void OnComboEventPlaceTChanged (object sender, System.EventArgs e)
	{
		TreeIter iter;
		int id;
		if(comboEventPlaceT.GetActiveIter(out iter) && comboEventPlaceT.Active > 0)
		{
			id = (int)comboEventPlaceT.Model.GetValue(iter,1);
			MainClass.ComboPlaceNoFill(comboboxEventPlaceNo,id);
		}
	}
	
	protected virtual void OnButtonEventLesseeCleanClicked (object sender, System.EventArgs e)
	{
		EventLessee_id = -1;
		entryEventLessee.Text = "";
	}
	
	protected virtual void OnButtonEventLesseeEditClicked (object sender, System.EventArgs e)
	{
		Reference LesseeSelect = new Reference();
		LesseeSelect.SetMode(false,true,false,true,false);
		LesseeSelect.FillList("lessees","Арендатор", "Арендаторы");
		LesseeSelect.Show();
		int result = LesseeSelect.Run();
		if((ResponseType)result == ResponseType.Ok)
		{
			EventLessee_id = LesseeSelect.SelectedID;
			entryEventLessee.Text = LesseeSelect.SelectedName;
			entryEventLessee.TooltipText = LesseeSelect.SelectedName;
		}
		LesseeSelect.Destroy();
	}

	protected virtual void OnEntryEventCauseActivated (object sender, System.EventArgs e)
	{
		UpdateEvents();
	}
	
	protected virtual void OnEntryEventActivityActivated (object sender, System.EventArgs e)
	{
		UpdateEvents();
	}

}