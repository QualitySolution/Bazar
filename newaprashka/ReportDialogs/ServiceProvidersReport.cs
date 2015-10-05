using System;
using QSProjectsLib;

namespace bazar
{
	public partial class ServiceProvidersPaymentReport : Gtk.Dialog
	{
		public ServiceProvidersPaymentReport ()
		{
			this.Build ();
			MainClass.ComboAccrualYearsFill (comboYear);
			comboMonth.Active = DateTime.Now.Month-1;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string args = "month=" + (comboMonth.Active+1).ToString()+"&year="+comboYear.ActiveText;
			ViewReportExt.Run("ServiceProviderReport", args);
		}
	}

}

