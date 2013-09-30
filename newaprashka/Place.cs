using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class Place : Gtk.Dialog
	{
		private bool NewPlace;
		string PlaceNumber;
		int lessee_id, contact_id, type_id, ContractId;
		bool contactNull = true;
		MeterTable[] Meters;

		Gtk.ListStore HistoryStore;
		
		AccelGroup grup;
		
		public Place (bool New)
		{
			this.Build ();
			ComboWorks.ComboFillReference(comboPType,"place_types",2);
			ComboWorks.ComboFillReference(comboOrg, "organizations", 2);
			contactNull = true;
			NewPlace = New;

			grup = new AccelGroup ();
			this.AddAccelGroup(grup);
			
			//Создаем таблицу "История"
			HistoryStore = new Gtk.ListStore (typeof(int), typeof (string), typeof (string), typeof (string), typeof (string),
			                                  typeof (int), typeof (string), typeof (string));
	 
			Gtk.TreeViewColumn CommentsColumn = new Gtk.TreeViewColumn ();
			CommentsColumn.Title = "Комментарии";
			Gtk.CellRendererText CommentsCell = new Gtk.CellRendererText ();
			CommentsCell.WrapMode = Pango.WrapMode.WordChar;
			CommentsCell.WrapWidth = 500;
			CommentsColumn.MaxWidth = 500;
			CommentsColumn.PackStart (CommentsCell, true);
			
			treeviewHistory.AppendColumn ("Договор", new Gtk.CellRendererText (), "text", 1);
			treeviewHistory.AppendColumn ("с", new Gtk.CellRendererText (), "text", 2);
			treeviewHistory.AppendColumn ("по", new Gtk.CellRendererText (), "text", 3);
			treeviewHistory.AppendColumn ("Расторгнут", new Gtk.CellRendererText (), "text", 4);
			treeviewHistory.AppendColumn ("Арендатор", new Gtk.CellRendererText (), "text", 6);
			treeviewHistory.AppendColumn(CommentsColumn);
			CommentsColumn.AddAttribute(CommentsCell, "text" , 7);
			
			treeviewHistory.Model = HistoryStore;
			treeviewHistory.ShowAll();
			if(New)
				notebookMain.GetNthPage (1).Hide (); //FIXME отключил вкладку что бы пользователь не мог создавать счетчики у не записаного места, надо исправить.
		}

		public void PlaceFill(int type, string place)
		{
			type_id = type;
			PlaceNumber = place;
			NewPlace = false;
			buttonOk.Sensitive = true;
			comboPType.Sensitive = false;
			entryNumber.Sensitive = false;
			buttonNewContract.Sensitive = true;
			TreeIter iter;
			
			MainClass.StatusMessage("Запрос сдаваемого места...");
			string sql = "SELECT places.*, place_types.name as type, contact_persons.name as contact, " +
				"contact_persons.telephones as telephones, contact_persons.comments as cp_comments," +
			 	"organizations.name as organization FROM places " +
				"LEFT JOIN place_types ON places.type_id = place_types.id " +
				"LEFT JOIN contact_persons ON places.contact_person_id = contact_persons.id " +
				"LEFT JOIN organizations ON places.org_id = organizations.id " +
				"WHERE places.type_id = @type_id AND places.place_no = @place";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@type_id", type_id);
			cmd.Parameters.AddWithValue("@place", PlaceNumber);
			MySqlDataReader rdr = cmd.ExecuteReader();

			rdr.Read ();

			ListStoreWorks.SearchListStore((ListStore)comboPType.Model, int.Parse(rdr["type_id"].ToString()), out iter);
			comboPType.SetActiveIter(iter);
			if(rdr["org_id"] != DBNull.Value)
				ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
			else
				ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
			comboOrg.SetActiveIter(iter);
			entryNumber.Text = rdr["place_no"].ToString();
			if(rdr["area"] != DBNull.Value)
				spinArea.Value = Convert.ToDouble(rdr["area"].ToString());
			if(rdr["contact_person_id"] != DBNull.Value)
			{
				contact_id = Convert.ToInt32(rdr["contact_person_id"].ToString());
				entryContact.Text = rdr["contact"].ToString();
				entryContact.TooltipText = rdr["contact"].ToString() + "\n" + rdr["telephones"].ToString() + "\n" + rdr["cp_comments"].ToString();
				contactNull = false;
				buttonContactOpen.Sensitive = true;
			}
			textviewComments.Buffer.Text = rdr["comments"].ToString();
			rdr.Close ();
			FillCurrentContract ();

			MainClass.StatusMessage("Ok");
			this.Title = "Место " + comboPType.ActiveText + " - " + place;
			TestCanSave();
			UpdateHistory();
		}

		void FillCurrentContract()
		{
			string sql = "SELECT lessees.name as lessee, lessees.comments as l_comments, " +
			 	"contracts.id as contract_id, contracts.lessee_id as contr_lessee_id, contracts.number as contr_number, " +
			 	"contracts.start_date as start_date, contracts.end_date as end_date, " +
			 	"contracts.cancel_date as cancel_date FROM contracts " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"WHERE contracts.place_type_id = @type AND contracts.place_no = @place AND " +
				"((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
				"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date))";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@type", type_id);
			cmd.Parameters.AddWithValue("@place", PlaceNumber);
			MySqlDataReader rdr = cmd.ExecuteReader();
					
			if(rdr.Read())
			{
				if( rdr["contract_id"] != DBNull.Value)
				{
					ContractId = rdr.GetInt32 ("contract_id");
					labelContractNumber.Text = rdr["contr_number"].ToString();
					if(rdr["cancel_date"] == DBNull.Value)
						labelContractDates.Text = DateTime.Parse (rdr["start_date"].ToString ()).ToShortDateString() +
							" - " + DateTime.Parse (rdr["end_date"].ToString ()).ToShortDateString();
					else
						labelContractDates.Text = DateTime.Parse (rdr["start_date"].ToString ()).ToShortDateString() +
							" - " + DateTime.Parse (rdr["cancel_date"].ToString ()).ToShortDateString() + "(досрочно)";
					buttonContract.Sensitive = true;
				}
				if( rdr["contr_lessee_id"] != DBNull.Value)
				{
					lessee_id = rdr.GetInt32("contr_lessee_id");
					labelLessee.Text = rdr["lessee"].ToString();
					labelLessee.TooltipText = rdr["lessee"].ToString() + "\n" + rdr["l_comments"].ToString();
					buttonLessee.Sensitive = true;
				}
			}
			else
			{
				ContractId = -1;
				labelContractNumber.Text = "Нет активного договора";
				labelContractDates.Text = String.Empty;
				buttonContract.Sensitive = false;
				lessee_id = 0;
				labelLessee.LabelProp = "<span background=\"green\">Свободно</span>";
				labelLessee.TooltipText = String.Empty;
				buttonLessee.Sensitive = false;
			}
			rdr.Close ();
		}
		
		protected virtual void OnEntryNumberChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		protected virtual void OnComboPTypeChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		protected	void TestCanSave ()
		{
			bool Numok = entryNumber.Text != "";
			bool typeok = comboPType.Active > 0;
			buttonOk.Sensitive = Numok && typeok;
		}
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string sql;
			TreeIter iter;
			MainClass.StatusMessage("Запись места...");
			if(NewPlace)
			{
				sql = "INSERT INTO places (type_id, place_no, area, contact_person_id, org_id, comments) " +
					"VALUES (@type_id, @place_no, @area, @contact, @org, @comments)";
			}
			else
			{
				sql = "UPDATE places SET area = @area, contact_person_id = @contact, " +
					"org_id = @org, comments = @comments " +
					"WHERE type_id = @type_id and place_no = @place_no";
			}
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				if(comboPType.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@type_id",comboPType.Model.GetValue(iter,1));
				}	
				cmd.Parameters.AddWithValue("@place_no", entryNumber.Text);
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
					cmd.Parameters.AddWithValue("@org",comboOrg.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@org", DBNull.Value);
				if(spinArea.Value == 0)
					cmd.Parameters.AddWithValue("@area", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@area", spinArea.Value);
				if(textviewComments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@comments", textviewComments.Buffer.Text);
				if(contactNull)
					cmd.Parameters.AddWithValue("@contact",DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@contact",contact_id);
				
				cmd.ExecuteNonQuery();
				
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
				
			} 
			catch (MySqlException ex) 
			{
				Console.WriteLine(ex.ToString());
				if(ex.Number == 1062)
				{
					MessageDialog md = new MessageDialog( this, DialogFlags.Modal,
                          MessageType.Error, 
                          ButtonsType.Close,"ошибка");
					md.UseMarkup = false;
					md.Text = "Место " + comboPType.ActiveText + " - " + entryNumber.Text + " уже существует в базе данных!";
					md.Run ();
					md.Destroy();
				}
				else
				{
					MainClass.StatusMessage("Ошибка записи места!");
					QSMain.ErrorMessage(this,ex);
				}
			}
		}
		
		protected virtual void OnButtonContactClicked (object sender, System.EventArgs e)
		{
			reference ContactSelect = new reference();
			ContactSelect.SetMode(false,true,true,true,false);
			ContactSelect.FillList("contact_persons","Контактное лицо", "Контактные лица");
			ContactSelect.Show();
			int result = ContactSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				contact_id = ContactSelect.SelectedID;
				contactNull = false;
				buttonContactOpen.Sensitive = true;
				entryContact.Text = ContactSelect.SelectedName;
				entryContact.TooltipText = ContactSelect.SelectedName;
			}
			ContactSelect.Destroy();
		}
		
		protected virtual void OnButtonContactCleanClicked (object sender, System.EventArgs e)
		{
			contactNull = true;
			buttonContactOpen.Sensitive = false;
			entryContact.Text = "";
			entryContact.TooltipText = "";
		}

		void UpdateHistory()
		{
	        MainClass.StatusMessage("Получаем историю места...");
			TreeIter iter;
			
			string sql = "SELECT contracts.*, lessees.name as lessee FROM contracts " +
			 	"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
			 	"WHERE place_type_id = @place_type AND place_no = @place_no";
	        MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			
			if(comboPType.GetActiveIter(out iter))
			{
				cmd.Parameters.AddWithValue("@place_type", comboPType.Model.GetValue(iter,1));
			}
			cmd.Parameters.AddWithValue("@place_no", entryNumber.Text);
			
			MySqlDataReader rdr = cmd.ExecuteReader();
				
			HistoryStore.Clear();
			string Cancel_date;
			while (rdr.Read())
			{
				if(rdr.GetInt32("id") == ContractId)
					continue;
				if(rdr["cancel_date"] != DBNull.Value)
					Cancel_date = ((DateTime)rdr["cancel_date"]).ToShortDateString();
				else
					Cancel_date = "";
				HistoryStore.AppendValues(rdr.GetInt32 ("id"),
											rdr["number"].ToString(),
											((DateTime)rdr["start_date"]).ToShortDateString(),
				                             ((DateTime)rdr["end_date"]).ToShortDateString(),
				                          	 Cancel_date,
											 rdr.GetInt32("lessee_id"),
				                             rdr["lessee"].ToString(),
				                             rdr["comments"].ToString());
	   		}
			rdr.Close();
			MainClass.StatusMessage("Ok");
		}
		
		[GLib.ConnectBefore]
		protected void OnTreeviewHistoryButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if((int)args.Event.Button == 3)
		    {       
				OnTreeviewHistoryPopupMenu(o, null);
		    }
		}
		
		[GLib.ConnectBefore]
		protected void OnTreeviewHistoryPopupMenu (object o, Gtk.PopupMenuArgs args)
		{
			bool ItemSelected = treeviewHistory.Selection.CountSelectedRows() == 1;
			Gtk.Menu popupBox = new Gtk.Menu();
			Gtk.MenuItem MenuItemOpenContract = new MenuItem("Открыть договор");
			MenuItemOpenContract.Activated += new EventHandler(OnHistoryOpenContract);
			MenuItemOpenContract.Sensitive = ItemSelected;
			popupBox.Add(MenuItemOpenContract);           
			Gtk.MenuItem MenuItemOpenLessee = new MenuItem("Открыть арендатора");
			MenuItemOpenLessee.Activated += new EventHandler(OnHistoryOpenLessee);
			MenuItemOpenLessee.Sensitive = ItemSelected;
			popupBox.Add(MenuItemOpenLessee);         
	        popupBox.ShowAll();
	        popupBox.Popup();
		}
				
		protected virtual void OnHistoryOpenContract (object o, EventArgs args)
		{
			int itemid;
			TreeIter iter;
			
			treeviewHistory.Selection.GetSelected(out iter);
			itemid = (int) HistoryStore.GetValue(iter,0);
			Contract winContract = new Contract();
			winContract.ContractFill(itemid);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			UpdateHistory ();
		}

		protected virtual void OnHistoryOpenLessee (object o, EventArgs args)
		{
			int itemid;
			TreeIter iter;
			
			treeviewHistory.Selection.GetSelected(out iter);
			itemid = Convert.ToInt32(HistoryStore.GetValue(iter,5));
			lessee winLessee = new lessee();
			winLessee.LesseeFill(itemid);
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
			UpdateHistory ();
		}

		protected void OnButtonContactOpenClicked (object sender, System.EventArgs e)
		{
			Contact ContactWin = new Contact();
			ContactWin.ContactFill(contact_id);
			ContactWin.Show();
			ContactWin.Run();
			ContactWin.Destroy();
		}
		protected void OnButtonLesseeClicked (object sender, EventArgs e)
		{
			lessee winLessee = new lessee();
			winLessee.LesseeFill(lessee_id);
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonContractClicked (object sender, EventArgs e)
		{
			Contract winContract = new Contract();
			winContract.ContractFill(ContractId);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonNewContractClicked (object sender, EventArgs e)
		{
			Contract winContract = new Contract();
			winContract.NewContract = true;
			winContract.Show();
			winContract.SetPlace (type_id, PlaceNumber);
			winContract.Run();
			winContract.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonAddMeterClicked(object sender, EventArgs e)
		{
			Meter WinMeter = new Meter (true);
			WinMeter.SetPlace (type_id, PlaceNumber);
			WinMeter.Show ();
			ResponseType result = (ResponseType) WinMeter.Run ();
			WinMeter.Destroy ();
			if(result == ResponseType.Ok)
				UpdateMeters ();
		}

		private void UpdateMeters()
		{
			MainClass.StatusMessage("Получаем счетчики места...");
			try
			{
				string sql = "SELECT meters.id, meters.name, meter_types.name as type FROM meters " +
					"LEFT JOIN meter_types ON meter_types.id = meters.meter_type_id " +
					"WHERE place_type_id = @place_type AND place_no = @place_no";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@place_type", ComboWorks.GetActiveId (comboPType));
				cmd.Parameters.AddWithValue("@place_no", entryNumber.Text);

				List<object[]> DBRead = new List<object[]>();
				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						object[] values = new object[rdr.FieldCount];
						rdr.GetValues (values);
						DBRead.Add (values);
					}
				}
				if(DBRead.Count == 0)
					return;
				if(Meters == null)
				{
					int i = 0;
					Meters = new MeterTable[DBRead.Count];
					foreach(object[] row in DBRead)
					{
						Meters[i] = AddMeterPage(Convert.ToInt32 (row[0]), (string)row[2]);
						if(i == 0) // Удаляем вкладку по умочанию, только после добавления новой, иначе проблемы с отображением.
						{
							notebookMeters.RemovePage (0); //FIXME Подумать о обработки ситуации когда все счетчики удалены.
							buttonEditMeter.Sensitive = true;
							buttonDeleteMeter.Sensitive = true;
							buttonAddReading.Sensitive = true;
						}
						i++;
					}
				}
				else
				{
					int offset = 0, i = 0;
					MeterTable[] NewMeters = new MeterTable[DBRead.Count];
					foreach(MeterTable meter in Meters)
					{
						bool Found = false;
						foreach(object[] row in DBRead)
						{
							if(Convert.ToInt32 (row[0]) == meter.ID && row[2].ToString () == meter.Name)
								Found = true;
						}
						if(!Found)
						{
							notebookMeters.RemovePage (i);
							offset++;
						}
						else
						{
							NewMeters[i] = meter;
							i++;
						}
					}

					foreach(object[] row in DBRead)
					{
						bool Found = false;
						foreach(MeterTable meter in NewMeters)
						{
							if(meter != null && meter.ID == Convert.ToInt32 (row[0]))
							{
								Found = true;
								break;
							}
						}
						if(Found)
							continue;
						NewMeters[i] = AddMeterPage(Convert.ToInt32 (row[0]), (string)row[2]);
						i++;
					}
					Meters = NewMeters;
				}
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о счётчиках!");
				QSMain.ErrorMessage(this, ex);
			}
		}
		
		private void UpdateReadings()
		{
			MainClass.StatusMessage("Получаем показания счетчика...");
			try
			{
				string sql = "SELECT meter_reading.id, meter_reading.date, meter_reading.value, meter_tariffs.name as tariff, units.name as unit FROM meter_reading " +
					"LEFT JOIN meter_tariffs ON meter_tariffs.id = meter_reading.meter_tariff_id " +
					"LEFT JOIN services ON services.id = meter_tariffs.service_id " +
					"LEFT JOIN units ON units.id = services.units_id " +
					"WHERE meter_reading.meter_id = @id " +
					"ORDER BY meter_reading.date DESC, tariff";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MeterTable meter = Meters[notebookMeters.CurrentPage];

				cmd.Parameters.AddWithValue("@id", meter.ID);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					meter.liststore.Clear();
					while (rdr.Read())
					{
						string ValueFormat = rdr["unit"] != DBNull.Value ? "{0} {1}": "{0}";
						meter.liststore.AppendValues (rdr.GetInt32 ("id"),
						                              String.Format ("{0:d}", rdr.GetDateTime ("date")),
						                              rdr["tariff"].ToString (),
						                              String.Format (ValueFormat, rdr.GetInt32 ("value"), rdr["unit"].ToString ())
						                              );
					}
				}
				meter.Filled = true;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения показаний счётчика!");
				QSMain.ErrorMessage(this, ex);
			}
		}		

		private MeterTable AddMeterPage(int id, string name)
		{
			MeterTable meter = new MeterTable ();
			meter.ID = id;
			meter.Name = name;
			meter.Filled = false;
			meter.liststore = new ListStore (typeof(int), // 0 - ID
			                             typeof(string), //1 - date
			                             typeof(string), //2 - tariff
			                             typeof(string) //3 - value
			                             );
			meter.treeview = new TreeView (meter.liststore);

			meter.treeview.AppendColumn ("Дата", new CellRendererText (), "text", 1);
			meter.treeview.AppendColumn ("Тариф", new CellRendererText (), "text", 2);
			meter.treeview.AppendColumn ("Показания", new CellRendererText (), "text", 3);

			ScrolledWindow Scroll = new ScrolledWindow ();
			notebookMeters.AppendPage (Scroll, new Label (name));

			Scroll.Add (meter.treeview);
			meter.treeview.CursorChanged += OnTreeviewReadingCursorChanged;
			meter.treeview.Show ();
			notebookMeters.ShowAll ();

			return meter;
		}

		protected void OnNotebookMainSwitchPage(object o, SwitchPageArgs args)
		{
			if (notebookMain.Page == 1 && Meters == null)
				UpdateMeters ();
		}

		protected void OnButtonEditMeterClicked(object sender, EventArgs e)
		{
			int itemid = Meters[notebookMeters.CurrentPage].ID;
			Meter winMeter = new Meter(false);
			winMeter.Fill(itemid);
			winMeter.Show();
			ResponseType result = (ResponseType)winMeter.Run();
			winMeter.Destroy();
			if(result == ResponseType.Ok)
				UpdateMeters();
	}

		protected void OnButtonDeleteMeterClicked(object sender, EventArgs e)
		{
			int itemid = Meters[notebookMeters.CurrentPage].ID;
			Delete winDelete = new Delete();
			if(winDelete.RunDeletion("meters", itemid))
				UpdateMeters();
		}

		protected void OnNotebookMetersSwitchPage(object o, SwitchPageArgs args)
		{
			if (notebookMeters.CurrentPage < 0)
				return;
			if (!Meters [notebookMeters.CurrentPage].Filled)
				UpdateReadings ();
			OnTreeviewReadingCursorChanged (null, null);
		}

		protected void OnButtonAddReadingClicked(object sender, EventArgs e)
		{
			int itemid = Meters[notebookMeters.CurrentPage].ID;
			MeterReading winMeterReading = new MeterReading(itemid);
			winMeterReading.Show();
			ResponseType result = (ResponseType)winMeterReading.Run();
			winMeterReading.Destroy();
			if(result == ResponseType.Ok)
				UpdateReadings();
		}

		protected void OnTreeviewReadingCursorChanged(object sender, EventArgs e)
		{
			bool isSelect = Meters[notebookMeters.CurrentPage].treeview.Selection.CountSelectedRows() == 1;
			buttonDeleteReading.Sensitive = isSelect;
		}

		protected void OnButtonDeleteReadingClicked(object sender, EventArgs e)
		{
			TreeIter iter;
			Meters[notebookMeters.CurrentPage].treeview.Selection.GetSelected (out iter);
			int itemid = (int) Meters [notebookMeters.CurrentPage].treeview.Model.GetValue (iter, 0);
			Delete winDelete = new Delete();
			if(winDelete.RunDeletion("meter_reading", itemid))
				UpdateReadings();
		}

		class MeterTable
		{
			public int ID;
			public string Name;
			public TreeView treeview;
			public ListStore liststore;
			public bool Filled;

			public MeterTable()
			{}
		}

	}
}

