using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using QSProjectsLib;

namespace bazar
{
	class MainClass
	{
		public static Label StatusBarLabel;
		public static MainWindow MainWin;

		public static void Main (string[] args)
		{
			Application.Init ();
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e) 
			{
				QSMain.ErrorMessage(MainWin, (Exception) e.ExceptionObject);
			};
			CreateProjectParam();
			//Настраиваем общую билиотеку
			QSMain.NewStatusText += delegate(object sender, QSProjectsLib.QSMain.NewStatusTextEventArgs e) 
			{
				StatusMessage (e.NewText);
			};
			// Создаем окно входа
			Login LoginDialog = new QSProjectsLib.Login ();
			LoginDialog.Logo = Gdk.Pixbuf.LoadFromResource ("bazar.icons.logo.png");
			LoginDialog.SetDefaultNames ("bazar");
			LoginDialog.DefaultLogin = "demo";
			LoginDialog.DefaultServer = "demo.qsolution.ru";
			LoginDialog.DemoServer = "demo.qsolution.ru";
			LoginDialog.DemoMessage = "Для подключения к демострационному серверу используйте следующие настройки:\n" +
								"\n" +
								"<b>Сервер:</b> demo.qsolution.ru\n" +
								"<b>Пользователь:</b> demo\n" +
								"<b>Пароль:</b> demo\n" +
								"\n" +
								"Для установки собственного сервера обратитесь к документации.";
			LoginDialog.UpdateFromGConf ();

			ResponseType LoginResult;
			LoginResult = (ResponseType) LoginDialog.Run();
			if (LoginResult == ResponseType.DeleteEvent || LoginResult == ResponseType.Cancel)
				return;

			LoginDialog.Destroy ();

			//Запускаем программу
			MainWin = new MainWindow ();
			if(QSMain.User.Login == "root")
				return;
			MainWin.Show ();
			Application.Run ();
		}

		static void CreateProjectParam()
		{
			QSMain.AdminFieldName = "admin";
			QSMain.ProjectPermission = new Dictionary<string, UserPermission>();
			QSMain.ProjectPermission.Add ("edit_slips", new UserPermission("edit_slips", "Изменение кассы задним числом",
			                                                             "Пользователь может изменять или добавлять кассовые документы задним числом."));

			QSMain.User = new UserInfo();
		}
		
		public static void ComboPlaceNoFill(ComboBox combo, int Type_id)
		{   //Заполняем комбобокс Номерами мест
			try
	        {
				MainClass.StatusMessage("Запрос номеров мест...");
				int count = 0;
				string sql = "SELECT place_no FROM places " +
					"WHERE type_id = @type_id";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@type_id", Type_id);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				while (rdr.Read())
				{
					combo.AppendText(rdr["place_no"].ToString());
					count++;
	   			}
				rdr.Close();
				if(count == 1)
					combo.Active = 0;

				MainClass.StatusMessage("Ok");
	       	}
	       	catch (Exception ex)
	       	{
	           	Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения номеров мест!");
	       	}
		}

		public static void ComboContractFill(ComboBox combo, int Lessee_id, bool OnlyCurrent)
		{   //Заполняем комбобокс текущими договорами по арендатору
			string sql = "SELECT id, number, sign_date FROM contracts " +
				"WHERE lessee_id = @lessee_id ";
			if(OnlyCurrent)
				sql += "AND ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) ";
			MySqlParameter[] Param = { new MySqlParameter("@lessee_id", Lessee_id) };
			string Display = "{1} от {2:d}";
			ComboWorks.ComboFillUniversal (combo, sql, Display, Param, 0, 0);
		}

		public static void ComboContractFill(ComboBox combo, int Month, int Year)
		{   //Заполняем комбобокс активными договорами на определенный месяц
			string sql = "SELECT id, number, sign_date FROM contracts " +
				"WHERE !(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date) ";
			DateTime BeginOfMonth = new DateTime(Year, Month, 1);
			DateTime EndOfMonth = new DateTime(Year, Month, DateTime.DaysInMonth (Year,Month));
			MySqlParameter[] Param = { new MySqlParameter("@start", BeginOfMonth),
										new MySqlParameter("@end", EndOfMonth) };
			string Display = "{1} от {2:d}";
			ComboWorks.ComboFillUniversal (combo, sql, Display, Param, 0, 0);
		}

		public static void ComboAccrualYearsFill(ComboBox combo)
		{   
			try
			{
				MainClass.StatusMessage("Запрос лет для начислений...");
				bool CurrentYear = false;
				bool NextYear = false;
				TreeIter iter;
				string sql = "SELECT DISTINCT year FROM accrual";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				((ListStore)combo.Model).Clear();
				while (rdr.Read())
				{
					if(rdr.GetUInt32 ("year") == DateTime.Now.Year)
						CurrentYear = true;
					if(rdr.GetUInt32 ("year") == DateTime.Now.AddYears (1).Year)
						NextYear = true;
					combo.AppendText(rdr["year"].ToString());
				}
				rdr.Close();
				if(!CurrentYear)
					combo.AppendText(Convert.ToString (DateTime.Today.Year));
				if(!NextYear)
					combo.AppendText(Convert.ToString (DateTime.Today.AddYears(1).Year));
				((ListStore)combo.Model).SetSortColumnId ( 0, SortType.Ascending);
				ListStoreWorks.SearchListStore ((ListStore)combo.Model, Convert.ToString (DateTime.Now.Year), out iter);
				combo.SetActiveIter(iter);
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения списка лет!");
			}
			
		}

		public static void FillServiceListStore(out ListStore list)
		{   			
            list = new ListStore (typeof (int), typeof (string), typeof (int), typeof (string), typeof (bool));
			MainClass.StatusMessage("Запрос справочника услуг...");
			try
	        {
				string sql = "SELECT services.*, units.name as units FROM services " +
					"LEFT JOIN units ON services.units_id = units.id";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader();
				int units_id;
				string units_name;

				while (rdr.Read())
				{
					if(rdr["units_id"] != DBNull.Value)
					{
						units_id = int.Parse(rdr["units_id"].ToString());
						units_name = rdr["units"].ToString();
					}
					else
					{
						units_id = -1;
						units_name = "";
					}
					list.AppendValues(int.Parse(rdr["id"].ToString()),
					                   rdr["name"].ToString(),
					                   units_id,
					                   units_name,
					                   rdr.GetBoolean("by_area"));
	   			}
				rdr.Close();
				MainClass.StatusMessage("Ok");
	       	}
	       	catch (Exception ex)
	       	{
	           	Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения данных справочника!");
	       	}

		}

		public static void ComboFillUsers(ComboBox combo, string TableDB)
		{   //Заполняем комбобокс пользователями
			
			MainClass.StatusMessage("Поиск пользователей в " + TableDB + "...");
			try
	        {
				string sql = "SELECT user FROM " + TableDB + " GROUP BY user" ;
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader();
		
				combo.AppendText("Все");
				string tempstr;
				
				while (rdr.Read())
				{
					tempstr = rdr["user"].ToString();
					if(tempstr != "")
						combo.AppendText(tempstr);
	   			}
				rdr.Close();
				MainClass.StatusMessage("Ok");
	       	}
	       	catch (Exception ex)
	       	{
	           	Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка обработки " + TableDB + "!");
	       	}

		}

		public static void StatusMessage(string message)
		{
			StatusBarLabel.Text = message;
			Console.WriteLine (message);
			while (GLib.MainContext.Pending())
			{
   				Gtk.Main.Iteration();
			}
		}
	}
}

