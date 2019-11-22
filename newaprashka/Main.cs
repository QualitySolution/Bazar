using System;
using System.Collections.Generic;
using System.Threading;
using Gtk;
using MySql.Data.MySqlClient;
using NLog;
using QS.Updater.DB;
using QSProjectsLib;
using QSSupportLib;

namespace bazar
{
	class MainClass
	{
		private static Logger logger = LogManager.GetCurrentClassLogger ();
		public static MainWindow MainWin;

		public static void Main (string[] args)
		{
			try {
				WindowStartupFix.WindowsCheck ();
				Application.Init ();
				QSMain.SubscribeToUnhadledExceptions ();
				QSMain.GuiThread = Thread.CurrentThread;
				MainSupport.Init ();
			} catch (Exception falalEx) {
				if (WindowStartupFix.IsWindows)
					WindowStartupFix.DisplayWindowsOkMessage (falalEx.ToString (), "Критическая ошибка");
				else
					Console.WriteLine (falalEx);

				logger.Fatal (falalEx);
				return;
			}

			CreateProjectParam ();
			// Создаем окно входа
			Login LoginDialog = new QSProjectsLib.Login ();
			LoginDialog.Logo = Gdk.Pixbuf.LoadFromResource ("Bazar.icons.logo.png");
			LoginDialog.SetDefaultNames ("bazar");
			LoginDialog.DefaultLogin = "demo";
			LoginDialog.DefaultServer = "demo.qsolution.ru";
			LoginDialog.DefaultConnection = "Демонстрационная база";
			Login.ApplicationDemoServer = "demo.qsolution.ru";
			LoginDialog.DemoMessage = "Для подключения к демострационному серверу используйте следующие настройки:\n" +
			"\n" +
			"<b>Сервер:</b> demo.qsolution.ru\n" +
			"<b>Пользователь:</b> demo\n" +
			"<b>Пароль:</b> demo\n" +
			"\n" +
			"Для установки собственного сервера обратитесь к документации.";
			LoginDialog.UpdateFromGConf ();

			ResponseType LoginResult;
			LoginResult = (ResponseType)LoginDialog.Run ();
			if (LoginResult == ResponseType.DeleteEvent || LoginResult == ResponseType.Cancel)
				return;

			LoginDialog.Destroy ();
			//Проверка на предмет использования SaaS и запуск обновления сессии.
			QSSaaS.Session.StartSessionRefresh ();

			//Запускаем программу
			MainWin = new MainWindow ();
			if (QSMain.User.Login == "root")
				return;
			MainWin.Show ();
			Application.Run ();
			//Остановка таймера обновления сессии.
			QSSaaS.Session.StopSessionRefresh ();
		}

