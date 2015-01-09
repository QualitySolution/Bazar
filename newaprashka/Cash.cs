using System;
using MySql.Data.MySqlClient;
using NLog;
using QSProjectsLib;

namespace bazar
{
	public partial class Cash : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
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

			logger.Info(String.Format ("Запрос кассы №{0}...", id));
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
				logger.Info("Ok");
				this.Title = entryName.Text;
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о кассе!", logger, ex);
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
			logger.Info("Запись кассы...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", Cashid);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				if(checkColor.Active)
				{
					Gdk.Color c = colorbuttonMarker.Color;
					string ColorStr = String.Format("#{0:x4}{1:x4}{2:x4}", c.Red, c.Green, c.Blue);
					Console.WriteLine(ColorStr);
					cmd.Parameters.AddWithValue("@color", ColorStr);
				}
				else
					cmd.Parameters.AddWithValue("@color", DBNull.Value);
			
				cmd.ExecuteNonQuery();
				logger.Info("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка записи кассы!", logger, ex);
			}
		}
	}
}

