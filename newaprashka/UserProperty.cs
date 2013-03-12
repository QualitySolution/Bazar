using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class UserProperty : Gtk.Dialog
	{
		public bool NewUser;
		string OriginLogin;

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
				OriginLogin = rdr["login"].ToString();
				entryName.Text = rdr["name"].ToString();
				entryPassword.Text = "nochanged";

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
				if(!CreateLogin ())
					return;
				sql = "INSERT INTO users (name, login, admin, edit_slips, description) " +
					"VALUES (@name, @login, @admin, @edit_slips, @description)";
			}
			else
			{
				if(OriginLogin != entryLogin.Text)
					if(!RenameLogin ())
						return;
				if(entryPassword.Text != "nochanged")
					ChangePassword ();
				sql = "UPDATE users SET name = @name, login = @login, admin = @admin, edit_slips = @edit_slips, " +
					"description = @description " +
						"WHERE id = @id";
			}
			UpdatePrivileges ();
			MainClass.StatusMessage("Запись пользователя...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", entryID.Text);
				cmd.Parameters.AddWithValue("@name", entryName.Text);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
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

		bool CreateLogin()
		{
			MainClass.StatusMessage("Создание учетной записи на сервере...");
			try 
			{
				//Проверка существует ли логин
				string sql = "SELECT COUNT(*) FROM users WHERE login = @login";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
				if(Convert.ToInt32(cmd.ExecuteScalar()) > 0)
				{
					string Message = "Пользователь с логином " + entryLogin.Text + " уже существует в базе. " +
						"Создание второго пользователя с таким же логином невозможно.";
					MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
					                                      MessageType.Warning, 
					                                      ButtonsType.Ok,
					                                      Message);
					md.Run ();
					md.Destroy();
					return false;
				}

				sql = "SELECT COUNT(*) from mysql.user WHERE USER = @login";
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
				try
				{
					if(Convert.ToInt32(cmd.ExecuteScalar()) > 0)
					{
						string Message = "Пользователь с логином " + entryLogin.Text + " уже существует на сервере. " +
							"Если он использовался для доступа к другим базам, может возникнуть путаница. " +
							"Использовать этот логин?";
						MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
						                                      MessageType.Warning, 
						                                      ButtonsType.YesNo,
						                                      Message);
						bool result = (ResponseType)md.Run () == ResponseType.Yes;
						md.Destroy();
						return result;
					}
				}
				catch (MySqlException ex)
				{
					if(ex.Number == 1045)
						MainClass.StatusMessage ("Нет доступа к таблице пользователей, пробую создать пользователя в слепую.");
					else
						return false;
				}
				//Создание пользователя.
				sql = "CREATE USER @login IDENTIFIED BY @password";
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
				cmd.Parameters.AddWithValue("@password", entryPassword.Text);
				cmd.ExecuteNonQuery();
				sql = "CREATE USER " + entryLogin.Text + "@localhost IDENTIFIED BY @password";
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@password", entryPassword.Text);
				cmd.ExecuteNonQuery();

				MainClass.StatusMessage("Ok");
				return true;
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка создания пользователя!");
				MainClass.ErrorMessage(this,ex);
				return false;
			}
		}

		bool RenameLogin()
		{
			MainClass.StatusMessage("Переименование учетной записи на сервере...");
			try 
			{
				//Проверка существует ли логин
				string sql = "SELECT COUNT(*) from mysql.user WHERE USER = @login";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
				try
				{
					if( Convert.ToInt32(cmd.ExecuteScalar()) > 0)
					{
						string Message = "Пользователь с логином " + entryLogin.Text + " уже существует на сервере. " +
							"Переименование невозможно.";
						MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
						                                      MessageType.Error, 
						                                      ButtonsType.Ok,
						                                      Message);
						md.Destroy();
						return false;
					}
				}
				catch (MySqlException ex)
				{
					if(ex.Number == 1045)
						MainClass.StatusMessage ("Нет доступа к таблице пользователей, пробую создать пользователя в слепую.");
					else
						return false;
				}
				
				//Переименование пользователя.
				sql = String.Format("RENAME USER {0} TO {1}, {0}@localhost TO {1}@localhost", OriginLogin, entryLogin.Text);
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				return true;
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка переименования пользователя!");
				MainClass.ErrorMessage(this,ex);
				return false;
			}
		}

		void ChangePassword()
		{
			MainClass.StatusMessage("Отправляем новый пароль на сервер...");
			string sql;
			try 
			{
				sql = String.Format("SET PASSWORD FOR {0} = PASSWORD('{1}')", entryLogin.Text, entryPassword.Text);
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				sql = String.Format("SET PASSWORD FOR {0}@localhost = PASSWORD('{1}')", entryLogin.Text, entryPassword.Text);
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Пароль изменен. Ok");
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка установки пароля!");
				MainClass.ErrorMessage(this,ex);
			}
		}

		void UpdatePrivileges()
		{
			MainClass.StatusMessage("Устанавливаем права...");
			try 
			{
				string privileges;
				if(checkAdmin.Active)
					privileges = "ALL";
				else
					privileges = "SELECT, INSERT, UPDATE, DELETE, EXECUTE, SHOW VIEW";
				string sql = "GRANT " + privileges + " ON " + MainClass.connectionDB.Database +".* TO @login";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", entryLogin.Text);
				cmd.ExecuteNonQuery();
				if(checkAdmin.Active)
				{
					sql = "GRANT CREATE USER, GRANT OPTION ON *.* TO " + entryLogin.Text + ", " + entryLogin.Text + "@localhost";
				}
				else
				{
					sql = "REVOKE CREATE USER, GRANT OPTION ON *.* FROM " + entryLogin.Text + ", " + entryLogin.Text + "@localhost";
				}
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				if(checkAdmin.Active)
				{
					sql = "GRANT SELECT, UPDATE ON mysql.* TO " + entryLogin.Text + ", " + entryLogin.Text + "@localhost";
				}
				else
				{
					sql = "REVOKE SELECT, UPDATE ON mysql.* FROM " + entryLogin.Text + ", " + entryLogin.Text + "@localhost";
				}
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Права установлены. Ok");
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка установки прав!");
				MainClass.ErrorMessage(this,ex);
			}
		}
	}
}

