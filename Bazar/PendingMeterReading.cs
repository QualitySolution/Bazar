﻿using System;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public class PendingMeterReading
	{
		public bool isNew;
		public int? id;
		public DateTime? date;
		public int? meterId;
		public int? meterTariffId;
		public double? value;
		public long accrualPayId;

		private string updateSQL= "UPDATE meter_reading SET value = @value " +
			"WHERE id = @id";
		private string insertSQL="INSERT INTO meter_reading (date, meter_id, meter_tariff_id, value, accrual_pay_id) " +
			"VALUES (@date, @meter_id, @meter_tariff_id, @value, @accrual_pay_id)";

		private string checkSQL = "SELECT COUNT(*) FROM meter_reading " +
				"WHERE meter_id = @meter_id AND meter_tariff_id = @meter_tariff_id AND `date` = @date";

		public PendingMeterReading ()
		{
			
		}

		public void Save(){
			string sql = isNew ? insertSQL : updateSQL;
			MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
			cmd.Parameters.AddWithValue("@id", id.Value);
			cmd.Parameters.AddWithValue("@date", date.Value);
			cmd.Parameters.AddWithValue("@meter_id", meterId.Value);
			cmd.Parameters.AddWithValue("@meter_tariff_id", meterTariffId.Value);
			cmd.Parameters.AddWithValue("@value", value.Value);
			cmd.Parameters.AddWithValue("@accrual_pay_id", accrualPayId);
			cmd.ExecuteNonQuery ();
		}

		public bool CheckExist ()
		{
			MySqlCommand cmd = new MySqlCommand (checkSQL, QSMain.connectionDB);
			cmd.Parameters.AddWithValue ("@date", date.Value);
			cmd.Parameters.AddWithValue ("@meter_id", meterId.Value);
			cmd.Parameters.AddWithValue ("@meter_tariff_id", meterTariffId.Value);
			var count = (long)cmd.ExecuteScalar ();

			return count > 0;
		}
	}
}

