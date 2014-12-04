using System;
using QSProjectsLib;
using MySql.Data.MySqlClient;
using Gtk;

namespace bazar
{
	public partial class LesseeRentReport : Gtk.Dialog
	{
		public LesseeRentReport ()
		{
			this.Build (); 
			ComboWorks.ComboFillReference (comboPlaceType, "place_types", ComboWorks.ListMode.OnlyItems);
			MainClass.ComboAccrualYearsFill (comboStartYear);
			MainClass.ComboAccrualYearsFill (comboEndYear);
		}
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboLessee) == null || String.IsNullOrEmpty(comboPlace.ActiveText))
				return;
			string param = "MonthStart=" + (comboStartMonth.Active + 1).ToString () +
				"&MonthEnd=" + (comboEndMonth.Active + 1).ToString() +
				"&YearStart=" + comboStartYear.ActiveText + 
				"&YearEnd=" + comboEndYear.ActiveText +
				"&LesseeId=" + ComboWorks.GetActiveId(comboLessee) +
				"&Place=" + comboPlace.ActiveText.Replace(@"\",@"\\") ;

			ViewReportExt.Run ("LesseeReport", param);
		}
			
		protected void OnComboPlaceChanged (object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(comboPlace.ActiveText)) {
				comboLessee.Active = -1;
				comboLessee.Sensitive = false;
				return;
			}
			string SQL = "SELECT DISTINCT lessees.id, lessees.name FROM lessees " +
				"LEFT JOIN contracts ON contracts.lessee_id = lessees.id " +
				"WHERE contracts.place_no = @place_no AND " +
				"contracts.place_type_id = @place_type_id";
			MySqlParameter[] Param = { 
				new MySqlParameter("@place_no", comboPlace.ActiveText),
				new MySqlParameter("@place_type_id", ComboWorks.GetActiveId(comboPlaceType)) 
			};
			ComboWorks.ComboFillUniversal (comboLessee, SQL, "{1}", Param, 0, ComboWorks.ListMode.OnlyItems, true);
			comboLessee.Sensitive = true;
		}

		protected void OnComboPlaceTypeChanged (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboPlaceType) == null) {
				comboPlace.Sensitive = comboLessee.Sensitive = false;
				return;
			}

			MainClass.ComboPlaceNoFill (comboPlace, ComboWorks.GetActiveId (comboPlaceType));
			comboPlace.Sensitive = true;
		}

		protected void OnComboLesseeChanged (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboLessee) != null && 
			    ComboWorks.GetActiveId(comboPlaceType) != null &&
			    !String.IsNullOrEmpty(comboPlace.ActiveText)) {
				buttonOk.Sensitive = true;
				string SQL = "SELECT MIN(start_date) AS start, MAX(end_date) AS end FROM contracts " +
					"WHERE lessee_id = @lessee_id AND place_type_id = @place_type_id AND place_no = @place_no";
				MySqlCommand cmd = new MySqlCommand(SQL, QSMain.connectionDB);
				cmd.Parameters.AddWithValue ("@lessee_id", ComboWorks.GetActiveId (comboLessee));
				cmd.Parameters.AddWithValue ("@place_type_id", ComboWorks.GetActiveId(comboPlaceType));
				cmd.Parameters.AddWithValue ("@place_no", comboPlace.ActiveText);
				MySqlDataReader rdr = cmd.ExecuteReader();
				if (!rdr.Read ())
					return;
				TreeIter iter;
				DateTime date = rdr.GetDateTime ("start");
				comboStartMonth.Active = date.Month - 1;
				if (ListStoreWorks.SearchListStore ((ListStore)comboStartYear.Model, Convert.ToString (date.Year), out iter))
					comboStartYear.SetActiveIter (iter);
				else
					comboStartYear.Active = 0;
				date = rdr.GetDateTime ("end");
				comboEndMonth.Active = date.Month - 1;
				if (ListStoreWorks.SearchListStore ((ListStore)comboEndYear.Model, Convert.ToString (date.Year), out iter))
					comboEndYear.SetActiveIter (iter);
				else
					comboEndYear.Active = 0;
				rdr.Close ();
			}
		}
	}
}