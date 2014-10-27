using System;
using Gtk;
using QSProjectsLib;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class MetersReport : Gtk.Dialog
	{
		ListStore MetersList;

		public MetersReport ()
		{
			this.Build ();

			MetersList = new ListStore (typeof(int), typeof(bool), typeof(string));

			CellRendererToggle CellSelect = new CellRendererToggle();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;

			treeviewMeters.AppendColumn ("Выб.", CellSelect, "active", 1);
			treeviewMeters.AppendColumn ("Тип счётчика", new CellRendererText (), "text", 2);

			treeviewMeters.Model = MetersList;

			MainClass.StatusMessage("Запрос типов счетчиков...");
			string sql = "SELECT id, name FROM meter_types";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						MetersList.AppendValues(rdr.GetInt32 ("id"),
						                             false,
						                             rdr.GetString ("name")
						                             );
					}
				}
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о типах счетчиков!");
				QSMain.ErrorMessage(this, ex);
			}
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;

			if (MetersList.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) MetersList.GetValue(iter,1);
				MetersList.SetValue(iter, 1, !old);
			}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			string param = "types=";
			foreach(object[] row in MetersList)
			{
				if ((bool) row [1])
					param += String.Format ("{0},", row [0]);
			}
			if (checkHandmade.Active)
				ViewReportExt.Run ("MetersFill", param.TrimEnd (','));
			else
				ViewReportExt.Run ("Meters", param.TrimEnd (','));
		}
	}
}

