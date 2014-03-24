using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class ExpenseSlip : Gtk.Dialog
	{
		public bool NewSlip;
		int Contractor_id;
		int Accountable_id;
		bool ContractorNull = true;
		bool AccountableNull = true;

		public ExpenseSlip ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboCash,"cash", ComboWorks.ListMode.WithNo);
			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithNo);
			ComboWorks.ComboFillReference(comboExpenseItem,"expense_items", ComboWorks.ListMode.WithNo);
			
			//Заполняем поля по умолчанию
			dateSlip.Date = DateTime.Now.Date;
			entryUser.Text = QSMain.User.Name;
			if(QSMain.User.Permissions["edit_slips"])
				dateSlip.Sensitive = true;
		}

		protected void TestCanSave ()
		{
			bool Orgok = comboOrg.Active > 0;
			bool Cashok = comboCash.Active > 0;
			bool Itemok = comboExpenseItem.Active > 0;
			bool Sumok;
			if(spinSum.Text != "")
				Sumok = Convert.ToDecimal (spinSum.Text) != 0; 
			else
				Sumok = false;
			bool Accountableok = !AccountableNull;

			switch (comboOperation.Active) 
			{
			case 0:
				buttonOk.Sensitive = Orgok && Cashok && Itemok && Sumok;
				break;
			case 1:
				buttonOk.Sensitive = Orgok && Cashok && Accountableok && Sumok;
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
		}
		
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string sql;
			TreeIter iter;
			
			if(NewSlip)
			{
				sql = "INSERT INTO debit_slips (operation, org_id, cash_id, contractor_id, user_id, date, sum, " +
					"expense_id, employee_id, details) " +
						"VALUES (@operation, @org_id, @cash_id, @contractor_id, @user_id, @date, @sum, " +
						"@expense_id, @employee_id, @details)";
			}
			else
			{
				sql = "UPDATE debit_slips SET operation = @operation, org_id = @org_id, cash_id = @cash_id, contractor_id = @contractor_id, " +
					"date = @date, sum = @sum, expense_id = @expense_id, employee_id = @employee_id, " +
						"details = @details " +
						"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись расходного ордера...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", entryNumber.Text);
				if(comboOperation.Active == 1)
					cmd.Parameters.AddWithValue ("@operation","advance");
				else
					cmd.Parameters.AddWithValue ("@operation","common");
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
					cmd.Parameters.AddWithValue("@org_id",comboOrg.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@org_id", DBNull.Value);
				if(comboCash.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@cash_id", comboCash.Model.GetValue(iter,1));
				}	
				if(comboOperation.Active == 0 && !ContractorNull)
					cmd.Parameters.AddWithValue("@contractor_id", Contractor_id);
				else
					cmd.Parameters.AddWithValue("@contractor_id", DBNull.Value);
				if(!AccountableNull)
					cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				else
					cmd.Parameters.AddWithValue("@employee_id", DBNull.Value);
				if(NewSlip)
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.id);
				if(dateSlip.IsEmpty)
					cmd.Parameters.AddWithValue("@date", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@date", dateSlip.Date);
				cmd.Parameters.AddWithValue("@sum", spinSum.Value);
				if(comboExpenseItem.GetActiveIter(out iter) && comboExpenseItem.Active > 0)
					cmd.Parameters.AddWithValue("@expense_id", comboExpenseItem.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@expense_id", DBNull.Value);
				if(textviewDetails.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@details", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@details", textviewDetails.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи расходного ордера!");
				QSMain.ErrorMessage(this,ex);
			}
			
		}
		
		public void SlipFill(int SlipId, bool Copy)
		{
			NewSlip = Copy;

			TreeIter iter;
			
			MainClass.StatusMessage(String.Format("Запрос расходного ордера №{0}...", SlipId));
			string sql = "SELECT debit_slips.*, contractors.name as contractor, users.name as user, " +
					"employees.name as employee FROM debit_slips " +
					"LEFT JOIN contractors ON debit_slips.contractor_id = contractors.id " +
					"LEFT JOIN users ON debit_slips.user_id = users.id " +
					"LEFT JOIN employees ON debit_slips.employee_id = employees.id " +
					"WHERE debit_slips.id = @id";
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
				default:
					comboOperation.Active = 0;
				break;
				}
				if(!Copy)
					entryNumber.Text = rdr["id"].ToString();
				if(rdr["contractor_id"] != DBNull.Value)
				{
					Contractor_id = Convert.ToInt32(rdr["contractor_id"].ToString());
					entryContractor.Text = rdr["contractor"].ToString();
					entryContractor.TooltipText = rdr["contractor"].ToString();
					ContractorNull = false;
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
				if(rdr["expense_id"] != DBNull.Value)
					ListStoreWorks.SearchListStore((ListStore)comboExpenseItem.Model, int.Parse(rdr["expense_id"].ToString()), out iter);
				else
					ListStoreWorks.SearchListStore((ListStore)comboExpenseItem.Model, -1, out iter);
				comboExpenseItem.SetActiveIter (iter);
				spinSum.Value = double.Parse (rdr["sum"].ToString());
				if(rdr["user"] != DBNull.Value && !Copy)
					entryUser.Text = rdr["user"].ToString ();
				textviewDetails.Buffer.Text = rdr["details"].ToString();

				rdr.Close();
				if(!NewSlip)
				{
					this.Title = "Расходный ордер №" + entryNumber.Text;
					buttonPrint.Sensitive = true;
				}
				// Проверяем права на редактирование
				if(!QSMain.User.Permissions["edit_slips"] && dateSlip.Date != DateTime.Now.Date && !Copy)
				{
					comboOperation.Sensitive = false;
					comboOrg.Sensitive = false;
					comboCash.Sensitive = false;
					buttonContractorEdit.Sensitive = false;
					buttonAccountableEdit.Sensitive = false;
					comboExpenseItem.Sensitive = false;
					spinSum.Sensitive = false;
					textviewDetails.Sensitive = false;
				}
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о расходном ордере!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected void OnButtonContractorEditClicked (object sender, EventArgs e)
		{
			Reference ContractorSelect = new Reference();
			ContractorSelect.SetMode(true,true,true,true,false);
			ContractorSelect.FillList("contractors","Контрагент", "Контрагенты");
			ContractorSelect.Show();
			int result = ContractorSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Contractor_id = ContractorSelect.SelectedID;
				ContractorNull = false;
				//buttonContactOpen.Sensitive = true;
				entryContractor.Text = ContractorSelect.SelectedName;
				entryContractor.TooltipText = ContractorSelect.SelectedName;
			}
			ContractorSelect.Destroy();
			TestCanSave ();
		}

		protected void OnComboOperationChanged (object sender, EventArgs e)
		{
			switch (comboOperation.Active) 
			{
			case 0: //common
				entryContractor.Sensitive = true;
				buttonContractorEdit.Sensitive = true;
				labelExpenseItem.LabelProp = "Статья расхода<span foreground=\"red\">*</span>:";
				labelAccountable.LabelProp = "Получатель:";
				break;
			case 1: //advance
				entryContractor.Sensitive = false;
				buttonContractorEdit.Sensitive = false;
				labelExpenseItem.LabelProp = "Статья расхода:";
				labelAccountable.LabelProp = "Подотчетное лицо<span foreground=\"red\">*</span>:";
				break;
			}
			TestCanSave ();
		}

		protected void OnButtonAccountableEditClicked (object sender, EventArgs e)
		{
			Reference AccountableSelect = new Reference();
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

		protected void OnButtonPrintClicked (object sender, EventArgs e)
		{
			string param = "id=" + entryNumber.Text;
			ViewReportExt.Run ("Expense", param);
		}
	}
}

