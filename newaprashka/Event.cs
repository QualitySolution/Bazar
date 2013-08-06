using System;
using System.Data;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class Event : Gtk.Dialog
	{
		public bool NewEvent;
		int Eventid, lessee_id, PlaceTypeId;
		DateTime EventDate;
		
		public Event ()
		{
			this.Build ();
			textviewCause.Buffer.Changed += OnTextViewChanged;
			textviewActivity.Buffer.Changed += OnTextViewChanged;
			ComboWorks.ComboFillReference(comboEventPlaceT, "place_types", 2);
			ComboWorks.ComboFillReference(comboEventType, "classes", 2);
			labelUser.Text = QSMain.User.Name;
		}
		
		public void EventFill(int id)
		{
			Eventid = id;
			NewEvent = false;
			TreeIter iter;
			
			MainClass.StatusMessage(String.Format("Запрос события №{0}...",id));
			string sql = "SELECT *, lessees.name as lessee FROM events " +
				"LEFT JOIN lessees ON events.lessee_id = lessees.id " +
				"WHERE events.id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", id);
		
				MySqlDataReader rdr = cmd.ExecuteReader();
					
				rdr.Read();
				
				labelDate.Text = rdr["date"].ToString();
				EventDate = (DateTime)rdr["date"];
				if(rdr["class_id"] != DBNull.Value)
				{
					ListStoreWorks.SearchListStore((ListStore)comboEventType.Model, (int)(uint)rdr["class_id"], out iter);
					comboEventType.SetActiveIter(iter);
				}
				if(rdr["lessee_id"] != DBNull.Value)
					lessee_id = (int)(uint)rdr["lessee_id"];
				entryLessee.Text = rdr["lessee"].ToString();
				entryLessee.TooltipText = rdr["lessee"].ToString();
				
				textviewCause.Buffer.Text = rdr["cause"].ToString();
				textviewActivity.Buffer.Text = rdr["activity"].ToString();
				labelUser.Text = rdr["user"].ToString();
				
				//Записываем значения в переменную что бы отпустить rdr
				object DBPlaceT, DBPlaceNo;
				DBPlaceT = rdr["place_type_id"];
				DBPlaceNo = rdr["place_no"];
				
				rdr.Close();
				MainClass.StatusMessage("Ok");
				
				if(DBPlaceT != DBNull.Value)
				{
					ListStoreWorks.SearchListStore((ListStore)comboEventPlaceT.Model, (int)(uint)DBPlaceT, out iter);
					comboEventPlaceT.SetActiveIter(iter);
					//MainClass.ComboPlaceNoFill(comboboxPlaceNo,(int)(uint)rdr["place_type_id"]);
				}
				// Перебераем комбобокс мест и устанавливаем место если находим
				if(comboboxPlaceNo.Model.GetIterFirst(out iter))
				{
					if( (string)comboboxPlaceNo.Model.GetValue(iter,0) == DBPlaceNo.ToString())
						comboboxPlaceNo.SetActiveIter(iter);
				}
				while (comboboxPlaceNo.Model.IterNext(ref iter)) 
				{
					if( (string)comboboxPlaceNo.Model.GetValue(iter,0) == DBPlaceNo.ToString())
						comboboxPlaceNo.SetActiveIter(iter);
				}
				//this.Title = ;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения информации о событии!");
				QSMain.ErrorMessage(this,ex);
			}
			TestCanSave();
		}
		
		protected	void TestCanSave ()
		{
			bool Lesseeok = entryLessee.Text != "";
			bool Placeok = comboboxPlaceNo.Active >= 0 && comboEventPlaceT.Active > 0;
			bool TypeEventok = comboEventType.Active > 0;
			bool Textok = textviewCause.Buffer.Text != "" || textviewActivity.Buffer.Text != "";
			buttonOk.Sensitive = (Lesseeok || Placeok) && TypeEventok && Textok; 
		}
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string sql;
			TreeIter iter;
			
			MainClass.StatusMessage("Запись события...");
			if(NewEvent)
			{
				sql = "INSERT INTO events (date, class_id, user, lessee_id, place_type_id, " +
					"place_no, cause, activity) " +
					"VALUES (@date, @class_id, @user, @lessee_id, @place_type_id, " +
					"@place_no, @cause, @activity)";
				EventDate = DateTime.Now;
			}
			else
			{
				sql = "UPDATE events SET date = @date, class_id = @class_id, user = @user, " +
					"lessee_id = @lessee_id, place_type_id = @place_type_id, place_no = @place_no, " +
					"cause = @cause, activity = @activity " +
					"WHERE id = @id";
			}
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Eventid);
				cmd.Parameters.AddWithValue("@date", EventDate);
				if(comboEventType.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@class_id", comboEventType.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@class_id", DBNull.Value);
				cmd.Parameters.AddWithValue("@user", QSMain.User.Name);
				if(entryLessee.Text != "")
					cmd.Parameters.AddWithValue("@lessee_id", lessee_id);
				else
					cmd.Parameters.AddWithValue("@lessee_id", DBNull.Value);
				if(comboEventPlaceT.GetActiveIter(out iter))
					cmd.Parameters.AddWithValue("@place_type_id", comboEventPlaceT.Model.GetValue(iter,1));
				else
					cmd.Parameters.AddWithValue("@place_type_id", DBNull.Value);
				if(comboboxPlaceNo.ActiveText == "")
					cmd.Parameters.AddWithValue("@place_no", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@place_no", comboboxPlaceNo.ActiveText);
				if(textviewCause.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@cause", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@cause", textviewCause.Buffer.Text);
				if(textviewActivity.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@activity", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@activity", textviewActivity.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошбика записи события!");
				QSMain.ErrorMessage(this,ex);
			}
		}
		
		protected virtual void OnComboEventPlaceTChanged (object sender, System.EventArgs e)
		{
			TreeIter iter;
			((ListStore)comboboxPlaceNo.Model).Clear();
			if(comboEventPlaceT.GetActiveIter(out iter) && comboEventPlaceT.Active > 0)
			{
				PlaceTypeId = (int)comboEventPlaceT.Model.GetValue(iter,1);
				MainClass.ComboPlaceNoFill(comboboxPlaceNo,PlaceTypeId);
			}
		}
		
		protected virtual void OnComboboxPlaceNoChanged (object sender, System.EventArgs e)
		{
			if(NewEvent && comboboxPlaceNo.ActiveText != null)
			{
				MainClass.StatusMessage("Запрос арендатора торгового места...");
				string sql = "SELECT lessee_id, lessees.name as lessee FROM contracts " +
					"LEFT JOIN lessees ON contracts.lessee_id = lessees.id " +
					"WHERE place_type_id = @type_id AND place_no = @place_no AND " +
					"((contracts.cancel_date IS NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.end_date) " +
					"OR (contracts.cancel_date IS NOT NULL AND CURDATE() BETWEEN contracts.start_date AND contracts.cancel_date))";
				try
				{
					MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
					
					cmd.Parameters.AddWithValue("@type_id", PlaceTypeId);
					cmd.Parameters.AddWithValue("@place_no", comboboxPlaceNo.ActiveText);
			
					MySqlDataReader rdr = cmd.ExecuteReader();
						
					if(rdr.Read() && rdr.GetValue(0) != DBNull.Value)
					{
						lessee_id = rdr.GetInt32(0);
						entryLessee.Text = rdr.GetString(1);
						entryLessee.TooltipText = rdr.GetString(1);
					}
					rdr.Close();
					MainClass.StatusMessage("Ok");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					MainClass.StatusMessage("Ошибка получения арендатора торгового места!");
					QSMain.ErrorMessage(this,ex);
				}				
			}
			TestCanSave();
		}
		
		protected virtual void OnComboEventTypeChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		protected virtual void OnTextViewChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		protected virtual void OnButtonLesseeCleanClicked (object sender, System.EventArgs e)
		{
			lessee_id = -1;
			entryLessee.Text = "";
		}
		
		protected virtual void OnButtonLesseeEditClicked (object sender, System.EventArgs e)
		{
			reference LesseeSelect = new reference();
			LesseeSelect.SetMode(false,true,true,true,false);
			LesseeSelect.FillList("lessees","Арендатор", "Арендаторы");
			LesseeSelect.Show();
			int result = LesseeSelect.Run();
			if((ResponseType)result == ResponseType.Ok)
			{
				lessee_id = LesseeSelect.SelectedID;
				entryLessee.Text = LesseeSelect.SelectedName;
			}
			LesseeSelect.Destroy();
		}
		
		
	}
}

