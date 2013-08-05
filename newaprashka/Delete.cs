using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using QSProjectsLib;

namespace bazar
{
	public partial class Delete : Gtk.Dialog
	{
		bool ErrorHappens = false;
		string ErrorString;
		
		TreeStore ObjectsTreeStore;
		private Dictionary<string,TableInfo> Tables;

		private class DeleteDependenceItem
		{
			public string sqlwhere;
			public PrimaryKeys SqlParam;

			public DeleteDependenceItem(string sqlwhere , string StrParamName, string IntParamName)
			{
				this.sqlwhere = sqlwhere;
				SqlParam = new PrimaryKeys(StrParamName,IntParamName);
			}
		}

		private class ClearDependenceItem
		{
			public string[] ClearFields;
			public string sqlwhere;
			public PrimaryKeys SqlParam;

			public ClearDependenceItem(string sqlwhere , string StrParamName, string IntParamName, string ClearField1, string ClearField2)
			{
				this.sqlwhere = sqlwhere;
				SqlParam = new PrimaryKeys(StrParamName,IntParamName);
				ClearFields = new string[]{ClearField1, ClearField2};
			}
			public ClearDependenceItem(string sqlwhere, string StrParamName, string IntParamName, string ClearField1)
			{
				this.sqlwhere = sqlwhere;
				SqlParam = new PrimaryKeys(StrParamName,IntParamName);
				ClearFields = new string[]{ClearField1};
			}
		}

		private class Params
		{
			public string ParamStr;
			public int ParamInt;

			public Params(int IntParameter, string StrParameter)
			{
				ParamInt = IntParameter;
				ParamStr = StrParameter;
			}
			public Params(){}
		}

		private class PrimaryKeys
		{
			public string ParamStr, ParamInt;

			public PrimaryKeys (string Str, string Int)
			{
				ParamStr = Str;
				ParamInt = Int;
			}
		}

		private class TableInfo
		{
			public string ObjectsName;
			public string ObjectName;
			public string SqlSelect;
			public string DisplayString;
			public PrimaryKeys PrimaryKey;
			public Dictionary<string, DeleteDependenceItem> DeleteItems;
			public Dictionary<string, ClearDependenceItem> ClearItems;

			public TableInfo()
			{
				DeleteItems = new Dictionary<string, DeleteDependenceItem>();
				ClearItems = new Dictionary<string, ClearDependenceItem>();
			}
		}

