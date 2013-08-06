using System;
using System.Data;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class lessee : Gtk.Dialog
	{
		public bool NewLessee;
		int Lesseeid, Goods_id;
		bool GoodsNull;
		
		Gtk.ListStore ContractsListStore;		
				
		AccelGroup grup;
		
		public lessee ()
		{
			this.Build ();
			
			GoodsNull = true;
			grup = new AccelGroup ();
			this.AddAccelGroup(grup);
									
			//Создаем таблицу "Договора"
			ContractsListStore = new Gtk.ListStore (typeof (bool), typeof (string), typeof (string), typeof (string),
			                                     typeof (int), typeof (string), typeof (string), typeof (string), 
			                                     typeof (int), typeof (string), typeof (string));
	 
			Gtk.TreeViewColumn PlaceTypeIdColumn = new Gtk.TreeViewColumn ();
			PlaceTypeIdColumn.Title = "idТип";
			PlaceTypeIdColumn.Visible = false;
			PlaceTypeIdColumn.PackStart (new Gtk.CellRendererText (), true);
			Gtk.TreeViewColumn PlaceNoColumn = new Gtk.TreeViewColumn ();
			PlaceNoColumn.Title = "Номер места";
			PlaceNoColumn.Visible = false;
			PlaceNoColumn.PackStart (new Gtk.CellRendererText (), true);

			Gtk.TreeViewColumn idContactsColumn = new Gtk.TreeViewColumn ();
			idContactsColumn.Title = "idКонтакт";
			idContactsColumn.Visible = false;
			idContactsColumn.PackStart (new Gtk.CellRendererText (), true);
			
			treeviewContracts.AppendColumn("Акт.", new Gtk.CellRendererToggle (), "active", 0);
			treeviewContracts.AppendColumn ("с", new Gtk.CellRendererText (), "text", 1);
			treeviewContracts.AppendColumn ("по", new Gtk.CellRendererText (), "text", 2);
			treeviewContracts.AppendColumn ("Договор", new Gtk.CellRendererText (), "text", 3);
			treeviewContracts.AppendColumn (PlaceTypeIdColumn);
			treeviewContracts.AppendColumn (PlaceNoColumn);
			treeviewContracts.AppendColumn ("Место", new Gtk.CellRendererText (), "text", 6);
			treeviewContracts.AppendColumn ("Площадь", new Gtk.CellRendererText (), "text", 7);
			treeviewContracts.AppendColumn (idContactsColumn);
			treeviewContracts.AppendColumn ("Контактное лицо", new Gtk.CellRendererText (), "text", 9);
			treeviewContracts.AppendColumn ("Расторгнут", new Gtk.CellRendererText (), "text", 10);
			
			treeviewContracts.Model = ContractsListStore;
			treeviewContracts.ShowAll();
			
		}
		
		public void LesseeFill(int id)
		{
			Lesseeid = id;
			NewLessee = false;
			
			MainClass.StatusMessage(String.Format("Запрос арендатора №{0}...", id));
			string sql = "SELECT lessees.*, goods.name as goods FROM lessees LEFT JOIN goods ON lessees.goods_id = goods.id WHERE lessees.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", id);
		
				MySqlDataReader rdr = cmd.ExecuteReader();
					
				rdr.Read();
				
				labelID.Text = rdr["id"].ToString();
				entryName.Text = rdr["name"].ToString();
				entryAddress.Text = rdr["address"].ToString();
				entryINN.Text = rdr["INN"].ToString();
				entryOGRN.Text = rdr["OGRN"].ToString();
				entryFIO.Text = rdr["FIO_dir"].ToString();
				entryPassSer.Text = rdr["passport_ser"].ToString();
				entryPassNo.Text = rdr["passport_no"].ToString();
				entryExit.Text = rdr["passport_exit"].ToString();
				checkBretail.Active = (bool)rdr["retail"];
				checkBwholesaler.Active = (bool)rdr["wholesaler"];
				if(rdr["goods_id"] != DBNull.Value)
				{
					Goods_id = Convert.ToInt32(rdr["goods_id"].ToString());
					entryGoods.Text = rdr["goods"].ToString();
					entryGoods.TooltipText = rdr["goods"].ToString();
					GoodsNull = false;
				}
				textviewComments.Buffer.Text = rdr["comments"].ToString();
				
				rdr.Close();
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о арендаторе!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
			UpdateContracts();
		}
		
		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			buttonOk.Sensitive = Nameok;
		}
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string sql;
			if(NewLessee)
			{
				sql = "INSERT INTO lessees (name, FIO_dir, passport_ser, passport_no, passport_exit, address, " +
					"INN, OGRN, wholesaler, retail, goods_id, comments) " +
					"VALUES (@name, @FIO, @passport_ser, @passport_no, @exit, @address, " +
					"@INN, @OGRN, @wholesaler, @retail, @goods_id, @comments)";
			}
			else
			{
				sql = "UPDATE lessees SET name = @name, FIO_dir = @FIO, passport_ser = @passport_ser, " +
					"passport_no = @passport_no, passport_exit = @exit, address = @address, INN = @INN, OGRN = @OGRN, " +
					"wholesaler = @wholesaler, retail = @retail, goods_id = @goods_id, comments = @comments " +
					"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись арендатора...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Lesseeid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(entryFIO.Text == "")
					cmd.Parameters.AddWithValue("@FIO", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@FIO", entryFIO.Text);
				if(entryPassSer.Text == "")
					cmd.Parameters.AddWithValue("@passport_ser", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@passport_ser", entryPassSer.Text);
				if(entryPassNo.Text == "")
					cmd.Parameters.AddWithValue("@passport_no", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@passport_no", entryPassNo.Text);
				if(entryExit.Text == "")
					cmd.Parameters.AddWithValue("@exit", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@exit", entryExit.Text);
				if(entryAddress.Text == "")
					cmd.Parameters.AddWithValue("@address", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@address", entryAddress.Text);
				if(entryINN.Text == "")
					cmd.Parameters.AddWithValue("@INN", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@INN", entryINN.Text);
				if(entryOGRN.Text == "")
					cmd.Parameters.AddWithValue("@OGRN", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@OGRN", entryOGRN.Text);
				cmd.Parameters.AddWithValue("@wholesaler", checkBwholesaler.Active);
				cmd.Parameters.AddWithValue("@retail", checkBretail.Active);				
				if(!GoodsNull)
					cmd.Parameters.AddWithValue("@goods_id", Goods_id);
				else
					cmd.Parameters.AddWithValue("@goods_id", DBNull.Value);
				if(textviewComments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@comments", textviewComments.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи арендатора!");
				QSMain.ErrorMessage(this,ex);
			}

		}

		protected virtual void OnEntryNameChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		void UpdateContracts()
		{
	        MainClass.StatusMessage("Получаем таблицу договоров...");
			
			string sql = "SELECT contracts.*, place_types.name as type, contact_persons.name as contact, " +
				"places.contact_person_id as contact_id, places.area as area FROM contracts " +
				"LEFT JOIN place_types ON contracts.place_type_id = place_types.id " +
				"LEFT JOIN places ON places.type_id = contracts.place_type_id AND places.place_no = contracts.place_no " +
				"LEFT JOIN contact_persons ON places.contact_person_id = contact_persons.id " +
				"WHERE contracts.lessee_id = @lessee";
			if(checkActiveContracts.Active)
				sql += " AND ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) ";
			
	        MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
	
			cmd.Parameters.AddWithValue("@lessee",Lesseeid);
		
			MySqlDataReader rdr = cmd.ExecuteReader();
			int contact_person_id;
			string cancel_date;
			bool ActiveContract;
				
			ContractsListStore.Clear();
			while (rdr.Read())
			{
				if(rdr["contact_id"] != DBNull.Value)
					contact_person_id = int.Parse (rdr["contact_id"].ToString());
				else
					contact_person_id = -1;
				if(rdr["cancel_date"] != DBNull.Value)
				{
					cancel_date = ((DateTime)rdr["cancel_date"]).ToShortDateString();
					ActiveContract = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["cancel_date"] >= DateTime.Now.Date);
				}
				else
				{
					cancel_date = "";
					ActiveContract = ((DateTime)rdr["start_date"] <= DateTime.Now.Date && (DateTime)rdr["end_date"] >= DateTime.Now.Date);
				}
				ContractsListStore.AppendValues(ActiveContract,
				                             ((DateTime)rdr["start_date"]).ToShortDateString(),
				                             ((DateTime)rdr["end_date"]).ToShortDateString(),
				                             rdr["number"].ToString(),
											 int.Parse(rdr["place_type_id"].ToString()),
				                             rdr["place_no"].ToString(),
				                             rdr["type"].ToString() + " - " + rdr["place_no"].ToString(),				                             
				                             rdr["area"].ToString(),
				                             contact_person_id,
				                             rdr["contact"].ToString(),
				                             cancel_date);
	   		}
			rdr.Close();
			
			MainClass.StatusMessage("Ok");
		}
		
		protected void OntreeviewContractsPopupMenu (object o, Gtk.PopupMenuArgs args)
		{
			bool ItemSelected = treeviewContracts.Selection.CountSelectedRows() == 1;
			Gtk.Menu popupBox = new Gtk.Menu();
			Gtk.MenuItem MenuItemOpenContract = new MenuItem("Открыть договор");
			MenuItemOpenContract.Activated += new EventHandler(OnContractsOpenContract);
			MenuItemOpenContract.Sensitive = ItemSelected;
			popupBox.Add(MenuItemOpenContract);           
			Gtk.MenuItem MenuItemOpenPlace = new MenuItem("Открыть место");
			MenuItemOpenPlace.Activated += new EventHandler(OnContractsOpenPlace);
			MenuItemOpenPlace.Sensitive = ItemSelected;
			popupBox.Add(MenuItemOpenPlace);         
	        popupBox.ShowAll();
	        popupBox.Popup();
		}
		
		[GLib.ConnectBefore]
		protected void OnTreeviewContractsButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
		{
			if((int)args.Event.Button == 3)
		    {       
				OntreeviewContractsPopupMenu(o, null);
		    }
		}

		protected virtual void OnContractsOpenContract (object o, EventArgs args)
		{
			string itemid;
			TreeIter iter;
			
			treeviewContracts.Selection.GetSelected(out iter);
			itemid = (ContractsListStore.GetValue(iter,3)).ToString();
			Contract winContract = new Contract();
			winContract.ContractFill(itemid);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			UpdateContracts ();
		}

		protected virtual void OnContractsOpenPlace (object o, EventArgs args)
		{
			int type;
			string place;
			TreeIter iter;
			
			treeviewContracts.Selection.GetSelected(out iter);
			type = Convert.ToInt32(ContractsListStore.GetValue(iter,4));
			place = (string)ContractsListStore.GetValue(iter,5);
			Place winPlace = new Place();
			winPlace.PlaceFill(type, place);
			winPlace.Show();
			winPlace.Run();
			winPlace.Destroy();
		}
		
		protected void OnButtonGoodsCleanClicked (object sender, System.EventArgs e)
		{
			GoodsNull = true;
			entryGoods.Text = "не указана";
			entryGoods.TooltipText = "";
		}

		protected void OnButtonGoodsEditClicked (object sender, System.EventArgs e)
		{
			reference GoodsSelect = new reference();
			GoodsSelect.SetMode(true,true,true,true,false);
			GoodsSelect.FillList("goods","Группа товаров", "Товары");
			GoodsSelect.Show();
			int result = GoodsSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Goods_id = GoodsSelect.SelectedID;
				GoodsNull = false;
				entryGoods.Text = GoodsSelect.SelectedName;
				entryGoods.TooltipText = GoodsSelect.SelectedName;
			}
			GoodsSelect.Destroy();
		}

		protected void OnCheckActiveContractsToggled (object sender, EventArgs e)
		{
			UpdateContracts();
		}		

	}
}

