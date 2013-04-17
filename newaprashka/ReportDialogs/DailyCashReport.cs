using System;

namespace bazar
{
	public partial class DailyCashReport : Gtk.Dialog
	{
		public DailyCashReport ()
		{
			this.Build ();
			datepickerCash.Date = DateTime.Now.Date;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string date = String.Format ("{0:u}", datepickerCash.Date).Substring (0, 10);
			string param = "date=" + date;
			Console.WriteLine (param);                        
			ReportsExt.ViewReport ("DailyCash", param, true);
		}
	}
}

