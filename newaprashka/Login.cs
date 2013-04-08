using System;
using Gtk;
using Nini.Config;
using System.IO;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class Login : Gtk.Dialog
	{
		String ConnectionError;
		string BaseName = "bazar";
		IniConfigSource Configsource;

		public Login ()
		{
			this.Build ();
			UpdateFromGConf();
		}
		
		void UpdateFromGConf ()
		{
			string configfile = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "bazar.ini");
			try
			{
				Configsource = new IniConfigSource(configfile);
				Configsource.Reload();
				entryServer.Text = Configsource.Configs["Login"].Get("Server");
				entryUser.Text = Configsource.Configs["Login"].Get("UserLogin");
				entryPassword.GrabFocus();
				BaseName = Configsource.Configs["Login"].Get("DataBase","bazar");
			} 
			catch (Exception ex)
        	{
				Console.WriteLine(ex.Message);
				Console.WriteLine("Конфигурационный фаил не найден. Создаем новый.");
				Configsource = new IniConfigSource();
				
				IConfig config = Configsource.AddConfig("Login");
				config.Set("UserLogin", "demo");
				config.Set("Server", "demo.qsolution.ru");
				Configsource.Save(configfile);
				
				entryServer.Text = config.Get("Server");
				entryUser.Text = config.Get("UserLogin");
        	}
		}
		
		protected virtual void OnButtonErrorInfoClicked (object sender, System.EventArgs e)
		{
			MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,
	                              MessageType.Error, 
                                  ButtonsType.Close,"ошибка");
			md.UseMarkup = false;
			md.Text = ConnectionError;
			md.Run ();
			md.Destroy();
		}
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string connStr = "server=" + entryServer.Text + ";user=" +
				entryUser.Text + ";database=" + BaseName + ";port=3306;password=" +
				entryPassword.Text + ";";
        	MainClass.connectionDB = new MySqlConnection(connStr);
        	try
        	{
            	Console.WriteLine("Connecting to MySQL...");
				labelLoginInfo.Text = "Соединяемся....";
				labelLoginInfo.GdkWindow.ProcessUpdates(true);
				
				Console.WriteLine(connStr);
				
				MainClass.connectionDB.Open();
            	// Perform databse operations
				labelLoginInfo.Text = "";
				buttonErrorInfo.Visible = false;
				Configsource.Configs["Login"].Set("Server", entryServer.Text);
				Configsource.Configs["Login"].Set("UserLogin", entryUser.Text);
				Configsource.Save();
				MainClass.ConnectionString = connStr;
				MainClass.User.Login = entryUser.Text.ToLower();
				this.Respond(ResponseType.Ok);
        	}
			catch (MySqlException ex)
        	{
            	if(ex.Number == 1045)
					labelLoginInfo.Text = "Доступ запрещен. Проверьте логин и пароль.";
				else
					labelLoginInfo.Text = "Ошибка соединения с базой данных.";
				buttonErrorInfo.Visible = true;
				ConnectionError = "Строка соединения: " + connStr + "\nИсключение: " + ex.ToString();
				Console.WriteLine(ex.ToString());
				MainClass.connectionDB.Close();
        	}

		}
		
		protected virtual void OnEntryPasswordActivated (object sender, System.EventArgs e)
		{
			buttonOk.Activate();
		}

		protected void OnEntryActivated (object sender, EventArgs e)
		{
			this.ChildFocus (DirectionType.TabForward);
		}

		protected void OnEntryServerChanged (object sender, EventArgs e)
		{
			buttonDemo.Visible = entryServer.Text.ToLower() == "demo.qsolution.ru";
		}

		protected void OnButtonDemoClicked (object sender, EventArgs e)
		{
			MessageDialog md = new MessageDialog( this, DialogFlags.DestroyWithParent,
			                                     MessageType.Info, 
			                                     ButtonsType.Ok,"Для подключения к демострационному серверу используйте следующие настройки:\n" +
			                                     "\n" +
			                                     "<b>Сервер:</b> demo.qsolution.ru\n" +
			                                     "<b>Пользователь:</b> demo\n" +
			                                     "<b>Пароль:</b> demo\n" +
			                                     "\n" +
			                                     "Для установки собственного сервера обратитесь к документации.");
			md.Run ();
			md.Destroy();
		}
	}
}

