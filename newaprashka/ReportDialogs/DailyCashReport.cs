using System;
using QSProjectsLib;

namespace bazar
{
	public partial class DailyCashReport : Gtk.Dialog
	{
		public DailyCashReport ()
		{
			this.Build ();
			datepickerCash.Date = DateTime.Now.Date;

			ComboWorks.ComboFillReference(comboCash, "cash", 0);
		}

		protected	void TestCanSave ()
		{
			bool Cashok = comboCash.Active >= 0;
			bool Dateok = !datepickerCash.IsEmpty;

			buttonOk.Sensitive = Cashok && Dateok;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string date = String.Format ("{0:u}", datepickerCash.Date).Substring (0, 10);
			string cash_id = String.Format ("{0}", ComboWorks.GetActiveId (comboCash));
			string param = "date=" + date + "&cash=" + cash_id;
			Console.WriteLine (param);                        
			ViewReportExt.Run ("DailyCash", param, true);
		}

		protected void OnDatepickerCashDateChanged(object sender, EventArgs e)
		{
			TestCanSave ();
		}

		protected void OnComboCashChanged(object sender, EventArgs e)
		{
			TestCanSave ();
		}
	}
}

