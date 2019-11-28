using System;
using Bazar;
using Bazar.Dialogs.Rental;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace Bazar.Dialogs.Payments
{
	public partial class IncomeSlipDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		public bool NewSlip;
		int Lessee_id;
		int Accountable_id;
		int Income_id;
		int OriginalAccrual = 0;
		int Payment = 0;
		bool LesseeNull = true;
		bool IncomeNull = true;
		bool AccountableNull = true;

		public IncomeSlipDlg ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboCash,"cash", ComboWorks.ListMode.WithNo);
			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithNo, OrderBy: "name");

			//Заполняем поля по умолчанию
			dateSlip.Date = DateTime.Now.Date;
			entryUser.Text = QSMain.User.Name;
			if(QSMain.User.Permissions["edit_slips"])
				dateSlip.Sensitive = true;
			OnComboOperationChanged (null, null);
		}

		protected void OnButtonLesseeEditClicked (object sender, EventArgs e)
		{
			Reference LesseeSelect = new Reference(orderBy: "name");
			LesseeSelect.SetMode(false,true,true,true,false);
			LesseeSelect.FillList("lessees","Арендатор", "Арендаторы");
			LesseeSelect.Show();
			int result = LesseeSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Lessee_id = LesseeSelect.SelectedID;
				LesseeNull = false;
				entryLessee.Text = LesseeSelect.SelectedName;
				entryLessee.TooltipText = LesseeSelect.SelectedName;
				MainClass.ComboContractFill (comboContract, Lessee_id, NewSlip);
			}
			LesseeSelect.Destroy();
			TestCanSave ();
		}

		protected void TestCanSave ()
		{
			bool Orgok = comboOrg.Active > 0;
			bool Cashok = comboCash.Active > 0;
			bool Lesseeok = !LesseeNull;
			bool Paymentok = separationpayment.CanSave;
			bool Rightok = QSMain.User.Permissions["edit_slips"] || NewSlip;
			bool Sumok = Convert.ToDecimal (spinSum.Value) > 0; 
			bool Accountableok = !AccountableNull;

			switch (comboOperation.Active) 
			{
			case 0:
				buttonOk.Sensitive = Orgok && Cashok && Rightok && Lesseeok && !IncomeNull && Sumok;
				break;
			case 1:
				buttonOk.Sensitive = Orgok && Cashok && Rightok && Accountableok && Sumok;
				break;
			case 2:
				buttonOk.Sensitive = Orgok && Cashok && Rightok && Lesseeok && Paymentok && Sumok;
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
					"contract_id, accrual_id, income_id, employee_id, details) " +
						"VALUES (@operation, @org_id, @cash_id, @lessee_id, @user_id, @date, @sum, " +
						"@contract_id, @accrual_id, @income_id, @employee_id, @details)";
			}
			else
			{
				sql = "UPDATE credit_slips SET operation = @operation, org_id = @org_id, cash_id = @cash_id, lessee_id = @lessee_id, " +
						"date = @date, sum = @sum, contract_id = @contract_id, accrual_id = @accrual_id, income_id = @income_id, " +
						"employee_id = @employee_id, details = @details " +
						"WHERE id = @id";
			}
			logger.Info("Запись Приходного ордера...");
			MySqlTransaction trans = QSMain.connectionDB.BeginTransaction ();
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
				
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
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.Id);
				if(dateSlip.IsEmpty)
					cmd.Parameters.AddWithValue("@date", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@date", dateSlip.Date);
				cmd.Parameters.AddWithValue("@sum", spinSum.Value);
				comboContract.GetActiveIter(out iter);
				if((comboOperation.Active == 0 || comboOperation.Active == 2) && comboContract.Active >= 0)
					cmd.Parameters.AddWithValue("@contract_id", comboContract.Model.GetValue (iter, 1));
				else
					cmd.Parameters.AddWithValue("@contract_id", DBNull.Value);
				int CurrentAccrualId;
				if((comboOperation.Active == 0 || comboOperation.Active == 2) && comboAccrual.Active > 0 && comboAccrual.GetActiveIter(out AccrualIter))
				{
					CurrentAccrualId = (int)comboAccrual.Model.GetValue(AccrualIter,1);
					cmd.Parameters.AddWithValue("@accrual_id", CurrentAccrualId);
				}
				else if(OriginalAccrual > 0)
				{
					CurrentAccrualId = OriginalAccrual;
					cmd.Parameters.AddWithValue("@accrual_id", CurrentAccrualId);
				}
				else
				{
					cmd.Parameters.AddWithValue("@accrual_id", DBNull.Value);
					CurrentAccrualId = -1;
				}
				if(!IncomeNull)
					cmd.Parameters.AddWithValue("@income_id", Income_id);
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
						cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
						cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
						cmd.Parameters.AddWithValue("@accrual", CurrentAccrualId);
						cmd.Parameters.AddWithValue("@slip", Slip_id);
						
						cmd.ExecuteNonQuery ();
						Payment = Convert.ToInt32(cmd.LastInsertedId);
					}

					// Записываем Операции оплаты.
					separationpayment.SavePaymentDetails (Payment, trans);
				}
				trans.Commit ();
				decimal Balance;

				if(OriginalAccrual != 0 && OriginalAccrual != CurrentAccrualId)
				{
					AccrualDlg.GetAccrualPaidBalance (OriginalAccrual);
				}
				if(CurrentAccrualId > 0)
				{
					Balance = AccrualDlg.GetAccrualPaidBalance(CurrentAccrualId);
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
				logger.Info("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				trans.Rollback ();
				QSMain.ErrorMessageWithLog(this, "Ошибка записи приходного ордера!", logger, ex);
			}

		}

		public void SlipFill(int SlipId, bool Copy)
		{
			NewSlip = Copy;

			TreeIter iter;
			
			logger.Info("Запрос приходного ордера №{0}...", SlipId);
			string sql = "SELECT credit_slips.*, lessees.name as lessee, users.name as user, " +
				"employees.name as employee, payments.id as payment, contracts.number as contract, " +
				"contracts.sign_date, income_items.name as income FROM credit_slips " +
				"LEFT JOIN lessees ON credit_slips.lessee_id = lessees.id " +
				"LEFT JOIN users ON credit_slips.user_id = users.id " +
				"LEFT JOIN employees ON credit_slips.employee_id = employees.id " +
				"LEFT JOIN payments ON payments.credit_slip_id = credit_slips.id " +
				"LEFT JOIN contracts ON contracts.id = credit_slips.contract_id " +
				"LEFT JOIN income_items ON income_items.id = credit_slips.income_id " +
				"WHERE credit_slips.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
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
				if(!Copy)
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
				if(rdr["date"] != DBNull.Value && !Copy)
					dateSlip.Date = DateTime.Parse( rdr["date"].ToString());
				if(rdr["org_id"] != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, int.Parse(rdr["org_id"].ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboOrg.Model, -1, out iter);
				comboOrg.SetActiveIter (iter);
				if(rdr["cash_id"] != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboCash.Model, int.Parse(rdr["cash_id"].ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboCash.Model, -1, out iter);
				comboCash.SetActiveIter (iter);
				if(rdr["income_id"] != DBNull.Value)
				{
					Income_id = Convert.ToInt32(rdr["income_id"].ToString());
					entryIncome.Text = rdr["income"].ToString();
					entryIncome.TooltipText = rdr["income"].ToString();
					IncomeNull = false;
				}
				spinSum.Value = double.Parse (rdr["sum"].ToString());
				if(rdr["user"] != DBNull.Value && !Copy)
					entryUser.Text = rdr["user"].ToString ();
				textviewDetails.Buffer.Text = rdr["details"].ToString();
				//запоминаем переменные что бы освободить соединение
				object DBContract_id = rdr["contract_id"];
				object DBContract_number = rdr["contract"];
				object DBContract_sign = rdr["sign_date"];
				object DBAccrual = rdr["accrual_id"];

				rdr.Close();

				MainClass.ComboContractFill (comboContract, Lessee_id, false);
				bool ContractOk = false;
				if(DBContract_id != DBNull.Value)
				{
					if(ListStoreWorks.SearchListStore((ListStore)comboContract.Model, Convert.ToInt32(DBContract_id), out iter))
					{
						comboContract.SetActiveIter (iter);
						OnComboContractChanged(null, null);
						ContractOk = true;
					}
					else
					{ //Возможно у договора поменялся арендатор.
						logger.Info("Договор не найден у арендатора! Добавляем в список...");
						string ContractText = String.Format("{0} от {1}", DBContract_number, DBContract_sign);
						iter = ((ListStore) comboContract.Model).AppendValues(ContractText, Convert.ToInt32 (DBContract_id));
						comboContract.SetActiveIter (iter);
						OnComboContractChanged(null, null);
						ContractOk = true;
					}
				}
				if(DBAccrual != DBNull.Value && ContractOk)
				{
					if(ListStoreWorks.SearchListStore((ListStore)comboAccrual.Model, Convert.ToInt32(DBAccrual) , out iter))
						comboAccrual.SetActiveIter (iter);
					OriginalAccrual = Convert.ToInt32(DBAccrual);
					// для возможности редактировать старые оплаты
					comboAccrual.Visible = true;
				}
				if(comboOperation.Active == 2)
				{
					separationpayment.PaymentId = Payment;
					separationpayment.AccrualId = OriginalAccrual;
					//FIXME Временно пока не налажено корректная обработка смены начисления у созданной оплаты
					comboContract.Sensitive = false;
					comboAccrual.Sensitive = false;
					buttonLesseeEdit.Sensitive = false;
				}

				if(!NewSlip)
				{
					this.Title = "Приходный ордер №" + entryNumber.Text;
					buttonPrint.Sensitive = true;
				}
				// Проверяем права на редактирование
				if(!QSMain.User.Permissions["edit_slips"] && dateSlip.Date != DateTime.Now.Date && !Copy)
				{
					comboOrg.Sensitive = false;
					comboCash.Sensitive = false;
					buttonLesseeEdit.Sensitive = false;
					buttonAccountableEdit.Sensitive = false;
					comboContract.Sensitive = false;
					comboAccrual.Sensitive = false;
					spinSum.Sensitive = false;
					textviewDetails.Sensitive = false;
					separationpayment.Sensitive = false;
				}
				comboOperation.Sensitive = false;

				logger.Info("Ok");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о приходном ордере!", logger, ex);
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
			entryIncome.Visible = visible;
			buttonIncome.Visible = visible;
		}

		protected void OnButtonAccountableEditClicked (object sender, EventArgs e)
		{
			Reference AccountableSelect = new Reference(orderBy: "name");
			AccountableSelect.SetMode(true,true,true,true,false);
			AccountableSelect.NameMaxLength = 45;
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
			TreeIter iter;
			string sql = "SELECT id, DATE(CONCAT('2012-', month, '-1')) as month, year FROM accrual " +
				"WHERE contract_id = @contract";
			comboContract.GetActiveIter (out iter);
			MySqlParameter[] Param = { new MySqlParameter("@contract", comboContract.Model.GetValue (iter, 1)) };
			string Display = "№{0} - {1:MMMM} {2}";
			ComboWorks.ComboFillUniversal (comboAccrual, sql, Display, Param, 0, ComboWorks.ListMode.WithNo);
			if (!comboAccrual.Visible)
				comboAccrual.Active = 0;
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
				ViewReportExt.Run ("CommonTicket", param);
				break;
			case 1:
				ViewReportExt.Run ("ReturnTicket", param);
				break;
			case 2:
				ViewReportExt.Run ("PaymentTicket", param);
				break;
			}
		}

		protected void OnSeparationpaymentCanSaveStateChanged (object sender, EventArgs e)
		{
			TestCanSave ();
		}

		protected void OnButtonIncomeClicked (object sender, EventArgs e)
		{
			Reference IncomeSelect = new Reference(orderBy: "name");
			IncomeSelect.SetMode(true,true,true,true,false);
			IncomeSelect.NameMaxLength = 45;
			IncomeSelect.FillList("income_items","Статья доходов", "Статьи доходов");
			IncomeSelect.Show();
			int result = IncomeSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Income_id = IncomeSelect.SelectedID;
				IncomeNull = false;
				entryIncome.Text = IncomeSelect.SelectedName;
				entryIncome.TooltipText = IncomeSelect.SelectedName;
			}
			IncomeSelect.Destroy();
			TestCanSave ();
		}
	}
}

