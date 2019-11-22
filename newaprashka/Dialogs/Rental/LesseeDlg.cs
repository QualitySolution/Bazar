using System;
using Bazar.Dialogs.Estate;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace Bazar.Dialogs.Rental
{
	public partial class LesseeDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public bool NewLessee;
		int Lesseeid, Goods_id;
		bool GoodsNull;
		
		Gtk.ListStore ContractsListStore;

		private enum ContractCol
		{
			id,
			active,
			begin_date,
			end_date,
			contract_no,
			place_id,
			_null,
			places_text,
			places_aria,
			_null2,
			_null3,
			canceled_date
		}

		AccelGroup grup;
		
		public LesseeDlg ()
		{
			this.Build ();
			
			GoodsNull = true;
			grup = new AccelGroup ();
			this.AddAccelGroup(grup);
									
			//Создаем таблицу "Договора"
			ContractsListStore = new Gtk.ListStore (typeof(int), typeof (bool), typeof (string), typeof (string), typeof (string),
			                                     typeof (int), typeof (string), typeof (string), typeof (string), 
			                                     typeof (int), typeof (string), typeof (string));

			treeviewContracts.AppendColumn("Акт.", new Gtk.CellRendererToggle (), "active", 1);
			treeviewContracts.AppendColumn ("с", new Gtk.CellRendererText (), "text", 2);
			treeviewContracts.AppendColumn ("по", new Gtk.CellRendererText (), "text", 3);
			treeviewContracts.AppendColumn ("Договор", new Gtk.CellRendererText (), "text", 4);
			treeviewContracts.AppendColumn ("Места", new Gtk.CellRendererText (), "text", 7);
			treeviewContracts.AppendColumn ("Площадь", new Gtk.CellRendererText (), "text", 8);
			treeviewContracts.AppendColumn ("Расторгнут", new Gtk.CellRendererText (), "text", 11);
			
			treeviewContracts.Model = ContractsListStore;
			treeviewContracts.ShowAll();
		}
		
		public void LesseeFill(int id)
		{
			Lesseeid = id;
			NewLessee = false;
			
			logger.Info("Запрос арендатора №{0}...", id);
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
				entryKPP.Text = rdr["KPP"].ToString();
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
				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о арендаторе!", logger, ex);
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
					"INN, KPP, OGRN, wholesaler, retail, goods_id, comments) " +
					"VALUES (@name, @FIO, @passport_ser, @passport_no, @exit, @address, " +
					"@INN, @KPP, @OGRN, @wholesaler, @retail, @goods_id, @comments)";
			}
			else
			{
				sql = "UPDATE lessees SET name = @name, FIO_dir = @FIO, passport_ser = @passport_ser, " +
					"passport_no = @passport_no, passport_exit = @exit, address = @address, INN = @INN, KPP = @KPP, OGRN = @OGRN, " +
					"wholesaler = @wholesaler, retail = @retail, goods_id = @goods_id, comments = @comments " +
					"WHERE id = @id";
			}
			logger.Info("Запись арендатора...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Lesseeid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@FIO", DBWorks.ValueOrNull (entryFIO.Text != "", entryFIO.Text));
				cmd.Parameters.AddWithValue("@passport_ser", DBWorks.ValueOrNull (entryPassSer.Text != "", entryPassSer.Text));
				cmd.Parameters.AddWithValue("@passport_no", DBWorks.ValueOrNull (entryPassNo.Text != "", entryPassNo.Text));
				cmd.Parameters.AddWithValue("@exit", DBWorks.ValueOrNull (entryExit.Text != "", entryExit.Text));
				cmd.Parameters.AddWithValue("@address", DBWorks.ValueOrNull (entryAddress.Text != "", entryAddress.Text));
				cmd.Parameters.AddWithValue("@INN", DBWorks.ValueOrNull (entryINN.Text != "", entryINN.Text));
				cmd.Parameters.AddWithValue("@KPP", DBWorks.ValueOrNull (entryKPP.Text != "", entryKPP.Text));
				cmd.Parameters.AddWithValue("@OGRN", DBWorks.ValueOrNull (entryOGRN.Text != "", entryOGRN.Text));
				cmd.Parameters.AddWithValue("@wholesaler", checkBwholesaler.Active);
				cmd.Parameters.AddWithValue("@retail", checkBretail.Active);				
				cmd.Parameters.AddWithValue("@goods_id", DBWorks.ValueOrNull (!GoodsNull, Goods_id));
				cmd.Parameters.AddWithValue("@comments", DBWorks.ValueOrNull (textviewComments.Buffer.Text != "", textviewComments.Buffer.Text));
				
				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка записи арендатора!", logger, ex);
			}
		}

		protected virtual void OnEntryNameChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		void UpdateContracts()
		{
	        logger.Info("Получаем таблицу договоров...");
			
			string sql = "SELECT contracts.*, GROUP_CONCAT(DISTINCT CONCAT(place_types.name, '-', places.place_no) SEPARATOR ', ') as places, " +
				"contract_pays.place_id, SUM(places.area) as area " +
				"FROM contracts " +
				"LEFT JOIN contract_pays ON contract_pays.contract_id = contracts.id " +
				"LEFT JOIN places ON places.id = contract_pays.place_id " +
				"LEFT JOIN place_types ON places.type_id = place_types.id " +
				"WHERE contracts.lessee_id = @lessee";
			if(checkActiveContracts.Active)
				sql += " AND ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) ";

			sql += " GROUP BY contracts.id";

	        MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
	
			cmd.Parameters.AddWithValue("@lessee",Lesseeid);
		
			MySqlDataReader rdr = cmd.ExecuteReader();
			string cancel_date;
			bool ActiveContract;
				
			ContractsListStore.Clear();
			while (rdr.Read())
			{
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
				ContractsListStore.AppendValues(rdr.GetInt32 ("id"),
				                                ActiveContract,
				                             ((DateTime)rdr["start_date"]).ToShortDateString(),
				                             ((DateTime)rdr["end_date"]).ToShortDateString(),
				                             rdr["number"].ToString(),
											 rdr.GetInt32("place_id"),
				                             null,
				                             rdr["places"],				                             
				                             rdr["area"].ToString(),
				                             null,
				                             null,
				                             cancel_date);
	   		}
			rdr.Close();
			
			logger.Info("Ok");
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
			int itemid;
			TreeIter iter;
			
			treeviewContracts.Selection.GetSelected(out iter);
			itemid = (int) ContractsListStore.GetValue(iter,0);
			ContractDlg winContract = new ContractDlg();
			winContract.ContractFill(itemid);
			winContract.Show();
			winContract.Run();
			winContract.Destroy();
			UpdateContracts ();
		}

		protected virtual void OnContractsOpenPlace (object o, EventArgs args)
		{
			int place_id;
			TreeIter iter;
			
			treeviewContracts.Selection.GetSelected(out iter);
			place_id = Convert.ToInt32(ContractsListStore.GetValue(iter, (int)ContractCol.place_id));
			PlaceDlg winPlace = new PlaceDlg(false);
			winPlace.PlaceFill(place_id);
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
			Reference GoodsSelect = new Reference(orderBy: "name");
			GoodsSelect.SetMode(true,true,true,true,false);
			GoodsSelect.NameMaxLength = 45;
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

