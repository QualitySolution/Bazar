using System;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class Cash : Gtk.Dialog
	{
		public bool NewItem;
		int Cashid;

		public Cash ()
		{
			this.Build ();
		}

		public void Fill(int id)
		{
			Cashid = id;
			NewItem = false;

			MainClass.StatusMessage(String.Format ("Запрос кассы №{0}...", id));
			string sql = "SELECT cash.* FROM cash WHERE cash.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", id);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{

					rdr.Read();

					labelId.Text = rdr["id"].ToString();
					entryName.Text = rdr["name"].ToString();
					checkColor.Active = rdr["color"] != DBNull.Value;
					if(rdr["color"] != DBNull.Value)
					{
						Gdk.Color TempColor = new Gdk.Color();
						Gdk.Color.Parse(rdr.GetString ("color"), ref TempColor);
						colorbuttonMarker.Color = TempColor;
					}
				}
				MainClass.StatusMessage("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о кассе!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Nameok = entryName.Text != "";
			buttonOk.Sensitive = Nameok;
		}
	
		protected void OnCheckColorClicked(object sender, EventArgs e)
		{
			colorbuttonMarker.Sensitive = checkColor.Active;
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string sql;
			if(NewItem)
			{
				sql = "INSERT INTO cash (name, color) " +
				      "VALUES (@name, @color)";
			}
			else
			{
				sql = "UPDATE cash SET name = @name, color = @color WHERE id = @id";
			}
			MainClass.StatusMessage("Запись кассы...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Cashid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(checkColor.Active)
				{
					Gdk.Color c = colorbuttonMarker.Color;
					string ColorStr = String.Format("#{0:X}{1:X}{2:X}", c.Red, c.Green, c.Blue);
					Console.WriteLine(ColorStr);
					cmd.Parameters.AddWithValue("@color", ColorStr);
				}
				else
					cmd.Parameters.AddWithValue("@color", DBNull.Value);
			
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи кассы!");
				QSMain.ErrorMessage(this,ex);
			}
		}
	}
}

