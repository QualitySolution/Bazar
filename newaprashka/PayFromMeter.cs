using System;
using Gtk;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class PayFromMeter : Gtk.Dialog
	{
		ListStore ReadingListStore;
		int accrual_detail_id;
		public int TotalCount;
		string Units;

		public double Price {
		get{return spinPrice.Value;}
		set{spinPrice.Value = value;}
		}

		public PayFromMeter ()
		{
			this.Build ();

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

			treeviewMeters.AppendColumn ("Счётчик/тариф", new Gtk.CellRendererText (), "text", 4);
			treeviewMeters.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 5);
			treeviewMeters.AppendColumn ("Предыдущие", new Gtk.CellRendererText (), RenderPreValueColumn);
			treeviewMeters.AppendColumn ("Дата", new Gtk.CellRendererText (), "text", 8);
			treeviewMeters.AppendColumn ("Текущие", CellValue, RenderValueColumn);
			treeviewMeters.AppendColumn ("Расход", new Gtk.CellRendererText (), RenderDeltaColumn);

			treeviewMeters.Model = ReadingListStore;
			treeviewMeters.ShowAll ();
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
			(cell as Gtk.CellRendererText).Text = String.Format("{0} {1}", Delta, Units);
		}

		protected void CalculateSum ()
		{
			TotalCount = 0;

			foreach(object[] row in ReadingListStore )
			{
				if((int) row[7] > 0)
				TotalCount += (int) row[7];
			}
			labelCount.Text = string.Format("{0} {1}", TotalCount, Units);
			labelSum.Text = string.Format("{0:C}", TotalCount * spinPrice.Value);
		}

		public void Fill(int AccrualRow, int service_id, int place_type_id, string place_no, string units)
		{
			TreeIter iter;
			MainClass.StatusMessage(String.Format("Запрос показаний счетчиков..."));
			accrual_detail_id = AccrualRow;
			Units = units;
			string sql = "SELECT meters.id as meterid, meter_tariffs.id as tariffid, meter_tariffs.name as tariff, meter_types.name as meter_type " +
				"FROM meter_tariffs " +
				"LEFT JOIN meter_types ON meter_types.id = meter_tariffs.meter_type_id " +
				"LEFT JOIN meters ON meters.meter_type_id = meter_types.id " +
				"WHERE meter_tariffs.service_id = @service_id AND meters.place_type_id = @place_type_id AND meters.place_no = @place_no";
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
									ReadingListStore.SetValue (iter, 9, true);
									ReadingListStore.SetValue (iter, 3, rdr.GetInt32 ("id"));
									ReadingListStore.SetValue (iter, 8, String.Format ("{0:d}", rdr.GetDateTime ("date")));
									ReadingListStore.SetValue (iter, 2, rdr.GetDouble ("value"));
								}
								else if((string) ReadingListStore.GetValue (iter, 5) == "" || (bool) ReadingListStore.GetValue (iter, 9))
								{
									ReadingListStore.SetValue (iter, 5, String.Format ("{0:d}", rdr.GetDateTime ("date")));
									ReadingListStore.SetValue (iter, 6, rdr.GetInt32 ("value"));
									ReadingListStore.SetValue (iter, 9, false);
								}
							} 
						} while(ReadingListStore.IterNext (ref iter));
					}
				}
				CalculateSum ();
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о типе счетчика!");
				QSMain.ErrorMessage(this, ex);
				this.Respond(Gtk.ResponseType.Reject);
			}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string sql;
			MainClass.StatusMessage("Запись показаний счётчиков...");
			MySqlCommand cmd;
			try 
			{
				TreeIter iter;
				ReadingListStore.GetIterFirst(out iter);
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
					cmd = new MySqlCommand(sql, QSMain.connectionDB);
					cmd.Parameters.AddWithValue("@id", ReadingListStore.GetValue(iter, 3));
					cmd.Parameters.AddWithValue("@date", DateTime.Today);
					cmd.Parameters.AddWithValue("@meter_id", ReadingListStore.GetValue(iter, 0));
					cmd.Parameters.AddWithValue("@meter_tariff_id", ReadingListStore.GetValue(iter, 1));
					cmd.Parameters.AddWithValue("@value", ReadingListStore.GetValue(iter, 2));
					cmd.Parameters.AddWithValue("@accrual_pay_id", accrual_detail_id);

					cmd.ExecuteNonQuery();
				}
				while(ReadingListStore.IterNext(ref iter));

				MainClass.StatusMessage("Ok");
				Respond (Gtk.ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи показаний счётчиков!");
				QSMain.ErrorMessage(this,ex);
			}
		}
	}
}

