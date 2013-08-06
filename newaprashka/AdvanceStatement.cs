using System;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class AdvanceStatement : Gtk.Dialog
	{
		public bool NewStatement;
		int Contractor_id;
		int Accountable_id;
		bool ContractorNull = true;
		bool AccountableNull = true;
		decimal Debt = 0;
		decimal Balance = 0;
		ListStore AdvancesListStore;

		public AdvanceStatement ()
		{
			this.Build ();

			ComboWorks.ComboFillReference(comboCash,"cash",2);
			ComboWorks.ComboFillReference(comboOrg, "organizations", 2);
			ComboWorks.ComboFillReference(comboExpenseItem,"expense_items",2);
			
			//Заполняем поля по умолчанию
			dateStatement.Date = DateTime.Now.Date;
			entryUser.Text = QSMain.User.Name;
			if(QSMain.User.Permissions["edit_slips"])
				dateStatement.Sensitive = true;

			//Создаем таблицу авансов
			AdvancesListStore = new Gtk.ListStore (typeof (int), typeof (string), typeof (int), typeof (string), typeof (int), typeof (string),
			                                          typeof (string));
			
			Gtk.TreeViewColumn OrgIDColumn2 = new Gtk.TreeViewColumn ();
			OrgIDColumn2.Title = "ID Организации";
			OrgIDColumn2.Visible = false;
			OrgIDColumn2.PackStart (new Gtk.CellRendererText (), true);
			Gtk.TreeViewColumn CashColumn2 = new Gtk.TreeViewColumn ();
			CashColumn2.Title = "ID Касса";
			CashColumn2.Visible = false;
			CashColumn2.PackStart (new Gtk.CellRendererText (), true);

			treeviewAdvances.AppendColumn("Номер", new Gtk.CellRendererText (), "text", 0);
			treeviewAdvances.AppendColumn("Дата", new Gtk.CellRendererText (), "text", 1);
			treeviewAdvances.AppendColumn(CashColumn2);
			treeviewAdvances.AppendColumn("Касса", new Gtk.CellRendererText (), "text", 3);
			treeviewAdvances.AppendColumn(OrgIDColumn2);
			treeviewAdvances.AppendColumn("Организация", new Gtk.CellRendererText (), "text", 5);
			treeviewAdvances.AppendColumn("Сумма", new Gtk.CellRendererText (), "text", 6);
			
			treeviewAdvances.Model = AdvancesListStore;
			treeviewAdvances.ShowAll();
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
			
			buttonOk.Sensitive = Orgok && Cashok && Accountableok && Itemok && Sumok;
		}

		protected void OnComboOrgChanged (object sender, EventArgs e)
		{
			if(!AccountableNull)
				FillDebt ();
			TestCanSave ();
		}

		protected void OnComboCashChanged (object sender, EventArgs e)
		{
			if(!AccountableNull)
				FillDebt ();
			TestCanSave ();
		}

		protected void OnComboExpenseItemChanged (object sender, EventArgs e)
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

			if(Balance < 0 && !checkCreateSlip.Active)
			{
				MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,
				                                      MessageType.Question, 
				                                      ButtonsType.YesNo, 
				                                      String.Format ("Если вы сохраните авансовый отчет, организация будет должна подотчетному лицу {0:C}. Вы уверены что хотите сохранит авансовый отчет?", Math.Abs (Balance)));
				ResponseType result = (ResponseType)md.Run ();
				md.Destroy();
				if(result == ResponseType.No)
				{
					return;
				}
			}

			if(NewStatement)
			{
				sql = "INSERT INTO advance (org_id, cash_id, contractor_id, user_id, date, sum, " +
					"expense_id, employee_id, details) " +
						"VALUES (@org_id, @cash_id, @contractor_id, @user_id, @date, @sum, " +
						"@expense_id, @employee_id, @details)";
			}
			else
			{
				sql = "UPDATE advance SET org_id = @org_id, cash_id = @cash_id, contractor_id = @contractor_id, " +
					"date = @date, sum = @sum, expense_id = @expense_id, employee_id = @employee_id, " +
						"details = @details " +
						"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись авансового отчета...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", entryNumber.Text);
				if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
					cmd.Parameters.AddWithValue("@org_id",comboOrg.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@org_id", DBNull.Value);
				if(comboCash.GetActiveIter(out iter))
				{
					cmd.Parameters.AddWithValue("@cash_id", comboCash.Model.GetValue(iter,1));
				}	
				if(!ContractorNull)
					cmd.Parameters.AddWithValue("@contractor_id", Contractor_id);
				else
					cmd.Parameters.AddWithValue("@contractor_id", DBNull.Value);
				if(!AccountableNull)
					cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				else
					cmd.Parameters.AddWithValue("@employee_id", DBNull.Value);
				if(NewStatement)
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.id);
				if(dateStatement.IsEmpty)
					cmd.Parameters.AddWithValue("@date", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@date", dateStatement.Date);
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

				if(checkCreateSlip.Active && Balance != 0)
				{
					string detailstext, messagetext;
					if(Balance > 0)
					{
						sql = "INSERT INTO credit_slips (operation, org_id, cash_id, user_id, date, sum, " +
							"employee_id, details) " +
							"VALUES (@operation, @org_id, @cash_id, @user_id, @date, @sum, " +
							"@employee_id, @details)";
						detailstext = "Возврат в кассу денежных средств по авансовому отчету №" + cmd.LastInsertedId.ToString ();
						messagetext = "Дополнительно создан приходный ордер №{0}, на сумму {1:C}.\nНе забудьте получить сдачу от подотчетного лица!";
					}
					else
					{
						sql = "INSERT INTO debit_slips (operation, org_id, cash_id, user_id, date, sum, " +
							"employee_id, details) " +
							"VALUES (@operation, @org_id, @cash_id, @user_id, @date, @sum, " +
							"@employee_id, @details)";
						detailstext = "Доплата денежных средств сотруднику по авансовому отчету №" + cmd.LastInsertedId.ToString ();
						messagetext = "Дополнительно создан расходный ордер №{0}, на сумму {1:C}.\nНе забудьте доплатить подотчетному лицу!";
					}

					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					
					cmd.Parameters.AddWithValue ("@operation","advance");

					if(comboOrg.GetActiveIter(out iter) && (int)comboOrg.Model.GetValue(iter,1) != -1)
						cmd.Parameters.AddWithValue("@org_id",comboOrg.Model.GetValue(iter,1));
					else
						cmd.Parameters.AddWithValue("@org_id", DBNull.Value);
					if(comboCash.GetActiveIter(out iter))
					{
						cmd.Parameters.AddWithValue("@cash_id", comboCash.Model.GetValue(iter,1));
					}	
					cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
					cmd.Parameters.AddWithValue("@user_id", QSMain.User.id);
					if(dateStatement.IsEmpty)
						cmd.Parameters.AddWithValue("@date", DBNull.Value);
					else
						cmd.Parameters.AddWithValue("@date", dateStatement.Date);
					cmd.Parameters.AddWithValue("@sum", Math.Abs (Balance));
					if(textviewDetails.Buffer.Text == "")
						cmd.Parameters.AddWithValue("@details", detailstext);
					else
						cmd.Parameters.AddWithValue("@details", detailstext + "\n" + textviewDetails.Buffer.Text);
					
					cmd.ExecuteNonQuery();
					MessageDialog md = new MessageDialog (this, DialogFlags.DestroyWithParent,
					                                      MessageType.Info, 
					                                      ButtonsType.Ok, 
					                                      String.Format(messagetext, cmd.LastInsertedId, Math.Abs (Balance)));
					md.Run ();
					md.Destroy();
				}

				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи авансового отчета!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		public void StatementFill(int StatementId, bool Copy)
		{
			if(Copy)
				NewStatement = true;
			else
				NewStatement = false;

			TreeIter iter;
			
			MainClass.StatusMessage("Запрос авансового отчета №" + StatementId +"...");
			string sql = "SELECT advance.*, contractors.name as contractor, users.name as user, " +
				"employees.name as employee FROM advance " +
					"LEFT JOIN contractors ON advance.contractor_id = contractors.id " +
					"LEFT JOIN users ON advance.user_id = users.id " +
					"LEFT JOIN employees ON advance.employee_id = employees.id " +
					"WHERE advance.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", StatementId);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				rdr.Read();
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
				if(!Copy)
				{
					if(rdr["date"] != DBNull.Value)
						dateStatement.Date = DateTime.Parse( rdr["date"].ToString());
				}
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
				if(!Copy)
				{
					if(rdr["user"] != DBNull.Value)
						entryUser.Text = rdr["user"].ToString ();
					else
						entryUser.Text = "";
				}
				textviewDetails.Buffer.Text = rdr["details"].ToString();
				
				rdr.Close();

				if(!NewStatement)
					this.Title = "Авансовый отчет №" + entryNumber.Text;
				// Проверяем права на редактирование
				if(!Copy)
				{
					if(!QSMain.User.Permissions["edit_slips"] && dateStatement.Date != DateTime.Now.Date)
					{
						comboOrg.Sensitive = false;
						comboCash.Sensitive = false;
						buttonContractorEdit.Sensitive = false;
						buttonAccountableEdit.Sensitive = false;
						comboExpenseItem.Sensitive = false;
						spinSum.Sensitive = false;
						textviewDetails.Sensitive = false;
					}
				}
				checkCreateSlip.Sensitive = false;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о авансовом отчете!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}

		protected void OnButtonContractorEditClicked (object sender, EventArgs e)
		{
			reference ContractorSelect = new reference();
			ContractorSelect.SetMode(true,true,true,true,false);
			ContractorSelect.FillList("contractors","Контрагент", "Контрагенты");
			ContractorSelect.Show();
			int result = ContractorSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				Contractor_id = ContractorSelect.SelectedID;
				ContractorNull = false;
				entryContractor.Text = ContractorSelect.SelectedName;
				entryContractor.TooltipText = ContractorSelect.SelectedName;
			}
			ContractorSelect.Destroy();
			TestCanSave ();
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
				FillDebt ();
			}
			AccountableSelect.Destroy();
			TestCanSave ();
		}

		void FillDebt()
		{
			if(!NewStatement)
				return;
			TreeIter iter;
			
			MainClass.StatusMessage("Получаем долг " + entryAccountable.Text +"...");
			string sqlwhere = "";
			if(comboOrg.Active > 0 && comboOrg.GetActiveIter(out iter))
				sqlwhere += " AND org_id = '" + comboOrg.Model.GetValue(iter,1) + "' ";
			if(comboCash.Active > 0 && comboCash.GetActiveIter(out iter))
				sqlwhere += " AND cash_id = '" + comboCash.Model.GetValue(iter,1) + "' ";
			string sql = "SELECT SUM(count) as debt FROM ( " +
				"SELECT SUM(debit_slips.sum) as count FROM debit_slips WHERE debit_slips.employee_id = @employee_id " + sqlwhere +
				"UNION ALL SELECT -SUM(credit_slips.sum) as count FROM credit_slips WHERE credit_slips.employee_id = @employee_id " + sqlwhere +
				"UNION ALL SELECT -SUM(advance.sum) as count FROM advance WHERE advance.employee_id = @employee_id " + sqlwhere + " ) AS slips";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				if(rdr.Read() && rdr["debt"] != DBNull.Value)
				{
					Debt = rdr.GetDecimal("debt");
					labelDebt.LabelProp = String.Format ("{0:C}", rdr["debt"]);
				}
				else
				{
					Debt = 0m;
					labelDebt.LabelProp = String.Format ("{0:C}", 0);
				}
				rdr.Close();
				AdvancesListStore.Clear();
				if(Debt <= 0)
				{
					CalculateBalance ();
					MainClass.StatusMessage("Ok");
					return;
				}

				sql = "SELECT debit_slips.*,  cash.name as cash, organizations.name as organization FROM debit_slips " +
						"LEFT JOIN cash ON debit_slips.cash_id = cash.id " +
						"LEFT JOIN organizations ON debit_slips.org_id = organizations.id " +
						"WHERE employee_id = @employee_id " + sqlwhere +
						" ORDER BY date DESC, id DESC " +
						"LIMIT 20";
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@employee_id", Accountable_id);
				rdr = cmd.ExecuteReader();

				decimal remainDebt = Debt;
				decimal slipsum;
				while (rdr.Read())
				{
					slipsum = Convert.ToDecimal(rdr["sum"]);
					remainDebt -= slipsum;
					AdvancesListStore.AppendValues(int.Parse (rdr["id"].ToString()),
					                                  DateTime.Parse(rdr["date"].ToString()).ToShortDateString(),
					                                  int.Parse(rdr["cash_id"].ToString ()),
					                                  rdr["cash"].ToString (),
					                                  int.Parse (rdr["org_id"].ToString ()),
					                                  rdr["organization"].ToString(),
					                               	  String.Format ("{0:C}", rdr["sum"]));
					if(remainDebt <= 0)
						break;
				}
				rdr.Close();
				CalculateBalance ();
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Не удалось получить долг!");
				QSMain.ErrorMessage(this,ex);
			}
		}

		protected void OnSpinSumChanged (object sender, EventArgs e)
		{
			CalculateBalance ();
		}

		protected void CalculateBalance()
		{
			if(!NewStatement)
				return;
			if(spinSum.Text != "")
				Balance = Debt - Convert.ToDecimal (spinSum.Text);
			else
				Balance = Debt - 0m;
			if(Balance < 0)
			{
				labelDiffName.LabelProp = "Доплата:";
				labelBalance.LabelProp = string.Format("<span foreground=\"red\">{0:C}</span>", Math.Abs(Balance));
				checkCreateSlip.Label = "Создать расходный ордер на сумму доплаты";
			}
			else
			{
				labelDiffName.LabelProp = "Остаток:";
				labelBalance.LabelProp = string.Format("{0:C}", Balance);
				checkCreateSlip.Label = "Создать приходный ордер на сумму остатка";
			}
			TestCanSave ();
		}
	}
}

