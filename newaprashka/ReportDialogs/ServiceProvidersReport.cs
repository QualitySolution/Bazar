using System;
using bazar.Tools;
using Gtk;
using QSProjectsLib;
using QSSupportLib;

namespace bazar
{
	public partial class ServiceProvidersPaymentReport : Gtk.Dialog
	{		
		private string[] months={
			"Январь","Февраль",
			"Март","Апрель","Май",
			"Июнь","Июль","Август",
			"Сентябрь","Октябрь","Ноябрь",
			"Декабрь"};
		private const string subReportName= "ServiceProviderSubReport";
		private string[] quarters={"I","II","III","IV"};
		private ListStore monthModel;
		private ListStore quarterModel;

		public ServiceProvidersPaymentReport ()
		{
			this.Build ();
			MainClass.ComboAccrualYearsFill (comboYear);
			comboPeriod.Active = DateTime.Now.Month-1;
			monthModel = new ListStore (typeof(string));
			quarterModel = new ListStore (typeof(string));
			foreach (string month in months) {
				monthModel.AppendValues (month);
			}
			foreach (string quarter in quarters) {
				quarterModel.AppendValues (quarter);
			}

			yspinLastReadingDay.ValueAsInt = BaseParameters.ReportLastReadingDay;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if (radioButtonMonth.Active) {
				int month = comboPeriod.Active + 1;
				int year = Convert.ToInt32 (comboYear.ActiveText);
				string args = "month=" + (comboPeriod.Active + 1).ToString () + "&year=" + comboYear.ActiveText;
				int paymentMonth = month + 1;
				int paymentYear = year;
				if (paymentMonth > 12) {
					paymentMonth %= 12;
					paymentYear = year + 1;
				}
				args += "&paymentMonth=" + paymentMonth + "&paymentYear=" + paymentYear + "&new_month_meters_day=" + yspinLastReadingDay.Text;
				ViewReportExt.RunWithSubreports ("ServiceProviderReport", args,new string[]{subReportName});
			} else if (radioButtonQuarter.Active) {
				int quarter = comboPeriod.Active + 1;
				int year = Convert.ToInt32 (comboYear.ActiveText);
				string args = "quarter=" + quarter + "&year=" + year + "&new_month_meters_day=" + yspinLastReadingDay.Text;
				ViewReportExt.RunWithSubreports ("ServiceProviderQuarterlyReport", args,new string[]{subReportName});
			}
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

		protected void OnYspinLastReadingDayValueChanged (object sender, EventArgs e)
		{
			MainSupport.BaseParameters.UpdateParameter (QSMain.ConnectionDB, BaseParameterNames.ReportLastReadingDay.ToString(), yspinLastReadingDay.Text);
		}
	}

}

