using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class Service : Gtk.Dialog
	{
		public bool NewService;
		int Serviceid;
		TreeIter iter;

		public Service ()
		{
			this.Build ();

			MainClass.ComboFillReference(comboUnits, "units", 0);
			MainClass.ComboFillReference(comboIncomeItem, "income_items", 2);
		}

		public void ServiceFill(int id)
		{
			Serviceid = id;
			NewService = false;
			
			MainClass.StatusMessage(String.Format ("Запрос услуги №{0}...", id));
			string sql = "SELECT services.*, units.name as unit FROM services LEFT JOIN units ON services.units_id = units.id WHERE services.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", id);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
				
				labelID.Text = rdr["id"].ToString();
				entryName.Text = rdr["name"].ToString();
				if(rdr["units_id"] != DBNull.Value)
				{
					MainClass.SearchListStore((ListStore)comboUnits.Model, rdr.GetInt32("units_id"), out iter);
					comboUnits.SetActiveIter(iter);
				}
				if(rdr["income_id"] != DBNull.Value)
				{
					MainClass.SearchListStore((ListStore)comboIncomeItem.Model, rdr.GetInt32("income_id"), out iter);
					comboIncomeItem.SetActiveIter(iter);
				}

				rdr.Close();
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о услуге!");
				MainClass.ErrorMessage(this,ex);
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
				sql = "INSERT INTO services (name, units_id, income_id) " +
					"VALUES (@name, @units_id, @income_id)";
			}
			else
			{
				sql = "UPDATE services SET name = @name, units_id = @units_id, income_id = @income_id " +
						"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись услуги...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Serviceid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(comboUnits.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@units_id",comboUnits.Model.GetValue(iter,1));
				if(comboIncomeItem.Active > 0 && comboIncomeItem.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@income_id",comboIncomeItem.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@income_id", DBNull.Value);

				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи услуги!");
				MainClass.ErrorMessage(this,ex);
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

