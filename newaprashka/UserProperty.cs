using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class UserProperty : Gtk.Dialog
	{
		public bool NewUser;

		public UserProperty ()
		{
			this.Build ();
		}

		public void UserFill(int UserId)
		{
			NewUser = false;
			
			MainClass.StatusMessage(String.Format ("Запрос пользователя №{0}...", UserId));
			string sql = "SELECT * FROM users " +
						 "WHERE users.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", UserId);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
				
				entryID.Text = rdr["id"].ToString();
				entryLogin.Text = rdr["login"].ToString();
				entryName.Text = rdr["name"].ToString();

				checkAdmin.Active = (bool)rdr["admin"];
				checkEditSlips.Active = (bool)rdr["edit_slips"];

				textviewComments.Buffer.Text = rdr["description"].ToString();
				rdr.Close();
				
				this.Title = entryName.Text;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о пользователе!");
				MainClass.ErrorMessage(this,ex);
			}
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;

			if(NewUser)
			{
				sql = "INSERT INTO users (org_id, cash_id, lessee_id, user_id, date, sum, " +
					"contract_no, income_id, details) " +
						"VALUES (@org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
						"@contract_no, @income_id, @details)";
			}
			else
			{
				sql = "UPDATE users SET name = @name, admin = @admin, edit_slips = @edit_slips, " +
					"description = @description " +
						"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись пользователя...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", entryID.Text);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@admin", checkAdmin.Active);
				cmd.Parameters.AddWithValue("@edit_slips", checkEditSlips.Active);
				if(textviewComments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@description", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@description", textviewComments.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				if(MainClass.User.Login == entryLogin.Text)
					MainClass.User.UpdateUserInfoByLogin ();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи пользователя!");
				MainClass.ErrorMessage(this,ex);
			}

		}
	}
}

