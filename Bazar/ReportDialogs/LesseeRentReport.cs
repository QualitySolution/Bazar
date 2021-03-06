﻿using System;
using Bazar;
using Bazar.Repositories.Estate;
using Gtk;
using MySql.Data.MySqlClient;
using QS.DomainModel.UoW;
using QSProjectsLib;

namespace bazar
{
	public partial class LesseeRentReport : Gtk.Dialog
	{
		IUnitOfWork UoW = UnitOfWorkFactory.CreateWithoutRoot();

		public LesseeRentReport ()
		{
			this.Build (); 
			ComboWorks.ComboFillReference (comboPlaceType, "place_types", ComboWorks.ListMode.OnlyItems, OrderBy: "name");
			MainClass.ComboAccrualYearsFill (comboStartYear);
			MainClass.ComboAccrualYearsFill (comboEndYear);
		}
		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboLessee) == null || String.IsNullOrEmpty(comboPlace.ActiveText))
				return;
			var placeId = PlaceRepository.GetPlaceId(UoW, ComboWorks.GetActiveId(comboPlaceType), comboPlace.ActiveText);
			string param = "MonthStart=" + (comboStartMonth.Active + 1).ToString () +
				"&MonthEnd=" + (comboEndMonth.Active + 1).ToString() +
				"&YearStart=" + comboStartYear.ActiveText + 
				"&YearEnd=" + comboEndYear.ActiveText +
				"&LesseeId=" + ComboWorks.GetActiveId(comboLessee) +
				"&Place=" + placeId.ToString();

			ViewReportExt.Run ("LesseeReport", param);
		}
			
		protected void OnComboPlaceChanged (object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(comboPlace.ActiveText)) {
				comboLessee.Active = -1;
				comboLessee.Sensitive = false;
				return;
			}
			string SQL = "SELECT DISTINCT lessees.id, lessees.name FROM lessees " +
				"LEFT JOIN contracts ON contracts.lessee_id = lessees.id " +
				"WHERE contracts.id IN " +
				"(SELECT contract_pays.contract_id FROM contract_pays WHERE contract_pays.place_id = @place_id)";
			var placeId = PlaceRepository.GetPlaceId(UoW, ComboWorks.GetActiveId(comboPlaceType), comboPlace.ActiveText);
			MySqlParameter[] Param = { 
				new MySqlParameter("@place_id", placeId)
			};
			ComboWorks.ComboFillUniversal (comboLessee, SQL, "{1}", Param, 0, ComboWorks.ListMode.OnlyItems, true);
			comboLessee.Sensitive = true;
		}

		protected void OnComboPlaceTypeChanged (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboPlaceType) == null) {
				comboPlace.Sensitive = comboLessee.Sensitive = false;
				return;
			}

			MainClass.ComboPlaceNoFill (comboPlace, ComboWorks.GetActiveId (comboPlaceType));
			comboPlace.Sensitive = true;
		}

		protected void OnComboLesseeChanged (object sender, EventArgs e)
		{
			if (ComboWorks.GetActiveIdOrNull (comboLessee) != null && 
			    ComboWorks.GetActiveIdOrNull (comboPlaceType) != null &&
			    !String.IsNullOrEmpty(comboPlace.ActiveText)) {
				buttonOk.Sensitive = true;
				var placeId = PlaceRepository.GetPlaceId(UoW, ComboWorks.GetActiveId(comboPlaceType), comboPlace.ActiveText);
				string SQL = "SELECT MIN(start_date) AS start, MAX(end_date) AS end FROM contracts, contract_pays " +
					"WHERE contract_pays.contract_id = contracts.id AND contracts.lessee_id = @lessee_id AND contract_pays.place_id = @place_id";
				MySqlCommand cmd = new MySqlCommand(SQL, QSMain.connectionDB);
				cmd.Parameters.AddWithValue ("@lessee_id", ComboWorks.GetActiveId (comboLessee));
				cmd.Parameters.AddWithValue ("@place_id", placeId);
				MySqlDataReader rdr = cmd.ExecuteReader();
				if (!rdr.Read ())
					return;
				TreeIter iter;
				DateTime date = rdr.GetDateTime ("start");
				comboStartMonth.Active = date.Month - 1;
				if (ListStoreWorks.SearchListStore ((ListStore)comboStartYear.Model, Convert.ToString (date.Year), out iter))
					comboStartYear.SetActiveIter (iter);
				else
					comboStartYear.Active = 0;
				date = rdr.GetDateTime ("end");
				comboEndMonth.Active = date.Month - 1;
				if (ListStoreWorks.SearchListStore ((ListStore)comboEndYear.Model, Convert.ToString (date.Year), out iter))
					comboEndYear.SetActiveIter (iter);
				else
					comboEndYear.Active = 0;
				rdr.Close ();
			}
		}
	}
}