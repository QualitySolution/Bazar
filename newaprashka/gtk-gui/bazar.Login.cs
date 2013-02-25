
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class Login
	{
		private global::Gtk.VBox vbox2;
		private global::Gtk.Image image80;
		private global::Gtk.Table table1;
		private global::Gtk.Button buttonDemo;
		private global::Gtk.Entry entryPassword;
		private global::Gtk.Entry entryServer;
		private global::Gtk.Entry entryUser;
		private global::Gtk.Label label1;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Label labelLoginInfo;
		private global::Gtk.Button buttonErrorInfo;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.Login
			this.Name = "bazar.Login";
			this.Title = "Вход в БазАр";
			this.Icon = global::Stetic.IconLoader.LoadIcon (this, "gtk-connect", global::Gtk.IconSize.Menu);
			this.WindowPosition = ((global::Gtk.WindowPosition)(1));
			// Internal child bazar.Login.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.image80 = new global::Gtk.Image ();
			this.image80.WidthRequest = 0;
			this.image80.HeightRequest = 0;
			this.image80.Name = "image80";
			this.image80.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("bazar.logo.png");
			this.vbox2.Add (this.image80);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.image80]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(33));
			// Container child table1.Gtk.Table+TableChild
			this.buttonDemo = new global::Gtk.Button ();
			this.buttonDemo.CanFocus = true;
			this.buttonDemo.Name = "buttonDemo";
			this.buttonDemo.UseUnderline = true;
			this.buttonDemo.Relief = ((global::Gtk.ReliefStyle)(2));
			// Container child buttonDemo.Gtk.Container+ContainerChild
			global::Gtk.Alignment w3 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w4 = new global::Gtk.HBox ();
			w4.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w5 = new global::Gtk.Image ();
			w5.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-info", global::Gtk.IconSize.Button);
			w4.Add (w5);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w7 = new global::Gtk.Label ();
			w4.Add (w7);
			w3.Add (w4);
			this.buttonDemo.Add (w3);
			this.table1.Add (this.buttonDemo);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonDemo]));
			w11.LeftAttach = ((uint)(2));
			w11.RightAttach = ((uint)(3));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryPassword = new global::Gtk.Entry ();
			this.entryPassword.CanFocus = true;
			this.entryPassword.Name = "entryPassword";
			this.entryPassword.IsEditable = true;
			this.entryPassword.Visibility = false;
			this.entryPassword.InvisibleChar = '●';
			this.table1.Add (this.entryPassword);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryPassword]));
			w12.TopAttach = ((uint)(2));
			w12.BottomAttach = ((uint)(3));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryServer = new global::Gtk.Entry ();
			this.entryServer.CanFocus = true;
			this.entryServer.Name = "entryServer";
			this.entryServer.IsEditable = true;
			this.entryServer.InvisibleChar = '●';
			this.table1.Add (this.entryServer);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryServer]));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryUser = new global::Gtk.Entry ();
			this.entryUser.CanFocus = true;
			this.entryUser.Name = "entryUser";
			this.entryUser.IsEditable = true;
			this.entryUser.InvisibleChar = '●';
			this.table1.Add (this.entryUser);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryUser]));
			w14.TopAttach = ((uint)(1));
			w14.BottomAttach = ((uint)(2));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = "Пользователь:";
			this.label1.Justify = ((global::Gtk.Justification)(1));
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w15.TopAttach = ((uint)(1));
			w15.BottomAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = "Пароль:";
			this.label2.Justify = ((global::Gtk.Justification)(1));
			this.table1.Add (this.label2);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
			w16.TopAttach = ((uint)(2));
			w16.BottomAttach = ((uint)(3));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = "Сервер:";
			this.table1.Add (this.label3);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add (this.table1);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.table1]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.labelLoginInfo = new global::Gtk.Label ();
			this.labelLoginInfo.Name = "labelLoginInfo";
			this.labelLoginInfo.Wrap = true;
			this.hbox1.Add (this.labelLoginInfo);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.labelLoginInfo]));
			w19.Position = 0;
			w19.Expand = false;
			w19.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonErrorInfo = new global::Gtk.Button ();
			this.buttonErrorInfo.CanFocus = true;
			this.buttonErrorInfo.Name = "buttonErrorInfo";
			this.buttonErrorInfo.UseUnderline = true;
			// Container child buttonErrorInfo.Gtk.Container+ContainerChild
			global::Gtk.Alignment w20 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w21 = new global::Gtk.HBox ();
			w21.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w22 = new global::Gtk.Image ();
			w22.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-error", global::Gtk.IconSize.Button);
			w21.Add (w22);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w24 = new global::Gtk.Label ();
			w24.LabelProp = "Подробнее..";
			w24.UseUnderline = true;
			w21.Add (w24);
			w20.Add (w21);
			this.buttonErrorInfo.Add (w20);
			this.hbox1.Add (this.buttonErrorInfo);
			global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonErrorInfo]));
			w28.Position = 1;
			w28.Expand = false;
			w28.Fill = false;
			this.vbox2.Add (this.hbox1);
			global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox1]));
			w29.Position = 2;
			w29.Expand = false;
			w29.Fill = false;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w30.Position = 0;
			w30.Expand = false;
			w30.Fill = false;
			// Internal child bazar.Login.ActionArea
			global::Gtk.HButtonBox w31 = this.ActionArea;
			w31.Name = "dialog1_ActionArea";
			w31.Spacing = 10;
			w31.BorderWidth = ((uint)(5));
			w31.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w32 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w33 = new global::Gtk.HBox ();
			w33.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w34 = new global::Gtk.Image ();
			w34.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w33.Add (w34);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w36 = new global::Gtk.Label ();
			w36.LabelProp = "О_тменить";
			w36.UseUnderline = true;
			w33.Add (w36);
			w32.Add (w33);
			this.buttonCancel.Add (w32);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w40 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w31 [this.buttonCancel]));
			w40.Expand = false;
			w40.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w41 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w42 = new global::Gtk.HBox ();
			w42.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w43 = new global::Gtk.Image ();
			w43.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w42.Add (w43);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w45 = new global::Gtk.Label ();
			w45.LabelProp = "_OK";
			w45.UseUnderline = true;
			w42.Add (w45);
			w41.Add (w42);
			this.buttonOk.Add (w41);
			w31.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w49 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w31 [this.buttonOk]));
			w49.Position = 1;
			w49.Expand = false;
			w49.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 396;
			this.DefaultHeight = 401;
			this.buttonDemo.Hide ();
			this.buttonErrorInfo.Hide ();
			this.Show ();
			this.Response += new global::Gtk.ResponseHandler (this.OnResponse);
			this.entryUser.Activated += new global::System.EventHandler (this.OnEntryActivated);
			this.entryServer.Activated += new global::System.EventHandler (this.OnEntryActivated);
			this.entryServer.Changed += new global::System.EventHandler (this.OnEntryServerChanged);
			this.entryPassword.Activated += new global::System.EventHandler (this.OnEntryPasswordActivated);
			this.buttonDemo.Clicked += new global::System.EventHandler (this.OnButtonDemoClicked);
			this.buttonErrorInfo.Clicked += new global::System.EventHandler (this.OnButtonErrorInfoClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
