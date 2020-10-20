using System;
using Gtk;
using QSProjectsLib;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class MetersReport : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		ListStore MetersList;
		ListStore ProvidersList;
		ListStore monthModel;
		ListStore quarterModel;

		public MetersReport ()
		{
			this.Build ();
			FillMeter();
			FillProviders();
			FillComboBox();
		}

		void FillMeter()
        {
			MetersList = new ListStore (typeof (int), typeof (bool), typeof (string));

			CellRendererToggle CellSelect = new CellRendererToggle ();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;

			treeviewMeters.AppendColumn ("Выб.", CellSelect, "active", 1);
			treeviewMeters.AppendColumn ("Тип счётчика", new CellRendererText (), "text", 2);

			treeviewMeters.Model = MetersList;

			logger.Info ("Запрос типов счетчиков...");
			string sql = "SELECT id, name FROM meter_types";
			try {
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);

				using (MySqlDataReader rdr = cmd.ExecuteReader ()) {
					while (rdr.Read ()) {
						MetersList.AppendValues (rdr.GetInt32 ("id"),
													 false,
													 rdr.GetString ("name")
													 );
					}
				}
				logger.Info ("Ok");
			} catch (Exception ex) {
				QSMain.ErrorMessageWithLog (this, "Ошибка получения информации о типах счетчиков!", logger, ex);
			}
		}

		void FillProviders()
        {
			ProvidersList = new ListStore (typeof (int), typeof (bool), typeof (string));

			CellRendererToggle CellSelect = new CellRendererToggle ();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled_Providers;

			treeviewProviders.AppendColumn ("Выб.", CellSelect, "active", 1);
			treeviewProviders.AppendColumn ("Поставщик", new CellRendererText (), "text", 2);

			treeviewProviders.Model = ProvidersList;

			logger.Info ("Запрос поставщиков...");
			string sql = "SELECT id, name FROM service_providers";
			try {
				MySqlCommand cmd = new MySqlCommand (sql, QSMain.connectionDB);

				using (MySqlDataReader rdr = cmd.ExecuteReader ()) {
					while (rdr.Read ()) {
						ProvidersList.AppendValues (rdr.GetInt32 ("id"),
													 false,
													 rdr.GetString ("name")
													 );
					}
				}
				logger.Info ("Ok");
			} catch (Exception ex) {
				QSMain.ErrorMessageWithLog (this, "Ошибка получения поставщиков!", logger, ex);
			}
		}

		void FillComboBox()
        {
			string [] months ={
			"Январь","Февраль",
			"Март","Апрель","Май",
			"Июнь","Июль","Август",
			"Сентябрь","Октябрь","Ноябрь",
			"Декабрь"};
			string [] quarters = { "I", "II", "III", "IV" };

			MainClass.ComboAccrualYearsFill (comboYear);
			comboPeriod.Active = DateTime.Now.Month - 1;
			monthModel = new ListStore (typeof (string));
			quarterModel = new ListStore (typeof (string));
			foreach (string month in months) {
				monthModel.AppendValues (month);
			}
			foreach (string quarter in quarters) {
				quarterModel.AppendValues (quarter);
			}
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			SetToggled (o, args, MetersList);
		}

		void onCellSelectToggled_Providers (object o, ToggledArgs args)
		{
			SetToggled (o, args, ProvidersList);
		}

		void SetToggled(object o, ToggledArgs args, ListStore list)
		{
			TreeIter iter;

			if (list.GetIter (out iter, new TreePath (args.Path))) {
				bool old = (bool)list.GetValue (iter, 1);
				list.SetValue (iter, 1, !old);
			}

		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string param = BuildParams();

			if (checkHandmade.Active)
				if (radioLetter.Active)
					ViewReportExt.Run ("MetersFill_vertical", param.TrimEnd (','));
				else
					ViewReportExt.Run ("MetersFill_horizontal", param.TrimEnd (','));
			else
				if (radioLetter.Active)
					ViewReportExt.Run ("Meters_vertical", param.TrimEnd (','));
				else	
					ViewReportExt.Run ("Meters_horizontal", param.TrimEnd (','));

		}

		protected string BuildParams()
		{
			bool isEmptyTypesList = true;
			string param = "types=";
			foreach (object [] row in MetersList) 
				if ((bool)row [1]) {
					param += String.Format ("{0},", row [0]);
					isEmptyTypesList = false;
				}

			if (isEmptyTypesList)
				param += String.Format ("{0},", -1);
			param = param.TrimEnd (',');

			param += "&providers=";

			bool isEmptyProvidersList = true;
			foreach (object [] row in ProvidersList) 
				if ((bool)row [1]) {
					param += String.Format ("{0},", row [0]);
					isEmptyProvidersList = true;
				}

			if (isEmptyProvidersList)
				param += String.Format ("{0},", -1);

			param = param.TrimEnd (',');
			param += GetPeriod ();

			return param;
		}

		protected void OnRadiobuttonQuarterToggled (object sender, EventArgs e)
		{
			if (radioButtonMonth.Active) {
				comboPeriod.Model = monthModel;
				comboPeriod.Active = 0;
			} else if (radioButtonQuarter.Active) {
				comboPeriod.Model = quarterModel;
				comboPeriod.Active = 0;
			}
		}

		string GetPeriod()
        {
			string args = "";
			if (radioButtonMonth.Active) {
				int month = comboPeriod.Active + 1;
				int year = Convert.ToInt32 (comboYear.ActiveText);
				args = "&month=" + (comboPeriod.Active + 1).ToString () + "&year=" + comboYear.ActiveText;
			} else if (radioButtonQuarter.Active) {
				int quarter = comboPeriod.Active + 1;
				switch (quarter) {
				case 2:
					quarter += 2;
					break;
				case 3:
					quarter += 4;
					break;
				case 4:
					quarter += 6;
					break;
				default:
					break;
				}
				string period = $"{quarter}, {++quarter}, {++quarter}";
				int year = Convert.ToInt32 (comboYear.ActiveText);
				args = "&month=" + period + "&year=" + year;
			}
			return args;
		}
	}
}

