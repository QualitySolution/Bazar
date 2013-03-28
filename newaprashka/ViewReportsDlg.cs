using System;
using System.IO;
using System.Drawing;
using Gtk;

namespace bazar
{
	public partial class ViewReportsDlg : Gtk.Dialog
	{
		private LibRdlCrossPlatformViewer.ReportViewer rv;

		public ViewReportsDlg ()
		{
			this.Build ();

			Xwt.Application.Initialize(Xwt.ToolkitType.Gtk);
			Xwt.Engine.Toolkit.ExitUserCode(null);
			
			rv = new LibRdlCrossPlatformViewer.ReportViewer();
			rv.DefaultBackend = LibRdlCrossPlatformViewer.Backend.XwtWinforms;
			
			Gtk.Widget w = (Gtk.Widget)Xwt.Engine.WidgetRegistry.GetNativeWidget(rv);
			
			this.VBox.Add (w);

		}

		public void LoadReport( string ReportName)
		{
			string[] str = new string[] {Directory.GetCurrentDirectory(), "Reports", ReportName + ".rdl"};
			rv.LoadReport(new Uri(System.IO.Path.Combine (str)), "id=65");
		}
	}
}

