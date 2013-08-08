using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class Place : Gtk.Dialog
	{
		public bool NewPlace;
		string PlaceNumber;
		int lessee_id, contact_id, type_id, ContractId;
		bool contactNull = true;

		Gtk.ListStore HistoryStore;
		
		AccelGroup grup;
		
		public Place ()
		{
			this.Build ();
			ComboWorks.ComboFillReference(comboPType,"place_types",2);
			ComboWorks.ComboFillReference(comboOrg, "organizations", 2);
			contactNull = true;

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

	}
}

