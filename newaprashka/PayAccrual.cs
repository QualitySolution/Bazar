using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace bazar
{
	public partial class PayAccrual : Gtk.Dialog
	{
		Gtk.ListStore ServiceListStore;
		TreeModel IncomeNameList;

		bool IncomeNotEmpty;
		decimal PaySum, PayableSum;
		int Accrual_Id;

		public PayAccrual ()
		{
			this.Build ();

			ComboBox IncomeCombo = new ComboBox();
			MainClass.ComboFillReference(IncomeCombo,"income_items", 0);
			IncomeNameList = IncomeCombo.Model;
			IncomeCombo.Destroy ();

			//Создаем таблицу "Услуги"
			ServiceListStore = new Gtk.ListStore (typeof (bool), typeof (int), typeof (string), typeof (int), typeof (string),
			                                      typeof (int), typeof (string), typeof (double), typeof(long));
			
			CellRendererToggle CellPay = new CellRendererToggle();
			CellPay.Activatable = true;
			CellPay.Toggled += onCellPayToggled;

			Gtk.TreeViewColumn IncomeItemsColumn = new Gtk.TreeViewColumn ();
			IncomeItemsColumn.Title = "Статья дохода";
			IncomeItemsColumn.MinWidth = 130;
			Gtk.CellRendererCombo CellIncomeItems = new CellRendererCombo();
			CellIncomeItems.TextColumn = 0;
			CellIncomeItems.Editable = true;
			CellIncomeItems.Model = IncomeNameList;
			CellIncomeItems.HasEntry = false;
			CellIncomeItems.Edited += OnIncomeItemComboEdited;
			IncomeItemsColumn.PackStart (CellIncomeItems, true);
			
			Gtk.TreeViewColumn SumColumn = new Gtk.TreeViewColumn ();
			SumColumn.Title = "Сумма";
			SumColumn.MinWidth = 90;
			Gtk.CellRendererSpin CellSum = new CellRendererSpin();
			CellSum.Editable = true;
			CellSum.Digits = 2;
			Adjustment adjPrice = new Adjustment(0,0,1000000,10,1000,0);
			CellSum.Adjustment = adjPrice;
			CellSum.Edited += OnSumSpinEdited;
			SumColumn.PackStart (CellSum, true);
			
			treeviewServices.AppendColumn("Оплатить", CellPay, "active", 0);
			// ID Услуги - 1
			treeviewServices.AppendColumn ("Услуга", new Gtk.CellRendererText (), "text", 2);
			//ID Кассы - 3
			treeviewServices.AppendColumn ("Касса", new Gtk.CellRendererText (), "text", 4);
			// ID Статьи дохода - 5
			treeviewServices.AppendColumn (IncomeItemsColumn);
			IncomeItemsColumn.AddAttribute (CellIncomeItems,"text", 6);
			treeviewServices.AppendColumn (SumColumn);
			SumColumn.AddAttribute (CellSum,"text", 7);
			//ID строки начисления - 8

			SumColumn.SetCellDataFunc (CellSum, RenderSumColumn);
			
			treeviewServices.Model = ServiceListStore;
			treeviewServices.ShowAll();
		}

		void onCellPayToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;
			
			if (ServiceListStore.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) ServiceListStore.GetValue(iter,0);
				ServiceListStore.SetValue(iter, 0, !old);
			}
			CalculateServiceSum ();
		}

		void OnIncomeItemComboEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			if(args.NewText == null)
			{
				Console.WriteLine("newtext is empty");
				return;
			}
			ServiceListStore.SetValue(iter, 6, args.NewText);
			TreeIter IncomeIter;
			if (!IncomeNameList.GetIterFirst (out IncomeIter))
				return;
			do
			{
				if(IncomeNameList.GetValue (IncomeIter,0).ToString () == args.NewText)
				{
					ServiceListStore.SetValue (iter, 5, IncomeNameList.GetValue (IncomeIter, 1));
					break;
				}
			}
			while(IncomeNameList.IterNext (ref IncomeIter));
			CalculateServiceSum ();
		}

		void OnSumSpinEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			double Sum = double.Parse(args.NewText);
			ServiceListStore.SetValue(iter, 7, Sum);
			CalculateServiceSum ();
		}

		private void RenderSumColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double Sum = (double) model.GetValue (iter, 7);
			(cell as Gtk.CellRendererSpin).Text = String.Format("{0:0.00}", Sum);
		}

		protected void CalculateServiceSum ()
		{
			TreeIter iter;
			PaySum = 0m;
			IncomeNotEmpty = true;
			
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if(Convert.ToBoolean(ServiceListStore.GetValue(iter,0)) == true)
				{
					PaySum = Convert.ToDecimal(ServiceListStore.GetValue(iter,7));
					if((int)ServiceListStore.GetValue(iter, 5) <= 0)
						IncomeNotEmpty = false;
				}
				while (ServiceListStore.IterNext(ref iter)) 
				{
					if(Convert.ToBoolean(ServiceListStore.GetValue(iter,0)) == true)
					{
						PaySum += Convert.ToDecimal(ServiceListStore.GetValue(iter,7));
						if((int)ServiceListStore.GetValue(iter, 5) <= 0)
							IncomeNotEmpty = false;
					}
				}
			}
			labelSum.Text = string.Format("Выбрано для оплаты: {0:C} ", PaySum);
			TestCanSave ();
		}

		protected void TestCanSave ()
		{
			bool SelectSumok = PaySum > 0;
			bool IncomeOk = IncomeNotEmpty;

			buttonOk.Sensitive = SelectSumok && IncomeOk;
		}

		public void FillPayTable(int AccrualId)
		{
			Accrual_Id = AccrualId;
			
			MainClass.StatusMessage("Запрос начисления №" + AccrualId +"...");
			try
			{

				this.Title = "Ввод оплаты по начислению №" + AccrualId;
				
				//Получаем таблицу услуг
				string sql = "SELECT accrual_pays.*, cash.name as cash, services.name as service, " +
					"services.income_id as income_id, income_items.name as income, paysum.sum as paid FROM accrual_pays " +
						"LEFT JOIN cash ON cash.id = accrual_pays.cash_id " +
						"LEFT JOIN services ON accrual_pays.service_id = services.id " +
						"LEFT JOIN income_items ON services.income_id = income_items.id " +
						"LEFT JOIN (" +
						"SELECT accrual_pay_id, SUM(sum) as sum FROM payment_details GROUP BY accrual_pay_id) as paysum " +
						"ON paysum.accrual_pay_id = accrual_pays.id " +
						"WHERE accrual_pays.accrual_id = @accrual_id";
				
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@accrual_id", AccrualId);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				int cash_id, income_id;
				double sum;
				decimal paid, accrual;
				PayableSum = 0m;
				
				while (rdr.Read())
				{
					if(rdr["paid"] != DBNull.Value)
						paid = rdr.GetDecimal ("paid");
					else
						paid = 0;
					accrual = rdr.GetInt32("count") * rdr.GetDecimal("price");
					sum = Convert.ToDouble (accrual - paid);
					if(sum <= 0)
						continue;
					PayableSum += Convert.ToDecimal(sum);
					if(rdr["cash_id"] != DBNull.Value)
						cash_id = rdr.GetInt32("cash_id");
					else
						cash_id = -1;
					if(rdr["income_id"] != DBNull.Value)
						income_id = rdr.GetInt32("income_id");
					else
						income_id = -1;

					ServiceListStore.AppendValues(false,
												  rdr.GetInt32("service_id"),
					                              rdr["service"].ToString(),
					                              cash_id,
					                              rdr["cash"].ToString(),
					                              income_id,
					                              rdr["income"].ToString(),
					                              sum,
					                              rdr.GetInt64 ("id"));
				}
				rdr.Close();

				labelTotal.LabelProp = String.Format ("Всего к оплате: {0:C} ", PayableSum);
		
				MainClass.StatusMessage("Ok");
				CalculateServiceSum();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о начисление!");
				MainClass.ErrorMessage(this,ex);
			}
			
			TestCanSave();
		}

		protected void OnCheckAllClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			if(ServiceListStore.GetIterFirst(out iter))
			{
				ServiceListStore.SetValue (iter, 0, checkAll.Active);
				while (ServiceListStore.IterNext(ref iter)) 
				{
					ServiceListStore.SetValue (iter, 0, checkAll.Active);
				}
			}
			CalculateServiceSum ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			List<CashDoc> PayList = new List<CashDoc>();
			TreeIter iter;
			//Составляем список касс
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if((bool)ServiceListStore.GetValue(iter, 0))
				{
					PayList.Add (new CashDoc((int) ServiceListStore.GetValue (iter, 3)));
				}
				while (ServiceListStore.IterNext(ref iter)) 
				{
					if( !(bool)ServiceListStore.GetValue(iter, 0))
						continue;
					bool exist = false;
					int Cashid = (int) ServiceListStore.GetValue (iter, 3);
					foreach (CashDoc item in PayList)
					{
						if( item.CashId == Cashid)
						{
							exist = true;
							break;
						}
					}
					if(!exist)
					{
						PayList.Add (new CashDoc(Cashid));
					}
				}
			}
			// Получаем общую информацию
			string sql = "SELECT contracts.org_id as org_id, contracts.lessee_id as lessee_id, " +
				"accrual.contract_no as contract_no, month, year FROM accrual " +
				"LEFT JOIN contracts ON contracts.number = accrual.contract_no " +
				"WHERE accrual.id = @id";
			MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
			cmd.Parameters.AddWithValue("@id", Accrual_Id);
			MySqlDataReader rdr = cmd.ExecuteReader();
			if(!rdr.Read())
			{
				MainClass.StatusMessage("Не удалось получить информацию по начислению!");
				rdr.Close ();
				return;
			}
			int org_id = rdr.GetInt32("org_id");
			int lessee_id = rdr.GetInt32 ("lessee_id");
			string contract_no = rdr.GetString ("contract_no");
			DateTime Month = new DateTime(rdr.GetInt32("year"), rdr.GetInt32 ("month"), 1);
			rdr.Close ();
			//Записываем оплату
			MySqlTransaction trans = MainClass.connectionDB.BeginTransaction ();

			try
			{
				sql = "INSERT INTO credit_slips (operation, org_id, cash_id, lessee_id, user_id, date, sum, " +
					"contract_no, accrual_id, details) " +
					"VALUES (@operation, @org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
					"@contract_no, @accrual_id, @details)";
				string sql2 = "INSERT INTO payments (createdate, credit_slip_id, accrual_id) " +
					"VALUES (@date, @slip, @accrual)";
				foreach(CashDoc item in PayList)
				{
					decimal sum = 0m;
					string details = String.Format ("Оплата по начислению № {0} ({1:MMMM yyyy}) за услуги: ", Accrual_Id, Month);
					foreach(object[] row in ServiceListStore)
					{
						if((bool)row[0] && (int)row[3] == item.CashId)
						{
							if(sum != 0)
								details += ", ";
							sum += Convert.ToDecimal (row[7]);
							details += (string) row[2];
						}
					}
					// Записываем приходный ордер
					cmd = new MySqlCommand(sql, MainClass.connectionDB, trans);
					cmd.Parameters.AddWithValue("@operation", "payment");
					cmd.Parameters.AddWithValue("@org_id", org_id);
					cmd.Parameters.AddWithValue("@cash_id", item.CashId);
					cmd.Parameters.AddWithValue("@lessee_id", lessee_id);
					cmd.Parameters.AddWithValue("@user_id", MainClass.User.id);
					cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
					cmd.Parameters.AddWithValue("@sum", sum);
					cmd.Parameters.AddWithValue("@contract_no", contract_no);
					cmd.Parameters.AddWithValue("@accrual_id", Accrual_Id);
					cmd.Parameters.AddWithValue("@details", details);

					cmd.ExecuteNonQuery ();
					item.DocId = Convert.ToInt32(cmd.LastInsertedId);

					// Записываем платеж
					cmd = new MySqlCommand(sql2, MainClass.connectionDB, trans);
					cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
					cmd.Parameters.AddWithValue("@accrual", Accrual_Id);
					cmd.Parameters.AddWithValue("@slip", item.DocId);
					
					cmd.ExecuteNonQuery ();
					item.PaymentId = Convert.ToInt32(cmd.LastInsertedId);
				}

				// Записываем строки оплаты
				sql = "INSERT INTO payment_details(payment_id, accrual_pay_id, sum, income_id)" +
					"VALUES (@payment_id, @accrual_pay_id, @sum, @income_id)";
				foreach(object[] row in ServiceListStore)
				{
					if((bool)row[0])
					{
						CashDoc CurrentDoc = PayList.Find( p => p.CashId == (int)row[3]);
						cmd = new MySqlCommand(sql, MainClass.connectionDB, trans);
						cmd.Parameters.AddWithValue("@payment_id", CurrentDoc.PaymentId);
						cmd.Parameters.AddWithValue("@accrual_pay_id", row[8]);
						cmd.Parameters.AddWithValue("@sum", row[7]);
						cmd.Parameters.AddWithValue("@income_id", row[5]);
						
						cmd.ExecuteNonQuery ();
					}
				}
				trans.Commit ();
			}
			catch (Exception ex) 
			{
				trans.Rollback ();
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи оплаты!");
				MainClass.ErrorMessage(this,ex);
			}
			Accrual.GetAccrualPaidBalance (Accrual_Id);
		}

		private class CashDoc
		{
			public int DocId;
			public int CashId;
			public int PaymentId;
			
			public CashDoc(int Cash)
			{
				CashId = Cash;
			}
		}
	}
}