using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;

namespace bazar
{
	public partial class Service : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public bool NewService;
		int Serviceid;
		TreeIter iter;

		public Service ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboUnits, "units", ComboWorks.ListMode.OnlyItems);
			ComboWorks.ComboFillReference(comboIncomeItem, "income_items", ComboWorks.ListMode.WithNo);
		}

		public void ServiceFill(int id)
		{
			Serviceid = id;
			NewService = false;
			
			logger.Info("Запрос услуги №{0}...", id);
			string sql = "SELECT services.*, units.name as unit FROM services LEFT JOIN units ON services.units_id = units.id WHERE services.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", id);
				
				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					rdr.Read();
					
					labelID.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
					if(rdr["units_id"] != DBNull.Value)
					{
						ListStoreWorks.SearchListStore((ListStore)comboUnits.Model, rdr.GetInt32("units_id"), out iter);
						comboUnits.SetActiveIter(iter);
					}
					if(rdr["income_id"] != DBNull.Value)
					{
						ListStoreWorks.SearchListStore((ListStore)comboIncomeItem.Model, rdr.GetInt32("income_id"), out iter);
						comboIncomeItem.SetActiveIter(iter);
					}
					checkArea.Active= Boolean.Parse(rdr["by_area"].ToString());
					checkIncomplete.Active= Boolean.Parse(rdr["incomplete_month"].ToString());
				}
				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о услуге!", logger, ex);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			bool Unitsok = comboUnits.Active >= 0;
			buttonOk.Sensitive = Nameok && Unitsok;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			if(NewService)
			{
				sql = "INSERT INTO services (name, units_id, income_id, by_area, incomplete_month) " +
					"VALUES (@name, @units_id, @income_id, @by_area, @incomplete_month)";
			}
			else
			{
				sql = "UPDATE services SET name = @name, units_id = @units_id, income_id = @income_id, " +
					"by_area = @by_area, incomplete_month = @incomplete_month WHERE id = @id";
			}
			logger.Info("Запись услуги...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Serviceid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(comboUnits.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@units_id",comboUnits.Model.GetValue(iter,1));
				if(comboIncomeItem.Active > 0 && comboIncomeItem.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@income_id",comboIncomeItem.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@income_id", DBNull.Value);
				cmd.Parameters.AddWithValue("by_area", checkArea.Active);
				cmd.Parameters.AddWithValue("incomplete_month", checkIncomplete.Active);
				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				logger.ErrorException ("Ошибка записи услуги!", ex);
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnEntryNameChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboUtitsChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}
	}
}