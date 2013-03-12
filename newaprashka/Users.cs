using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class Users : Gtk.Dialog
	{
		Gtk.ListStore UsersListStore;

		public Users ()
		{
			this.Build ();

			//Создаем таблицу "Пользователей"
			UsersListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (string), 
			                                         typeof (bool), typeof (bool));
			
			treeviewUsers.AppendColumn("Код", new Gtk.CellRendererText (), "text", 0);
			treeviewUsers.AppendColumn("Логин", new Gtk.CellRendererText (), "text", 1);
			treeviewUsers.AppendColumn("Имя", new Gtk.CellRendererText (), "text", 2);
			treeviewUsers.AppendColumn("Администратор", new Gtk.CellRendererToggle (), "active", 3);
			treeviewUsers.AppendColumn("Касса", new Gtk.CellRendererToggle (), "active", 4);

			treeviewUsers.Model = UsersListStore;
			treeviewUsers.ShowAll();
			UpdateUsers ();

			if(MainClass.connectionDB.DataSource == "demo.qsolution.ru")
				buttonAdd.Sensitive = false;
		}

		void UpdateUsers()
		{
			MainClass.StatusMessage("Получаем таблицу пользователей...");

			string sql = "SELECT * FROM users ";
			MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
			
			MySqlDataReader rdr = cmd.ExecuteReader();

			UsersListStore.Clear();
			while (rdr.Read())
			{
				UsersListStore.AppendValues(int.Parse(rdr["id"].ToString()),
				                            rdr["login"].ToString(),
				                            rdr["name"].ToString(),
				                            (bool)rdr["admin"],
				                            (bool)rdr["edit_slips"]);
			}
			rdr.Close();
			
			MainClass.StatusMessage("Ok");
			
			OnTreeviewUsersCursorChanged (null,null);
		}

		protected void OnTreeviewUsersCursorChanged (object sender, EventArgs e)
		{
			bool isSelect = treeviewUsers.Selection.CountSelectedRows() == 1;
			buttonEdit.Sensitive = isSelect;
			if(MainClass.connectionDB.DataSource == "demo.qsolution.ru")
				isSelect = false;
			buttonDelete.Sensitive = isSelect;
		}

		protected void OnTreeviewUsersRowActivated (object o, RowActivatedArgs args)
		{
			buttonEdit.Click ();
		}

		protected void OnButtonAddClicked (object sender, EventArgs e)
		{
			UserProperty winUser = new UserProperty();
			winUser.NewUser = true;
			winUser.Show();
			winUser.Run();
			winUser.Destroy();
			UpdateUsers();
		}

		protected void OnButtonEditClicked (object sender, EventArgs e)
		{
			TreeIter iter;

			treeviewUsers.Selection.GetSelected(out iter);
			int itemid = Convert.ToInt32(UsersListStore.GetValue(iter,0));
			UserProperty winUser = new  UserProperty();
			winUser.UserFill(itemid);
			winUser.Show();
			winUser.Run();
			winUser.Destroy();
			UpdateUsers();
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			
			treeviewUsers.Selection.GetSelected(out iter);
			int itemid = Convert.ToInt32(UsersListStore.GetValue(iter,0));
			string loginname = UsersListStore.GetValue(iter,1).ToString ();
			Delete winDel = new Delete();
			if (winDel.RunDeletion("users",itemid))
			{
				MainClass.StatusMessage("Удаляем пользователя с сервера...");
				string sql;
				sql = String.Format("DROP USER {0}, {0}@localhost", loginname);
				try 
				{
					MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
					cmd.ExecuteNonQuery();
					MainClass.StatusMessage("Пользователь удалён. Ok");
				} 
				catch (Exception ex) 
				{
					Console.WriteLine(ex.ToString());
					MainClass.StatusMessage("Ошибка удаления пользователя!");
					MainClass.ErrorMessage(this,ex);
				}
			}
			winDel.Destroy();
			UpdateUsers();
		}
	}
}

