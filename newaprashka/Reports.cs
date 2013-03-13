using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using MonoReports.Model;
using MonoReports.Model.Data;

namespace bazar
{
	public class Reports
	{
		Report RepEngine;

		public Reports (string ReportName)
		{
			RepEngine = new Report ();
			RepEngine.Load ( ReportName + ".mrp");
		}

		static public void RunDesigner()
		{

		}

		public void ViewReport()
		{

		}

		public void ViewPay(long itemid)
		{
			MainClass.StatusMessage("Запрос начисления №" + itemid +"...");
			string sql = "SELECT accrual.*, users.name as user, lessees.name as lessee, place_types.name as type, " +
				"contracts.place_no as place_no " +
				"FROM accrual " +
				"LEFT JOIN users ON users.id = accrual.user_id " +
				"LEFT JOIN contracts ON contracts.number = accrual.contract_no " +
				"LEFT JOIN lessees ON lessees.id = contracts.lessee_id " +
				"LEFT JOIN place_types ON place_types.id = contracts.place_type_id " +
				"WHERE accrual.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@id", itemid);
				MySqlDataReader rdr = cmd.ExecuteReader();
				
				if(!rdr.Read())
					return;

				DateTime Month = new DateTime(rdr.GetInt32("year"), rdr.GetInt32 ("month"), 1);
				var parameters = new Dictionary<string,object>{ 
					{"Month", String.Format ("{0:MMMM yyyy}", Month)},
					{"Lessee", rdr["lessee"]},				
					{"Contract", rdr["contract_no"]},
					{"Place", rdr["type"].ToString() + " - " + rdr["place_no"].ToString() },
				};
				rdr.Close ();
				decimal Total = 0;
				sql = "SELECT services.name as service, units.name as units, count, price FROM accrual_pays " +
						"LEFT JOIN services ON accrual_pays.service_id = services.id " +
						"LEFT JOIN units ON services.units_id = units.id " +
						"WHERE accrual_pays.accrual_id = @accrual_id";
				
				cmd = new MySqlCommand(sql, MainClass.connectionDB);
				cmd.Parameters.AddWithValue("@accrual_id", itemid);
				rdr = cmd.ExecuteReader();

				List<PayRow> rows = new List<PayRow>();
				while (rdr.Read())
				{
					PayRow row = new PayRow();
					row.Service = rdr.GetString ("service");
					row.Units = rdr.GetString ("units");
					row.Count = rdr.GetInt32 ("count");
					row.Price = rdr.GetDecimal ("price");
					row.Sum = row.Count * row.Price;
					rows.Add (row);
					Total += row.Sum;
				} 
				rdr.Close();
				parameters.Add ("Total", Total);

				ObjectDataSource<PayRow> objectDataSource = new ObjectDataSource<PayRow>(rows);
				objectDataSource.AddField ("Service",x=>x.Service);
				objectDataSource.AddField ("Units",x=>x.Units);
				objectDataSource.AddField ("Count",x=>x.Count);
				objectDataSource.AddField ("Price",x=>x.Price);
				objectDataSource.AddField ("Sum",x=>x.Sum);

				RepEngine.DataSource = objectDataSource;
				string PdfFilePath = System.IO.Path.Combine (System.IO.Path.GetTempPath (), "PayList.pdf");
				RepEngine.ExportToPdf (PdfFilePath, parameters);
				System.Diagnostics.Process.Start(PdfFilePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о начисление!");
				//MainClass.ErrorMessage(this,ex);
			}
		}

		class PayRow
		{
			public string Service;
			public string Units;
			public int Count;
			public decimal Price;
			public decimal Sum;
		}
	}
}

