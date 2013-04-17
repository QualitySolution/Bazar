using System;

namespace bazar
{
	public partial class DocRegister : Gtk.Dialog
	{
		public DocRegister ()
		{
			this.Build ();
			radioCashToday.Click ();
			comboDoc.Active = 0;
		}

		protected void OnRadioCashTodayClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.Date;
			dateCashEnd.Date = DateTime.Now.Date;
		}
		
		protected void OnRadioCashWeekClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddDays(-7);
			dateCashEnd.Date = DateTime.Now.Date;
		}
		
		protected void OnRadioMonthClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddDays(-30);
			dateCashEnd.Date = DateTime.Now.Date;
		}
		
		protected void OnRadioCash6MonthClicked (object sender, EventArgs e)
		{
			dateCashStart.Date = DateTime.Now.AddDays(-183);
			dateCashEnd.Date = DateTime.Now.Date;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string param;
			string Startdate = String.Format ("{0:u}", dateCashStart.Date).Substring (0, 10);
			string Enddate = String.Format ("{0:u}", dateCashEnd.Date).Substring (0, 10);
			switch (comboDoc.Active) {
			case 0:
				param = "Start=" + Startdate + "&End=" + Enddate;
				ReportsExt.ViewReport ("IncomeRegister", param);
				break;
			case 1:
				param = "Start=" + Startdate + "&End=" + Enddate;
				ReportsExt.ViewReport ("ExpenseRegister", param);
				break;
			default:
				break;
			}
		}
	}
}

