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
			int month = comboMonth.Active+1;
			int year = Convert.ToInt32(comboYear.ActiveText);

			string args = "month=" + (comboMonth.Active+1).ToString()+"&year="+comboYear.ActiveText;
			int paymentMonth = month + 1;
			int paymentYear = year;
			if (paymentMonth > 12) {
				paymentMonth %= 12;
				paymentYear= year+1;
			}
			args += "&paymentMonth=" + paymentMonth + "&paymentYear=" + paymentYear;
			ViewReportExt.Run("ServiceProviderReport", args);
		}
	}

}

