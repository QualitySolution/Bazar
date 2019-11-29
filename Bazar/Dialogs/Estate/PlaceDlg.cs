using System;
using System.Collections.Generic;
using bazar;
using Bazar.Dialogs.Rental;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace Bazar.Dialogs.Estate
{
	public partial class PlaceDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private bool NewPlace;
		string PlaceNumber;
		int place_id, lessee_id, contact_id, type_id, ContractId;
		bool contactNull = true;
		MeterTable[] Meters;

		Gtk.ListStore HistoryStore;
		
		AccelGroup grup;
		
		public PlaceDlg (bool New)
		{
			this.Build ();
			ComboWorks.ComboFillReference(comboPType,"place_types", ComboWorks.ListMode.WithNo, OrderBy: "name");
			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithNo, OrderBy: "name");
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

		public void PlaceFill(int id)
		{
			this.place_id = id;
			NewPlace = false;
			buttonOk.Sensitive = true;
			comboPType.Sensitive = false;
			entryNumber.Sensitive = false;
			buttonNewContract.Sensitive = true;
			TreeIter iter;
			
			logger.Info("Запрос сдаваемого места...");
			string sql = "SELECT places.*, place_types.name as type, contact_persons.name as contact, " +
				"contact_persons.telephones as telephones, contact_persons.comments as cp_comments," +
			 	"organizations.name as organization FROM places " +
				"LEFT JOIN place_types ON places.type_id = place_types.id " +
				"LEFT JOIN contact_persons ON places.contact_person_id = contact_persons.id " +
				"LEFT JOIN organizations ON places.org_id = organizations.id " +
				"WHERE places.id = @id";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", place_id);
			MySqlDataReader rdr = cmd.ExecuteReader();

			var res = rdr.Read ();

			type_id = DBWorks.GetInt (rdr, "type_id").Value;
			ListStoreWorks.SearchListStore((ListStore)comboPType.Model, int.Parse(rdr["type_id"].ToString()), out iter);
			comboPType.SetActiveIter(iter);
			if(rdr["org_id"] != DBNull.Value)
				ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
			else
				ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
			comboOrg.SetActiveIter(iter);
			PlaceNumber = entryNumber.Text = rdr["place_no"].ToString();
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

			logger.Info("Ok");
			this.Title = "Место " + comboPType.ActiveText + " - " + PlaceNumber;
			TestCanSave();
			UpdateHistory();
		}

		void FillCurrentContract()
		{
			string sql = "SELECT lessees.name as lessee, lessees.comments as l_comments, " +
			 	"contracts.id as contract_id, contracts.lessee_id as contr_lessee_id, contracts.number as contr_number, " +
			 	"contracts.start_date as start_date, contracts.end_date as end_date, " +
			 	"contracts.cancel_date as cancel_date " +
			 	"FROM contract_pays " +
				"LEFT JOIN contracts ON contract_pays.contract_id = contracts.id " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"WHERE contract_pays.place_id = @place_id AND " +
				"((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
				"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date))";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@place_id", place_id);
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
			logger.Info("Запись места...");
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
				
				logger.Info("Ok");
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
					QSMain.ErrorMessageWithLog(this, "Ошибка записи места!", logger, ex);
				}
			}
		}
		
		protected virtual void OnButtonContactClicked (object sender, System.EventArgs e)
		{
			Reference ContactSelect = new Reference(orderBy: "name");
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
	        logger.Info("Получаем историю места...");
			
			string sql = "SELECT contracts.*, lessees.name as lessee FROM contracts " +
			 	"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
			 	"LEFT JOIN contract_pays ON contract_pays.contract_id = contracts.id " +
			 	"WHERE contract_pays.place_id = @place_id " +
			 	"GROUP BY contracts.id";
	        MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@place_id", place_id);
			
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
			logger.Info("Ok");
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
			ContractDlg winContract = new ContractDlg();
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
			LesseeDlg winLessee = new LesseeDlg();
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
			LesseeDlg winLessee = new LesseeDlg();
			winLessee.LesseeFill(lessee_id);
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonContractClicked (object sender, EventArgs e)
		{
			ContractDlg winContract = new ContractDlg();
			winContract.ContractFill(ContractId);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonNewContractClicked (object sender, EventArgs e)
		{
			ContractDlg winContract = new ContractDlg();
			winContract.NewContract = true;
			winContract.Show();
			winContract.SetPlace (place_id);
			winContract.Run();
			winContract.Destroy();
			FillCurrentContract ();
		}

		protected void OnButtonAddMeterClicked(object sender, EventArgs e)
		{
			MeterDlg WinMeter = new MeterDlg (true);
			var title = String.Format("{0}-{1}", comboPType.ActiveText, entryNumber.Text);
			WinMeter.SetPlace (place_id, title);
			WinMeter.Show ();
			ResponseType result = (ResponseType) WinMeter.Run ();
			WinMeter.Destroy ();
			if(result == ResponseType.Ok)
				UpdateMeters ();
		}

		private void UpdateMeters()
		{
			logger.Info("Получаем счетчики места...");
			try
			{
				string sql = "SELECT meters.id, meters.name, meter_types.name as type, meters.disabled, meter_types.reading_ratio FROM meters " +
					"LEFT JOIN meter_types ON meter_types.id = meters.meter_type_id " +
					"WHERE place_id = @place_id " +
					"ORDER BY disabled";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@place_id", place_id);

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
						Meters[i] = AddMeterPage(Convert.ToInt32 (row[0]), (string)row[2], (bool)row[3], (double)row [4]);
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
							if(Convert.ToInt32 (row[0]) == meter.ID && row[2].ToString () == meter.Name && (bool)row[3] == meter.Disabled)
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
						NewMeters[i] = AddMeterPage(Convert.ToInt32 (row[0]), (string)row[2], (bool)row[3], (double)row [4]);
						i++;
					}
					Meters = NewMeters;
				}
				logger.Info("Ok");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о счётчиках!", logger, ex);
			}
		}
		
		private void UpdateReadings()
		{
			logger.Info("Получаем показания счетчика...");
			try
			{
				string sql = "SELECT meter_reading.id, meter_reading.meter_tariff_id, meter_reading.date, meter_reading.value, meter_tariffs.name as tariff, units.name as unit FROM meter_reading " +
					"LEFT JOIN meter_tariffs ON meter_tariffs.id = meter_reading.meter_tariff_id " +
					"LEFT JOIN services ON services.id = meter_tariffs.service_id " +
					"LEFT JOIN units ON units.id = services.units_id " +
					"WHERE meter_reading.meter_id = @id " +
					"ORDER BY meter_reading.date DESC, tariff, meter_reading.value DESC";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MeterTable meter = Meters[notebookMeters.CurrentPage];
				Dictionary<int, TreeIter> LaterReading = new Dictionary<int, TreeIter>();

				cmd.Parameters.AddWithValue("@id", meter.ID);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					meter.liststore.Clear();
					while (rdr.Read())
					{
						string ValueFormat = rdr["unit"] != DBNull.Value ? "{0} {1}": "{0}";
						TreeIter CurIter = meter.liststore.AppendValues (rdr.GetInt32 ("id"),
						                              String.Format ("{0:d}", rdr.GetDateTime ("date")),
						                              rdr["tariff"].ToString (),
						                              String.Format (ValueFormat, rdr.GetInt32 ("value"), rdr["unit"].ToString ()),
						                              String.Empty,
						                              rdr.GetInt32 ("value"),
													  String.Empty
													  );
						int tariff_id = rdr.GetInt32 ("meter_tariff_id");
						if(LaterReading.ContainsKey (tariff_id))
						{
							int delta = (int)meter.liststore.GetValue (LaterReading[tariff_id], 5) - rdr.GetInt32 ("value");
							meter.liststore.SetValue (LaterReading[tariff_id], 4, String.Format (ValueFormat, delta, rdr["unit"].ToString ()));
							if(!meter.RatioIsOne)
								meter.liststore.SetValue (LaterReading [tariff_id], 6, String.Format (ValueFormat, delta * meter.ReadingRatio, rdr ["unit"].ToString ()));
						}
						LaterReading[tariff_id] = CurIter;
					}
				}
				meter.Filled = true;
				logger.Info("Ok");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения показаний счётчика!", logger, ex);
			}
		}		

		private MeterTable AddMeterPage(int id, string name, bool disable, double ratio)
		{
			MeterTable meter = new MeterTable ();
			meter.ID = id;
			meter.Name = name;
			meter.Filled = false;
			meter.Disabled = disable;
			meter.ReadingRatio = ratio;
			meter.liststore = new ListStore (typeof(int), // 0 - ID
			                             typeof(string), //1 - date
			                             typeof(string), //2 - tariff
			                             typeof(string), //3 - value
			                             typeof(string),  //4 - delta
			                             typeof(int),     //5 - digital value
										 typeof (string) //6 - delta * ratio
											 );
			meter.treeview = new TreeView (meter.liststore);

			meter.treeview.AppendColumn ("Дата", new CellRendererText (), "text", 1);
			meter.treeview.AppendColumn ("Тариф", new CellRendererText (), "text", 2);
			meter.treeview.AppendColumn ("Показания", new CellRendererText (), "text", 3);
			meter.treeview.AppendColumn (meter.RatioIsOne ? "Расход" : "Расход по показаниям", new CellRendererText (), "text", 4);
			if (!meter.RatioIsOne)
				meter.treeview.AppendColumn ($"Расход с коэф. {meter.ReadingRatio}", new CellRendererText (), "text", 6);

			ScrolledWindow Scroll = new ScrolledWindow ();
			if (disable)
				name += "(отк.)";
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
			MeterDlg winMeter = new MeterDlg(false);
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
			buttonAddReading.Sensitive = !Meters [notebookMeters.CurrentPage].Disabled;
			OnTreeviewReadingCursorChanged (null, null);
		}

		protected void OnButtonAddReadingClicked(object sender, EventArgs e)
		{
			int itemid = Meters[notebookMeters.CurrentPage].ID;
			MeterReadingDlg winMeterReading = new MeterReadingDlg(itemid);
			winMeterReading.Show();
			ResponseType result = (ResponseType)winMeterReading.Run();
			winMeterReading.Destroy();
			if(result == ResponseType.Ok)
				UpdateReadings();
		}

		protected void OnTreeviewReadingCursorChanged(object sender, EventArgs e)
		{
			bool isSelect = Meters[notebookMeters.CurrentPage].treeview.Selection.CountSelectedRows() == 1;
			bool isEnabled = !Meters[notebookMeters.CurrentPage].Disabled;
			buttonDeleteReading.Sensitive = isSelect && isEnabled;
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
			public double ReadingRatio;
			public bool RatioIsOne => Math.Abs (ReadingRatio - 1) < 0.001;
			public bool Disabled;
			public bool Filled;
		}

	}
}

