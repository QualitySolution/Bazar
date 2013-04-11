using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace bazar
{
	class MainClass
	{
		public static MySqlConnection connectionDB;
		public static string ConnectionString;
		public static Label StatusBarLabel;
		public static UserInfo User = new UserInfo();
		public static MainWindow MainWin;
		public static void Main (string[] args)
		{
			try
			{
				Application.Init ();
				Login LoginDialog = new Login ();
				ResponseType LoginResult;
				LoginResult = (ResponseType) LoginDialog.Run();
				LoginDialog.Destroy ();
				if (LoginResult == ResponseType.DeleteEvent || LoginResult == ResponseType.Cancel)
					return;
				MainWin = new MainWindow ();
				if(User.Login == "root")
					return;
				MainWin.Show ();
			}
			catch (Exception ex)
			{
				MessageDialog md = new MessageDialog ( (Window) MainWin, DialogFlags.DestroyWithParent,
				                                      MessageType.Error, 
				                                      ButtonsType.Close,"ошибка");
				md.UseMarkup = false;
				md.Text = "Произошла ошибка запуска. Пожалуйста сообщите разработчику об ошибке.\nТехническая информация:\n" + ex.ToString();
				md.Run ();
				md.Destroy();
			}

			try
			{
				Application.Run ();
			}
			catch (Exception ex)
			{
				MessageDialog md = new MessageDialog ( (Window) MainWin, DialogFlags.DestroyWithParent,
			                              MessageType.Error, 
		                                  ButtonsType.Close,"ошибка");
				md.UseMarkup = false;
				md.Text = "Произошла критическая ошибка в программе. Пожалуйста сообщите разработчику об ошибке, " +
					"указав сообщение об ошибке и какие действия вызвали ошибку.\nТехническая информация:\n" + ex.ToString();
				md.Run ();
				md.Destroy();
			}
		}
		
		public static void ComboPlaceNoFill(ComboBox combo, int Type_id)
		{   //Заполняем комбобокс Номерами мест
			try
	        {
				MainClass.StatusMessage("Запрос номеров мест...");
				int count = 0;
				string sql = "SELECT place_no FROM places " +
					"WHERE type_id = @type_id";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
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
			try
			{
				MainClass.StatusMessage("Запрос договоров...");
				int count = 0;
				string sql = "SELECT number FROM contracts " +
					"WHERE lessee_id = @lessee_id ";
				if(OnlyCurrent)
					sql += "AND ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) ";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@lessee_id", Lessee_id);
				MySqlDataReader rdr = cmd.ExecuteReader();

				((ListStore)combo.Model).Clear();
				while (rdr.Read())
				{
					combo.AppendText(rdr["number"].ToString());
					count++;
				}
				rdr.Close();
				if (count == 1)
					combo.Active = 0;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения списка договоров!");
			}
			
		}

		public static void ComboContractFill(ComboBox combo, int Month, int Year)
		{   //Заполняем комбобокс активными договорами на определенный месяц
			try
			{
				MainClass.StatusMessage("Запрос договоров...");
				int count = 0;
				string sql = "SELECT number FROM contracts " +
					"WHERE !(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date) ";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				DateTime BeginOfMonth = new DateTime(Year, Month, 1);
				DateTime EndOfMonth = new DateTime(Year, Month, DateTime.DaysInMonth (Year,Month));
				cmd.Parameters.AddWithValue("@start", BeginOfMonth);
				cmd.Parameters.AddWithValue("@end", EndOfMonth);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				((ListStore)combo.Model).Clear();
				while (rdr.Read())
				{
					combo.AppendText(rdr["number"].ToString());
					count++;
				}
				rdr.Close();
				if (count == 1)
					combo.Active = 0;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения списка договоров!");
			}
			
		}

		public static void ComboAccrualYearsFill(ComboBox combo)
		{   //Заполняем комбобокс активными договорами на определенный месяц
			try
			{
				MainClass.StatusMessage("Запрос лет для начислений...");
				bool CurrentYear = false;
				bool NextYear = false;
				TreeIter iter;
				string sql = "SELECT DISTINCT year FROM accrual";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
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
				MainClass.SearchListStore ((ListStore)combo.Model, Convert.ToString (DateTime.Now.Year), out iter);
				combo.SetActiveIter(iter);
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения списка лет!");
			}
			
		}

		public static void ComboFillReference(ComboBox combo, string TableRef, int listmode)
		{   //Заполняем комбобокс элементами справочника
			// Режимы списка:
			// 0 - Только элементы справочника
			// 1 - Есть пункт "Все" с кодом 0
			// 2 - Есть пункт "Нет" с кодом -1
			
			combo.Clear ();
			CellRendererText text = new CellRendererText ();
            ListStore store = new ListStore (typeof (string), typeof (int));
			combo.Model = store;
            combo.PackStart (text, false);
            combo.AddAttribute (text, "text", 0);
			MainClass.StatusMessage("Запрос справочника " + TableRef + "...");
			try
	        {
				int count = 0;
				string sql = "SELECT id, name FROM " + TableRef;
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				switch (listmode) {
				case 1: //Все
					store.AppendValues("Все", 0);
					break;
				case 2: //Нет
					store.AppendValues("нет", -1);
					break;
				default:
				break;
				}
				
				while (rdr.Read())
				{
					store.AppendValues(rdr["name"].ToString(),int.Parse(rdr["id"].ToString()));
					count++;
	   			}
				rdr.Close();
				if(listmode == 2 && count == 1)
					combo.Active = 1;
				if(listmode == 0 && count == 1)
					combo.Active = 0;
				MainClass.StatusMessage("Ok");
	       	}
	       	catch (Exception ex)
	       	{
	           	Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения данных справочника!");
	       	}

		}

		public static void ComboFillUniversal(ComboBox combo, string SqlSelect, string DisplayString, MySqlParameter[] Parameters, int KeyField, int listmode)
		{   //Заполняем универсальный комбобокс
			// Режимы списка:
			// 0 - Только элементы
			// 1 - Есть пункт "Все" с кодом 0
			// 2 - Есть пункт "Нет" с кодом -1
			
			combo.Clear ();
			CellRendererText text = new CellRendererText ();
			ListStore store = new ListStore (typeof (string), typeof (int));
			combo.Model = store;
			combo.PackStart (text, false);
			combo.AddAttribute (text, "text", 0);
			MainClass.StatusMessage("Запрос элементов комбобокс...");
			try
			{
				int count = 0;
				MySqlCommand cmd = new MySqlCommand(SqlSelect, MainClass.connectionDB);
				cmd.Parameters.AddRange (Parameters);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				switch (listmode) {
				case 1: //Все
					store.AppendValues("Все", 0);
					break;
				case 2: //Нет
					store.AppendValues("нет", -1);
					break;
				default:
					break;
				}
				
				while (rdr.Read())
				{
					object[] Fields = new object[rdr.FieldCount];
					rdr.GetValues(Fields);
					store.AppendValues(String.Format(DisplayString, Fields),
					                   Convert.ToInt32(Fields[KeyField]));
					count++;
				}
				rdr.Close();
				if(listmode == 2 && count == 1)
					combo.Active = 1;
				if(listmode == 0 && count == 1)
					combo.Active = 0;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения данных для комбобокс!");
			}
			
		}

		public static void FillServiceListStore(out ListStore list)
		{   			
            list = new ListStore (typeof (int), typeof (string), typeof (int), typeof (string));
			MainClass.StatusMessage("Запрос справочника услуг...");
			try
	        {
				string sql = "SELECT services.*, units.name as units FROM services " +
					"LEFT JOIN units ON services.units_id = units.id";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
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
					                   units_name);
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
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
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

		public static bool SearchListStore( ListStore list, int id, out TreeIter iter)
		{
			return SearchListStore (list, id, 1, out iter);
		}

		public static bool SearchListStore( ListStore list, object searchvalue, int column, out TreeIter iter)
		{   // Перебираем список, ищем id, возвращаем iter
			if(list.GetIterFirst(out iter))
			{
				if( searchvalue.Equals (list.GetValue(iter, column)))
					return true;
			}
			else
				return false;
			while (list.IterNext(ref iter)) 
			{
				if( searchvalue.Equals (list.GetValue(iter, column)))
					return true;
			}
			return false;		
		}

		public static bool SearchListStore( ListStore list, string text, out TreeIter iter)
		{   // Перебираем список, ищем Строку, возвращаем iter
			if(list.GetIterFirst(out iter))
			{
				if( list.GetValue(iter,0).ToString() == text)
					return true;
			}
			else
				return false;
			while (list.IterNext(ref iter)) 
			{
				if(list.GetValue(iter,0).ToString() == text)
					return true;
			}
			return false;		
		}

		public static void ErrorMessage(Window parent, Exception ex)
		{
			MessageDialog md = new MessageDialog ( parent, DialogFlags.DestroyWithParent,
		                              MessageType.Error, 
	                                  ButtonsType.Close,"ошибка");
			md.UseMarkup = false;
			md.Text = ex.ToString();
			md.Run ();
			md.Destroy();
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

	class UserInfo
	{
		public string Name, Login;
		public int id;
		public bool admin, edit_slips;

		public bool TestUserExistByLogin(bool CreateNotExist)
		{
			MainClass.StatusMessage("Проверка наличия пользователя в базе...");
			try
			{
				string sql = "SELECT COUNT(*) AS cnt FROM users WHERE login = @login";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", Login);
				MySqlDataReader rdr = cmd.ExecuteReader();
				rdr.Read();
				bool Exist = false;
				if (rdr["cnt"].ToString() != "0")
					Exist = true;
				rdr.Close();

				if( CreateNotExist && !Exist)
				{
					bool FirstUser = false;
					sql = "SELECT COUNT(*) AS cnt FROM users";
					cmd = new MySqlCommand(sql, MainClass.connectionDB);
					rdr = cmd.ExecuteReader();
					rdr.Read();
					if (rdr["cnt"].ToString() == "0")
						FirstUser = true;
					rdr.Close();
					MainClass.StatusMessage("Создаем пользователя");
					sql = "INSERT INTO users (login, name, admin) " +
							"VALUES (@login, @login, @admin)";
					cmd = new MySqlCommand(sql, MainClass.connectionDB);
					cmd.Parameters.AddWithValue("@login", Login);
					cmd.Parameters.AddWithValue("@admin", FirstUser);
					cmd.ExecuteNonQuery();
					Exist = true;
				}
				return Exist;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка проверки пользователя!");
				return false;
			}
		}

		public void UpdateUserInfoByLogin()
		{
			MainClass.StatusMessage("Чтение информации о пользователе...");
			try
			{
				string sql = "SELECT * FROM users WHERE login = @login";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@login", Login);
				MySqlDataReader rdr = cmd.ExecuteReader();
				rdr.Read();

				Name = rdr["name"].ToString();
				id = Convert.ToInt32(rdr["id"].ToString());
				admin = Convert.ToBoolean (rdr["admin"].ToString());
				edit_slips = Convert.ToBoolean (rdr["edit_slips"].ToString());
				rdr.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка чтения информации о пользователе!");
			}

		}
	}
}

