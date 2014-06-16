using System;
using Gtk;
using QSProjectsLib;
using MySql.Data.MySqlClient;

namespace bazar
{
	public partial class LesseeDebtsReport : Gtk.Dialog
	{
		ListStore ServicesList;

		public LesseeDebtsReport ()
		{
			this.Build ();

			ComboWorks.ComboFillReference (comboCash, "cash", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference (comboOrg, "organizations", ComboWorks.ListMode.WithAll);

			dateReport.Date = DateTime.Today;

			ServicesList = new ListStore (typeof(int), typeof(bool), typeof(string));

			CellRendererToggle CellSelect = new CellRendererToggle();
			CellSelect.Activatable = true;
			CellSelect.Toggled += onCellSelectToggled;

			treeviewServices.AppendColumn ("Выб.", CellSelect, "active", 1);
			treeviewServices.AppendColumn ("Услуги", new CellRendererText (), "text", 2);

			treeviewServices.Model = ServicesList;

			MainClass.StatusMessage("Запрос типов услуг...");
			string sql = "SELECT id, name FROM services";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);

				using(MySqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						ServicesList.AppendValues(rdr.GetInt32 ("id"),
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
				MainClass.StatusMessage("Ошибка получения информации о услугах!");
				QSMain.ErrorMessage(this, ex);
			}

			comboCash.Active = 0;
			comboOrg.Active = 0;
		}

		protected	void TestCanSave ()
		{
			bool Cashok = comboCash.Active > 0;
			bool Servicesok = !checkDetail.Active;
			foreach(object[] row in ServicesList)
			{
				if ((bool) row [1])
				{
					Servicesok = true;
					break;
				}
			}

			buttonOk.Sensitive = Cashok && Servicesok;
		}

		void onCellSelectToggled(object o, ToggledArgs args) 
		{
			TreeIter iter;

			if (ServicesList.GetIter (out iter, new TreePath(args.Path))) 
			{
				bool old = (bool) ServicesList.GetValue(iter,1);
				ServicesList.SetValue(iter, 1, !old);
				TestCanSave ();
			}
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if(dateReport.IsEmpty)
				return;
			string param = "Day=" + dateReport.Date.Day.ToString() +
				"&Month=" + dateReport.Date.Month.ToString() +
				"&Year=" + dateReport.Date.Year.ToString() + 
				"&Cash=" + ComboWorks.GetActiveId(comboCash) +
				"&Org=" + ComboWorks.GetActiveId(comboOrg);
			if(checkDetail.Active)
			{
				param += "&Services=";
				foreach(object[] row in ServicesList)
				{
					if ((bool) row [1])
						param += String.Format ("{0},", row [0]);
				}
				ViewReportExt.Run ("LesseeDebtsDetail", param.TrimEnd (','));
			}
			else
				ViewReportExt.Run ("LesseeDebts", param);
		}

		protected void OnCheckDetailClicked(object sender, EventArgs e)
		{
			treeviewServices.Sensitive = checkDetail.Active;
			TestCanSave ();
		}

		protected void OnComboCashChanged(object sender, EventArgs e)
		{
			TestCanSave ();
		}
	}
}

