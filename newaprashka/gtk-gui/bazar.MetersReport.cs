
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class MetersReport
	{
		private global::Gtk.Label label1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeviewMeters;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Table table1;

		private global::Gtk.VBox vbox5;

		private global::Gtk.HBox hbox4;

		private global::Gtk.VBox vbox2;

		private global::Gtk.CheckButton checkHandmade;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label2;

		private global::Gtk.VBox vbox3;

		private global::Gtk.RadioButton radioLetter;

		private global::Gtk.RadioButton radioAlbum;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget bazar.MetersReport
			this.Name = "bazar.MetersReport";
			this.Title = global::Mono.Unix.Catalog.GetString("Отчет по счётчикам");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.MetersReport.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Счетчики");
			w1.Add(this.label1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(w1[this.label1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			this.GtkScrolledWindow.BorderWidth = ((uint)(9));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeviewMeters = new global::Gtk.TreeView();
			this.treeviewMeters.CanFocus = true;
			this.treeviewMeters.Name = "treeviewMeters";
			this.GtkScrolledWindow.Add(this.treeviewMeters);
			w1.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1[this.GtkScrolledWindow]));
			w4.Position = 1;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.hbox2.BorderWidth = ((uint)(3));
			// Container child hbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(1)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.hbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.table1]));
			w5.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			this.vbox5.Add(this.hbox4);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.hbox4]));
			w6.Position = 0;
			this.hbox2.Add(this.vbox5);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.vbox5]));
			w7.Position = 1;
			w1.Add(this.hbox2);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(w1[this.hbox2]));
			w8.Position = 2;
			w8.Expand = false;
			w8.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.checkHandmade = new global::Gtk.CheckButton();
			this.checkHandmade.CanFocus = true;
			this.checkHandmade.Name = "checkHandmade";
			this.checkHandmade.Label = global::Mono.Unix.Catalog.GetString("Форма для ручного заполнения");
			this.checkHandmade.DrawIndicator = true;
			this.checkHandmade.UseUnderline = true;
			this.vbox2.Add(this.checkHandmade);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.checkHandmade]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Ориентация страницы:");
			this.hbox1.Add(this.label2);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label2]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.radioLetter = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Книжная"));
			this.radioLetter.CanFocus = true;
			this.radioLetter.Name = "radioLetter";
			this.radioLetter.DrawIndicator = true;
			this.radioLetter.UseUnderline = true;
			this.radioLetter.Group = new global::GLib.SList(global::System.IntPtr.Zero);
			this.vbox3.Add(this.radioLetter);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioLetter]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.radioAlbum = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Альбомная"));
			this.radioAlbum.CanFocus = true;
			this.radioAlbum.Name = "radioAlbum";
			this.radioAlbum.DrawIndicator = true;
			this.radioAlbum.UseUnderline = true;
			this.radioAlbum.Group = this.radioLetter.Group;
			this.vbox3.Add(this.radioAlbum);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioAlbum]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			this.hbox1.Add(this.vbox3);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox3]));
			w13.Position = 1;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w15.PackType = ((global::Gtk.PackType)(1));
			w15.Position = 4;
			w15.Expand = false;
			w15.Fill = false;
			// Internal child bazar.MetersReport.ActionArea
			global::Gtk.HButtonBox w16 = this.ActionArea;
			w16.Name = "dialog1_ActionArea";
			w16.Spacing = 10;
			w16.BorderWidth = ((uint)(5));
			w16.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16[this.buttonCancel]));
			w17.Expand = false;
			w17.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w18 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w16[this.buttonOk]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 558;
			this.DefaultHeight = 514;
			this.Show();
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
