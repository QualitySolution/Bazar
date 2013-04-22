using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class IncomeSlip : Gtk.Dialog
	{
		public bool NewSlip;
		int Lessee_id;
		int Accountable_id;
		int OriginalAccrual = 0;
		int Payment = 0;
		bool LesseeNull = true;
		bool AccountableNull = true;

		public IncomeSlip ()
		{
			this.Build ();

			MainClass.ComboFillReference(comboCash,"cash",2);
			MainClass.ComboFillReference(comboOrg, "organizations", 2);
			MainClass.ComboFillReference(comboIncomeItem,"income_items",2);

			//Заполняем поля по умолчанию
			dateSlip.Date = DateTime.Now.Date;
			entryUser.Text = MainClass.User.Name;
			if(MainClass.User.edit_slips)
				dateSlip.Sensitive = true;
			OnComboOperationChanged (null, null);
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
				Lessee_id = LesseeSelect.SelectedID;
				LesseeNull = false;
				//buttonContactOpen.Sensitive = true;
				entryLessee.Text = LesseeSelect.SelectedName;
				entryLessee.TooltipText = LesseeSelect.SelectedName;
				MainClass.ComboContractFill (comboContract, Lessee_id, NewSlip);
			}
			LesseeSelect.Destroy();
			TestCanSave ();
		}

		protected	void TestCanSave ()
		{
			bool Orgok = comboOrg.Active > 0;
			bool Cashok = comboCash.Active > 0;
			bool Itemok = comboIncomeItem.Active > 0;
			bool Lesseeok = !LesseeNull;
			bool Paymentok = separationpayment.CanSave;
			bool Sumok;
			if(spinSum.Text != "")
				Sumok = Convert.ToDecimal (spinSum.Text) != 0; 
			else
				Sumok = false;
			bool Accountableok = !AccountableNull;

			switch (comboOperation.Active) 
			{
			case 0:
				buttonOk.Sensitive = Orgok && Cashok && Lesseeok && Itemok && Sumok;
				break;
			case 1:
				buttonOk.Sensitive = Orgok && Cashok && Accountableok && Sumok;
				break;
			case 2:
				buttonOk.Sensitive = Orgok && Cashok && Lesseeok && Paymentok && Sumok;
				break;
			default:
				break;
			}

		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			TestCanSave ();
		}

		protected void OnComboCashChanged (object sender, EventArgs e)
		{
			TestCanSave ();
		}

		protected void OnComboIncomeItemChanged (object sender, EventArgs e)
		{
			TestCanSave ();
		}

		protected void OnSpinSumValueChanged (object sender, EventArgs e)
		{
			TestCanSave ();
			if(comboOperation.Active == 2)
				separationpayment.PaymentSum = Convert.ToDecimal (spinSum.Value);
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			TreeIter iter, AccrualIter;
			int Slip_id;

			if(NewSlip)
			{
				sql = "INSERT INTO credit_slips (operation, org_id, cash_id, lessee_id, user_id, date, sum, " +
					"contract_no, accrual_id, income_id, employee_id, details) " +
						"VALUES (@operation, @org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
						"@contract_no, @accrual_id, @income_id, @employee_id, @details)";
			}
			else
			{
				sql = "UPDATE credit_slips SET operation = @operation, org_id = @org_id, cash_id = @cash_id, lessee_id = @lessee_id, " +
						"date = @date, sum = @sum, contract_no = @contract_no, accrual_id = @accrual_id, income_id = @income_id, " +
						"employee_id = @employee_id, details = @details " +
						"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись Приходного ордера...");
			MySqlTransaction trans = MainClass.connectionDB.BeginTransaction ();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB, trans);
				
				cmd.Parameters.AddWithValue("@id", entryNumber.Text);
				switch (comboOperation.Active) 
				{
				case 1:
					cmd.Parameters.AddWithValue ("@operation","advance");
					break;
				case 2:
					cmd.Parameters.AddWithValue ("@operation","payment");
					break;
				default:
					cmd.Parameters.AddWithValue ("@operation","common");
				break;
				}
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
					cmd.Parameters.AddWithValue("@org_id",comboOrg.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@org_id", DBNull.Value);
				if(comboCash.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@cash_id", comboCash.Model.GetValue(iter,1));
				}	
				if((comboOperation.Active == 0 || comboOperation.Active == 2) && !LesseeNull)
					cmd.Parameters.AddWithValue("@lessee_id", Lessee_id);
				else
					cmd.Parameters.AddWithValue("@lessee_id", DBNull.Value);
				if(comboOperation.Active == 1)
					cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				else
					cmd.Parameters.AddWithValue("@employee_id", DBNull.Value);
				if(NewSlip)
					cmd.Parameters.AddWithValue("@user_id", MainClass.User.id);
				if(dateSlip.IsEmpty)
					cmd.Parameters.AddWithValue("@date", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@date", dateSlip.Date);
				cmd.Parameters.AddWithValue("@sum", spinSum.Value);
				if((comboOperation.Active == 0 || comboOperation.Active == 2) && comboContract.Active >= 0)
					cmd.Parameters.AddWithValue("@contract_no", comboContract.ActiveText);
				else
					cmd.Parameters.AddWithValue("@contract_no", DBNull.Value);
				int CurrentAccrualId;
				if((comboOperation.Active == 0 || comboOperation.Active == 2) && comboAccrual.Active > 0 && comboAccrual.GetActiveIter(out AccrualIter))
				{
					CurrentAccrualId = (int)comboAccrual.Model.GetValue(AccrualIter,1);
					cmd.Parameters.AddWithValue("@accrual_id", CurrentAccrualId);
				}
				else
				{
					cmd.Parameters.AddWithValue("@accrual_id", DBNull.Value);
					CurrentAccrualId = -1;
				}
				if(comboIncomeItem.GetActiveIter(out iter) && comboIncomeItem.Active > 0)
					cmd.Parameters.AddWithValue("@income_id", comboIncomeItem.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@income_id", DBNull.Value);
				if(textviewDetails.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@details", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@details", textviewDetails.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				// Создаем платеж
				if(comboOperation.Active == 2)
				{
					// Здесь возможно надо будет обновлять информацию в платеже о начислении. пока только создание.
					if(NewSlip)
					{
						Slip_id = Convert.ToInt32(cmd.LastInsertedId);
						sql = "INSERT INTO payments (createdate, credit_slip_id, accrual_id) " +
							"VALUES (@date, @slip, @accrual)";
						// Записываем платеж
						cmd = new MySqlCommand(sql, MainClass.connectionDB, trans);
						cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
						cmd.Parameters.AddWithValue("@accrual", CurrentAccrualId);
						cmd.Parameters.AddWithValue("@slip", Slip_id);
						
						cmd.ExecuteNonQuery ();
						Payment = Convert.ToInt32(cmd.LastInsertedId);
					}

					// Записываем Операции оплаты.
					//separationpayment.PaymentId = Payment;
					separationpayment.SavePaymentDetails (Payment, trans);
				}
				trans.Commit ();
				decimal Balance;

				if(OriginalAccrual != 0 && OriginalAccrual != CurrentAccrualId)
				{
					Accrual.GetAccrualPaidBalance (OriginalAccrual);
				}
				if(CurrentAccrualId > 0)
				{
					Balance = Accrual.GetAccrualPaidBalance(CurrentAccrualId);
					if(Balance < 0)
					{
						MessageDialog md = new MessageDialog( this , DialogFlags.Modal,
						                                     MessageType.Warning, 
						                                     ButtonsType.Ok,"ошибка");
						md.UseMarkup = false;
						md.Text = String.Format ("Внимание! По выбранному начислению произошла переплата на сумму {0:C}", Math.Abs (Balance)) ;
						md.Run ();
						md.Destroy();
					}
				}
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				trans.Rollback ();
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи приходного ордера!");
				MainClass.ErrorMessage(this,ex);
			}

		}

		public void SlipFill(int SlipId)
		{
			NewSlip = false;
			TreeIter iter;
			
			MainClass.StatusMessage(String.Format ("Запрос приходного ордера №{0}...", SlipId));
			string sql = "SELECT credit_slips.*, lessees.name as lessee, users.name as user, " +
				"employees.name as employee, payments.id as payment FROM credit_slips " +
				"LEFT JOIN lessees ON credit_slips.lessee_id = lessees.id " +
				"LEFT JOIN users ON credit_slips.user_id = users.id " +
				"LEFT JOIN employees ON credit_slips.employee_id = employees.id " +
				"LEFT JOIN payments ON payments.credit_slip_id = credit_slips.id " +
				"WHERE credit_slips.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", SlipId);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();

				switch (rdr["operation"].ToString()) 
				{
				case "advance":
					comboOperation.Active = 1;
					break;
				case "payment":
					comboOperation.Active = 2;
					break;
				default:
					comboOperation.Active = 0;
					break;
				}
				entryNumber.Text = rdr["id"].ToString();
				if(rdr["payment"] != DBNull.Value)
					Payment = rdr.GetInt32 ("payment");
				if(rdr["lessee_id"] != DBNull.Value)
				{
					Lessee_id = Convert.ToInt32(rdr["lessee_id"].ToString());
					entryLessee.Text = rdr["lessee"].ToString();
					entryLessee.TooltipText = rdr["lessee"].ToString();
					LesseeNull = false;
				}
				if(rdr["employee_id"] != DBNull.Value)
				{
					Accountable_id = Convert.ToInt32(rdr["employee_id"].ToString());
					entryAccountable.Text = rdr["employee"].ToString();
					entryAccountable.TooltipText = rdr["employee"].ToString();
					AccountableNull = false;
				}
				if(rdr["date"] != DBNull.Value)
					dateSlip.Date = DateTime.Parse( rdr["date"].ToString());
				if(rdr["org_id"] != DBNull.Value)
					MainClass.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
				else
					MainClass.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
				comboOrg.SetActiveIter (iter);
				if(rdr["cash_id"] != DBNull.Value)
					MainClass.SearchListStore((ListStore)comboCash.Model, int.Parse(rdr["cash_id"].ToString()), out iter);
				else
					MainClass.SearchListStore((ListStore)comboCash.Model, -1, out iter);
				comboCash.SetActiveIter (iter);
				if(rdr["income_id"] != DBNull.Value)
					MainClass.SearchListStore((ListStore)comboIncomeItem.Model, int.Parse(rdr["income_id"].ToString()), out iter);
				else
					MainClass.SearchListStore((ListStore)comboIncomeItem.Model, -1, out iter);
				comboIncomeItem.SetActiveIter (iter);
				spinSum.Value = double.Parse (rdr["sum"].ToString());
				if(rdr["user"] != DBNull.Value)
					entryUser.Text = rdr["user"].ToString ();
				else
					entryUser.Text = "";
				textviewDetails.Buffer.Text = rdr["details"].ToString();
				//запоминаем переменные что бы освободить соединение
				object DBContract_no = rdr["contract_no"];
				object DBAccrual = rdr["accrual_id"];

				rdr.Close();

				MainClass.ComboContractFill (comboContract, Lessee_id, false);
				if(DBContract_no != DBNull.Value)
					if(MainClass.SearchListStore((ListStore)comboContract.Model, DBContract_no.ToString(), out iter))
					{
						comboContract.SetActiveIter (iter);
						OnComboContractChanged(null, null);
					}

				if(DBAccrual != DBNull.Value)
				{
					if(MainClass.SearchListStore((ListStore)comboAccrual.Model, Convert.ToInt32(DBAccrual) , out iter))
						comboAccrual.SetActiveIter (iter);
					OriginalAccrual = Convert.ToInt32(DBAccrual);
					// для возможности редактировать старые оплаты
					comboAccrual.Visible = true;
				}
				if(comboOperation.Active == 2)
				{
					separationpayment.PaymentId = Payment;
					separationpayment.AccrualId = OriginalAccrual;
					buttonPrint.Sensitive = true;
					//FIXME Временно пока не налажено корректная обработка смены начисления у созданной оплаты
					comboContract.Sensitive = false;
					comboAccrual.Sensitive = false;
					buttonLesseeEdit.Sensitive = false;
				}
				if(comboOperation.Active == 0)
					buttonPrint.Sensitive = true;

				this.Title = "Приходный ордер №" + entryNumber.Text;
				// Проверяем права на редактирование
				if(!MainClass.User.edit_slips && dateSlip.Date != DateTime.Now.Date)
				{
					comboOrg.Sensitive = false;
					comboCash.Sensitive = false;
					buttonLesseeEdit.Sensitive = false;
					buttonAccountableEdit.Sensitive = false;
					comboContract.Sensitive = false;
					comboAccrual.Sensitive = false;
					comboIncomeItem.Sensitive = false;
					spinSum.Sensitive = false;
					textviewDetails.Sensitive = false;
					separationpayment.Sensitive = false;
				}
				comboOperation.Sensitive = false;

				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о приходном ордере!");
				MainClass.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected void OnComboOperationChanged (object sender, EventArgs e)
		{
			switch (comboOperation.Active) 
			{
			case 0: //common
				VisibleLessee (true);
				VisibleContract (true);
				VisibleAccrual (false);
				VisibleAccountable (false);
				VisibleIncomeItems (true);
				separationpayment.Visible = false;
				break;
			case 1: //advance
				VisibleLessee (false);
				VisibleContract (false);
				VisibleAccrual (false);
				VisibleAccountable (true);
				VisibleIncomeItems (false);
				separationpayment.Visible = false;
				break;
			case 2: //payment
				VisibleLessee (true);
				VisibleContract (true);
				VisibleAccrual (true);
				VisibleAccountable (false);
				VisibleIncomeItems (false);
				separationpayment.Visible = true;
				break;
			}
			this.Resize (1, 1);
			TestCanSave ();
		}

		private void VisibleLessee( bool visible)
		{
			labelLessee.Visible = visible;
			hboxLessee.Visible = visible;
		}

		private void VisibleContract( bool visible)
		{
			labelContract.Visible = visible;
			comboContract.Visible = visible;
		}

		private void VisibleAccrual( bool visible)
		{
			labelAccrual.Visible = visible;
			comboAccrual.Visible = visible;
		}

		private void VisibleAccountable( bool visible)
		{
			labelAccountable.Visible = visible;
			hboxAccountable.Visible = visible;
		}

		private void VisibleIncomeItems( bool visible)
		{
			labelIncomeItem.Visible = visible;
			comboIncomeItem.Visible = visible;
		}

		protected void OnButtonAccountableEditClicked (object sender, EventArgs e)
		{
			reference AccountableSelect = new reference();
			AccountableSelect.SetMode(true,true,true,true,false);
			AccountableSelect.FillList("employees","Сотрудник", "Сотрудники");
			AccountableSelect.Show();
			int result = AccountableSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Accountable_id = AccountableSelect.SelectedID;
				AccountableNull = false;
				entryAccountable.Text = AccountableSelect.SelectedName;
				entryAccountable.TooltipText = AccountableSelect.SelectedName;
			}
			AccountableSelect.Destroy();
			TestCanSave ();
		}

		protected void OnSpinSumChangeValue (object o, ChangeValueArgs args)
		{
			TestCanSave ();
		}

		protected void OnComboContractChanged (object sender, EventArgs e)
		{
			string sql = "SELECT id, DATE(CONCAT('2012-', month, '-1')) as month, year FROM accrual " +
				"WHERE contract_no = @contract";
			MySqlParameter[] Param = { new MySqlParameter("@contract", comboContract.ActiveText) };
			string Display = "№{0} - {1:MMMM} {2}";
			MainClass.ComboFillUniversal (comboAccrual, sql, Display, Param, 0, 2);
		}

		protected void OnComboAccrualChanged (object sender, EventArgs e)
		{
			TreeIter AccrualIter;
			if(comboAccrual.Active > 0 && comboAccrual.GetActiveIter(out AccrualIter))
				separationpayment.AccrualId = (int)comboAccrual.Model.GetValue(AccrualIter,1);
			else
				separationpayment.AccrualId = 0;
		}

		protected void OnButtonPrintClicked (object sender, EventArgs e)
		{
			string param = "id=" + entryNumber.Text;
			switch(comboOperation.Active)
			{
			case 0:
				ReportsExt.ViewReport ("CommonTicket", param);
				break;
			case 2:
				ReportsExt.ViewReport ("PaymentTicket", param);
				break;
			}
		}

		protected void OnSeparationpaymentCanSaveStateChanged (object sender, EventArgs e)
		{
			TestCanSave ();
		}
	}
}