		public Delete ()
		{
			this.Build ();

			ObjectsTreeStore = new TreeStore(typeof(string), typeof(string));
			treeviewObjects.AppendColumn("Объект", new Gtk.CellRendererText (), "text", 0);

			treeviewObjects.Model = ObjectsTreeStore;
			treeviewObjects.ShowAll ();

			Tables = new Dictionary<string, TableInfo>();
			TableInfo PrepareTable;

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Места";
			PrepareTable.ObjectName = "место";
			PrepareTable.SqlSelect = "SELECT place_types.name as type, place_no, area , type_id FROM places " +
				"LEFT JOIN place_types ON places.type_id = place_types.id ";
			PrepareTable.DisplayString = "Место {0}-{1} с площадью {2} кв.м.";
			PrepareTable.PrimaryKey = new PrimaryKeys("place_no","type_id");
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new DeleteDependenceItem ("WHERE contracts.place_type_id = @type_id AND contracts.place_no = @place_no", "@place_no", "@type_id"));
			PrepareTable.DeleteItems.Add ("events", 
			                              new DeleteDependenceItem ("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NULL", "@place_no", "@type_id"));
			PrepareTable.ClearItems.Add ("events", 
			                             new ClearDependenceItem ("WHERE place_type_id = @type_id AND place_no = @place_no AND lessee_id IS NOT NULL", "@place_no", "@type_id", "place_type_id", "place_no"));
			Tables.Add ("places", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Договора"; 
			PrepareTable.ObjectName = "договор"; 
			PrepareTable.SqlSelect = "SELECT number, sign_date, lessees.name as lessee FROM contracts " +
				"LEFT JOIN lessees ON lessees.id = lessee_id ";
			PrepareTable.DisplayString = "Договор №{0} от {1:d} с арендатором {2}";
			PrepareTable.PrimaryKey = new PrimaryKeys("number","");
			PrepareTable.DeleteItems.Add ("accrual", 
			                              new DeleteDependenceItem ("WHERE contract_no = @contract_no ", "@contract_no", ""));
			PrepareTable.ClearItems.Add ("credit_slips", 
			                              new ClearDependenceItem ("WHERE contract_no = @contract_no ", "@contract_no", "", "contract_no"));
			Tables.Add ("contracts", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Арендаторы";
			PrepareTable.ObjectName = "арендатора"; 
			PrepareTable.SqlSelect = "SELECT name , id FROM lessees ";
			PrepareTable.DisplayString = "Арендатор {0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new DeleteDependenceItem ("WHERE lessee_id = @lessee_id ", "", "@lessee_id"));
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new DeleteDependenceItem ("WHERE lessee_id = @lessee_id ", "", "@lessee_id"));
			PrepareTable.DeleteItems.Add ("events", 
			                              new DeleteDependenceItem ("WHERE lessee_id = @lessee_id AND (place_type_id IS NULL OR place_no IS NULL) ", "", "@lessee_id"));
			PrepareTable.ClearItems.Add ("events", 
			                             new ClearDependenceItem ("WHERE lessee_id = @lessee_id AND place_type_id IS NOT NULL AND place_no IS NOT NULL", "", "@lessee_id", "lessee_id"));
			Tables.Add ("lessees", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "События";
			PrepareTable.ObjectName = "событие"; 
			PrepareTable.SqlSelect = "SELECT classes.name as class, date, place_types.name as type, place_no, lessees.name as lessee, events.id as id FROM events "+
				"LEFT JOIN place_types ON events.place_type_id = place_types.id " +
					"LEFT JOIN lessees ON events.lessee_id = lessees.id " +
					"LEFT JOIN classes ON events.class_id = classes.id ";
			PrepareTable.DisplayString = "{0} в {1} на месте {2}-{3} c арендатором {4}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("events", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Контактные лица";
			PrepareTable.ObjectName = "контактное лицо"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM contact_persons ";
			PrepareTable.DisplayString = "Контакт {0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.ClearItems.Add ("places", 
			                             new ClearDependenceItem ("WHERE contact_person_id = @id", "", "@id", "contact_person_id"));
			Tables.Add ("contact_persons", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Начисления";
			PrepareTable.ObjectName = "начисление"; 
			PrepareTable.SqlSelect = "SELECT DATE(CONCAT('2012-', month, '-1')) as month, year, lessees.name as lessee, contract_no, accrual.id as id FROM accrual " +
				"LEFT JOIN contracts ON accrual.contract_no = contracts.number " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id ";
			PrepareTable.DisplayString = "Начисление за {0:MMMM} {1} арендатору {2} по договору {3}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.ClearItems.Add ("credit_slips", 
			                             new ClearDependenceItem ("WHERE accrual_id = @id", "", "@id", "accrual_id"));
			Tables.Add ("accrual", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Авансовые отчеты";
			PrepareTable.ObjectName = "авансовый отчет"; 
			PrepareTable.SqlSelect = "SELECT advance.id as id, date, employees.name as employee FROM advance " +
				"LEFT JOIN employees ON employees.id = employee_id ";
			PrepareTable.DisplayString = "Авансовый отчет №{0} от {1:d} на {2}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("advance", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Кассы";
			PrepareTable.ObjectName = "кассу"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM cash ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("accrual_pays", 
			                              new DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contract_pays", 
			                              new DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new DeleteDependenceItem ("WHERE cash_id = @id ", "", "@id"));
			Tables.Add ("cash", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Строки начислений";
			PrepareTable.ObjectName = "строку начисления"; 
			PrepareTable.SqlSelect = "SELECT accrual_id, services.name as service, (count * price) as sum, accrual_pays.id as id FROM accrual_pays " +
				"LEFT JOIN services ON service_id = services.id "; 
			PrepareTable.DisplayString = "Строка в начислении {0} услуги {1} на сумму {2:C}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("accrual_pays", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Строки услуг в договоре";
			PrepareTable.ObjectName = "строку услуги в договоре"; 
			PrepareTable.SqlSelect = "SELECT services.name as service, contract_no, (count * price) as sum, contract_pays.id as id FROM contract_pays " +
				"LEFT JOIN services ON service_id = services.id "; 
			PrepareTable.DisplayString = "Строка услуги {0} в договоре {1} на сумму {2:C}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("contract_pays", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Типы событий";
			PrepareTable.ObjectName = "тип события"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM classes ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("events", 
			                              new DeleteDependenceItem ("WHERE class_id = @id ", "", "@id"));
			Tables.Add ("classes", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Контрагенты";
			PrepareTable.ObjectName = "контрагента"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM contractors ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.ClearItems.Add ("debit_slips", 
			                             new ClearDependenceItem ("WHERE contractor_id = @id", "", "@id", "contractor_id"));
			PrepareTable.ClearItems.Add ("advance", 
			                             new ClearDependenceItem ("WHERE contractor_id = @id", "", "@id", "contractor_id"));
			Tables.Add ("contractors", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Приходные ордера";
			PrepareTable.ObjectName = "приходный ордер"; 
			PrepareTable.SqlSelect = "SELECT id, date, sum FROM credit_slips ";
			PrepareTable.DisplayString = "Приходный ордер №{0} от {1:d} на сумму {2:C}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("credit_slips", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Расходные ордера";
			PrepareTable.ObjectName = "расходный ордер"; 
			PrepareTable.SqlSelect = "SELECT id, date, sum FROM debit_slips ";
			PrepareTable.DisplayString = "Расходный ордер №{0} от {1:d} на сумму {2:C}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			Tables.Add ("debit_slips", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Сотрудники";
			PrepareTable.ObjectName = "сотрудника"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM employees ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new DeleteDependenceItem ("WHERE employee_id = @id ", "", "@id"));
			Tables.Add ("employees", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Статьи расходов";
			PrepareTable.ObjectName = "статью расходов"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM expense_items ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new DeleteDependenceItem ("WHERE expense_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new DeleteDependenceItem ("WHERE expense_id = @id ", "", "@id"));
			Tables.Add ("expense_items", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Группы товаров";
			PrepareTable.ObjectName = "группу товаров"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM goods ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.ClearItems.Add ("lessees", 
			                             new ClearDependenceItem ("WHERE goods_id = @id", "", "@id", "goods_id"));
			Tables.Add ("goods", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Статьи доходов";
			PrepareTable.ObjectName = "статью доходов"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM income_items ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new DeleteDependenceItem ("WHERE income_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("services", 
			                             new ClearDependenceItem ("WHERE income_id = @id", "", "@id", "income_id"));
			Tables.Add ("income_items", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Организации";
			PrepareTable.ObjectName = "организацию"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM organizations ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("advance", 
			                              new DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("debit_slips", 
			                              new DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contracts", 
			                              new DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("credit_slips", 
			                              new DeleteDependenceItem ("WHERE org_id = @id ", "", "@id"));
			PrepareTable.ClearItems.Add ("places", 
			                             new ClearDependenceItem ("WHERE org_id = @id", "", "@id", "org_id"));
			Tables.Add ("organizations", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Типы мест";
			PrepareTable.ObjectName = "тип места"; 
			PrepareTable.SqlSelect = "SELECT name, description, id FROM place_types ";
			PrepareTable.DisplayString = "{0} - {1}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("places", 
			                              new DeleteDependenceItem ("WHERE type_id = @id ", "", "@id"));
			Tables.Add ("place_types", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Услуги";
			PrepareTable.ObjectName = "услугу"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM services ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("accrual_pays", 
			                              new DeleteDependenceItem ("WHERE service_id = @id ", "", "@id"));
			PrepareTable.DeleteItems.Add ("contract_pays", 
			                              new DeleteDependenceItem ("WHERE service_id = @id ", "", "@id"));
			Tables.Add ("services", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Единицы измерения";
			PrepareTable.ObjectName = "единицу измерения"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM units ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.DeleteItems.Add ("services", 
			                              new DeleteDependenceItem ("WHERE units_id = @id ", "", "@id"));
			Tables.Add ("units", PrepareTable);

			PrepareTable = new TableInfo();
			PrepareTable.ObjectsName = "Пользователи";
			PrepareTable.ObjectName = "пользователя"; 
			PrepareTable.SqlSelect = "SELECT name, id FROM users ";
			PrepareTable.DisplayString = "{0}";
			PrepareTable.PrimaryKey = new PrimaryKeys("","id");
			PrepareTable.ClearItems.Add ("advance", 
			                             new ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("debit_slips", 
			                             new ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("accrual", 
			                             new ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			PrepareTable.ClearItems.Add ("credit_slips", 
			                             new ClearDependenceItem ("WHERE user_id = @id", "", "@id", "user_id"));
			Tables.Add ("users", PrepareTable);
		}
		
		public bool RunDeletion(string table, int IntKey, string StrKey)
		{
			int CountReferenceItems = 0;
			bool result = false;
			Params OutParam = new Params(IntKey,StrKey);
			try
			{
				CountReferenceItems = FillObjects (table, new TreeIter(), OutParam);
			}
			catch (Exception ex)
			{
				ErrorHappens = true;
				ErrorString = ex.ToString ();
			}
			if(ErrorHappens)
			{
				MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,
				                                      MessageType.Error, 
				                                      ButtonsType.Close,
				                                      "ошибка");
				md.UseMarkup = false;
				md.Text = "При выполении поиска зависимостей удаляемого объекта произошла ошибка. Убедитесь что версия базы данных соответствует версии программы. Если версия базы данных правильная, сообщите разработчику об ошибке в программе.\n" + ErrorString;
				md.Run ();
				md.Destroy();
				return false;
			}
			if(CountReferenceItems > 0)
			{
				if(CountReferenceItems < 10)
					treeviewObjects.ExpandAll ();
				this.Title = String.Format("Удалить {0}?", Tables[table].ObjectName);
				result = (ResponseType)this.Run () == ResponseType.Yes;
			}
			else
			{
				this.Hide();
				result = SimpleDialog(Tables[table].ObjectName) == ResponseType.Yes;
			}
			if(result)
				DeleteObjects (table,OutParam);
			return result;
		}

		public bool RunDeletion(string table, int IntKey)
		{
			return RunDeletion (table, IntKey, "");
		}

		public bool RunDeletion(string table, string StrKey)
		{
			return RunDeletion (table, 0, StrKey);
		}

		ResponseType SimpleDialog(string ObjectName)
		{
			MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,
	                              MessageType.Question, 
                                  ButtonsType.YesNo,"Вы уверены что хотите удалить "+ ObjectName + "?");
			ResponseType result = (ResponseType)md.Run ();
			md.Destroy();
			return result;
		}

		int FillObjects(string Table, TreeIter root, Params ParametersIn)
		{
			TreeIter DeleteIter, ClearIter, GroupIter, ItemIter;
			int Totalcount = 0;
			int DelCount = 0;
			int ClearCount = 0;
			int GroupCount;
			MySqlCommand cmd;
			MySqlDataReader rdr;

			if(!Tables.ContainsKey(Table))
			{
				ErrorString = "Нет описания для таблицы " + Table;
				Console.WriteLine(ErrorString);
				MainClass.StatusMessage(ErrorString);
				ErrorHappens = true;
				return 0;
			}
			if(Tables[Table].DeleteItems.Count > 0)
			{
				if(!ObjectsTreeStore.IterIsValid(root))
					DeleteIter = ObjectsTreeStore.AppendNode();
				else
					DeleteIter = ObjectsTreeStore.AppendNode (root);
				foreach(KeyValuePair<string, DeleteDependenceItem> pair in Tables[Table].DeleteItems)
				{
					GroupCount = 0;
					string sql = Tables[pair.Key].SqlSelect + pair.Value.sqlwhere;
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					if(pair.Value.SqlParam.ParamStr != "")
						cmd.Parameters.AddWithValue(pair.Value.SqlParam.ParamStr, ParametersIn.ParamStr);
					if(pair.Value.SqlParam.ParamInt != "")
						cmd.Parameters.AddWithValue (pair.Value.SqlParam.ParamInt, ParametersIn.ParamInt);
					rdr = cmd.ExecuteReader();
					if(!rdr.HasRows)
					{
						rdr.Close ();
						continue;
					}
					GroupIter = ObjectsTreeStore.AppendNode(DeleteIter);
					int IndexIntParam = 0;
					int IndexStrParam = 0;
					if(Tables[pair.Key].PrimaryKey.ParamInt != "")
						IndexIntParam = rdr.GetOrdinal(Tables[pair.Key].PrimaryKey.ParamInt);
					if(Tables[pair.Key].PrimaryKey.ParamStr != "")
						IndexStrParam = rdr.GetOrdinal(Tables[pair.Key].PrimaryKey.ParamStr);
					List<object[]> ReadedDate = new List<object[]>();
					while(rdr.Read())
					{
						object[] Fields = new object[rdr.FieldCount];
						rdr.GetValues(Fields);
						ReadedDate.Add(Fields);
					}
					rdr.Close ();

					foreach(object[] row in ReadedDate)
					{
						ItemIter = ObjectsTreeStore.AppendValues(GroupIter, String.Format(Tables[pair.Key].DisplayString, row));
						if(Tables[pair.Key].DeleteItems.Count > 0 || Tables[pair.Key].ClearItems.Count > 0)
						{
							Params OutParam = new Params();
							if(Tables[pair.Key].PrimaryKey.ParamInt != "")
								OutParam.ParamInt = Convert.ToInt32(row[IndexIntParam]);
							if(Tables[pair.Key].PrimaryKey.ParamStr != "")
								OutParam.ParamStr = row[IndexStrParam].ToString();
							Totalcount += FillObjects (pair.Key,ItemIter,OutParam);
						}
						GroupCount++;
						Totalcount++;
						DelCount++;
					}
					ObjectsTreeStore.SetValues(GroupIter, Tables[pair.Key].ObjectsName + "(" + GroupCount.ToString() + ")");
				}
				if(DelCount > 0)
					ObjectsTreeStore.SetValues(DeleteIter, String.Format ("Будет удалено ({0}/{1}) объектов:",DelCount,Totalcount));
				else
					ObjectsTreeStore.Remove (ref DeleteIter);
			}

			if(Tables[Table].ClearItems.Count > 0)
			{
				if(!ObjectsTreeStore.IterIsValid(root))
					ClearIter = ObjectsTreeStore.AppendNode();
				else
					ClearIter = ObjectsTreeStore.AppendNode (root);
				foreach(KeyValuePair<string, ClearDependenceItem> pair in Tables[Table].ClearItems)
				{
					GroupCount = 0;
					string sql = Tables[pair.Key].SqlSelect + pair.Value.sqlwhere;
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					if(pair.Value.SqlParam.ParamStr != "")
						cmd.Parameters.AddWithValue(pair.Value.SqlParam.ParamStr, ParametersIn.ParamStr);
					if(pair.Value.SqlParam.ParamInt != "")
						cmd.Parameters.AddWithValue (pair.Value.SqlParam.ParamInt, ParametersIn.ParamInt);
					rdr = cmd.ExecuteReader();
					if(!rdr.HasRows)
					{
						rdr.Close ();
						continue;
					}
					GroupIter = ObjectsTreeStore.AppendNode(ClearIter);
					
					while(rdr.Read())
					{
						object[] Fields = new object[rdr.FieldCount];
						rdr.GetValues(Fields);
						ItemIter = ObjectsTreeStore.AppendValues(GroupIter, String.Format(Tables[pair.Key].DisplayString,Fields));
						GroupCount++;
						Totalcount++;
						ClearCount++;
					}
					ObjectsTreeStore.SetValues(GroupIter, Tables[pair.Key].ObjectsName + "(" + GroupCount.ToString() + ")");
					rdr.Close ();
				}
				if(ClearCount > 0)
					ObjectsTreeStore.SetValues(ClearIter, String.Format ("Будет очищено ссылок у {0} объектов:",ClearCount));
				else
					ObjectsTreeStore.Remove (ref ClearIter);
			}
			return Totalcount;
		}

		private void DeleteObjects(string Table, Params ParametersIn)
		{
			MySqlCommand cmd;
			MySqlDataReader rdr;
			string sql;
			if(!Tables.ContainsKey(Table))
			{
				ErrorString = "Нет описания для таблицы " + Table;
				Console.WriteLine(ErrorString);
				MainClass.StatusMessage(ErrorString);
				ErrorHappens = true;
				return;
			}
			if(Tables[Table].DeleteItems.Count > 0)
			{
				foreach(KeyValuePair<string, DeleteDependenceItem> pair in Tables[Table].DeleteItems)
				{
					if(Tables[pair.Key].DeleteItems.Count > 0 || Tables[pair.Key].ClearItems.Count > 0)
					{
						sql = "SELECT * FROM " +pair.Key + " " + pair.Value.sqlwhere;
						cmd = new MySqlCommand(sql, QSMain.connectionDB);
						if(pair.Value.SqlParam.ParamStr != "")
							cmd.Parameters.AddWithValue(pair.Value.SqlParam.ParamStr, ParametersIn.ParamStr);
						if(pair.Value.SqlParam.ParamInt != "")
							cmd.Parameters.AddWithValue (pair.Value.SqlParam.ParamInt, ParametersIn.ParamInt);
						rdr = cmd.ExecuteReader();
						List<Params> ReadedDate = new List<Params>();
						string IntFieldName = Tables[pair.Key].PrimaryKey.ParamInt;
						string StrFieldName = Tables[pair.Key].PrimaryKey.ParamStr;
						while(rdr.Read())
						{
							Params OutParam = new Params();
							if(IntFieldName != "")
								OutParam.ParamInt = rdr.GetInt32(IntFieldName);
							if(StrFieldName != "")
								OutParam.ParamStr = rdr.GetString(StrFieldName);
							ReadedDate.Add(OutParam);
						}
						rdr.Close ();

						foreach(Params row in ReadedDate)
						{
							DeleteObjects (pair.Key, row);
						}
					}

					sql = "DELETE FROM " + pair.Key + " " + pair.Value.sqlwhere;
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					if(pair.Value.SqlParam.ParamStr != "")
						cmd.Parameters.AddWithValue(pair.Value.SqlParam.ParamStr, ParametersIn.ParamStr);
					if(pair.Value.SqlParam.ParamInt != "")
						cmd.Parameters.AddWithValue (pair.Value.SqlParam.ParamInt, ParametersIn.ParamInt);
					cmd.ExecuteNonQuery();
				}
			}
			
			if(Tables[Table].ClearItems.Count > 0)
			{
				foreach(KeyValuePair<string, ClearDependenceItem> pair in Tables[Table].ClearItems)
				{
					sql = "UPDATE " + pair.Key + " SET "; 
					bool first = true;
					foreach (string FieldName in pair.Value.ClearFields)
					{
						if(!first)
							sql += ", ";
						sql += FieldName + " = NULL ";
						first = false;
					}
					sql += pair.Value.sqlwhere;
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					if(pair.Value.SqlParam.ParamStr != "")
						cmd.Parameters.AddWithValue(pair.Value.SqlParam.ParamStr, ParametersIn.ParamStr);
					if(pair.Value.SqlParam.ParamInt != "")
						cmd.Parameters.AddWithValue (pair.Value.SqlParam.ParamInt, ParametersIn.ParamInt);
					cmd.ExecuteNonQuery ();
				}
			}

			sql = "DELETE FROM " + Table + " WHERE ";
			bool FirstKey = true;
			if(Tables[Table].PrimaryKey.ParamInt != "")
			{
				sql += Tables[Table].PrimaryKey.ParamInt + " = @IntParam ";
				FirstKey = false;
			}
			if(Tables[Table].PrimaryKey.ParamStr != "")
			{
				if(!FirstKey)
					sql += "AND ";
				sql += Tables[Table].PrimaryKey.ParamStr + " = @StrParam ";
				FirstKey = false;
			}
			cmd = new MySqlCommand(sql, QSMain.connectionDB);
			if(Tables[Table].PrimaryKey.ParamStr != "")
				cmd.Parameters.AddWithValue("@StrParam", ParametersIn.ParamStr);
			if(Tables[Table].PrimaryKey.ParamInt != "")
				cmd.Parameters.AddWithValue("@IntParam", ParametersIn.ParamInt);
			cmd.ExecuteNonQuery();
		}
	}
}

