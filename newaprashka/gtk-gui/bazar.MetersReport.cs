
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class MetersReport
	{
		private global::Gtk.Label label3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gtk.TreeView treeviewProviders;

		private global::Gtk.Label label1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeviewMeters;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Table table1;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Label label6;

		private global::Gtk.VBox vbox4;

		private global::Gtk.RadioButton radioButtonMonth;

		private global::Gtk.RadioButton radioButtonQuarter;

		private global::Gtk.VBox vbox5;

		private global::Gtk.HBox hbox4;

		private global::Gtk.Label label5;

		private global::Gtk.ComboBox comboPeriod;

		private global::Gtk.ComboBox comboYear;

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
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Поставщики");
			w1.Add(this.label3);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(w1[this.label3]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewProviders = new global::Gtk.TreeView();
			this.treeviewProviders.CanFocus = true;
			this.treeviewProviders.Name = "treeviewProviders";
			this.GtkScrolledWindow1.Add(this.treeviewProviders);
			w1.Add(this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1[this.GtkScrolledWindow1]));
			w4.Position = 1;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Счетчики");
			w1.Add(this.label1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1[this.label1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
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
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(w1[this.GtkScrolledWindow]));
			w7.Position = 3;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.hbox2.BorderWidth = ((uint)(3));
			// Container child hbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(2)), ((uint)(3)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			this.table1.Add(this.hbox3);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox3]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.LeftAttach = ((uint)(2));
			w8.RightAttach = ((uint)(3));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label();
			this.label6.Name = "label6";
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("Отчет за:");
			this.table1.Add(this.label6);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.label6]));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.vbox4 = new global::Gtk.VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.radioButtonMonth = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Месяц"));
			this.radioButtonMonth.CanFocus = true;
			this.radioButtonMonth.Name = "radioButtonMonth";
			this.radioButtonMonth.DrawIndicator = true;
			this.radioButtonMonth.UseUnderline = true;
			this.radioButtonMonth.Group = new global::GLib.SList(global::System.IntPtr.Zero);
			this.vbox4.Add(this.radioButtonMonth);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.radioButtonMonth]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.radioButtonQuarter = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Квартал"));
			this.radioButtonQuarter.CanFocus = true;
			this.radioButtonQuarter.Name = "radioButtonQuarter";
			this.radioButtonQuarter.DrawIndicator = true;
			this.radioButtonQuarter.UseUnderline = true;
			this.radioButtonQuarter.Group = this.radioButtonMonth.Group;
			this.vbox4.Add(this.radioButtonQuarter);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.radioButtonQuarter]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.table1.Add(this.vbox4);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1[this.vbox4]));
			w12.LeftAttach = ((uint)(2));
			w12.RightAttach = ((uint)(3));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			this.hbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.table1]));
			w13.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Период:");
			this.hbox4.Add(this.label5);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.label5]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.comboPeriod = global::Gtk.ComboBox.NewText();
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Январь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Февраль"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Март"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Апрель"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Май"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Июнь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Июль"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Август"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Сентябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Октябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Ноябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Декабрь"));
			this.comboPeriod.Name = "comboPeriod";
			this.hbox4.Add(this.comboPeriod);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.comboPeriod]));
			w15.Position = 1;
			w15.Expand = false;
			w15.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.comboYear = global::Gtk.ComboBox.NewText();
			this.comboYear.Name = "comboYear";
			this.hbox4.Add(this.comboYear);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.comboYear]));
			w16.Position = 2;
			w16.Expand = false;
			w16.Fill = false;
			this.vbox5.Add(this.hbox4);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.hbox4]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			this.hbox2.Add(this.vbox5);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.vbox5]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			w1.Add(this.hbox2);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(w1[this.hbox2]));
			w19.Position = 4;
			w19.Expand = false;
			w19.Fill = false;
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
			global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.checkHandmade]));
			w20.Position = 0;
			w20.Expand = false;
			w20.Fill = false;
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
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label2]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
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
			global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioLetter]));
			w22.Position = 0;
			w22.Expand = false;
			w22.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.radioAlbum = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Альбомная"));
			this.radioAlbum.CanFocus = true;
			this.radioAlbum.Name = "radioAlbum";
			this.radioAlbum.DrawIndicator = true;
			this.radioAlbum.UseUnderline = true;
			this.radioAlbum.Group = this.radioLetter.Group;
			this.vbox3.Add(this.radioAlbum);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioAlbum]));
			w23.Position = 1;
			w23.Expand = false;
			w23.Fill = false;
			this.hbox1.Add(this.vbox3);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox3]));
			w24.Position = 1;
			this.vbox2.Add(this.hbox1);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
			w25.Position = 1;
			w25.Expand = false;
			w25.Fill = false;
			w1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(w1[this.vbox2]));
			w26.PackType = ((global::Gtk.PackType)(1));
			w26.Position = 6;
			w26.Expand = false;
			w26.Fill = false;
			// Internal child bazar.MetersReport.ActionArea
			global::Gtk.HButtonBox w27 = this.ActionArea;
			w27.Name = "dialog1_ActionArea";
			w27.Spacing = 10;
			w27.BorderWidth = ((uint)(5));
			w27.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w28 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w27[this.buttonCancel]));
			w28.Expand = false;
			w28.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w29 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w27[this.buttonOk]));
			w29.Position = 1;
			w29.Expand = false;
			w29.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 558;
			this.DefaultHeight = 651;
			this.Show();
			this.radioButtonMonth.Toggled += new global::System.EventHandler(this.OnRadiobuttonQuarterToggled);
			this.radioButtonQuarter.Toggled += new global::System.EventHandler(this.OnRadiobuttonQuarterToggled);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
