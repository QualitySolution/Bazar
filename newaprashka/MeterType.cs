using System;
using System.Collections.Generic;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class MeterType : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public bool NewItem;
		int Itemid;

		ListStore TariffListStore;
		TreeModel ServiceNameList;
		CellEditable ServiceCellEdit;

		public MeterType ( bool New)
		{
			this.Build ();

			ComboBox ServiceCombo = new ComboBox();
			ComboWorks.ComboFillReference(ServiceCombo,"services", ComboWorks.ListMode.WithNo, OrderBy: "name");
			ServiceNameList = ServiceCombo.Model;
			ServiceCombo.Destroy ();

			TariffListStore = new Gtk.ListStore (typeof (int), 	// 0 - tariff id
			                                      typeof (string),	// 1 - Name
			                                      typeof (int),		// 2 - Service id
			                                     typeof (string)	// 3 - Service name
			                                      );

			Gtk.CellRendererCombo CellService = new CellRendererCombo();
			CellService.TextColumn = 0;
			CellService.Editable = true;
			CellService.Model = ServiceNameList;
			CellService.HasEntry = false;
			CellService.Edited += OnServiceComboEdited;
			CellService.EditingStarted += OnServiceComboStartEdited;

			Gtk.CellRendererText CellName = new CellRendererText();
			CellName.Editable = true;
			CellName.Edited += OnCellNameEdited;

			//treeviewTariff.AppendColumn ("Код", new Gtk.CellRendererText (), "text", 0);
			treeviewTariff.AppendColumn ("Название", CellName, "text", 1);
			treeviewTariff.AppendColumn ("Услуга для оплаты", CellService, "text", 3);

			treeviewTariff.Model = TariffListStore;
			treeviewTariff.ShowAll();

			if(New)
			{
				NewItem = New;
				TariffListStore.AppendValues (-1,
				                              "Основной",
				                              -1,
				                              "нет");
			}
		}

		public void Fill(int id)
		{
			Itemid = id;
			NewItem = false;

			logger.Info("Запрос типа счетчиков №{0}...", id);
			string sql = "SELECT * FROM meter_types WHERE id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{		
					rdr.Read();

					labelID.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
				}
				this.Title = entryName.Text;

				//Получаем таблицу тарифов
				sql = "SELECT meter_tariffs.*, services.name as service FROM meter_tariffs " +
						"LEFT JOIN services ON meter_tariffs.service_id = services.id " +
						"WHERE meter_tariffs.meter_type_id = @id";

				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@id", Itemid);
				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						TariffListStore.AppendValues(rdr.GetInt32 ("id"),
						                              rdr["name"].ToString(),
						                             DBWorks.GetInt (rdr, "service_id", -1),
						                             DBWorks.GetString (rdr, "service", "нет")
						                              );
					}
				}
				logger.Info("Ok");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о типе счетчика!", logger, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
			TestCanSave();
		}

		protected void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool TariffsOk = TariffListStore.IterNChildren () > 0;
			buttonOk.Sensitive = Nameok && TariffsOk;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if (ServiceCellEdit != null)
				ServiceCellEdit.FinishEditing (); // Небходимо для исправления #29
			string sql;
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction ();
			if(NewItem)
			{
				sql = "INSERT INTO meter_types (name) " +
					"VALUES (@name)";
			}
			else
			{
				sql = "UPDATE meter_types SET name = @name WHERE id = @id";
			}
			logger.Info("Запись типа счетчика...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);

				cmd.Parameters.AddWithValue("@id", Itemid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);

				cmd.ExecuteNonQuery();

				if(NewItem)
					Itemid = (int) cmd.LastInsertedId;
				//Записываем таблицу тарифов
				TreeIter iter;
				TariffListStore.GetIterFirst(out iter);
				do
				{
					if(!TariffListStore.IterIsValid (iter))
						break;
					if((int)TariffListStore.GetValue(iter, 0) > 0)
						sql = "UPDATE meter_tariffs SET meter_type_id = @meter_type_id, service_id = @service_id, name = @name " +
							"WHERE id = @id";
					else
						sql = "INSERT INTO meter_tariffs (meter_type_id, service_id, name) " +
							"VALUES (@meter_type_id, @service_id, @name)";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@meter_type_id", Itemid);
					if((int)TariffListStore.GetValue(iter, 2) > 0)
						cmd.Parameters.AddWithValue("@service_id", TariffListStore.GetValue(iter, 2));
					else
						cmd.Parameters.AddWithValue("@service_id", DBNull.Value);
					cmd.Parameters.AddWithValue("@name", TariffListStore.GetValue(iter, 1));
					cmd.Parameters.AddWithValue("@id", TariffListStore.GetValue(iter, 0));

					cmd.ExecuteNonQuery();
				}
				while(TariffListStore.IterNext(ref iter));

				logger.Info("Ok");
				trans.Commit();
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				trans.Rollback ();
				QSMain.ErrorMessageWithLog(this, "Ошибка записи типа счетчика!", logger, ex);
			}
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		void OnServiceComboEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!TariffListStore.GetIterFromString (out iter, args.Path))
				return;
			if(args.NewText == null)
			{
				return;
			}

			TreeIter ServiceIter;
			if(ListStoreWorks.SearchListStore ((ListStore)ServiceNameList, args.NewText, out ServiceIter))
			{
				TariffListStore.SetValue (iter, 2, ServiceNameList.GetValue (ServiceIter, 1));
				TariffListStore.SetValue (iter, 3, args.NewText);
			}
		}

		void OnServiceComboStartEdited (object o, EditingStartedArgs args)
		{
			ServiceCellEdit = args.Editable;
		}

		private void OnCellNameEdited (object o, Gtk.EditedArgs args)
		{
			TreeIter iter;
			if (!TariffListStore.GetIterFromString (out iter, args.Path))
				return;

			if(args.NewText == null || args.NewText == "")
			{
				return;
			}

			TariffListStore.SetValue (iter, 1, args.NewText);
		}

		protected void OnButtonAddClicked(object sender, EventArgs e)
		{
			TariffListStore.AppendValues (-1,
			                          "Новый тариф",
			                          -1,
			                          "нет");
			OnTreeviewTariffCursorChanged (null, null);
			TestCanSave ();
		}

		protected void OnTreeviewTariffCursorChanged(object sender, EventArgs e)
		{
			bool isSelect = treeviewTariff.Selection.CountSelectedRows() == 1;
			buttonDelete.Sensitive = isSelect;
		}

		protected void OnButtonDeleteClicked(object sender, EventArgs e)
		{
			TreeIter iter;
			treeviewTariff.Selection.GetSelected (out iter);
			if ((int)TariffListStore.GetValue (iter, 0) > 0) 
			{
				int itemid = (int)TariffListStore.GetValue (iter, 0);
				Delete winDelete = new Delete();
				if(winDelete.RunDeletion("meter_tariffs", itemid))
					TariffListStore.Remove(ref iter);
			}
			else
			{
				TariffListStore.Remove(ref iter);
			}
			TestCanSave ();
			OnTreeviewTariffCursorChanged (null, null);
		}
	}
}

