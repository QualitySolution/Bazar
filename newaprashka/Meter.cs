using System;
using MySql.Data.MySqlClient;
using Gtk;
using QSProjectsLib;

namespace bazar
{
	public partial class Meter : Gtk.Dialog
	{
		bool NewItem;
		int Itemid, ParentId;

		public Meter (bool New)
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboPlaceType,"place_types", ComboWorks.ListMode.WithNo);
			ComboWorks.ComboFillReference(comboMeterType, "meter_types", ComboWorks.ListMode.WithNo);
			NewItem = New;
		}

		public void Fill(int id)
		{
			Itemid = id;
			NewItem = false;

			MainClass.StatusMessage(String.Format("Запрос счетчика №{0}...", id));
			string sql = "SELECT * FROM meters WHERE id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				object DBPlaceT, DBPlaceNo;
				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{		
					rdr.Read();

					labelID.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
					ComboWorks.SetActiveItem (comboMeterType, rdr.GetInt32 ("meter_type_id"));
					ParentId = DBWorks.GetInt (rdr, "parent_meter_id", -1);
					checkDisabled.Active = rdr.GetBoolean ("disabled");
				
					//запоминаем переменные что бы освободить соединение
					DBPlaceT = rdr["place_type_id"];
					DBPlaceNo = rdr["place_no"];
				}
				if(ParentId > 0)
				{
					sql = "SELECT name FROM meters WHERE id = @id";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", ParentId);
					entryParent.Text = cmd.ExecuteScalar().ToString ();
					entryParent.TooltipText = entryParent.Text;
				}

				if(DBPlaceT != DBNull.Value)
					ComboWorks.SetActiveItem (comboPlaceType, Convert.ToInt32 (DBPlaceT));
				else
					ComboWorks.SetActiveItem (comboPlaceType, -1);
				if(DBPlaceNo != DBNull.Value)
				{
					TreeIter iter;
					ListStoreWorks.SearchListStore((ListStore)comboPlaceNo.Model, DBPlaceNo.ToString(), out iter);
					comboPlaceNo.SetActiveIter(iter);
				}
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о счётчике!");
				QSMain.ErrorMessage(this, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
			TestCanSave();
		}

		public void SetPlace(int PlaceTypeId, string PlaceNo, bool DefaultName = true)
		{
			ComboWorks.SetActiveItem (comboPlaceType, PlaceTypeId);
			ComboWorks.SetActiveItem (comboPlaceNo, PlaceNo);
			comboPlaceType.Sensitive = false;
			comboPlaceNo.Sensitive = false;
			entryName.Text = String.Format ("{0}-{1}", comboPlaceType.ActiveText, comboPlaceNo.ActiveText);
		}

		protected void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool MeterTypeOk = ComboWorks.GetActiveId (comboMeterType) > 0;
			bool PlaceOk = ComboWorks.GetActiveId (comboMeterType) > 0 && comboPlaceNo.Active >= 0;
			buttonOk.Sensitive = Nameok && MeterTypeOk && PlaceOk;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO meters (name, meter_type_id, place_type_id, place_no, parent_meter_id, disabled) " +
					"VALUES (@name, @meter_type_id, @place_type_id, @place_no, @parent_meter_id, @disabled)";
			}
			else
			{
				sql = "UPDATE meters SET name = @name, meter_type_id = @meter_type_id, place_type_id = @place_type_id, " +
					"place_no = @place_no, parent_meter_id = @parent_meter_id, disabled = @disabled WHERE id = @id";
			}
			MainClass.StatusMessage("Запись счётчика...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Itemid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@meter_type_id", DBWorks.ValueOrNull (comboMeterType.Active > 0, ComboWorks.GetActiveId (comboMeterType)));
				cmd.Parameters.AddWithValue("@place_type_id", DBWorks.ValueOrNull (comboPlaceType.Active > 0, ComboWorks.GetActiveId (comboPlaceType)));
				cmd.Parameters.AddWithValue("@place_no", DBWorks.ValueOrNull(comboPlaceNo.Active >= 0, comboPlaceNo.ActiveText));
				cmd.Parameters.AddWithValue("@parent_meter_id", DBWorks.ValueOrNull(ParentId > 0, ParentId));
				cmd.Parameters.AddWithValue("@disabled", checkDisabled.Active );

				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи счётчика!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnEntryNameChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboMeterTypeChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboPlaceTypeChanged(object sender, EventArgs e)
		{
			TreeIter iter;
			int id;
			((ListStore)comboPlaceNo.Model).Clear();
			if(comboPlaceType.GetActiveIter(out iter) && comboPlaceType.Active > 0)
			{
				id = (int)comboPlaceType.Model.GetValue(iter,1);
				MainClass.ComboPlaceNoFill(comboPlaceNo, id);
			}
			TestCanSave();
		}

		protected void OnComboPlaceNoChanged(object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnButtonParentCleanClicked(object sender, EventArgs e)
		{
			ParentId = -1;
			entryParent.Text = "";
		}

		protected void OnButtonParentEditClicked(object sender, EventArgs e)
		{
			Reference MeterSelect = new Reference();
			MeterSelect.SetMode(false, true, false, false, false);
			MeterSelect.SqlSelect = "SELECT meters.id, meters.name, meter_types.name as type FROM meters " +
				"LEFT JOIN meter_types ON meters.meter_type_id = meter_types.id " +
				"WHERE meters.disabled = FALSE ";
			MeterSelect.Columns.Add (new Reference.ColumnInfo ("Тип счётчика", "{2}"));
			MeterSelect.FillList("meters", "Счётчики", "Счётчик");
			MeterSelect.Show();
			int result = MeterSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				ParentId = MeterSelect.SelectedID;
				entryParent.Text = MeterSelect.SelectedName;
			}
			MeterSelect.Destroy ();
		}
	}
}

