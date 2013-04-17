using System;
using System.IO;
using System.Xml;

namespace bazar
{
	public class ReportsExt
	{
		public ReportsExt ()
		{
		}

		public static void ViewReport(string ReportName, string Params)
		{
			ViewReport(ReportName, Params, false);
		}

		public static void ViewReport(string ReportName, string Params, bool UserVar)
		{
			string ReportPath = System.IO.Path.Combine( Directory.GetCurrentDirectory(), "Reports", ReportName + ".rdl");

			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(ReportPath);
			
			foreach (XmlNode node in xmlDoc.GetElementsByTagName("ConnectString"))
			{
				if(UserVar)
					node.InnerText = MainClass.ConnectionString + "Allow User Variables=True";
				else
					node.InnerText = MainClass.ConnectionString;
			}

			ReportPath = System.IO.Path.Combine (System.IO.Path.GetTempPath (), ReportName + ".rdl");
			xmlDoc.Save(ReportPath);

			string arg = "-r \"" + ReportPath +"\" -p \"" + Params + "\"";
			System.Diagnostics.Process.Start ("RdlReader.exe", arg);
		}
	}
}

