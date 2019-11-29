using System;
using Bazar;
using Gtk;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace Bazar.Dialogs.Estate
{
	public partial class MeterDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		bool NewItem;
		int Itemid, ParentId, placeId;

		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		public MeterDlg (bool New)
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboMeterType, "meter_types", ComboWorks.ListMode.WithNo, OrderBy: "name");
			NewItem = New;
		}

		public void Fill(int id)
		{
			Itemid = id;
			NewItem = false;

			logger.Info("Запрос счетчика №{0}...", id);
			string sql = "SELECT * FROM meters WHERE id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{		
					rdr.Read();

					labelID.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
					ComboWorks.SetActiveItem (comboMeterType, rdr.GetInt32 ("meter_type_id"));
					ParentId = DBWorks.GetInt (rdr, "parent_meter_id", -1);
					checkDisabled.Active = rdr.GetBoolean ("disabled");
					placeId = rdr.GetInt32("place_id");
				}
				if(ParentId > 0)
				{
					sql = "SELECT name FROM meters WHERE id = @id";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", ParentId);
					entryParent.Text = cmd.ExecuteScalar().ToString ();
					entryParent.TooltipText = entryParent.Text;
				}

				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о счётчике!", logger, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
			TestCanSave();
		}

		public void SetPlace(int placeId, string DefaultName)
		{
			this.placeId = placeId;
			entryName.Text = DefaultName;
		}

		protected void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool MeterTypeOk = ComboWorks.GetActiveId (comboMeterType) > 0;
			buttonOk.Sensitive = Nameok && MeterTypeOk;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO meters (name, meter_type_id, place_id, parent_meter_id, disabled) " +
					"VALUES (@name, @meter_type_id, @place_id, @parent_meter_id, @disabled)";
			}
			else
			{
				sql = "UPDATE meters SET name = @name, meter_type_id = @meter_type_id, place_id = @place_id, " +
					"parent_meter_id = @parent_meter_id, disabled = @disabled WHERE id = @id";
			}
			logger.Info("Запись счётчика...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Itemid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@meter_type_id", DBWorks.ValueOrNull (comboMeterType.Active > 0, ComboWorks.GetActiveId (comboMeterType)));
				cmd.Parameters.AddWithValue("@place_id", placeId);
				cmd.Parameters.AddWithValue("@parent_meter_id", DBWorks.ValueOrNull(ParentId > 0, ParentId));
				cmd.Parameters.AddWithValue("@disabled", checkDisabled.Active );

				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка записи счётчика!", logger, ex);
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

