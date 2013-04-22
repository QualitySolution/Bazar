using System;
using System.Data;
using System.Collections.Generic;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class Contract : Gtk.Dialog
	{
		public bool NewContract;

		Gtk.ListStore ServiceListStore, ServiceRefListStore;
		TreeModel ServiceNameList, CashNameList;
		int LesseeId;
		bool LesseeisNull = true;
		string OriginalNumber;
		List<int> DeletedRowId = new List<int>();

		public Contract ()
		{
			this.Build ();

			MainClass.ComboFillReference(comboPlaceT,"place_types",2);
			MainClass.ComboFillReference(comboOrg, "organizations", 2);

			ComboBox ServiceCombo = new ComboBox();
			MainClass.ComboFillReference(ServiceCombo,"services",0);
			ServiceNameList = ServiceCombo.Model;
			ServiceCombo.Destroy ();

			ComboBox CashCombo = new ComboBox();
			MainClass.ComboFillReference(CashCombo,"cash",0);
			CashNameList = CashCombo.Model;
			CashCombo.Destroy ();

			MainClass.FillServiceListStore(out ServiceRefListStore);

			//Создаем таблицу "Услуги"
			ServiceListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (int), typeof (string),
			                                      typeof (int), typeof (string), typeof (int), typeof (double),
			                                      typeof (double), typeof (int));
			
			Gtk.TreeViewColumn idServiceColumn = new Gtk.TreeViewColumn ();
			idServiceColumn.Title = "idServiceColumn";
			idServiceColumn.Visible = false;
			idServiceColumn.PackStart (new Gtk.CellRendererText (), true);

			Gtk.TreeViewColumn ServiceColumn = new Gtk.TreeViewColumn ();
			ServiceColumn.Title = "Наименование";
			Gtk.CellRendererCombo CellService = new CellRendererCombo();
			CellService.TextColumn = 0;
			CellService.Editable = true;
			CellService.Model = ServiceNameList;
			CellService.HasEntry = false;
			CellService.Edited += OnServiceComboEdited;
			ServiceColumn.PackStart (CellService, true);

			Gtk.TreeViewColumn idCashColumn = new Gtk.TreeViewColumn ();
			idCashColumn.Title = "idКасса";
			idCashColumn.Visible = false;
			idCashColumn.PackStart (new Gtk.CellRendererText (), true);

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

			Gtk.TreeViewColumn idUnitsColumn = new Gtk.TreeViewColumn ();
			idUnitsColumn.Title = "idЕдиница";
			idUnitsColumn.Visible = false;
			idUnitsColumn.PackStart (new Gtk.CellRendererText (), true);

			Gtk.TreeViewColumn CountColumn = new Gtk.TreeViewColumn ();
			CountColumn.Title = "Количество";
			Gtk.CellRendererSpin CellCount = new CellRendererSpin();
			CellCount.Editable = true;
			Adjustment adjCount = new Adjustment(0,0,1000000,1,10,0);
        	CellCount.Adjustment = adjCount; 
			CellCount.Edited += OnCountSpinEdited;
			CountColumn.PackStart (CellCount, true);

			Gtk.TreeViewColumn PriceColumn = new Gtk.TreeViewColumn ();
			PriceColumn.Title = "Цена";
			PriceColumn.MinWidth = 90;
			Gtk.CellRendererSpin CellPrice = new CellRendererSpin();
			CellPrice.Editable = true;
			CellPrice.Digits = 2;
			Adjustment adjPrice = new Adjustment(0,0,100000000,10,1000,0);
        	CellPrice.Adjustment = adjPrice;
			CellPrice.Edited += OnPriceSpinEdited;
			PriceColumn.PackStart (CellPrice, true);

			Gtk.TreeViewColumn SumColumn = new Gtk.TreeViewColumn ();
			SumColumn.Title = "Сумма";
			Gtk.CellRendererText CellSum = new CellRendererText();
			SumColumn.PackStart (CellSum, true);

			treeviewServices.AppendColumn (idServiceColumn);
			treeviewServices.AppendColumn (ServiceColumn);
			ServiceColumn.AddAttribute (CellService,"text", 1);
			treeviewServices.AppendColumn (idCashColumn);
			treeviewServices.AppendColumn (CashColumn);
			CashColumn.AddAttribute (CellCash,"text", 3);
			treeviewServices.AppendColumn (idUnitsColumn);
			treeviewServices.AppendColumn ("Ед. изм.", new Gtk.CellRendererText (), "text", 5);
			treeviewServices.AppendColumn (CountColumn);
			CountColumn.AddAttribute (CellCount,"text", 6);
			treeviewServices.AppendColumn (PriceColumn);
			PriceColumn.AddAttribute (CellPrice,"text", 7);
			treeviewServices.AppendColumn (SumColumn);
			SumColumn.AddAttribute (CellSum,"text", 8);

			PriceColumn.SetCellDataFunc (CellPrice, RenderPriceColumn);
			SumColumn.SetCellDataFunc (CellSum, RenderSumColumn);

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
				Console.WriteLine("newtext is empty");
				return;
			}
			ServiceListStore.SetValue(iter, 1, args.NewText);
			TreeIter ServiceIter;
			if (!ServiceRefListStore.GetIterFirst (out ServiceIter))
				return;
			do
			{
				if(args.NewText.Equals (ServiceRefListStore.GetValue (ServiceIter, 1).ToString ()))
				{
					ServiceListStore.SetValue (iter, 0, ServiceRefListStore.GetValue (ServiceIter,0));
					ServiceListStore.SetValue (iter, 4, ServiceRefListStore.GetValue (ServiceIter,2));
					ServiceListStore.SetValue (iter, 5, ServiceRefListStore.GetValue (ServiceIter,3));
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
				Console.WriteLine("newtext is empty");
				return;
			}
			ServiceListStore.SetValue(iter, 3, args.NewText);
			TreeIter CashIter;
			if (!CashNameList.GetIterFirst (out CashIter))
				return;
			do
			{
				if(CashNameList.GetValue (CashIter,0).ToString () == args.NewText)
				{
					ServiceListStore.SetValue (iter, 2, CashNameList.GetValue (CashIter, 1));
					break;
				}
			}
			while(CashNameList.IterNext (ref CashIter));
			TestCanSave ();
		}

		void OnCountSpinEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			double Price = (double)ServiceListStore.GetValue (iter, 7);
			int count = int.Parse(args.NewText);
			ServiceListStore.SetValue(iter, 6, count);
			ServiceListStore.SetValue(iter, 8, Price * count);
			CalculateServiceSum ();
		}

		void OnPriceSpinEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			double Price = double.Parse(args.NewText);
			int count = (int)ServiceListStore.GetValue (iter, 6);
			ServiceListStore.SetValue(iter, 7, Price);
			ServiceListStore.SetValue(iter, 8, Price * count);
			CalculateServiceSum ();
		}

		private void RenderPriceColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double Price = (double) model.GetValue (iter, 7);
			(cell as Gtk.CellRendererSpin).Text = String.Format("{0:0.00}", Price);
		}

		private void RenderSumColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double Sum = (double) model.GetValue (iter, 8);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Sum);
		}

		public void ContractFill(string ContractNumber)
		{
			NewContract = false;
			TreeIter iter;
			
			MainClass.StatusMessage("Запрос договора №" + ContractNumber +"...");
			string sql = "SELECT contracts.*, lessees.name as lessee, places.area FROM contracts " +
				"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
				"LEFT JOIN places ON places.type_id = contracts.place_type_id AND places.place_no = contracts.place_no " +
				"WHERE contracts.number = @number";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@number", ContractNumber);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
				
				entryNumber.Text = rdr["number"].ToString();
				OriginalNumber = rdr["number"].ToString();
				if(rdr["lessee_id"] != DBNull.Value)
				{
					LesseeId = Convert.ToInt32(rdr["lessee_id"].ToString());
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
					MainClass.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
				else
					MainClass.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
				comboOrg.SetActiveIter (iter);

				if(rdr["pay_day"] != DBNull.Value)
					comboPayDay.Active = Convert.ToInt32(rdr["pay_day"].ToString());
				else
					comboPayDay.Active = 0;

				float area = 0;
				if(rdr["area"] != DBNull.Value)
					area = rdr.GetFloat("area");
				labelArea.LabelProp = String.Format ("{0} м<sup>2</sup>", area);

				textComments.Buffer.Text = rdr["comments"].ToString();
				//запоминаем переменные что бы освободить соединение
				object DBPlaceT = rdr["place_type_id"];
				object DBPlaceNo = rdr["place_no"];
				
				rdr.Close();

				if(DBPlaceT != DBNull.Value)
					MainClass.SearchListStore((ListStore)comboPlaceT.Model, int.Parse(DBPlaceT.ToString()), out iter);
				else
					MainClass.SearchListStore((ListStore)comboPlaceT.Model, -1, out iter);
				comboPlaceT.SetActiveIter (iter);
				if(DBPlaceNo != DBNull.Value)
				{
					MainClass.SearchListStore((ListStore)comboPlaceNo.Model, DBPlaceNo.ToString(), out iter);
					comboPlaceNo.SetActiveIter(iter);
				}
				this.Title = "Договор №" + entryNumber.Text;

				//Получаем таблицу услуг
				sql = "SELECT contract_pays.*, cash.name as cash, services.name as service, " +
					"units.id as units_id, units.name as units FROM contract_pays " +
					"LEFT JOIN cash ON cash.id = contract_pays.cash_id " +
					"LEFT JOIN services ON contract_pays.service_id = services.id " +
					"LEFT JOIN units ON services.units_id = units.id " +
					"WHERE contract_pays.contract_no = @contract_no";

				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@contract_no", ContractNumber);
				rdr = cmd.ExecuteReader();

				int cash_id, units_id, count;
				double price, sum;

				while (rdr.Read())
				{
					if(rdr["cash_id"] != DBNull.Value)
						cash_id = int.Parse(rdr["cash_id"].ToString());
					else
						cash_id = -1;
					if(rdr["units_id"] != DBNull.Value)
						units_id = int.Parse(rdr["units_id"].ToString());
					else
						units_id = -1;
					count = int.Parse(rdr["count"].ToString());
					price = double.Parse(rdr["price"].ToString());
					sum = count * price;

					ServiceListStore.AppendValues(int.Parse(rdr["service_id"].ToString()),
					                             rdr["service"].ToString(),
					                             cash_id,
					                             rdr["cash"].ToString(),
					                             units_id,
					                             rdr["units"].ToString(),
					                             count,
					                             price,
					                             sum,
					                             int.Parse(rdr["id"].ToString()));
				}
				rdr.Close();
				CalculateServiceSum();

				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о договоре!");
				MainClass.ErrorMessage(this,ex);
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
			TreeIter iter;
			
			if(ServiceListStore == null)
				return true;
			
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if((int)ServiceListStore.GetValue(iter,0) <= 0 || (int)ServiceListStore.GetValue(iter, 2) <= 0)
					return false;
				while(ServiceListStore.IterNext(ref iter))
					if((int)ServiceListStore.GetValue(iter,0) <= 0 || (int)ServiceListStore.GetValue(iter, 2) <= 0)
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
			if(NewContract && comboPlaceNo.ActiveText != null)
			{
				MainClass.StatusMessage("Запрос информации о месте...");
				string sql = "SELECT org_id, area FROM places " +
					"WHERE type_id = @type_id AND place_no = @place_no";
				try
				{
					MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
					
					if(comboPlaceT.GetActiveIter(out iter))
					{
						cmd.Parameters.AddWithValue("@type_id", comboPlaceT.Model.GetValue(iter,1));
					}	
					cmd.Parameters.AddWithValue("@place_no", comboPlaceNo.ActiveText);
			
					MySqlDataReader rdr = cmd.ExecuteReader();
						
					if(rdr.Read() )
					{
						if(rdr["org_id"] != DBNull.Value)
							MainClass.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
						else
							MainClass.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
						comboOrg.SetActiveIter (iter);
						float area = 0;
						if(rdr["area"] != DBNull.Value)
							area = rdr.GetFloat("area");
						labelArea.LabelProp = String.Format ("{0} м<sup>2</sup>", area);
					}
					rdr.Close();
					MainClass.StatusMessage("Ok");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					MainClass.StatusMessage("Ошибка получения места!");
					MainClass.ErrorMessage(this,ex);
				}				
			}
			TestCanSave();
		}

		protected void OnButtonLesseeEditClicked (object sender, EventArgs e)
		{
			reference LesseeSelect = new reference();
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
				string sql = "SELECT COUNT(*) AS cnt FROM contracts WHERE number = @number";
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				MySqlDataReader rdr = cmd.ExecuteReader();
				rdr.Read();
				
				if( rdr["cnt"].ToString() != "0" && OriginalNumber != entryNumber.Text)
				{
					MainClass.StatusMessage("Договор уже существует!");
					MessageDialog md = new MessageDialog( this, DialogFlags.Modal,
                          MessageType.Error, 
                          ButtonsType.Ok,"ошибка");
					md.UseMarkup = false;
					md.Text = "Договор с номером " + entryNumber.Text + " уже существует в базе данных!";
					md.Run ();
					md.Destroy();
					rdr.Close();
					return;
				}
				rdr.Close();
				// Проверка не занято ли место другим арендатором
				sql = "SELECT number, start_date AS start, IFNULL(cancel_date,end_date) AS end FROM contracts " +
					"WHERE place_type_id = @type_id AND place_no = @place_no AND " +
						"!(@start > DATE(IFNULL(cancel_date,end_date)) OR @end < start_date)" ;
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
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
				rdr = cmd.ExecuteReader();

				while(rdr.Read())
				{
					if(rdr["number"].ToString () == OriginalNumber)
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
						"WHERE number = @oldnumber";
				}

				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@number", entryNumber.Text);
				cmd.Parameters.AddWithValue("@oldnumber", OriginalNumber);
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

				//записываем таблицу услуг
				ServiceListStore.GetIterFirst(out iter);
				do
				{
					if(!ServiceListStore.IterIsValid (iter))
						break;
					if((int)ServiceListStore.GetValue(iter,0) < 1)
						break; // не указано название услуги
					if((int)ServiceListStore.GetValue(iter,9) > 0)
						sql = "UPDATE contract_pays SET service_id = @service_id, " +
							"cash_id = @cash_id, count = @count, price = @price " +
							"WHERE id = @id";
					else
						sql = "INSERT INTO contract_pays (contract_no, service_id, cash_id, count, price) " +
							"VALUES (@contract_no, @service_id, @cash_id, @count, @price)";
					cmd = new MySqlCommand(sql, MainClass.connectionDB);
					cmd.Parameters.AddWithValue("@contract_no", entryNumber.Text);
					cmd.Parameters.AddWithValue("@service_id", ServiceListStore.GetValue(iter,0));
					if((int)ServiceListStore.GetValue(iter,2) > 0)
						cmd.Parameters.AddWithValue("@cash_id", ServiceListStore.GetValue(iter,2));
					else
						cmd.Parameters.AddWithValue("@cash_id", DBNull.Value);
					cmd.Parameters.AddWithValue("@count", ServiceListStore.GetValue(iter,6));
					cmd.Parameters.AddWithValue("@price", ServiceListStore.GetValue(iter,7));
					cmd.Parameters.AddWithValue("@id", ServiceListStore.GetValue(iter,9));

					cmd.ExecuteNonQuery();
				}
				while(ServiceListStore.IterNext(ref iter));

				//Удаляем удаленные строки из базы данных
				sql = "DELETE FROM contract_pays WHERE id = @id";
				foreach( int id in DeletedRowId)
				{
					cmd = new MySqlCommand(sql, MainClass.connectionDB);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.ExecuteNonQuery();
				}
				 
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи договора!");
				MainClass.ErrorMessage(this,ex);
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
			ServiceListStore.SetValue(iter, 6, 1);
			if(CashNameList.IterNChildren() == 1)
			{
				CashNameList.GetIterFirst (out CashIter);
				ServiceListStore.SetValue(iter, 3, CashNameList.GetValue (CashIter, 0));
				ServiceListStore.SetValue (iter, 2, CashNameList.GetValue (CashIter, 1));
			}
			TestCanSave ();
			OnTreeviewServicesCursorChanged (null, null);
		}

		protected void CalculateServiceSum ()
		{
			double ServiceSum = 0;
			TreeIter iter;
			
			if(ServiceListStore.GetIterFirst(out iter))
			{
				ServiceSum = (double)ServiceListStore.GetValue(iter,8);
				while (ServiceListStore.IterNext(ref iter)) 
				{
					ServiceSum += (double)ServiceListStore.GetValue(iter,8);
				}
			}
			labelSum.Text = string.Format("Сумма: {0:C} ", ServiceSum);
		}		

		protected void OnButtonDelServiceClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			treeviewServices.Selection.GetSelected (out iter);
			if((int)ServiceListStore.GetValue(iter, 9) > 0)
				DeletedRowId.Add ((int)ServiceListStore.GetValue(iter, 9));
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
				MainClass.SearchListStore((ListStore)comboPlaceT.Model, place_type_id, out iter);
				comboPlaceT.SetActiveIter (iter);
				MainClass.SearchListStore((ListStore)comboPlaceNo.Model, place_no, out iter);
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

