using System;
using Gtk;
using QSProjectsLib;
using MySql.Data.MySqlClient;

namespace bazar.ReportDialogs
{
	public partial class MetersStatisticsDlg : Gtk.Dialog
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger ();
		ListStore ProvidersList;
		ListStore quarterModel;

		public MetersStatisticsDlg ()
		{
			this.Build ();
			FillProviders ();
			FillComboBox ();
			this.Title = "Статистика по счетчикам";
		}

		void FillProviders ()
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

		void FillComboBox ()
		{
			string [] quarters = { "I", "II", "III", "IV" };

			MainClass.ComboAccrualYearsFill (comboYear);
			quarterModel = new ListStore (typeof (string));
			foreach (string quarter in quarters) {
				quarterModel.AppendValues (quarter);
			}
			comboPeriod.Model = quarterModel;
			comboPeriod.Active = 0;
		}

		void onCellSelectToggled_Providers (object o, ToggledArgs args)
		{
			TreeIter iter;

			if (ProvidersList.GetIter (out iter, new TreePath (args.Path))) {
				bool old = (bool)ProvidersList.GetValue (iter, 1);
				ProvidersList.SetValue (iter, 1, !old);
			}
		}

		protected string BuildParams ()
		{
			bool isEmptyTypesList = true;

			string param = "providers=";

			bool isEmptyProvidersList = true;
			foreach (object [] row in ProvidersList)
				if ((bool)row [1]) {
					param += String.Format ("{0},", row [0]);
					isEmptyProvidersList = false;
				}

			if (isEmptyProvidersList)
				param += String.Format ("{0},", -1);

			param = param.TrimEnd (',');
			param += GetPeriod ();

			return param;
		}

		string GetPeriod ()
		{
			string args = "";

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
				int year = Convert.ToInt32 (comboYear.ActiveText);
				args = "&month_1=" + quarter + "&month_2=" + ++quarter + "&month_3=" + ++quarter + "&year=" + year;
				args += "&onlyAccruals=" + (checkbuttonOnlyAccruals.Active? 1: 0);

				return args;
		}
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string param = BuildParams ();

			ViewReportExt.Run ("Meters_statistic", param.TrimEnd (','));
		}
	}
}
