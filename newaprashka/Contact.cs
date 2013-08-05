using System;
using System.Data;
using Gtk;
using MySql.Data;
using MySql.Data.MySqlClient;
using QSProjectsLib;

namespace bazar
{
	public partial class Contact : Gtk.Dialog
	{
		public bool NewContact;
		int Contactid;
		
		public Contact ()
		{
			this.Build ();
		}
		
		public void ContactFill(int id)
		{
			Contactid = id;
			NewContact = false;
			
			MainClass.StatusMessage(String.Format("Запрос контактного лица №{0}...", id));
			string sql = "SELECT * FROM contact_persons WHERE id = @id";
			try
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", id);
		
				MySqlDataReader rdr = cmd.ExecuteReader();
					
				rdr.Read();
				
				labelNumber.Text = rdr["id"].ToString();
				entryFIO.Text = rdr["name"].ToString();
				entryTel.Text = rdr["telephones"].ToString();
				textviewcomments.Buffer.Text = rdr["comments"].ToString();
				
				rdr.Close();
				this.Title = entryFIO.Text;
				MainClass.StatusMessage("Ok");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка получения контактного лица!");
				MainClass.ErrorMessage(this,ex);
			}
			TestCanSave();
		}
		
		protected	void TestCanSave ()
		{
			bool FIOok = entryFIO.Text != "";
			buttonOk.Sensitive = FIOok;
		}

		protected virtual void OnEntryFIOChanged (object sender, System.EventArgs e)
		{
			TestCanSave();
		}
		
		protected virtual void OnButtonOkClicked (object sender, System.EventArgs e)
		{
			string sql;
			if(NewContact)
			{
				sql = "INSERT INTO contact_persons (name, telephones, comments) " +
					"VALUES (@name, @tel, @comments)";
			}
			else
			{
				sql = "UPDATE contact_persons SET name = @name, telephones = @tel, comments = @comments " +
					"WHERE id = @id";
			}
			MainClass.StatusMessage("Запись контактного лица...");
			try 
			{
				MySqlCommand cmd = new MySqlCommand(sql, QSMain.connectionDB);
				
				cmd.Parameters.AddWithValue("@id", Contactid);
				cmd.Parameters.AddWithValue("@name", entryFIO.Text);
				cmd.Parameters.AddWithValue("@tel", entryTel.Text);
				if(textviewcomments.Buffer.Text == "")
					cmd.Parameters.AddWithValue("@comments", DBNull.Value);
				else
					cmd.Parameters.AddWithValue("@comments", textviewcomments.Buffer.Text);
				
				cmd.ExecuteNonQuery();
				MainClass.StatusMessage("Ok");
				Respond (ResponseType.Ok);
			} 
			catch (Exception ex) 
			{
				Console.WriteLine(ex.ToString());
				MainClass.StatusMessage("Ошибка записи контактного лица!");
				MainClass.ErrorMessage(this,ex);
			}

		}
		
		protected void OnEntryActivated (object sender, EventArgs e)
		{
			this.ChildFocus (DirectionType.TabForward);
		}
	}
}

