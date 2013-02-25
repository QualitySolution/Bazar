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
		decimal PaySum, PayableSum, PaidSum;
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
			                                      typeof (int), typeof (string), typeof (double));
			
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
					"services.income_id as income_id, income_items.name as income FROM accrual_pays " +
						"LEFT JOIN cash ON cash.id = accrual_pays.cash_id " +
						"LEFT JOIN services ON accrual_pays.service_id = services.id " +
						"LEFT JOIN income_items ON services.income_id = income_items.id " +
						"WHERE accrual_pays.accrual_id = @accrual_id";
				
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@accrual_id", AccrualId);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				int cash_id, income_id;
				double sum;
				PayableSum = 0m;
				
				while (rdr.Read())
				{
					if(rdr["cash_id"] != DBNull.Value)
						cash_id = rdr.GetInt32("cash_id");
					else
						cash_id = -1;
					if(rdr["income_id"] != DBNull.Value)
						income_id = rdr.GetInt32("income_id");
					else
						income_id = -1;
					sum = Convert.ToDouble(rdr.GetInt32("count") * rdr.GetDecimal("price"));
					PayableSum += Convert.ToDecimal(sum);
					
					ServiceListStore.AppendValues(false,
												  rdr.GetInt32("service_id"),
					                              rdr["service"].ToString(),
					                              cash_id,
					                              rdr["cash"].ToString(),
					                              income_id,
					                              rdr["income"].ToString(),
					                              sum);
				}
				rdr.Close();

				sql = "SELECT SUM(sum) FROM credit_slips WHERE credit_slips.accrual_id = @id";
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@id", AccrualId);
				object result = cmd.ExecuteScalar ();
				if(result != DBNull.Value)
				{
					PaidSum = Convert.ToDecimal (result);
					labelTotal.LabelProp = String.Format ("Всего к оплате: {0:C} Уже оплачено: <span background=\"orange\">{1:C}</span> ", PayableSum, PaidSum);
				}
				else
				{
					PaidSum = 0m;
					labelTotal.LabelProp = String.Format ("Всего к оплате: {0:C} Уже оплачено: {1:C} ", PayableSum, PaidSum);
				}

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
			List<int[]> PayList = new List<int[]>();
			TreeIter iter;
			//Составляем список статей дохода
			if(ServiceListStore.GetIterFirst(out iter))
			{
				if((bool)ServiceListStore.GetValue(iter, 0))
				{
					PayList.Add (new int[2] { 
						(int) ServiceListStore.GetValue (iter, 3),
						(int) ServiceListStore.GetValue (iter, 5)
					});
				}
				while (ServiceListStore.IterNext(ref iter)) 
				{
					if( !(bool)ServiceListStore.GetValue(iter, 0))
						continue;
					bool exist = false;
					int Cashid = (int) ServiceListStore.GetValue (iter, 3);
					int IncomeId = (int) ServiceListStore.GetValue (iter, 5);
					foreach (int[] item in PayList)
					{
						if( item[0] == Cashid && item[1] == IncomeId)
						{
							exist = true;
							break;
						}
					}
					if(!exist)
					{
						PayList.Add (new int[2] { Cashid, IncomeId});
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
				return;
			}
			int org_id = rdr.GetInt32("org_id");
			int lessee_id = rdr.GetInt32 ("lessee_id");
			string contract_no = rdr.GetString ("contract_no");
			DateTime Month = new DateTime(rdr.GetInt32("year"), rdr.GetInt32 ("month"), 1);
			rdr.Close ();
			// Записываем Приходники
			sql = "INSERT INTO credit_slips (operation, org_id, cash_id, lessee_id, user_id, date, sum, " +
				"contract_no, accrual_id, income_id, details) " +
					"VALUES (@operation, @org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
					"@contract_no, @accrual_id, @income_id, @details)";
			foreach(int[] item in PayList)
			{
				decimal sum = 0m;
				string details = String.Format ("Оплата по начислению № {0} ({1:MMMM yyyy}) за услуги: ", Accrual_Id, Month);
				foreach(object[] row in ServiceListStore)
				{
					if((bool)row[0] && (int)row[3] == item[0] && (int)row[5] == item[1])
					{
						if(sum != 0)
							details += ", ";
						sum += Convert.ToDecimal (row[7]);
						details += (string) row[2];
					}
				}
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@operation", "common");
				cmd.Parameters.AddWithValue("@org_id", org_id);
				cmd.Parameters.AddWithValue("@cash_id", item[0]);
				cmd.Parameters.AddWithValue("@lessee_id", lessee_id);
				cmd.Parameters.AddWithValue("@user_id", MainClass.User.id);
				cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
				cmd.Parameters.AddWithValue("@sum", sum);
				cmd.Parameters.AddWithValue("@contract_no", contract_no);
				cmd.Parameters.AddWithValue("@accrual_id", Accrual_Id);
				cmd.Parameters.AddWithValue("@income_id", item[1]);
				cmd.Parameters.AddWithValue("@details", details);

				cmd.ExecuteNonQuery ();
			}

			Accrual.GetAccrualPaidBalance (Accrual_Id);
		}
	}
}