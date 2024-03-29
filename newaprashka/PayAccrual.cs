using System;
using Gtk;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using QSProjectsLib;
using NLog;

namespace bazar
{
	public partial class PayAccrual : Gtk.Dialog
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		Gtk.ListStore ServiceListStore;
		TreeModel IncomeNameList;
		List<CashDoc> PayList;

		bool IncomeNotEmpty;
		decimal PaySum, PayableSum;
		int Accrual_Id;

		private enum ServiceCol{
			pay,
			service_id,
			service,
			cash_id,
			cash,
			income_id, //5
			income,
			sum,
			accrual_pay_id
		}

		public PayAccrual ()
		{
			this.Build ();

			ComboBox IncomeCombo = new ComboBox();
			ComboWorks.ComboFillReference(IncomeCombo,"income_items", ComboWorks.ListMode.OnlyItems, OrderBy: "name");
			IncomeNameList = IncomeCombo.Model;
			IncomeCombo.Destroy ();

			//Создаем таблицу "Услуги"
			ServiceListStore = new Gtk.ListStore (typeof (bool),
			                                      typeof (int),
			                                      typeof (string),
			                                      typeof (int),
			                                      typeof (string),
			                                      typeof (int),
			                                      typeof (string),
			                                      typeof (decimal),
			                                      typeof(long));
			
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
			Gtk.CellRendererText CellSum = new CellRendererText();
			CellSum.Editable = true;
			CellSum.Edited += OnSumTextEdited;
			SumColumn.PackStart (CellSum, true);
			
			treeviewServices.AppendColumn("Оплатить", CellPay, "active", (int)ServiceCol.pay);
			treeviewServices.AppendColumn ("Услуга", new Gtk.CellRendererText (), "text", (int)ServiceCol.service);
			treeviewServices.AppendColumn ("Касса", new Gtk.CellRendererText (), "text", (int)ServiceCol.cash);
			treeviewServices.AppendColumn (IncomeItemsColumn);
			IncomeItemsColumn.AddAttribute (CellIncomeItems, "text", (int)ServiceCol.income);
			treeviewServices.AppendColumn (SumColumn);
			SumColumn.SetCellDataFunc (CellSum, RenderSumColumn);
			
			treeviewServices.Model = ServiceListStore;
			treeviewServices.ShowAll();

			var menu = new Menu ();
			var itemSimplePaymentPrint = new MenuItem ("Простая квитанция");
			itemSimplePaymentPrint.Activated += ItemSimplePaymentPrint_Activated;
			menu.Add (itemSimplePaymentPrint);
			var itemDetailPaymentPrint = new MenuItem ("Подробная квитанция");
			itemDetailPaymentPrint.Activated += ItemDetailPaymentPrint_Activated;
			menu.Add (itemDetailPaymentPrint);
			var itemDetailPaymentPrintWithMeter = new MenuItem ("Квитанция с показаниями");
			itemDetailPaymentPrintWithMeter.Activated += ItemDetailPaymentWithMetersPrint_Activated;
			menu.Add (itemDetailPaymentPrintWithMeter);
			menu.ShowAll ();
			buttonOkPrint.Menu = menu;
		}

		void onCellPayToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;
			
