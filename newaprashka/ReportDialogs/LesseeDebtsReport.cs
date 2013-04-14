using System;

namespace bazar
{
	public partial class LesseeDebtsReport : Gtk.Dialog
	{
		public LesseeDebtsReport ()
		{
			this.Build ();

			MainClass.ComboAccrualYearsFill (comboYear);
			comboMonth.Active = DateTime.Now.Month;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if(comboMonth.Active == 0)
				return;
			string param = "Month=" + comboMonth.Active.ToString () + "&Year=" + comboYear.ActiveText;
			ReportsExt.ViewReport ("LesseeDebts", param);
		}
	}
}

