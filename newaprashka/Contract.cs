using System;
using System.Data;
using System.Collections.Generic;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using NLog;

namespace bazar
{
	public partial class Contract : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public bool NewContract;

		Gtk.ListStore ServiceListStore, ServiceRefListStore;
		TreeModel ServiceNameList, CashNameList;
		int LesseeId; 
		int ContractId = -1;
		int OrigLesseeId = -1;
		bool LesseeisNull = true;
		List<int> DeletedRowId = new List<int>();
		decimal PlaceArea = 0;

		private enum ServiceCol{
			service_id,
			service,
			cash_id,
			cash,
			units,
			count,
			price,
			sum,
			id,
			by_aria,
			min_pay,
			row_color
		}
		public Contract ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithNo);
			ComboWorks.ComboFillReference(comboPlaceT,"place_types", ComboWorks.ListMode.WithNo);

			ComboBox ServiceCombo = new ComboBox();
			ComboWorks.ComboFillReference(ServiceCombo,"services", ComboWorks.ListMode.OnlyItems);
			ServiceNameList = ServiceCombo.Model;
			ServiceCombo.Destroy ();

			ComboBox CashCombo = new ComboBox();
			string sqlSelect = "SELECT name, id, color FROM cash";
			ComboWorks.ComboFillUniversal(CashCombo, sqlSelect, "{0}", null, 1, ComboWorks.ListMode.OnlyItems, true);
			CashNameList = CashCombo.Model;
			CashCombo.Destroy ();

			MainClass.FillServiceListStore(out ServiceRefListStore);

			//Создаем таблицу "Услуги"
			ServiceListStore = new Gtk.ListStore (typeof(int), 	// 0 - idServiceColumn
			                                      typeof(string),	// 1 - Наименование
			                                      typeof(int),		// 2 - idКасса
			                                      typeof(string),	// 3 - Касса
			                                      typeof(string), 	// 5 - Ед. изм.
			                                      typeof(decimal), 	// 6 - Количество
			                                      typeof(decimal),	// 7 - Цена
			                                      typeof(decimal),	// 8 - Сумма
			                                      typeof(int), 	// 9 - id
			                                      typeof(bool),	// 10 - Есть ли расчет по метражу
			                                      typeof(decimal),	// 11 - Минимальный платеж.
			                                      typeof(string) 	// 12 - Цвет строки
			);
			
			Gtk.TreeViewColumn ServiceColumn = new Gtk.TreeViewColumn ();
			ServiceColumn.Title = "Наименование";
			ServiceColumn.MinWidth = 180;
			Gtk.CellRendererCombo CellService = new CellRendererCombo();
			CellService.TextColumn = 0;
			CellService.Editable = true;
			CellService.Model = ServiceNameList;
			CellService.HasEntry = false;
			CellService.Edited += OnServiceComboEdited;
			ServiceColumn.PackStart (CellService, true);

			Gtk.TreeViewColumn CashColumn = new Gtk.TreeViewColumn ();
			CashColumn.Title = "Касса";
			CashColumn.MinWidth = 130;
			Gtk.CellRendererCombo CellCash = new CellRendererCombo();
			CellCash.TextColumn = 0;
			CellCash.Editable = true;
			CellCash.Model = CashNameList;
			CellCash.HasEntry = false;
			CellCash.Edited += OnCashComboEdited;
			CashColumn.PackStart (CellCash, true);

			Gtk.TreeViewColumn CountColumn = new Gtk.TreeViewColumn ();
			CountColumn.Title = "Количество";
			Gtk.CellRendererText CellCount = new CellRendererText();
			CellCount.Editable = true;
			CellCount.Edited += OnServiceCountEdited;
			CountColumn.PackStart (CellCount, true);
			Gtk.CellRendererText CellUnits = new CellRendererText ();
			CountColumn.PackStart (CellUnits, false);

			Gtk.CellRendererText CellPrice = new CellRendererText();
			CellPrice.Editable = true;
			CellPrice.Edited += OnServicePriceEdited;

			Gtk.CellRendererText CellMinSum = new CellRendererText();
			CellMinSum.Editable = true;
			CellMinSum.Edited += OnServiceMinSumEdited;

			treeviewServices.AppendColumn (ServiceColumn);
			ServiceColumn.AddAttribute (CellService, "text", (int)ServiceCol.service);
			treeviewServices.AppendColumn (CashColumn);
			CashColumn.AddAttribute (CellCash,"text", (int)ServiceCol.cash);
			treeviewServices.AppendColumn (CountColumn);
			CountColumn.AddAttribute (CellUnits,"text", (int)ServiceCol.units);
			CountColumn.SetCellDataFunc (CellCount, RenderCountColumn);
			treeviewServices.AppendColumn ("Цена", CellPrice, RenderPriceColumn);
			treeviewServices.AppendColumn ("Сумма", new Gtk.CellRendererText (), RenderSumColumn);
			treeviewServices.AppendColumn ("Мин. платеж", CellMinSum, RenderMinSumColumn);

			foreach(TreeViewColumn column in treeviewServices.Columns)
			{
				foreach(CellRenderer render in column.CellRenderers)
				{
					column.AddAttribute (render, "background", (int)ServiceCol.row_color);
				}
			}

			treeviewServices.Columns[3].MinWidth = 90;

			treeviewServices.Model = ServiceListStore;
			treeviewServices.ShowAll();

			OnTreeviewServicesCursorChanged(null, null);
		}

		void OnServiceComboEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			if(args.NewText == null)
			{
				logger.Warn("newtext is empty");
				return;
			}
			ServiceListStore.SetValue(iter, (int)ServiceCol.service, args.NewText);
			TreeIter ServiceIter;
			if (!ServiceRefListStore.GetIterFirst (out ServiceIter))
				return;
			do
			{
				if(args.NewText.Equals (ServiceRefListStore.GetValue (ServiceIter, 1).ToString ()))
				{
					ServiceListStore.SetValue (iter, (int)ServiceCol.service_id, ServiceRefListStore.GetValue (ServiceIter,0));
					ServiceListStore.SetValue (iter, (int)ServiceCol.units, ServiceRefListStore.GetValue (ServiceIter,3));

					bool choice = (bool) ServiceRefListStore.GetValue (ServiceIter,4);
					ServiceListStore.SetValue (iter, (int)ServiceCol.by_aria, choice);
					if(choice)
						ServiceListStore.SetValue (iter, (int)ServiceCol.count, PlaceArea);
					break;
				}
			}
			while(ServiceRefListStore.IterNext (ref ServiceIter));
			TestCanSave ();
		}

		void OnCashComboEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			if(args.NewText == null)
			{
				logger.Warn("newtext is empty");
				return;
			}
			ServiceListStore.SetValue(iter, (int)ServiceCol.cash, args.NewText);
			TreeIter CashIter;
			if (!CashNameList.GetIterFirst (out CashIter))
				return;
			do
			{
				if(CashNameList.GetValue (CashIter,0).ToString () == args.NewText)
				{
					ServiceListStore.SetValue (iter, (int)ServiceCol.cash_id, CashNameList.GetValue (CashIter, 1));
					object[] Values = (object[]) CashNameList.GetValue (CashIter, 2);
					ServiceListStore.SetValue (iter, (int)ServiceCol.row_color, Values[2] != DBNull.Value ? (string)Values[2] : null) ;
					break;
				}
			}
			while(CashNameList.IterNext (ref CashIter));
			TestCanSave ();
			CalculateServiceSum ();
		}

		void OnServiceCountEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			decimal Price = (decimal)ServiceListStore.GetValue (iter, (int)ServiceCol.price);
			decimal count;
			if(decimal.TryParse(args.NewText, out count))
			{
				logger.Debug ("Parsed:{0}", count);
				ServiceListStore.SetValue (iter, (int)ServiceCol.count, count);
				ServiceListStore.SetValue (iter, (int)ServiceCol.sum, Price * count);
				CalculateServiceSum ();
			}
		}

		void OnServicePriceEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			decimal Price;
			decimal count = (decimal)ServiceListStore.GetValue (iter, (int)ServiceCol.count);
			if (decimal.TryParse (args.NewText, out Price)) 
			{
				ServiceListStore.SetValue (iter, (int)ServiceCol.price, Price);
				ServiceListStore.SetValue (iter, (int)ServiceCol.sum, Price * count);
				CalculateServiceSum ();
			}
		}

		void OnServiceMinSumEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			decimal MinSum;
			if (decimal.TryParse (args.NewText, out MinSum)) 
			{
				ServiceListStore.SetValue (iter, (int)ServiceCol.min_pay, MinSum);
			}
		}

		private void RenderCountColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Count = (decimal) model.GetValue (iter, (int)ServiceCol.count);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Count);
		}

		private void RenderPriceColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Price = (decimal) model.GetValue (iter, (int)ServiceCol.price);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Price);
		}

		private void RenderSumColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Sum = (decimal) model.GetValue (iter, (int)ServiceCol.sum);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Sum);
		}

		private void RenderMinSumColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Sum = (decimal) model.GetValue (iter, (int)ServiceCol.min_pay);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Sum);
		}

		public void ContractFill(int Id)
		{
			NewContract = false;
			ContractId = Id;
			TreeIter iter;
			
			MainClass.StatusMessage("Запрос договора ID:" + Id +"...");
			string sql = "SELECT contracts.*, lessees.name as lessee, places.area FROM contracts " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"LEFT JOIN places ON places.type_id = contracts.place_type_id AND places.place_no = contracts.place_no " +
				"WHERE contracts.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Id);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
				
				entryNumber.Text = rdr["number"].ToString();
				if(rdr["lessee_id"] != DBNull.Value)
				{
					LesseeId = Convert.ToInt32(rdr["lessee_id"].ToString());
					OrigLesseeId = LesseeId;
					entryLessee.Text = rdr["lessee"].ToString();
					entryLessee.TooltipText = rdr["lessee"].ToString();
					LesseeisNull = false;
				}
				if(rdr["sign_date"] != DBNull.Value)
					datepickerSign.Date = DateTime.Parse( rdr["sign_date"].ToString());
				datepickerStart.Date = DateTime.Parse(rdr["start_date"].ToString());
				datepickerEnd.Date = DateTime.Parse(rdr["end_date"].ToString());
				if(rdr["cancel_date"] != DBNull.Value)
					datepickerCancel.Date = DateTime.Parse(rdr["cancel_date"].ToString());
				if(rdr["org_id"] != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
				comboOrg.SetActiveIter (iter);

				if(rdr["pay_day"] != DBNull.Value)
					comboPayDay.Active = Convert.ToInt32(rdr["pay_day"].ToString());
				else
					comboPayDay.Active = 0;

				decimal area = 0;
				if(rdr["area"] != DBNull.Value)
					area = rdr.GetDecimal("area");
				labelArea.LabelProp = String.Format ("{0} м<sup>2</sup>", area);
				PlaceArea = area;
				textComments.Buffer.Text = rdr["comments"].ToString();
				//запоминаем переменные что бы освободить соединение
				object DBPlaceT = rdr["place_type_id"];
				object DBPlaceNo = rdr["place_no"];
				
				rdr.Close();

				if(DBPlaceT != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboPlaceT.Model, int.Parse(DBPlaceT.ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboPlaceT.Model, -1, out iter);
				comboPlaceT.SetActiveIter (iter);
				if(DBPlaceNo != DBNull.Value)
				{
					ListStoreWorks.SearchListStore((ListStore)comboPlaceNo.Model, DBPlaceNo.ToString(), out iter);
					comboPlaceNo.SetActiveIter(iter);
				}
				this.Title = "Договор №" + entryNumber.Text;

				//Получаем таблицу услуг
				sql = "SELECT contract_pays.*, cash.name as cash, cash.color as cashcolor, services.name as service, services.by_area as by_area," +
					"units.name as units FROM contract_pays " +
					"LEFT JOIN cash ON cash.id = contract_pays.cash_id " +
					"LEFT JOIN services ON contract_pays.service_id = services.id " +
					"LEFT JOIN units ON services.units_id = units.id " +
					"WHERE contract_pays.contract_id = @contract_id";

				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@contract_id", ContractId);
				rdr = cmd.ExecuteReader();

				decimal count, price;

				while (rdr.Read())
				{
					count = DBWorks.GetDecimal (rdr, "count", 0);
					price = DBWorks.GetDecimal (rdr, "price", 0);

					ServiceListStore.AppendValues(int.Parse(rdr["service_id"].ToString()),
					                             rdr["service"].ToString(),
					                             DBWorks.GetInt (rdr, "cash_id", -1),
					                             rdr["cash"].ToString(),
					                             rdr["units"].ToString(),
					                             count,
					                             price,
					                             count * price,
					                             int.Parse(rdr["id"].ToString()),
					                             rdr.GetBoolean("by_area"),
					                              DBWorks.GetDecimal (rdr, "min_sum", 0),
					                              DBWorks.GetString(rdr, "cashcolor", null)
					                             );
				}
				rdr.Close();
				CalculateServiceSum();

				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о договоре!");
				QSMain.ErrorMessage(this,ex);
			}

			TestCanSave();
			OnTreeviewServicesCursorChanged(null, null);
		}

		protected void OnComboPlaceTChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			int id;
			((ListStore)comboPlaceNo.Model).Clear();
			if(comboPlaceT.GetActiveIter(out iter) && comboPlaceT.Active > 0)
			{
				id = (int)comboPlaceT.Model.GetValue(iter,1);
				MainClass.ComboPlaceNoFill(comboPlaceNo,id);
			}
			TestCanSave();
		}

		protected	void TestCanSave ()
		{
			bool Numberok = (entryNumber.Text != "");
			bool Orgok = comboOrg.Active > 0;
			bool Placeok = comboPlaceT.Active > 0 && comboPlaceNo.Active >= 0;
			bool Lesseeok = !LesseeisNull;
			bool DatesCorrectok = TestCorrectDates (false);
			bool ServiceOk = TestServiceAndCash ();

			buttonLesseeOpen.Sensitive = Lesseeok;
			buttonOk.Sensitive = Numberok && Orgok && Placeok && Lesseeok && DatesCorrectok && ServiceOk;
		}

		protected bool TestServiceAndCash()
		{
			if(ServiceListStore == null)
				return true;
			
			foreach(object[] row in ServiceListStore)
			{
				if( (int) row[(int)ServiceCol.service_id] <= 0 || (int) row[(int)ServiceCol.cash_id] <= 0)
					return false;
			}
			return true;
		}

		protected bool TestCorrectDates(bool DisplayMessage)
		{
			bool DateCorrectok = false;
			bool DateCancelok = false;
			bool DatesIsEmpty = datepickerStart.IsEmpty || datepickerEnd.IsEmpty;
			if( !DatesIsEmpty)
				DateCorrectok = datepickerEnd.Date.CompareTo(datepickerStart.Date) > 0;
			if(datepickerCancel.IsEmpty)
				DateCancelok = true;
			else
				DateCancelok = datepickerCancel.Date > datepickerStart.Date && datepickerCancel.Date < datepickerEnd.Date;
			if(DisplayMessage && !DateCorrectok && !DatesIsEmpty)
			{
				MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				                                      MessageType.Warning, 
				                                      ButtonsType.Ok, 
				                                      "Дата окончания аренды должна быть больше даты начала аренды.");
				md.Run ();
				md.Destroy();
			}
			if(DisplayMessage && !DateCancelok)
			{
				MessageDialog md = new MessageDialog ( this, DialogFlags.DestroyWithParent,
				                                      MessageType.Warning, 
				                                      ButtonsType.Ok, 
				                                      "Дата досрочного расторжения должна входить в период между датой начала аренды и датой ее окончания.");
				md.Run ();
				md.Destroy();
			}
			return DateCorrectok && DateCancelok;
		}

		protected void OnEntryNumberChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			TestCanSave();
		}

		protected void OnComboPlaceNoChanged (object sender, EventArgs e)
		{
			TreeIter iter;
			if(comboPlaceNo.ActiveText != null)
			{
				MainClass.StatusMessage("Запрос информации о месте...");
				string sql = "SELECT org_id, area FROM places " +
					"WHERE type_id = @type_id AND place_no = @place_no";
				try
				{
					MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
					
					if(comboPlaceT.GetActiveIter(out iter))
					{
						cmd.Parameters.AddWithValue("@type_id", comboPlaceT.Model.GetValue(iter,1));
					}	
					cmd.Parameters.AddWithValue("@place_no", comboPlaceNo.ActiveText);
			
					MySqlDataReader rdr = cmd.ExecuteReader();
						
					if(rdr.Read() )
					{
						if(rdr["org_id"] != DBNull.Value)
							ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
						else
							ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
						comboOrg.SetActiveIter (iter);
						decimal old_area = PlaceArea;
						if(rdr["area"] != DBNull.Value)
							PlaceArea = rdr.GetDecimal("area");
						labelArea.LabelProp = String.Format ("{0} м<sup>2</sup>", PlaceArea);

						TreeIter ServiceIter;
						if (ServiceListStore != null && ServiceListStore.GetIterFirst (out ServiceIter))
						{
							do
							{
								bool b = (bool) ServiceListStore.GetValue(ServiceIter, (int)ServiceCol.by_aria);
								decimal i = (decimal) ServiceListStore.GetValue(ServiceIter, (int)ServiceCol.count);
								if( b && i == old_area)
								{
									ServiceListStore.SetValue(ServiceIter, (int)ServiceCol.count, PlaceArea);
									decimal Price = (decimal)ServiceListStore.GetValue (ServiceIter, (int)ServiceCol.price);
									ServiceListStore.SetValue(ServiceIter, (int)ServiceCol.sum, Price * PlaceArea);
								}
							}
							while(ServiceListStore.IterNext (ref ServiceIter));
							CalculateServiceSum ();
						}
					}
					rdr.Close();
					MainClass.StatusMessage("Ok");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					MainClass.StatusMessage("Ошибка получения места!");
					QSMain.ErrorMessage(this,ex);
				}				
			}
			TestCanSave();
		}

		protected void OnButtonLesseeEditClicked (object sender, EventArgs e)
		{
			Reference LesseeSelect = new Reference();
			LesseeSelect.SetMode(false,true,true,true,false);
			LesseeSelect.FillList("lessees","Арендатор", "Арендаторы");
			LesseeSelect.Show();
			int result = LesseeSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				LesseeId = LesseeSelect.SelectedID;
				LesseeisNull = false;
				entryLessee.Text = LesseeSelect.SelectedName;
				entryLessee.TooltipText = LesseeSelect.SelectedName;
			}
			LesseeSelect.Destroy();
			TestCanSave();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			TreeIter iter;

			MainClass.StatusMessage("Запись договора...");
			try 
			{
				// Проверка номера договора на дубликат
				string sql = "SELECT COUNT(*) AS cnt FROM contracts WHERE number = @number AND sign_date = @sign_date AND id <> @id ";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				cmd.Parameters.AddWithValue("@id", ContractId);
				if(datepickerSign.IsEmpty)
					cmd.Parameters.AddWithValue("@sign_date", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@sign_date", datepickerSign.Date);
				long Count = (long) cmd.ExecuteScalar();

				if( Count > 0)
				{
					MainClass.StatusMessage("Договор уже существует!");
					MessageDialog md = new MessageDialog( this, DialogFlags.Modal,
                          MessageType.Error, 
                          ButtonsType.Ok,"ошибка");
					md.UseMarkup = false;
					md.Text = String.Format ("Договор с номером {0} от {1:d}, уже существует в базе данных!",  entryNumber.Text, datepickerSign.Date);
					md.Run ();
					md.Destroy();
					return;
				}
				// Проверка не занято ли место другим арендатором
				sql = "SELECT id, number, start_date AS start, IFNULL(cancel_date,end_date) AS end FROM contracts " +
					"WHERE place_type_id = @type_id AND place_no = @place_no AND " +
						"!(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date)" ;
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				if(comboPlaceT.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@type_id", comboPlaceT.Model.GetValue(iter,1));
				}
				if(comboPlaceNo.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@place_no", comboPlaceNo.Model.GetValue(iter,0));
				}	
				cmd.Parameters.AddWithValue("@start", datepickerStart.Date);
				if(datepickerCancel.IsEmpty)
					cmd.Parameters.AddWithValue("@end", datepickerEnd.Date);
				else
					cmd.Parameters.AddWithValue("@end", datepickerCancel.Date);
				MySqlDataReader rdr = cmd.ExecuteReader();

				while(rdr.Read())
				{
					if(rdr.GetInt32("id") == ContractId)
						continue;
					MainClass.StatusMessage("Место уже занято!");
					MessageDialog md = new MessageDialog( this, DialogFlags.Modal,
					                                     MessageType.Error, 
					                                     ButtonsType.Ok,"ошибка");
					md.UseMarkup = false;
					md.Text = "Период действия договора пересекается с договором №" + rdr["number"].ToString () + 
						", по которому это место уже сдается в аренду с " + rdr.GetDateTime ("start").ToShortDateString() +
							" по " + rdr.GetDateTime ("end").ToShortDateString() + ". \n Вы должны, либо изменить даты " +
							"аренды в текущем договоре, либо досрочно расторгнуть предыдущий договор на это место.";
					md.Run ();
					md.Destroy();
					rdr.Close();
					return;
				}
				rdr.Close();
				// записываем
				if(NewContract)
				{
					sql = "INSERT INTO contracts (number, lessee_id, org_id, place_type_id, place_no, sign_date, " +
						"start_date, end_date, pay_day, cancel_date, comments) " +
							"VALUES (@number, @lessee_id, @org_id, @place_type_id, @place_no, @sign_date, " +
							"@start_date, @end_date, @pay_day, @cancel_date, @comments)";
				}
				else
				{
					sql = "UPDATE contracts SET number = @number, lessee_id = @lessee_id, org_id = @org_id, " +
						"place_type_id = @place_type_id, place_no = @place_no, sign_date = @sign_date, start_date = @start_date, " +
						"end_date = @end_date, pay_day = @pay_day, cancel_date = @cancel_date, comments = @comments " +
						"WHERE id = @id";
				}

				cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@id", ContractId);
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				cmd.Parameters.AddWithValue("@lessee_id", LesseeId);
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
					cmd.Parameters.AddWithValue("@org_id",comboOrg.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@org_id", DBNull.Value);

				if(comboPlaceT.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@place_type_id", comboPlaceT.Model.GetValue(iter,1));
				}	
				if(comboPlaceNo.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@place_no", comboPlaceNo.Model.GetValue(iter,0));
				}	
				if(!datepickerSign.IsEmpty)
					cmd.Parameters.AddWithValue("@sign_date", datepickerSign.Date);
				else
					cmd.Parameters.AddWithValue("@sign_date", DBNull.Value);
				if(!datepickerStart.IsEmpty)
					cmd.Parameters.AddWithValue("@start_date", datepickerStart.Date);
				if(!datepickerEnd.IsEmpty)
					cmd.Parameters.AddWithValue("@end_date", datepickerEnd.Date);
				if(!datepickerCancel.IsEmpty)
					cmd.Parameters.AddWithValue("@cancel_date", datepickerCancel.Date);
				else
					cmd.Parameters.AddWithValue("@cancel_date", DBNull.Value);

				if(comboPayDay.Active > 0)
					cmd.Parameters.AddWithValue("@pay_day", comboPayDay.Active);
				else
					cmd.Parameters.AddWithValue("@pay_day", DBNull.Value);

				if(textComments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@comments", textComments.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				if(NewContract)
					ContractId = (int) cmd.LastInsertedId;
				//записываем таблицу услуг
				ServiceListStore.GetIterFirst(out iter);
				do
				{
					if(!ServiceListStore.IterIsValid (iter))
						break;
					if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.service_id) < 1)
						break; // не указано название услуги
					if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.id) > 0)
						sql = "UPDATE contract_pays SET service_id = @service_id, " +
							"cash_id = @cash_id, count = @count, price = @price, min_sum = @min_sum " +
							"WHERE id = @id";
					else
						sql = "INSERT INTO contract_pays (contract_id, service_id, cash_id, count, price, min_sum) " +
							"VALUES (@contract_id, @service_id, @cash_id, @count, @price, @min_sum)";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@contract_id", ContractId);
					cmd.Parameters.AddWithValue("@service_id", ServiceListStore.GetValue(iter, (int)ServiceCol.service_id));
					if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.cash_id) > 0)
						cmd.Parameters.AddWithValue("@cash_id", ServiceListStore.GetValue(iter, (int)ServiceCol.cash_id));
					else
						cmd.Parameters.AddWithValue("@cash_id", DBNull.Value);
					cmd.Parameters.AddWithValue("@count", ServiceListStore.GetValue(iter, (int)ServiceCol.count));
					cmd.Parameters.AddWithValue("@price", ServiceListStore.GetValue(iter, (int)ServiceCol.price));
					cmd.Parameters.AddWithValue("@min_sum", DBWorks.ValueOrNull ((decimal) ServiceListStore.GetValue(iter, (int)ServiceCol.min_pay) > 0, ServiceListStore.GetValue(iter, (int)ServiceCol.min_pay)));
					cmd.Parameters.AddWithValue("@id", ServiceListStore.GetValue(iter, (int)ServiceCol.id));

					cmd.ExecuteNonQuery();
				}
				while(ServiceListStore.IterNext(ref iter));

				//Удаляем удаленные строки из базы данных
				sql = "DELETE FROM contract_pays WHERE id = @id";
				foreach( int id in DeletedRowId)
				{
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.ExecuteNonQuery();
				}
				//Корректная смена арендатора
				if(!NewContract && OrigLesseeId != LesseeId && !LesseeisNull)
				{
					MainClass.StatusMessage("Арендатор изменился...");
					sql = "SELECT COUNT(*) FROM credit_slips WHERE contract_id = @contract AND lessee_id = @old_lessee";
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@contract", ContractId);
					cmd.Parameters.AddWithValue("@old_lessee", OrigLesseeId);
					long rowcount = (long) cmd.ExecuteScalar();
					if( rowcount > 0)
					{
						MessageDialog md = new MessageDialog( this, DialogFlags.Modal,
						                                     MessageType.Warning, 
						                                     ButtonsType.YesNo, "Предупреждение");
						md.UseMarkup = false;
						md.Text = String.Format("У договора изменился арендатор, но поэтому договору уже " +
							"было создано {0} приходных ордеров. Заменить арендатора в приходных ордерах?", rowcount);
						int result = md.Run ();
						md.Destroy();

						if(result == (int) ResponseType.Yes)
						{
							MainClass.StatusMessage("Меняем арендатора в приходных ордерах...");
							sql = "UPDATE credit_slips SET lessee_id = @lessee_id " +
								"WHERE contract_id = @contract AND lessee_id = @old_lessee ";
							cmd = new MySqlCommand(sql, QSMain.connectionDB);
							cmd.Parameters.AddWithValue("@contract", ContractId);
							cmd.Parameters.AddWithValue("@old_lessee", OrigLesseeId);
							cmd.Parameters.AddWithValue("@lessee_id", LesseeId);
							cmd.ExecuteNonQuery();
						}
					}
				}

				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи договора!");
				QSMain.ErrorMessage(this,ex);
			}

		}

		protected void OnDatepickerStartDateChanged (object sender, EventArgs e)
		{
			TestCorrectDates (true);
			TestCanSave();
		}

		protected void OnDatepickerEndDateChanged (object sender, EventArgs e)
		{
			TestCorrectDates (true);
			TestCanSave();
		}

		protected void OnButtonLesseeOpenClicked (object sender, EventArgs e)
		{
			lessee winLessee = new lessee();
			winLessee.LesseeFill(LesseeId);
			winLessee.Show();
			winLessee.Run();
			winLessee.Destroy();
		}		

		protected void OnButtonAddServiceClicked (object sender, EventArgs e)
		{
			TreeIter iter, CashIter;
			iter = ServiceListStore.Append();
			ServiceListStore.SetValue(iter, (int)ServiceCol.count, 1m);
			ServiceListStore.SetValue(iter, (int)ServiceCol.price, 0m);
			ServiceListStore.SetValue(iter, (int)ServiceCol.sum, 0m);
			ServiceListStore.SetValue(iter, (int)ServiceCol.min_pay, 0m);
			if(CashNameList.IterNChildren() == 1)
			{
				CashNameList.GetIterFirst (out CashIter);
				ServiceListStore.SetValue(iter, (int)ServiceCol.cash, CashNameList.GetValue (CashIter, 0));
				ServiceListStore.SetValue (iter, (int)ServiceCol.cash_id, CashNameList.GetValue (CashIter, 1));
			}
			TestCanSave ();
			OnTreeviewServicesCursorChanged (null, null);
		}

		protected void CalculateServiceSum ()
		{
			Dictionary<int, decimal> CashSum = new Dictionary<int, decimal> ();
			decimal TotalSum = 0;
			TreeIter iter;
			
			foreach(object[] row in ServiceListStore)
			{
				if (!CashSum.ContainsKey ((int)row [(int)ServiceCol.cash_id]))
					CashSum.Add ((int)row [(int)ServiceCol.cash_id], 0);
				CashSum [(int)row [(int)ServiceCol.cash_id]] += (decimal)row [(int)ServiceCol.sum];
				TotalSum += (decimal)row [(int)ServiceCol.sum];
			}

			string Text = "";
			if(CashSum.Count > 1)
			{
				foreach(KeyValuePair<int, decimal> pair in CashSum)
				{
					ListStoreWorks.SearchListStore ((ListStore)CashNameList, pair.Key, out iter);
					Text += string.Format("{1}: {0:C} \n", pair.Value, (string) CashNameList.GetValue(iter, 0));
				}
			}
			Text += string.Format("Всего: {0:C} ", TotalSum);
			labelSum.LabelProp = Text; 
		}		

		protected void OnButtonDelServiceClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			treeviewServices.Selection.GetSelected (out iter);
			if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.id) > 0)
				DeletedRowId.Add ((int)ServiceListStore.GetValue(iter, (int)ServiceCol.id));
			ServiceListStore.Remove(ref iter);
			CalculateServiceSum ();
			TestCanSave ();
			OnTreeviewServicesCursorChanged (null, null);
		}

		public bool SetPlace(int place_type_id, string place_no)
		{
			TreeIter iter;
			try
			{
				ListStoreWorks.SearchListStore((ListStore)comboPlaceT.Model, place_type_id, out iter);
				comboPlaceT.SetActiveIter (iter);
				ListStoreWorks.SearchListStore((ListStore)comboPlaceNo.Model, place_no, out iter);
				comboPlaceNo.SetActiveIter(iter);
				return true;
			}
			catch
			{
				return false;
			}
		}

		protected void OnDatepickerCancelDateChanged (object sender, EventArgs e)
		{
			TestCorrectDates (true);
			TestCanSave();
		}

		protected void OnTreeviewServicesCursorChanged (object sender, EventArgs e)
		{
			bool isSelect = treeviewServices.Selection.CountSelectedRows() == 1;
			buttonDelService.Sensitive = isSelect;
		}

		protected void OnEntryActivated (object sender, EventArgs e)
		{
			this.ChildFocus (DirectionType.TabForward);
		}
	}
}