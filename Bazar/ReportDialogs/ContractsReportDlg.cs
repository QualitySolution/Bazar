using System;
using QSProjectsLib;

namespace bazar
{
	public partial class ContractsReportDlg : Gtk.Dialog
	{
		public ContractsReportDlg ()
		{
			this.Build ();
			Configure ();
		}

		public void Configure()
		{
			ComboWorks.ComboFillReference (comboOrg, "organizations", ComboWorks.ListMode.WithAll, OrderBy: "name");
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			string param = "org_id=" + ComboWorks.GetActiveId(comboOrg) + "&active=" + (radiobuttonActive.Active ? "1" : "0");
			ViewReportExt.Run ("Contracts", param);
		}
	}
}

