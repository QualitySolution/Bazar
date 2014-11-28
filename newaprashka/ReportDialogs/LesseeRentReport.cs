using System;
using QSProjectsLib;

namespace bazar
{
	public partial class LesseeRentReport : Gtk.Dialog
	{
		public LesseeRentReport ()
		{
			this.Build ();
			ComboWorks.ComboFillReference (comboLessee, "lessees", ComboWorks.ListMode.OnlyItems);

		}
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if (dateStart.Date.Year < 2010 || dateStart.Date.Year > dateEnd.Date.Year)
				return;
			if (ComboWorks.GetActiveIdOrNull (comboLessee) == null || String.IsNullOrEmpty(comboPlace.ActiveText))
				return;
			string param = "MonthStart=" + dateStart.Date.Month.ToString() +
				"&MonthEnd=" + dateEnd.Date.Month.ToString() +
				"&YearStart=" + dateStart.Date.Year.ToString() + 
				"&YearEnd=" + dateEnd.Date.Year.ToString() +
				"&LesseeId=" + ComboWorks.GetActiveId(comboLessee) +
				"&Place=" + comboPlace.ActiveText;

			ViewReportExt.Run ("LesseeReport", param);
		}

		protected void OnComboLesseeChanged (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboLessee) == null) {
				comboPlace.Sensitive = false;
				return;
			}
			string SQL = "SELECT id, place_no FROM contracts WHERE lessee_id = " + ComboWorks.GetActiveId(comboLessee);
			ComboWorks.ComboFillUniversal (comboPlace, SQL, "{1}", null, 0, ComboWorks.ListMode.OnlyItems, true);
			comboPlace.Sensitive = true;
		}
	}
}

