using System;
using Gtk;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using QSProjectsLib;
using NLog;

namespace bazar
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class SeparationPayment : Gtk.Bin
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private decimal _PaymentSum;
		private int _AccrualId, _PaymentId;
		private bool _CanSave = false;
		private Gtk.ListStore ServiceListStore, AccrualRowsListStore;
		private TreeModel IncomeNameList;
		TreeModelFilter AccrualRowsFilter;
		private List<long> DeletedRowId = new List<long>();

		public event EventHandler CanSaveStateChanged;

		public decimal PaymentSum {
			get {return _PaymentSum;}
			set {_PaymentSum = value;
				CalculateTotal ();}
		}

		public int PaymentId {
			get {return _PaymentId;}
			set {_PaymentId = value;
				FillPaymentDetails ();}
		}

		public int AccrualId {
			get {return _AccrualId;}
			set {_AccrualId = value;
				FillAccrualRowsList ();}
		}

		public bool CanSave {
			get {return _CanSave;}
		}

		public SeparationPayment ()
		{
			this.Build ();

			ComboBox IncomeCombo = new ComboBox();
			ComboWorks.ComboFillReference(IncomeCombo,"income_items", ComboWorks.ListMode.OnlyItems, OrderBy: "name");
			IncomeNameList = IncomeCombo.Model;
			IncomeCombo.Destroy ();

			//Создаем таблицу "Услуги"
			ServiceListStore = new Gtk.ListStore (typeof (long), //0 payment row id
			                                      typeof (long), //1 accrual row id
			                                      typeof (string), //2 service name
			                                      typeof (int),//3 income id
			                                      typeof (string), //4 income name
			                                      typeof (decimal),// 5 accrual sum
			                                      typeof (decimal), // 6 paid sum
			                                      typeof (decimal) // 7 other paid
			                                      );
			
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

			Gtk.TreeViewColumn AccrualColumn = new Gtk.TreeViewColumn ();
			AccrualColumn.Title = "Начислено";
			Gtk.CellRendererText CellAccrual = new CellRendererText();
			AccrualColumn.PackStart (CellAccrual, true);
			
			Gtk.TreeViewColumn PaidColumn = new Gtk.TreeViewColumn ();
			PaidColumn.Title = "Оплата";
			PaidColumn.MinWidth = 90;
			Gtk.CellRendererText CellPaid = new CellRendererText();
			CellPaid.Editable = true;
			CellPaid.Edited += OnPaidTextEdited;
			PaidColumn.PackStart (CellPaid, true);
			
			Gtk.TreeViewColumn OtherPaidColumn = new Gtk.TreeViewColumn ();
			OtherPaidColumn.Title = "Другие платежи";
			Gtk.CellRendererText CellOtherPaid = new CellRendererText();
			OtherPaidColumn.PackStart (CellOtherPaid, true);
			
			//ID payment row - 0
			//ID accrual row - 1
			treeviewServices.AppendColumn ("Услуга", new Gtk.CellRendererText (), "text", 2);
			treeviewServices.AppendColumn (IncomeItemsColumn);
			IncomeItemsColumn.AddAttribute (CellIncomeItems,"text", 4);
			treeviewServices.AppendColumn (AccrualColumn);
			AccrualColumn.SetCellDataFunc (CellAccrual, RenderAccrualColumn);
			treeviewServices.AppendColumn (PaidColumn);
			PaidColumn.SetCellDataFunc (CellPaid, RenderPaidColumn);
			treeviewServices.AppendColumn (OtherPaidColumn);
			OtherPaidColumn.SetCellDataFunc (CellOtherPaid, RenderOtherPaidColumn);

			treeviewServices.Model = ServiceListStore;
			treeviewServices.ShowAll();

			buttonAdd.Sensitive = false;
			buttonDel.Sensitive = false;
		}

		private bool FillAccrualRowsList()
		{
			if(_AccrualId <= 0)
				return false;
			AccrualRowsListStore = new ListStore (typeof (long), // 0 - ID accrual row
			                                      typeof (string), //1 - service name
			                                      typeof (decimal), //2 - accrual sum
			                                      typeof (string), //3 - accrual sum
			                                      typeof (decimal), //4 - Other Paid sum
			                                      typeof (string), // 5 - Other Paid sum
			                                      typeof (int)); // 6 - default income item
			AccrualRowsFilter = new TreeModelFilter( AccrualRowsListStore, null);
			AccrualRowsFilter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTreeAccrualRows);

			logger.Info("Запрос начисления...");
			try
			{
				string sql = "SELECT accrual_pays.id as rowid, (accrual_pays.price * accrual_pays.count) as sum," +
					"services.name as service, services.income_id as income_id, paysum.sum as paid FROM accrual_pays " +
						"LEFT JOIN services ON accrual_pays.service_id = services.id " +
						"LEFT JOIN (" +
						"SELECT accrual_pay_id, SUM(sum) as sum FROM payment_details WHERE payment_id != @current_payment GROUP BY accrual_pay_id) as paysum " +
						"ON paysum.accrual_pay_id = accrual_pays.id " +
						"WHERE accrual_pays.accrual_id = @accrual_id";
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue ("@accrual_id", _AccrualId);
				cmd.Parameters.AddWithValue ("@current_payment", _PaymentId);
				MySqlDataReader rdr = cmd.ExecuteReader();
				int income_id;
				decimal paidsum;
				
				while (rdr.Read())
				{
					paidsum = DBWorks.GetDecimal (rdr, "paid", 0);
					income_id = DBWorks.GetInt (rdr, "income_id", -1);

					AccrualRowsListStore.AppendValues(rdr.GetInt64("rowid"),
					                  				  rdr["service"].ToString(),
					                  				  rdr.GetDecimal ("sum"),
					                                  String.Format ("{0:C}", rdr.GetDecimal ("sum")),
					                  				  paidsum,
					                                  String.Format ("{0:C}", paidsum),
					                  				  income_id);
				}
				rdr.Close();
				logger.Info("Ok");
				buttonAdd.Sensitive = true;
				return true;
			}
			catch (Exception ex)
			{
				logger.Error (ex, "Ошибка получения начисления!");
				return false;
			}
		}

		private void FillPaymentDetails()
		{
			logger.Info("Запрос услуг по оплате №" + _PaymentId +"...");
			try
			{
				//Получаем таблицу оплаты
				string sql = "SELECT payment_details.*, services.name as service, " +
					"income_items.name as income, paysum.sum as otherpaid, " +
					"(accrual_pays.count * accrual_pays.price) as accrualsum FROM payment_details " +
					"LEFT JOIN accrual_pays ON accrual_pays.id = payment_details.accrual_pay_id " +
					"LEFT JOIN services ON accrual_pays.service_id = services.id " +
					"LEFT JOIN income_items ON payment_details.income_id = income_items.id " +
					"LEFT JOIN (" +
					"SELECT accrual_pay_id, SUM(sum) as sum FROM payment_details WHERE payment_id != @current_payment GROUP BY accrual_pay_id) as paysum " +
					"ON paysum.accrual_pay_id = payment_details.accrual_pay_id " +
					"WHERE payment_details.payment_id = @current_payment";
				
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@current_payment", _PaymentId);
				using (MySqlDataReader rdr = cmd.ExecuteReader())
				{
					int income_id;
					decimal sum, otherpaid, accrual;
					
					while (rdr.Read())
					{
						otherpaid = DBWorks.GetDecimal (rdr, "otherpaid", 0);
						accrual = rdr.GetDecimal("accrualsum");
						sum = rdr.GetDecimal ("sum");
						income_id = DBWorks.GetInt (rdr, "income_id", -1);
						
						ServiceListStore.AppendValues(rdr.GetInt64("id"),
						                              rdr.GetInt64 ("accrual_pay_id"),
						                              rdr["service"].ToString(),
						                              income_id,
						                              rdr["income"].ToString(),
						                              accrual,
						                              sum,
						                              otherpaid);
					}
				}
				
				logger.Info("Ok");
				CalculateTotal();
			}
			catch (Exception ex)
			{
				logger.Error (ex, "Ошибка получения оплат!");
			}
		}

		private bool FilterTreeAccrualRows (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			if(model.GetValue (iter, 0) == null)
				return false;
			long rowid = (long) model.GetValue (iter, 0);

			foreach (object[] row in ServiceListStore)
			{
				if((long)row[1] == rowid)
					return false;
			}

			return true;
		}

		protected void OnButtonAddClicked (object sender, EventArgs e)
		{
			TreeIter iter, iter2;
			SelectAccrualRow WinSelect = new SelectAccrualRow(AccrualRowsFilter);
			if(WinSelect.GetResult ( out iter))
			{
				ListStoreWorks.SearchListStore ((ListStore) IncomeNameList, (int)AccrualRowsFilter.GetValue (iter, 6), out iter2);
				string IncomeName = (string) IncomeNameList.GetValue (iter2, 0);
				ServiceListStore.AppendValues (null,
				                               AccrualRowsFilter.GetValue (iter, 0),
				                               AccrualRowsFilter.GetValue (iter, 1),
				                               AccrualRowsFilter.GetValue (iter, 6),
				                               IncomeName,
				                               AccrualRowsFilter.GetValue (iter, 2),
				                               0,
				                               AccrualRowsFilter.GetValue (iter, 4));
				AccrualRowsFilter.Refilter ();
			}
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
			ServiceListStore.SetValue(iter, 4, args.NewText);
			TreeIter IncomeIter;
			if (!IncomeNameList.GetIterFirst (out IncomeIter))
				return;
			do
			{
				if(IncomeNameList.GetValue (IncomeIter,0).ToString () == args.NewText)
				{
					ServiceListStore.SetValue (iter, 3, IncomeNameList.GetValue (IncomeIter, 1));
					break;
				}
			}
			while(IncomeNameList.IterNext (ref IncomeIter));
		}

		void OnPaidTextEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ServiceListStore.GetIterFromString (out iter, args.Path))
				return;
			decimal Paid;
			if (decimal.TryParse (args.NewText, out Paid)) 
			{
				ServiceListStore.SetValue (iter, 6, Paid);
				CalculateTotal ();
			}
		}

		private void RenderAccrualColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Accrual = (decimal) model.GetValue (iter, 5);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:C}", Accrual);
		}

		private void RenderPaidColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal Paid = (decimal) model.GetValue (iter, 6);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:0.00}", Paid);
		}

		private void RenderOtherPaidColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			decimal OtherPaid = (decimal) model.GetValue (iter, 7);
			(cell as Gtk.CellRendererText).Text = String.Format("{0:C}", OtherPaid);
		}

		protected void OnButtonDelClicked (object sender, EventArgs e)
		{
			TreeIter iter;
			treeviewServices.Selection.GetSelected (out iter);
			if((long)ServiceListStore.GetValue(iter, 0) > 0)
				DeletedRowId.Add ((long)ServiceListStore.GetValue(iter, 0));
			ServiceListStore.Remove(ref iter);
			AccrualRowsFilter.Refilter ();
			OnTreeviewServicesCursorChanged (null, null);
		}

		protected void OnTreeviewServicesCursorChanged (object sender, EventArgs e)
		{
			bool isSelect = treeviewServices.Selection.CountSelectedRows() == 1;
			buttonDel.Sensitive = isSelect;
		}

		private void CalculateTotal()
		{
			decimal paid = 0;
			foreach (object[] row in ServiceListStore)
			{
				paid += (decimal)row [6];
			}
			labelNotSeparated.Text = String.Format ("Не разнесено: {0:C}", _PaymentSum - paid);
			bool OldCanSave = _CanSave;
			_CanSave = ((_PaymentSum - paid) == 0);
			if(_CanSave != OldCanSave && CanSaveStateChanged != null)
				CanSaveStateChanged(this, EventArgs.Empty);
		}

		public bool SavePaymentDetails(int Payment_id, MySqlTransaction trans)
		{
			logger.Info ("Записываем строки оплаты...");
			string sql;
			MySqlCommand cmd;
			try
			{
				foreach(object[] row in ServiceListStore)
				{
					//Не записываем строки оплаты с нулевыми суммами это дает возможность удалить строку начисления без удаления строки оплаты.
					if((decimal)row[6] <= 0)
					{
						if((long)row[0] > 0)
							DeletedRowId.Add((long)row[0]);
						else
							continue;
					}

					if((long)row[0] > 0)
						sql = "UPDATE payment_details SET payment_id = @payment_id, accrual_pay_id = @accrual_pay_id, " +
							"sum = @sum, income_id = @income_id WHERE id = @id";
					else
						sql = "INSERT INTO payment_details(payment_id, accrual_pay_id, sum, income_id)" +
							"VALUES (@payment_id, @accrual_pay_id, @sum, @income_id)";
						
					cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
					cmd.Parameters.AddWithValue("@id", row[0]);
					cmd.Parameters.AddWithValue("@payment_id", Payment_id);
					cmd.Parameters.AddWithValue("@accrual_pay_id", row[1]);
					cmd.Parameters.AddWithValue("@sum", row[6]);
					cmd.Parameters.AddWithValue("@income_id", row[3]);
					
					cmd.ExecuteNonQuery ();
				}
				
				//Удаляем удаленные строки из базы данных
				sql = "DELETE FROM payment_details WHERE id = @id";
				foreach( long id in DeletedRowId)
				{
					cmd = new MySqlCommand(sql, QSMain.connectionDB, trans);
					cmd.Parameters.AddWithValue("@id", id);
					cmd.ExecuteNonQuery();
				}
				DeletedRowId.Clear ();
				return true;
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Ошибка записи строк оплаты!");
				return false;
			}
		}
	}
}