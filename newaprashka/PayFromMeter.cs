using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;
using System.Collections.Generic;

namespace bazar
{
	public partial class PayFromMeter : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		ListStore ReadingListStore, ChildListStore;
		int accrual_detail_id, ChildCount;
		private List<MySqlCommand> saveReadingCommands;
		public List<MySqlCommand> SaveCommands{ get { return saveReadingCommands; } }
		public decimal TotalCount;
		string Units;
		bool LastValues;
		private decimal MinSum = 0m;

		public decimal Price {
			get{return (decimal) spinPrice.Value;}
			set{spinPrice.Value = (double) value;}
		}

		public PayFromMeter ()
		{
			this.Build ();
			//Main meters
			ReadingListStore = new Gtk.ListStore (typeof (int), 	// 0 - meter id
			                                     typeof (int),		// 1 - tariff id
			                                     typeof (double),	// 2 - value
			                                      typeof(int), 		// 3 - current reading id
			                                     typeof (string),	// 4 - meter name
			                                      typeof(string),	// 5 - date of last reading
			                                      typeof (int),		// 6 - Last reading value
			                                      typeof (int),		// 7 - delta of value
			                                      typeof(string),	// 8 - current reading date
			                                      typeof(bool)		// 9 - take next as PreValue (for fill only)
			                                      );

			Gtk.CellRendererSpin CellValue = new CellRendererSpin();
			CellValue.Editable = true;
			Adjustment adjValue = new Adjustment(0,0,1000000,1,10,0);
			CellValue.Adjustment = adjValue;
			CellValue.Edited += OnValueSpinEdited;

			treeviewMeters.AppendColumn ("Тип счетчика/тариф", new Gtk.CellRendererText (), "text", 4);
			treeviewMeters.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 5);
			treeviewMeters.AppendColumn ("Предыдущие", new Gtk.CellRendererText (), RenderPreValueColumn);
			treeviewMeters.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 8);
			treeviewMeters.AppendColumn ("Текущие", CellValue, RenderValueColumn);
			treeviewMeters.AppendColumn ("Расход", new Gtk.CellRendererText (), RenderDeltaColumn);

			treeviewMeters.Model = ReadingListStore;
			treeviewMeters.ShowAll ();

			//Child meters
			ChildListStore = new Gtk.ListStore (typeof (string), 	// 0 - meter name
			                                      typeof (string),	// 1 - date
			                                      typeof (int)		// 2 - value
			                                      );

			treeviewChilds.AppendColumn ("Тип счетчика/тариф", new Gtk.CellRendererText (), "text", 0);
			treeviewChilds.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 1);
			treeviewChilds.AppendColumn ("Расход", new Gtk.CellRendererText (), RenderChildValueColumn);
			treeviewChilds.Model = ChildListStore;
			treeviewChilds.ShowAll ();
		}

		void OnValueSpinEdited (object o, EditedArgs args)
		{
			TreeIter iter;
			if (!ReadingListStore.GetIterFromString (out iter, args.Path))
				return;
			int PreValue = (int)ReadingListStore.GetValue (iter, 6);
			int Value;
			if (int.TryParse (args.NewText, out Value)) 
			{
				ReadingListStore.SetValue (iter, 2, (double) Value);
				ReadingListStore.SetValue (iter, 7, Value - PreValue);
				CalculateSum ();
			}
		}

		private void RenderPreValueColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			int LastReading = (int) model.GetValue (iter, 6);

			(cell as Gtk.CellRendererText).Text = String.Format("{0} {1}", LastReading, Units);
		}

		private void RenderChildValueColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			int ChildDelta = (int) model.GetValue (iter, 2);

			(cell as Gtk.CellRendererText).Text = String.Format("{0} {1}", ChildDelta, Units);
		}

		private void RenderValueColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			double CurReading = (double) model.GetValue (iter, 2);

			(cell as Gtk.CellRendererSpin).Text = String.Format("{0} {1}", CurReading, Units);
		}

		private void RenderDeltaColumn (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			int Delta = (int) model.GetValue (iter, 7);
			if (Delta > 0) 
			{
				(cell as Gtk.CellRendererText).Foreground = "black";
			} 
			else 
			{
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
			if((double) model.GetValue (iter, 2) > 0)
				(cell as Gtk.CellRendererText).Text = String.Format("{0} {1}", Delta, Units);
			else
				(cell as Gtk.CellRendererText).Text = "-//-";
		}

		protected void CalculateSum ()
		{
			TotalCount = 0;
			int MeterValues = 0;

			foreach(object[] row in ReadingListStore )
			{
				if((int) row[7] > 0)
					MeterValues += (int) row[7];
			}
			TotalCount = MeterValues - ChildCount;
			labelCount.LabelProp = string.Format("{0} {1}", MeterValues, Units);
			labelTotal.LabelProp = string.Format("{0} {1}", TotalCount, Units);
			labelSum.Text = string.Format("{0:C}", TotalCount * Price);
			if (TotalCount * Price < MinSum)
				TotalCount = (spinPrice.Value == 0) ? 0 : ( MinSum / Price);
		}

		public void Fill(int AccrualRow, int service_id, int place_type_id, string place_no, string units)
		{
			TreeIter iter;
			logger.Info("Запрос показаний счетчиков...");
			accrual_detail_id = AccrualRow;
			Units = units;
			string sql = "SELECT meters.id as meterid, meter_tariffs.id as tariffid, meter_tariffs.name as tariff, meter_types.name as meter_type " +
				"FROM meter_tariffs " +
				"LEFT JOIN meter_types ON meter_types.id = meter_tariffs.meter_type_id " +
				"LEFT JOIN meters ON meters.meter_type_id = meter_types.id " +
				"WHERE meter_tariffs.service_id = @service_id AND meters.place_type_id = @place_type_id AND meters.place_no = @place_no AND meters.disabled = 'FALSE'";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				cmd.Parameters.AddWithValue("@service_id", service_id);
				cmd.Parameters.AddWithValue("@place_type_id", place_type_id);
				cmd.Parameters.AddWithValue("@place_no", place_no);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{		
					while(rdr.Read())
					{
						ReadingListStore.AppendValues (rdr.GetInt32 ("meterid"),
						                               rdr.GetInt32 ("tariffid"),
						                               0.0,
						                               -1,
						                               String.Format ("{0}/{1}", rdr["meter_type"].ToString (), rdr["tariff"].ToString ()),
						                               "",
						                               0,
						                               0,
						                               "",
						                               false
							);
					}
				}

				//Получаем информацию о последних показаниях
				LastValues = true;
				ReadingListStore.GetIterFirst (out iter);
				string MetersIds = ReadingListStore.GetValue (iter, 0).ToString ();
				while(ReadingListStore.IterNext (ref iter))
				{
					MetersIds += ", " + ReadingListStore.GetValue (iter, 0).ToString ();
				}

				sql = "SELECT meter_reading.* FROM meter_reading " +
					"WHERE meter_id IN ( " + MetersIds + " )" +
					"ORDER BY meter_reading.date DESC, value DESC";

				cmd = new MySqlCommand(sql, QSMain.connectionDB);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						ReadingListStore.GetIterFirst(out iter);
						do
						{
							if((int) ReadingListStore.GetValue (iter, 0) == rdr.GetInt32 ("meter_id") && (int) ReadingListStore.GetValue (iter, 1) == rdr.GetInt32 ("meter_tariff_id"))
							{
								if(rdr["accrual_pay_id"] != DBNull.Value && rdr.GetInt32 ("accrual_pay_id") == AccrualRow)
								{
									if((string) ReadingListStore.GetValue (iter, 5) != "")
										LastValues = false;
									ReadingListStore.SetValue (iter, 9, true);
									ReadingListStore.SetValue (iter, 3, rdr.GetInt32 ("id"));
									ReadingListStore.SetValue (iter, 8, String.Format ("{0:d}", rdr.GetDateTime ("date")));
									ReadingListStore.SetValue (iter, 2, rdr.GetDouble ("value"));
									ReadingListStore.SetValue (iter, 7, rdr.GetInt32 ("value") - (int) ReadingListStore.GetValue (iter, 6));
								}
								else if((string) ReadingListStore.GetValue (iter, 5) == "" || (bool) ReadingListStore.GetValue (iter, 9))
								{
									ReadingListStore.SetValue (iter, 5, String.Format ("{0:d}", rdr.GetDateTime ("date")));
									ReadingListStore.SetValue (iter, 6, rdr.GetInt32 ("value"));
									ReadingListStore.SetValue (iter, 7, Convert.ToInt32 (ReadingListStore.GetValue (iter, 2)) - rdr.GetInt32 ("value"));
									ReadingListStore.SetValue (iter, 9, false);
								}
							} 
						} while(ReadingListStore.IterNext (ref iter));
					}
				}
				if(LastValues)
					hboxMessage.Visible = false;
				else
				{
					(treeviewMeters.Columns[4].CellRenderers[0] as CellRendererSpin).Editable = false;
					buttonOk.Sensitive = false;
				}

				// take childs values
				sql = "SELECT meters.name, meter_tariffs.name as tariff, reading.* FROM meters " +
					"LEFT JOIN (" +
					"SELECT  selectedmeters.*, " +
					"(SELECT date FROM meter_reading WHERE selectedmeters.meter_id = meter_reading.meter_id AND selectedmeters.meter_tariff_id = meter_reading.meter_tariff_id ORDER BY meter_reading.date DESC, value DESC LIMIT 1) as lastdate, " +
					"(SELECT value FROM meter_reading WHERE selectedmeters.meter_id = meter_reading.meter_id AND selectedmeters.meter_tariff_id = meter_reading.meter_tariff_id ORDER BY meter_reading.date DESC, value DESC LIMIT 1) as lastvalue, " +
					"(SELECT value FROM meter_reading WHERE selectedmeters.meter_id = meter_reading.meter_id AND selectedmeters.meter_tariff_id = meter_reading.meter_tariff_id ORDER BY meter_reading.date DESC, value DESC LIMIT 1, 1) as prevalue " +
					"FROM (SELECT DISTINCT meter_reading.meter_id, meter_reading.meter_tariff_id FROM meter_reading) as selectedmeters" +
					") as reading ON meters.id = reading.meter_id " +
					"LEFT JOIN meter_tariffs ON meter_tariffs.id = reading.meter_tariff_id " +
					"WHERE meters.parent_meter_id IN (" + MetersIds + ") AND meters.disabled = 'FALSE'";

				cmd = new MySqlCommand(sql, QSMain.connectionDB);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					if(!rdr.HasRows || !LastValues)
					{
						vboxChilds.Visible = false;
						labelChilds.Visible = false;
						labelChildText.Visible = false;
						labelTotal.Visible = false;
						labelTotalText.Visible = false;
					}
					ChildCount = 0;
					while (rdr.Read())
					{
						ChildListStore.AppendValues (rdr["name"].ToString () + "/" + rdr["tariff"].ToString (),
						                             String.Format ("{0:d}", DBWorks.GetDateTime (rdr, "lastdate", new DateTime(1,1,1))),
						                             DBWorks.GetInt (rdr,"lastvalue", 0) - DBWorks.GetInt (rdr,"prevalue", 0)
													);
						ChildCount += DBWorks.GetInt (rdr,"lastvalue", 0) - DBWorks.GetInt (rdr,"prevalue", 0);
					}
					labelChilds.LabelProp = string.Format("{0} {1}", ChildCount, Units);
				}
				sql = "SELECT contract_pays.min_sum FROM contract_pays " +
					"LEFT JOIN contracts ON contracts.id = contract_pays.contract_id " +
					"LEFT JOIN accrual ON accrual.contract_id = contracts.id " +
					"LEFT JOIN accrual_pays ON accrual_pays.accrual_id = accrual.id " +
					"WHERE accrual_pays.id = @accrual_pay_id AND contract_pays.service_id = @service_id";
				cmd = new MySqlCommand(sql, QSMain.connectionDB);
				cmd.Parameters.AddWithValue("@service_id", service_id);
				cmd.Parameters.AddWithValue("@accrual_pay_id", accrual_detail_id);
				object min_sum = cmd.ExecuteScalar();
				if(min_sum != DBNull.Value)
				{
					MinSum = Convert.ToDecimal (min_sum);
					labelMinSum.LabelProp = String.Format ("{0:C}", MinSum);
				}
				else
				{
					labelMinSum.Visible = false;
					labelMinSumText.Visible = false;
				}

				CalculateSum ();
				logger.Info("Ok");
			}
			catch (Exception ex)
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка получения информации о типе счетчика!", logger, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string sql;
			logger.Info("Запись показаний счётчиков...");
			try 
			{
				TreeIter iter;
				ReadingListStore.GetIterFirst(out iter);
				saveReadingCommands = new List<MySqlCommand>();
				do
				{
					if(!ReadingListStore.IterIsValid (iter))
						break;
					if((int)ReadingListStore.GetValue(iter, 7) < 0)
						continue; // Не записывать если дельта отрицательная
					if((int)ReadingListStore.GetValue(iter, 7) == 0 && (int)ReadingListStore.GetValue(iter, 3) <= 0)
						continue; // Не записывать если показания не введены
					if((int)ReadingListStore.GetValue(iter, 3) > 0)
						sql = "UPDATE meter_reading SET value = @value " +
							"WHERE id = @id";
					else
						sql = "INSERT INTO meter_reading (date, meter_id, meter_tariff_id, value, accrual_pay_id) " +
							"VALUES (@date, @meter_id, @meter_tariff_id, @value, @accrual_pay_id)";

					MySqlCommand saveReadingCommand;
					saveReadingCommand = new MySqlCommand(sql, QSMain.connectionDB);
					saveReadingCommand.Parameters.AddWithValue("@id", ReadingListStore.GetValue(iter, 3));
					saveReadingCommand.Parameters.AddWithValue("@date", DateTime.Today);
					saveReadingCommand.Parameters.AddWithValue("@meter_id", ReadingListStore.GetValue(iter, 0));
					saveReadingCommand.Parameters.AddWithValue("@meter_tariff_id", ReadingListStore.GetValue(iter, 1));
					saveReadingCommand.Parameters.AddWithValue("@value", ReadingListStore.GetValue(iter, 2));
					saveReadingCommands.Add(saveReadingCommand);
				}
				while(ReadingListStore.IterNext(ref iter));

				logger.Info("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				QSMain.ErrorMessageWithLog(this, "Ошибка записи показаний счётчиков!", logger, ex);
			}
		}

		protected void OnSpinPriceValueChanged(object sender, EventArgs e)
		{
			CalculateSum ();
		}
	}
}

