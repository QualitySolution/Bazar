using System;
using QSProjectsLib;

namespace bazar
{
	public partial class LesseeDebtsReport : Gtk.Dialog
	{
		public LesseeDebtsReport ()
		{
			this.Build ();

			dateReport.Date = DateTime.Today;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if(dateReport.IsEmpty)
				return;
			string param = "Day=" + dateReport.Date.Day.ToString() +
				"&Month=" + dateReport.Date.Month.ToString() +
					"&Year=" + dateReport.Date.Year.ToString();
			ViewReportExt.Run ("LesseeDebts", param);
		}
	}
}