			if (ServiceListStore.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) ServiceListStore.GetValue(iter, (int)ServiceCol.pay);
				ServiceListStore.SetValue(iter, (int)ServiceCol.pay, !old);
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
				return;
			}
			ServiceListStore.SetValue(iter, (int)ServiceCol.income, args.NewText);
			TreeIter IncomeIter;
			if (!IncomeNameList.GetIterFirst (out IncomeIter))
				return;
			do
			{
				if(IncomeNameList.GetValue (IncomeIter,0).ToString () == args.NewText)
				{
					ServiceListStore.SetValue (iter, (int)ServiceCol.income_id, IncomeNameList.GetValue (IncomeIter, 1));
					break;
				}
			}
			while(IncomeNameList.IterNext (ref IncomeIter));
			CalculateServiceSum ();
		}

		void OnSumTextEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			decimal Sum;
			if(decimal.TryParse(args.NewText, out Sum))
			{
				ServiceListStore.SetValue(iter, (int)ServiceCol.sum, Sum);
				CalculateServiceSum ();
			}
		}

		private void RenderSumColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Sum = (decimal) model.GetValue (iter, (int)ServiceCol.sum);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Sum);
		}

		protected void CalculateServiceSum ()
		{
			TreeIter iter;
			PaySum = 0m;
			IncomeNotEmpty = true;
			
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if(Convert.ToBoolean(ServiceListStore.GetValue(iter, (int)ServiceCol.pay)) == true)
				{
					PaySum = (decimal)ServiceListStore.GetValue(iter, (int)ServiceCol.sum);
					if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.income_id) <= 0)
						IncomeNotEmpty = false;
				}
				while (ServiceListStore.IterNext(ref iter)) 
				{
					if(Convert.ToBoolean(ServiceListStore.GetValue(iter, (int)ServiceCol.pay)) == true)
					{
						PaySum += (decimal) ServiceListStore.GetValue(iter, (int)ServiceCol.sum);
						if((int)ServiceListStore.GetValue(iter, (int)ServiceCol.income_id) <= 0)
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

			buttonOk.Sensitive = buttonOkPrint.Sensitive = SelectSumok && IncomeOk;
		}

		public void FillPayTable(int AccrualId)
		{
			Accrual_Id = AccrualId;
			
			logger.Info("Запрос начисления №" + AccrualId +"...");
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
				
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@accrual_id", AccrualId);
				MySqlDataReader rdr = cmd.ExecuteReader();

				decimal sum, paid, accrual;
				PayableSum = 0m;
				
				while (rdr.Read())
				{
					paid = DBWorks.GetDecimal (rdr, "paid", 0);
					accrual = rdr.GetDecimal("count") * rdr.GetDecimal("price");
					sum = accrual - paid;
					if(sum <= 0)
						continue;
					PayableSum += sum;

					ServiceListStore.AppendValues(false,
												  rdr.GetInt32("service_id"),
					                              rdr["service"].ToString(),
					                              DBWorks.GetInt (rdr, "cash_id", -1),
					                              rdr["cash"].ToString(),
					                              DBWorks.GetInt (rdr, "income_id", -1),
					                              rdr["income"].ToString(),
					                              sum,
					                              rdr.GetInt64 ("id"));
				}
				rdr.Close();

				labelTotal.LabelProp = String.Format ("Всего к оплате: {0:C} ", PayableSum);
		
				logger.Info("Ok");
				CalculateServiceSum();
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о начисление!", logger, ex);
			}
			
			TestCanSave();
		}

		protected void OnCheckAllClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			if(ServiceListStore.GetIterFirst(out iter))
			{
				ServiceListStore.SetValue (iter, (int)ServiceCol.pay, checkAll.Active);
				while (ServiceListStore.IterNext(ref iter)) 
				{
					ServiceListStore.SetValue (iter, (int)ServiceCol.pay, checkAll.Active);
				}
			}
			CalculateServiceSum ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			Save ();
		}

		private void Save()
		{ 
			PayList = new List<CashDoc>();
			TreeIter iter;
			//Составляем список касс
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if((bool)ServiceListStore.GetValue(iter, (int)ServiceCol.pay))
				{
					PayList.Add (new CashDoc((int) ServiceListStore.GetValue (iter, (int)ServiceCol.cash_id)));
				}
				while (ServiceListStore.IterNext(ref iter)) 
				{
					if( !(bool)ServiceListStore.GetValue(iter, (int)ServiceCol.pay))
						continue;
					bool exist = false;
					int Cashid = (int) ServiceListStore.GetValue (iter, (int)ServiceCol.cash_id);
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
				"accrual.contract_id as contract_id, month, year FROM accrual " +
				"LEFT JOIN contracts ON contracts.id = accrual.contract_id " +
				"WHERE accrual.id = @id";
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", Accrual_Id);
			MySqlDataReader rdr = cmd.ExecuteReader();
			if(!rdr.Read())
			{
				logger.Info("Не удалось получить информацию по начислению!");
				rdr.Close ();
				return;
			}
			int org_id = rdr.GetInt32("org_id");
			int lessee_id = rdr.GetInt32 ("lessee_id");
			int contract_id = rdr.GetInt32 ("contract_id");
			DateTime Month = new DateTime(rdr.GetInt32("year"), rdr.GetInt32 ("month"), 1);
			rdr.Close ();
			//Записываем оплату
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction ();

			try
			{
				sql = "INSERT INTO credit_slips (operation, org_id, cash_id, lessee_id, user_id, date, sum, " +
					"contract_id, accrual_id, details) " +
					"VALUES (@operation, @org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
					"@contract_id, @accrual_id, @details)";
				string sql2 = "INSERT INTO payments (createdate, credit_slip_id, accrual_id) " +
					"VALUES (@date, @slip, @accrual)";
				foreach(CashDoc item in PayList)
				{
					decimal sum = 0m;
					string details = String.Format ("Оплата по начислению № {0} ({1:MMMM yyyy}) за услуги: ", Accrual_Id, Month);
					foreach(object[] row in ServiceListStore)
					{
						if((bool)row[(int)ServiceCol.pay] && (int)row[(int)ServiceCol.cash_id] == item.CashId)
						{
							if(sum != 0)
								details += ", ";
							sum += (decimal) row[(int)ServiceCol.sum];
							details += (string) row[(int)ServiceCol.service];
						}
					}
					// Записываем приходный ордер
					cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
					cmd.Parameters.AddWithValue("@operation", "payment");
					cmd.Parameters.AddWithValue("@org_id", org_id);
					cmd.Parameters.AddWithValue("@cash_id", item.CashId);
					cmd.Parameters.AddWithValue("@lessee_id", lessee_id);
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.Id);
					cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
					cmd.Parameters.AddWithValue("@sum", sum);
					cmd.Parameters.AddWithValue("@contract_id", contract_id);
					cmd.Parameters.AddWithValue("@accrual_id", Accrual_Id);
					cmd.Parameters.AddWithValue("@details", details);

					cmd.ExecuteNonQuery ();
					item.DocId = Convert.ToInt32(cmd.LastInsertedId);

					// Записываем платеж
					cmd = new MySqlCommand(sql2, QSMain.connectionDB, trans);
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
					if((bool)row[(int)ServiceCol.pay])
					{
						CashDoc CurrentDoc = PayList.Find( p => p.CashId == (int)row[(int)ServiceCol.cash_id]);
						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@payment_id", CurrentDoc.PaymentId);
						cmd.Parameters.AddWithValue("@accrual_pay_id", row[(int)ServiceCol.accrual_pay_id]);
						cmd.Parameters.AddWithValue("@sum", row[(int)ServiceCol.sum]);
						cmd.Parameters.AddWithValue("@income_id", row[(int)ServiceCol.income_id]);
						
						cmd.ExecuteNonQuery ();
					}
				}
				trans.Commit ();
			}
			catch (Exception ex) 
			{
				logger.Error (ex);
				trans.Rollback ();
				QSMain.ErrorMessageWithLog(this, "Ошибка записи оплаты!", logger, ex);
			}
			Accrual.GetAccrualPaidBalance (Accrual_Id);
		}

		void ItemSimplePaymentPrint_Activated (object sender, EventArgs e)
		{
			Save ();
			foreach(var doc in PayList) {
				string param = $"id={doc.DocId}";
				ViewReportExt.Run ("PaymentTicket_Simple", param);
			}
			Respond (ResponseType.Ok);
		}

		void ItemDetailPaymentPrint_Activated (object sender, EventArgs e)
		{
			Save ();
			foreach (var doc in PayList) {
				string param = $"id={doc.DocId}";
				ViewReportExt.Run ("PaymentTicket", param);
			}
			Respond (ResponseType.Ok);
		}

		void ItemDetailPaymentWithMetersPrint_Activated (object sender, EventArgs e)
		{
			Save ();
			foreach (var doc in PayList) {
				string param = $"id={doc.DocId}";
				ViewReportExt.Run ("PaymentTicketWithMeters", param);
			}
			Respond (ResponseType.Ok);
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