		static void CreateProjectParam ()
		{
			QSMain.ProjectPermission = new Dictionary<string, UserPermission> ();
			QSMain.ProjectPermission.Add ("edit_slips", new UserPermission ("edit_slips", "Изменение кассы задним числом",
			                                                                "Пользователь может изменять или добавлять кассовые документы задним числом."));
			//Скрипты создания базы
			DBCreator.AddBaseScript (
				new Version(2,3),
				"Чистая база",
				"bazar.SQLScripts.new-2.3.sql"
			);

			//Настраиваем обновления
			DBUpdater.AddUpdate (
				new Version (2, 2),
				new Version (2, 3),
				"bazar.SQLScripts.Update 2.2 to 2.3.sql");

			DBUpdater.AddMicroUpdate (
				new Version (2, 3),
				new Version (2, 3, 1),
				"bazar.SQLScripts.Update 2.3.1.sql");

			DBUpdater.AddMicroUpdate (
				new Version (2, 3, 1),
				new Version (2, 3, 4),
				"bazar.SQLScripts.2.3.4.sql");

			DBUpdater.AddUpdate (
				new Version (2, 3),
				new Version (2, 4),
				"bazar.SQLScripts.2.4.sql");

			//Параметры удаления
			Dictionary<string, TableInfo> Tables = new Dictionary<string, TableInfo> ();
			QSMain.ProjectTables = Tables;
			TableInfo PrepareTable;

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Места";
			PrepareTable.ObjectName = "место";
			PrepareTable.SqlSelect = "SELECT place_types.name as type, place_no, area , type_id FROM places " +
			"LEFT JOIN place_types ON places.type_id = place_types.id ";
			PrepareTable.DisplayString = "Место {0}-{1} с площадью {2} кв.м.";
			PrepareTable.PrimaryKey = new  TableInfo.PrimaryKeys ("type_id", "place_no");
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new TableInfo.DeleteDependenceItem ("WHERE contracts.place_type_id = @type_id AND contracts.place_no = @place_no", "@place_no", "@type_id"));
			PrepareTable.DeleteItems.Add ("meters", 
			                              new TableInfo.DeleteDependenceItem ("WHERE meters.place_type_id = @type_id AND meters.place_no = @place_no ", "@place_no", "@type_id"));
			PrepareTable.DeleteItems.Add ("events", 
			                              new TableInfo.DeleteDependenceItem ("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NULL", "@place_no", "@type_id"));
			PrepareTable.ClearItems.Add ("events", 
			                             new TableInfo.ClearDependenceItem ("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NOT NULL", "@place_no", "@type_id", "place_type_id", "place_no"));
			Tables.Add ("places", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Договора"; 
			PrepareTable.ObjectName = "договор"; 
			PrepareTable.SqlSelect = "SELECT number, sign_date, lessees.name as lessee, contracts.id as id FROM contracts " +
			"LEFT JOIN lessees ON lessees.id = lessee_id ";
			PrepareTable.DisplayString = "Договор №{0} от {1:d} с арендатором {2}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("accrual", 
			                              new TableInfo.DeleteDependenceItem ("WHERE contract_id = @contract_id ", "", "@contract_id"));
			PrepareTable.ClearItems.Add ("credit_slips", 
			                             new TableInfo.ClearDependenceItem ("WHERE contract_id = @contract_id ", "", "@contract_id", "contract_id"));
			Tables.Add ("contracts", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Арендаторы";
			PrepareTable.ObjectName = "арендатора"; 
			PrepareTable.SqlSelect = "SELECT name , id FROM lessees ";
			PrepareTable.DisplayString = "Арендатор {0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE lessee_id = @lessee_id ", "", "@lessee_id"));
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new TableInfo.DeleteDependenceItem ("WHERE lessee_id = @lessee_id ", "", "@lessee_id"));
			PrepareTable.DeleteItems.Add ("events", 
			                              new TableInfo.DeleteDependenceItem ("WHERE lessee_id = @lessee_id AND (place_type_id IS NULL OR place_no IS NULL) ", "", "@lessee_id"));
			PrepareTable.ClearItems.Add ("events", 
			                             new TableInfo.ClearDependenceItem ("WHERE lessee_id = @lessee_id AND place_type_id IS NOT NULL AND place_no IS NOT NULL", "", "@lessee_id", "lessee_id"));
			Tables.Add ("lessees", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "События";
			PrepareTable.ObjectName = "событие"; 
			PrepareTable.SqlSelect = "SELECT classes.name as class, date, place_types.name as type, place_no, lessees.name as lessee, events.id as id FROM events " +
			"LEFT JOIN place_types ON events.place_type_id = place_types.id " +
			"LEFT JOIN lessees ON events.lessee_id = lessees.id " +
			"LEFT JOIN classes ON events.class_id = classes.id ";
			PrepareTable.DisplayString = "{0} в {1} на месте {2}-{3} c арендатором {4}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("events", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Контактные лица";
			PrepareTable.ObjectName = "контактное лицо"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM contact_persons ";
			PrepareTable.DisplayString = "Контакт {0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("places", 
			                             new TableInfo.ClearDependenceItem ("WHERE contact_person_id = @id", "", "@id", "contact_person_id"));
			Tables.Add ("contact_persons", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Начисления";
			PrepareTable.ObjectName = "начисление"; 
			PrepareTable.SqlSelect = "SELECT DATE(CONCAT('2012-', month, '-1')) as month, year, lessees.name as lessee, contracts.number, accrual.id as id FROM accrual " +
			"LEFT JOIN contracts ON accrual.contract_id = contracts.id " +
			"LEFT JOIN lessees ON contracts.lessee_id = lessees.id ";
			PrepareTable.DisplayString = "Начисление за {0:MMMM} {1} арендатору {2} по договору {3}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE accrual_id = @id AND operation = 'payment'", "", "@id"));
			PrepareTable.ClearItems.Add ("credit_slips", 
			                             new TableInfo.ClearDependenceItem ("WHERE accrual_id = @id AND operation <> 'payment'", "", "@id", "accrual_id"));
			Tables.Add ("accrual", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Оплаты";
			PrepareTable.ObjectName = "оплату"; 
			PrepareTable.SqlSelect = "SELECT payments.createdate, accrual_id, payments.id FROM payments ";
			PrepareTable.DisplayString = "Оплата от {0:d} по начислению № {1}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("payments", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Авансовые отчеты";
			PrepareTable.ObjectName = "авансовый отчет"; 
			PrepareTable.SqlSelect = "SELECT advance.id as id, date, employees.name as employee FROM advance " +
			"LEFT JOIN employees ON employees.id = employee_id ";
			PrepareTable.DisplayString = "Авансовый отчет №{0} от {1:d} на {2}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("advance", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Кассы";
			PrepareTable.ObjectName = "кассу"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM cash ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new TableInfo.DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("accrual_pays", 
			                              new TableInfo.DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contract_pays", 
			                              new TableInfo.DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			Tables.Add ("cash", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Строки начислений";
			PrepareTable.ObjectName = "строку начисления"; 
			PrepareTable.SqlSelect = "SELECT accrual_id, services.name as service, (count * price) as sum, accrual_pays.id as id FROM accrual_pays " +
			"LEFT JOIN services ON service_id = services.id "; 
			PrepareTable.DisplayString = "Строка в начислении {0} услуги {1} на сумму {2:C}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("meter_reading", 
			                             new TableInfo.ClearDependenceItem ("WHERE accrual_pay_id = @id", "", "@id", "accrual_pay_id"));
			PrepareTable.DeleteItems.Add ("payment_details", 
			                              new TableInfo.DeleteDependenceItem ("WHERE accrual_pay_id = @id ", "", "@id"));
			Tables.Add ("accrual_pays", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Строки оплаты";
			PrepareTable.ObjectName = "строку оплаты"; 
			PrepareTable.SqlSelect = "SELECT payment_id, services.name as service, sum, payment_details.id as id FROM payment_details " +
			"LEFT JOIN accrual_pays ON accrual_pays.id = payment_details.accrual_pay_id " +
			"LEFT JOIN services ON accrual_pays.service_id = services.id "; 
			PrepareTable.DisplayString = "Строка оплаты услуги {1} на сумму {2:C} в платеже {0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("payment_details", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Строки услуг в договоре";
			PrepareTable.ObjectName = "строку услуги в договоре"; 
			PrepareTable.SqlSelect = "SELECT services.name as service, contracts.number, (count * price) as sum, contract_pays.id as id FROM contract_pays " +
			"LEFT JOIN services ON service_id = services.id " +
			"LEFT JOIN contracts ON contracts.id = contract_pays.contract_id "; 
			PrepareTable.DisplayString = "Строка услуги {0} в договоре {1} на сумму {2:C}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("contract_pays", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Типы событий";
			PrepareTable.ObjectName = "тип события"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM classes ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("events", 
			                              new TableInfo.DeleteDependenceItem ("WHERE class_id = @id ", "", "@id"));
			Tables.Add ("classes", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Контрагенты";
			PrepareTable.ObjectName = "контрагента"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM contractors ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("debit_slips", 
			                             new TableInfo.ClearDependenceItem ("WHERE contractor_id = @id", "", "@id", "contractor_id"));
			PrepareTable.ClearItems.Add ("advance", 
			                             new TableInfo.ClearDependenceItem ("WHERE contractor_id = @id", "", "@id", "contractor_id"));
			Tables.Add ("contractors", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Приходные ордера";
			PrepareTable.ObjectName = "приходный ордер"; 
			PrepareTable.SqlSelect = "SELECT id, date, sum FROM credit_slips ";
			PrepareTable.DisplayString = "Приходный ордер №{0} от {1:d} на сумму {2:C}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("payments", 
			                              new TableInfo.DeleteDependenceItem ("WHERE credit_slip_id = @id ", "", "@id"));
			Tables.Add ("credit_slips", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Расходные ордера";
			PrepareTable.ObjectName = "расходный ордер"; 
			PrepareTable.SqlSelect = "SELECT id, date, sum FROM debit_slips ";
			PrepareTable.DisplayString = "Расходный ордер №{0} от {1:d} на сумму {2:C}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("debit_slips", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Сотрудники";
			PrepareTable.ObjectName = "сотрудника"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM employees ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new TableInfo.DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			Tables.Add ("employees", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Статьи расходов";
			PrepareTable.ObjectName = "статью расходов"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM expense_items ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new TableInfo.DeleteDependenceItem ("WHERE expense_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE expense_id = @id ", "", "@id"));
			Tables.Add ("expense_items", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Группы товаров";
			PrepareTable.ObjectName = "группу товаров"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM goods ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("lessees", 
			                             new TableInfo.ClearDependenceItem ("WHERE goods_id = @id", "", "@id", "goods_id"));
			Tables.Add ("goods", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Статьи доходов";
			PrepareTable.ObjectName = "статью доходов"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM income_items ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE income_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("services", 
			                             new TableInfo.ClearDependenceItem ("WHERE income_id = @id", "", "@id", "income_id"));
			Tables.Add ("income_items", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Организации";
			PrepareTable.ObjectName = "организацию"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM organizations ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new TableInfo.DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new TableInfo.DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new TableInfo.DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("places", 
			                             new TableInfo.ClearDependenceItem ("WHERE org_id = @id", "", "@id", "org_id"));
			Tables.Add ("organizations", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Типы мест";
			PrepareTable.ObjectName = "тип места"; 
			PrepareTable.SqlSelect = "SELECT name, description, id FROM place_types ";
			PrepareTable.DisplayString = "{0} - {1}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("places", 
			                              new TableInfo.DeleteDependenceItem ("WHERE type_id = @id ", "", "@id"));
			Tables.Add ("place_types", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Услуги";
			PrepareTable.ObjectName = "услугу"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM services ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("accrual_pays", 
			                              new TableInfo.DeleteDependenceItem ("WHERE service_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contract_pays", 
			                              new TableInfo.DeleteDependenceItem ("WHERE service_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("meter_tariffs", 
			                             new TableInfo.ClearDependenceItem ("WHERE service_id = @id", "", "@id", "service_id"));
			Tables.Add ("services", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Единицы измерения";
			PrepareTable.ObjectName = "единицу измерения"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM units ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("services", 
			                              new TableInfo.DeleteDependenceItem ("WHERE units_id = @id ", "", "@id"));
			Tables.Add ("units", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Показания счётчика";
			PrepareTable.ObjectName = "показание счётчика"; 
			PrepareTable.SqlSelect = "SELECT date, value, id FROM meter_reading ";
			PrepareTable.DisplayString = "Показания {1} на {0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			Tables.Add ("meter_reading", PrepareTable); 

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Тарифы счётчиков";
			PrepareTable.ObjectName = "тариф счётчика"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM meter_tariffs ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("meter_reading", 
			                              new TableInfo.DeleteDependenceItem ("WHERE meter_tariff_id = @id ", "", "@id"));
			Tables.Add ("meter_tariffs", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Типы счётчиков";
			PrepareTable.ObjectName = "тип счётчика"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM meter_types ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("meter_tariffs", 
			                              new TableInfo.DeleteDependenceItem ("WHERE meter_type_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("meters", 
			                              new TableInfo.DeleteDependenceItem ("WHERE meters.meter_type_id = @id ", "", "@id"));
			Tables.Add ("meter_types", PrepareTable);


			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Поставщики услуг";
			PrepareTable.ObjectName = "поставщика"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM service_providers ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("services", 
			                              new TableInfo.ClearDependenceItem ("WHERE service_provider_id = @id ", "", "@id","service_provider_id"));
			Tables.Add ("service_providers", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Cчётчики";
			PrepareTable.ObjectName = "счётчик"; 
			PrepareTable.SqlSelect = "SELECT meters.name, meter_types.name as type, meters.id FROM meters " +
			"LEFT JOIN meter_types ON meters.meter_type_id = meter_types.id ";
			PrepareTable.DisplayString = "{0} ({1})";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.DeleteItems.Add ("meter_reading", 
			                              new TableInfo.DeleteDependenceItem ("WHERE meter_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("meters", 
			                             new TableInfo.ClearDependenceItem ("WHERE meters.parent_meter_id = @id", "", "@id", "parent_meter_id"));
			Tables.Add ("meters", PrepareTable);

			PrepareTable = new TableInfo ();
			PrepareTable.ObjectsName = "Пользователи";
			PrepareTable.ObjectName = "пользователя"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM users ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new TableInfo.PrimaryKeys ("id");
			PrepareTable.ClearItems.Add ("advance", 
			                             new TableInfo.ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("debit_slips", 
			                             new TableInfo.ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("accrual", 
			                             new TableInfo.ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("credit_slips", 
			                             new TableInfo.ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			Tables.Add ("users", PrepareTable);

		}

		public static void ComboPlaceNoFill (ComboBox combo, int Type_id)
		{   //Заполняем комбобокс Номерами мест
			try {
				logger.Info ("Запрос номеров мест...");
				int count = 0;
				string sql = "SELECT place_no FROM places " +
				             "WHERE type_id = @type_id";
				((ListStore)combo.Model).Clear ();
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue ("@type_id", Type_id);
				MySqlDataReader rdr = cmd.ExecuteReader ();

				while (rdr.Read ()) {
					combo.AppendText (rdr ["place_no"].ToString ());
					count++;
				}
				rdr.Close ();
				if (count == 1)
					combo.Active = 0;
				logger.Info ("Ok");
			} catch (Exception ex) {
				logger.Warn (ex, "Ошибка получения номеров мест!", ex);
			}
		}

		public static void ComboContractFill (ComboBox combo, int Lessee_id, bool OnlyCurrent)
		{   //Заполняем комбобокс текущими договорами по арендатору
			string sql = "SELECT id, number, sign_date FROM contracts " +
			             "WHERE lessee_id = @lessee_id ";
			if (OnlyCurrent)
				sql += "AND ((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
				"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date)) ";
			MySqlParameter[] Param = { new MySqlParameter ("@lessee_id", Lessee_id) };
			string Display = "{1} от {2:d}";
			ComboWorks.ComboFillUniversal (combo, sql, Display, Param, 0, ComboWorks.ListMode.OnlyItems);
		}

		public static void ComboContractFill (ComboBox combo, int Month, int Year)
		{   //Заполняем комбобокс активными договорами на определенный месяц
			string sql = "SELECT id, number, sign_date FROM contracts " +
			             "WHERE !(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date) ";
			DateTime BeginOfMonth = new DateTime (Year, Month, 1);
			DateTime EndOfMonth = new DateTime (Year, Month, DateTime.DaysInMonth (Year, Month));
			MySqlParameter[] Param = { new MySqlParameter ("@start", BeginOfMonth),
				new MySqlParameter ("@end", EndOfMonth)
			};
			string Display = "{1} от {2:d}";
			ComboWorks.ComboFillUniversal (combo, sql, Display, Param, 0, ComboWorks.ListMode.OnlyItems);
		}

		public static void ComboAccrualYearsFill (ComboBox combo, params string[] firstItems)
		{   
			try {
				logger.Info ("Запрос лет для начислений...");
				TreeIter iter;
				string sql = "SELECT DISTINCT year FROM accrual ORDER BY year DESC";
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader ();
				
				((ListStore)combo.Model).Clear ();

				foreach(var item in firstItems)
				{
					combo.AppendText (item);
				}
					
				combo.AppendText (Convert.ToString (DateTime.Now.AddYears (1).Year));
				combo.AppendText (Convert.ToString (DateTime.Now.Year));
				while (rdr.Read ()) {
					if (rdr.GetUInt32 ("year") == DateTime.Now.Year || rdr.GetUInt32 ("year") == DateTime.Now.AddYears (1).Year)
						continue;
					combo.AppendText (rdr ["year"].ToString ());
				}
				rdr.Close ();
				((ListStore)combo.Model).SetSortColumnId (0, SortType.Descending);
				ListStoreWorks.SearchListStore ((ListStore)combo.Model, Convert.ToString (DateTime.Now.Year), out iter);
				combo.SetActiveIter (iter);
				logger.Info ("Ok");
			} catch (Exception ex) {
				logger.Warn (ex, "Ошибка получения списка лет!");
			}
			
		}

		public static void FillServiceListStore (out ListStore list)
		{   			
			list = new ListStore (typeof(int), typeof(string), typeof(int), typeof(string), typeof(bool));
			logger.Info ("Запрос справочника услуг...");
			try {
				string sql = "SELECT services.*, units.name as units FROM services " +
				             "LEFT JOIN units ON services.units_id = units.id";
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader ();
				int units_id;
				string units_name;

				while (rdr.Read ()) {
					if (rdr ["units_id"] != DBNull.Value) {
						units_id = int.Parse (rdr ["units_id"].ToString ());
						units_name = rdr ["units"].ToString ();
					} else {
						units_id = -1;
						units_name = "";
					}
					list.AppendValues (int.Parse (rdr ["id"].ToString ()),
					                   rdr ["name"].ToString (),
					                   units_id,
					                   units_name,
					                   rdr.GetBoolean ("by_area"));
				}
				rdr.Close ();
				logger.Info ("Ok");
			} catch (Exception ex) {
				logger.Warn (ex, "Ошибка получения данных справочника!");
			}

		}

		public static void ComboFillUsers (ComboBox combo, string TableDB)
		{   //Заполняем комбобокс пользователями
			
			logger.Info ("Поиск пользователей в " + TableDB + "...");
			try {
				string sql = "SELECT user FROM " + TableDB + " GROUP BY user";
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);
				MySqlDataReader rdr = cmd.ExecuteReader ();
		
				combo.AppendText ("Все");
				string tempstr;
				
				while (rdr.Read ()) {
					tempstr = rdr ["user"].ToString ();
					if (tempstr != "")
						combo.AppendText (tempstr);
				}
				rdr.Close ();
				logger.Info ("Ok");
			} catch (Exception ex) {
				logger.Warn (ex, "Ошибка обработки " + TableDB + "!");
			}

		}
	}
}

