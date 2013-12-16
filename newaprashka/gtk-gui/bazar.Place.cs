
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class Place
	{
		private global::Gtk.VBox vbox2;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Frame frame2;
		private global::Gtk.Alignment GtkAlignment2;
		private global::Gtk.Table table2;
		private global::Gtk.ComboBox comboOrg;
		private global::Gtk.ComboBox comboPType;
		private global::Gtk.Entry entryNumber;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Entry entryContact;
		private global::Gtk.Button buttonContactOpen;
		private global::Gtk.Button buttonContactClean;
		private global::Gtk.Button buttonContact;
		private global::Gtk.Label label1;
		private global::Gtk.Label label10;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;
		private global::Gtk.Label label7;
		private global::Gtk.SpinButton spinArea;
		private global::Gtk.Label GtkLabel8;
		private global::Gtk.VBox vbox3;
		private global::Gtk.Frame frame3;
		private global::Gtk.Alignment GtkAlignment9;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TextView textviewComments;
		private global::Gtk.Label GtkLabel9;
		private global::Gtk.Frame frame1;
		private global::Gtk.Alignment GtkAlignment10;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Table table1;
		private global::Gtk.Label label11;
		private global::Gtk.Label label4;
		private global::Gtk.Label label8;
		private global::Gtk.Label labelContractDates;
		private global::Gtk.Label labelContractNumber;
		private global::Gtk.Label labelLessee;
		private global::Gtk.HBox hbox1;
		private global::Gtk.Button buttonContract;
		private global::Gtk.Button buttonLessee;
		private global::Gtk.Button buttonNewContract;
		private global::Gtk.Label GtkLabel10;
		private global::Gtk.Notebook notebookMain;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.TreeView treeviewHistory;
		private global::Gtk.Label label5;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Notebook notebookMeters;
		private global::Gtk.Label label9;
		private global::Gtk.VBox vbox5;
		private global::Gtk.Button buttonAddMeter;
		private global::Gtk.Button buttonEditMeter;
		private global::Gtk.Button buttonDeleteMeter;
		private global::Gtk.HSeparator hseparator1;
		private global::Gtk.Button buttonAddReading;
		private global::Gtk.Button buttonDeleteReading;
		private global::Gtk.Label label6;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.Place
			this.Name = "bazar.Place";
			this.Title = global::Mono.Unix.Catalog.GetString ("Новое место");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.Place.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.frame2 = new global::Gtk.Frame ();
			this.frame2.Name = "frame2";
			this.frame2.BorderWidth = ((uint)(3));
			// Container child frame2.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.table2 = new global::Gtk.Table (((uint)(5)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(3));
			// Container child table2.Gtk.Table+TableChild
			this.comboOrg = new global::Gtk.ComboBox ();
			this.comboOrg.Name = "comboOrg";
			this.table2.Add (this.comboOrg);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table2 [this.comboOrg]));
			w2.TopAttach = ((uint)(2));
			w2.BottomAttach = ((uint)(3));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.comboPType = new global::Gtk.ComboBox ();
			this.comboPType.Name = "comboPType";
			this.table2.Add (this.comboPType);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table2 [this.comboPType]));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.entryNumber = new global::Gtk.Entry ();
			this.entryNumber.CanFocus = true;
			this.entryNumber.Name = "entryNumber";
			this.entryNumber.IsEditable = true;
			this.entryNumber.InvisibleChar = '●';
			this.table2.Add (this.entryNumber);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryNumber]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.entryContact = new global::Gtk.Entry ();
			this.entryContact.CanFocus = true;
			this.entryContact.Name = "entryContact";
			this.entryContact.IsEditable = false;
			this.entryContact.InvisibleChar = '●';
			this.hbox3.Add (this.entryContact);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryContact]));
			w5.Position = 0;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonContactOpen = new global::Gtk.Button ();
			this.buttonContactOpen.TooltipMarkup = "Открыть текущее контактное лицо";
			this.buttonContactOpen.Sensitive = false;
			this.buttonContactOpen.CanFocus = true;
			this.buttonContactOpen.Name = "buttonContactOpen";
			this.buttonContactOpen.UseUnderline = true;
			global::Gtk.Image w6 = new global::Gtk.Image ();
			w6.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-file", global::Gtk.IconSize.Menu);
			this.buttonContactOpen.Image = w6;
			this.hbox3.Add (this.buttonContactOpen);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonContactOpen]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonContactClean = new global::Gtk.Button ();
			this.buttonContactClean.TooltipMarkup = "Убрать контактное лицо";
			this.buttonContactClean.CanFocus = true;
			this.buttonContactClean.Name = "buttonContactClean";
			this.buttonContactClean.UseUnderline = true;
			global::Gtk.Image w8 = new global::Gtk.Image ();
			w8.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Menu);
			this.buttonContactClean.Image = w8;
			this.hbox3.Add (this.buttonContactClean);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonContactClean]));
			w9.Position = 2;
			w9.Expand = false;
			w9.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonContact = new global::Gtk.Button ();
			this.buttonContact.TooltipMarkup = "Выбрать контактное лицо";
			this.buttonContact.CanFocus = true;
			this.buttonContact.Name = "buttonContact";
			this.buttonContact.UseUnderline = true;
			global::Gtk.Image w10 = new global::Gtk.Image ();
			w10.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Menu);
			this.buttonContact.Image = w10;
			this.hbox3.Add (this.buttonContact);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonContact]));
			w11.Position = 3;
			w11.Expand = false;
			w11.Fill = false;
			this.table2.Add (this.hbox3);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table2 [this.hbox3]));
			w12.TopAttach = ((uint)(4));
			w12.BottomAttach = ((uint)(5));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("Тип места<span foreground=\"red\">*</span>:");
			this.label1.UseMarkup = true;
			this.table2.Add (this.label1);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table2 [this.label1]));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.Xalign = 1F;
			this.label10.LabelProp = global::Mono.Unix.Catalog.GetString ("Организация:");
			this.table2.Add (this.label10);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table2 [this.label10]));
			w14.TopAttach = ((uint)(2));
			w14.BottomAttach = ((uint)(3));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Номер места<span foreground=\"red\">*</span>:");
			this.label2.UseMarkup = true;
			this.table2.Add (this.label2);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table2 [this.label2]));
			w15.TopAttach = ((uint)(1));
			w15.BottomAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Площадь (м<sup>2</sup>):");
			this.label3.UseMarkup = true;
			this.table2.Add (this.label3);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table2 [this.label3]));
			w16.TopAttach = ((uint)(3));
			w16.BottomAttach = ((uint)(4));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("Контактное лицо:");
			this.table2.Add (this.label7);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table2 [this.label7]));
			w17.TopAttach = ((uint)(4));
			w17.BottomAttach = ((uint)(5));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.spinArea = new global::Gtk.SpinButton (0, 100000, 1);
			this.spinArea.CanFocus = true;
			this.spinArea.Name = "spinArea";
			this.spinArea.Adjustment.PageIncrement = 100;
			this.spinArea.ClimbRate = 1;
			this.spinArea.Digits = ((uint)(2));
			this.spinArea.Numeric = true;
			this.table2.Add (this.spinArea);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table2 [this.spinArea]));
			w18.TopAttach = ((uint)(3));
			w18.BottomAttach = ((uint)(4));
			w18.LeftAttach = ((uint)(1));
			w18.RightAttach = ((uint)(2));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			this.GtkAlignment2.Add (this.table2);
			this.frame2.Add (this.GtkAlignment2);
			this.GtkLabel8 = new global::Gtk.Label ();
			this.GtkLabel8.Name = "GtkLabel8";
			this.GtkLabel8.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Сдаваемое место</b>");
			this.GtkLabel8.UseMarkup = true;
			this.frame2.LabelWidget = this.GtkLabel8;
			this.hbox2.Add (this.frame2);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.frame2]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.frame3 = new global::Gtk.Frame ();
			this.frame3.Name = "frame3";
			// Container child frame3.Gtk.Container+ContainerChild
			this.GtkAlignment9 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment9.Name = "GtkAlignment9";
			this.GtkAlignment9.LeftPadding = ((uint)(12));
			this.GtkAlignment9.BorderWidth = ((uint)(6));
			// Container child GtkAlignment9.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.textviewComments = new global::Gtk.TextView ();
			this.textviewComments.CanFocus = true;
			this.textviewComments.Name = "textviewComments";
			this.textviewComments.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow.Add (this.textviewComments);
			this.GtkAlignment9.Add (this.GtkScrolledWindow);
			this.frame3.Add (this.GtkAlignment9);
			this.GtkLabel9 = new global::Gtk.Label ();
			this.GtkLabel9.Name = "GtkLabel9";
			this.GtkLabel9.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Комментарии</b>");
			this.GtkLabel9.UseMarkup = true;
			this.frame3.LabelWidget = this.GtkLabel9;
			this.vbox3.Add (this.frame3);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame3]));
			w25.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame ();
			this.frame1.Name = "frame1";
			this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment10 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment10.Name = "GtkAlignment10";
			this.GtkAlignment10.LeftPadding = ((uint)(12));
			// Container child GtkAlignment10.Gtk.Container+ContainerChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.label11 = new global::Gtk.Label ();
			this.label11.Name = "label11";
			this.label11.Xalign = 1F;
			this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("Арендатор:");
			this.table1.Add (this.label11);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table1 [this.label11]));
			w26.TopAttach = ((uint)(2));
			w26.BottomAttach = ((uint)(3));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Номер договора:");
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Период аренды:");
			this.table1.Add (this.label8);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table1 [this.label8]));
			w28.TopAttach = ((uint)(1));
			w28.BottomAttach = ((uint)(2));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelContractDates = new global::Gtk.Label ();
			this.labelContractDates.Name = "labelContractDates";
			this.labelContractDates.Xalign = 0F;
			this.table1.Add (this.labelContractDates);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelContractDates]));
			w29.TopAttach = ((uint)(1));
			w29.BottomAttach = ((uint)(2));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelContractNumber = new global::Gtk.Label ();
			this.labelContractNumber.Name = "labelContractNumber";
			this.labelContractNumber.Xalign = 0F;
			this.labelContractNumber.LabelProp = global::Mono.Unix.Catalog.GetString ("Нет активного договора");
			this.table1.Add (this.labelContractNumber);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelContractNumber]));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelLessee = new global::Gtk.Label ();
			this.labelLessee.Name = "labelLessee";
			this.labelLessee.Xalign = 0F;
			this.labelLessee.LabelProp = global::Mono.Unix.Catalog.GetString ("<span background=\"green\">Свободно</span>");
			this.labelLessee.UseMarkup = true;
			this.table1.Add (this.labelLessee);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelLessee]));
			w31.TopAttach = ((uint)(2));
			w31.BottomAttach = ((uint)(3));
			w31.LeftAttach = ((uint)(1));
			w31.RightAttach = ((uint)(2));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox4.Add (this.table1);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.table1]));
			w32.Position = 0;
			w32.Expand = false;
			w32.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonContract = new global::Gtk.Button ();
			this.buttonContract.Sensitive = false;
			this.buttonContract.CanFocus = true;
			this.buttonContract.Name = "buttonContract";
			this.buttonContract.UseUnderline = true;
			this.buttonContract.Label = global::Mono.Unix.Catalog.GetString ("Договор");
			global::Gtk.Image w33 = new global::Gtk.Image ();
			w33.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-file", global::Gtk.IconSize.Menu);
			this.buttonContract.Image = w33;
			this.hbox1.Add (this.buttonContract);
			global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonContract]));
			w34.Position = 0;
			w34.Expand = false;
			w34.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonLessee = new global::Gtk.Button ();
			this.buttonLessee.Sensitive = false;
			this.buttonLessee.CanFocus = true;
			this.buttonLessee.Name = "buttonLessee";
			this.buttonLessee.UseUnderline = true;
			this.buttonLessee.Label = global::Mono.Unix.Catalog.GetString ("Арендатор");
			global::Gtk.Image w35 = new global::Gtk.Image ();
			w35.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-open", global::Gtk.IconSize.Menu);
			this.buttonLessee.Image = w35;
			this.hbox1.Add (this.buttonLessee);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonLessee]));
			w36.Position = 1;
			w36.Expand = false;
			w36.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonNewContract = new global::Gtk.Button ();
			this.buttonNewContract.Sensitive = false;
			this.buttonNewContract.CanFocus = true;
			this.buttonNewContract.Name = "buttonNewContract";
			this.buttonNewContract.UseUnderline = true;
			this.buttonNewContract.Label = global::Mono.Unix.Catalog.GetString ("Новый договор");
			global::Gtk.Image w37 = new global::Gtk.Image ();
			w37.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-new", global::Gtk.IconSize.Menu);
			this.buttonNewContract.Image = w37;
			this.hbox1.Add (this.buttonNewContract);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonNewContract]));
			w38.Position = 3;
			w38.Expand = false;
			w38.Fill = false;
			this.vbox4.Add (this.hbox1);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox1]));
			w39.Position = 1;
			w39.Expand = false;
			w39.Fill = false;
			this.GtkAlignment10.Add (this.vbox4);
			this.frame1.Add (this.GtkAlignment10);
			this.GtkLabel10 = new global::Gtk.Label ();
			this.GtkLabel10.Name = "GtkLabel10";
			this.GtkLabel10.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Текущий договор аренды</b>");
			this.GtkLabel10.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel10;
			this.vbox3.Add (this.frame1);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame1]));
			w42.Position = 1;
			w42.Expand = false;
			w42.Fill = false;
			this.hbox2.Add (this.vbox3);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox3]));
			w43.Position = 1;
			this.vbox2.Add (this.hbox2);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox2]));
			w44.Position = 0;
			w44.Expand = false;
			w44.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.notebookMain = new global::Gtk.Notebook ();
			this.notebookMain.CanFocus = true;
			this.notebookMain.Name = "notebookMain";
			this.notebookMain.CurrentPage = 1;
			// Container child notebookMain.Gtk.Notebook+NotebookChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			this.GtkScrolledWindow1.BorderWidth = ((uint)(3));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewHistory = new global::Gtk.TreeView ();
			this.treeviewHistory.CanFocus = true;
			this.treeviewHistory.Name = "treeviewHistory";
			this.GtkScrolledWindow1.Add (this.treeviewHistory);
			this.notebookMain.Add (this.GtkScrolledWindow1);
			// Notebook tab
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("История арендаторов");
			this.notebookMain.SetTabLabel (this.GtkScrolledWindow1, this.label5);
			this.label5.ShowAll ();
			// Container child notebookMain.Gtk.Notebook+NotebookChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.notebookMeters = new global::Gtk.Notebook ();
			this.notebookMeters.CanFocus = true;
			this.notebookMeters.Name = "notebookMeters";
			this.notebookMeters.CurrentPage = 0;
			this.notebookMeters.TabPos = ((global::Gtk.PositionType)(0));
			// Notebook tab
			global::Gtk.Label w47 = new global::Gtk.Label ();
			w47.Visible = true;
			this.notebookMeters.Add (w47);
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("нет счетчиков");
			this.notebookMeters.SetTabLabel (w47, this.label9);
			this.label9.ShowAll ();
			this.hbox4.Add (this.notebookMeters);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.notebookMeters]));
			w48.Position = 0;
			// Container child hbox4.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox ();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.buttonAddMeter = new global::Gtk.Button ();
			this.buttonAddMeter.CanFocus = true;
			this.buttonAddMeter.Name = "buttonAddMeter";
			this.buttonAddMeter.UseUnderline = true;
			this.buttonAddMeter.Label = global::Mono.Unix.Catalog.GetString ("Добавить счётчик");
			global::Gtk.Image w49 = new global::Gtk.Image ();
			w49.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonAddMeter.Image = w49;
			this.vbox5.Add (this.buttonAddMeter);
			global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonAddMeter]));
			w50.Position = 0;
			w50.Expand = false;
			w50.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.buttonEditMeter = new global::Gtk.Button ();
			this.buttonEditMeter.Sensitive = false;
			this.buttonEditMeter.CanFocus = true;
			this.buttonEditMeter.Name = "buttonEditMeter";
			this.buttonEditMeter.UseUnderline = true;
			this.buttonEditMeter.Label = global::Mono.Unix.Catalog.GetString ("Изменить счётчик");
			global::Gtk.Image w51 = new global::Gtk.Image ();
			w51.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Menu);
			this.buttonEditMeter.Image = w51;
			this.vbox5.Add (this.buttonEditMeter);
			global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonEditMeter]));
			w52.Position = 1;
			w52.Expand = false;
			w52.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.buttonDeleteMeter = new global::Gtk.Button ();
			this.buttonDeleteMeter.Sensitive = false;
			this.buttonDeleteMeter.CanFocus = true;
			this.buttonDeleteMeter.Name = "buttonDeleteMeter";
			this.buttonDeleteMeter.UseUnderline = true;
			this.buttonDeleteMeter.Label = global::Mono.Unix.Catalog.GetString ("Удалить счётчик");
			global::Gtk.Image w53 = new global::Gtk.Image ();
			w53.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDeleteMeter.Image = w53;
			this.vbox5.Add (this.buttonDeleteMeter);
			global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonDeleteMeter]));
			w54.Position = 2;
			w54.Expand = false;
			w54.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hseparator1 = new global::Gtk.HSeparator ();
			this.hseparator1.Name = "hseparator1";
			this.vbox5.Add (this.hseparator1);
			global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.hseparator1]));
			w55.Position = 3;
			w55.Expand = false;
			w55.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.buttonAddReading = new global::Gtk.Button ();
			this.buttonAddReading.Sensitive = false;
			this.buttonAddReading.CanFocus = true;
			this.buttonAddReading.Name = "buttonAddReading";
			this.buttonAddReading.UseUnderline = true;
			this.buttonAddReading.Label = global::Mono.Unix.Catalog.GetString ("Новое показание");
			global::Gtk.Image w56 = new global::Gtk.Image ();
			w56.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonAddReading.Image = w56;
			this.vbox5.Add (this.buttonAddReading);
			global::Gtk.Box.BoxChild w57 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonAddReading]));
			w57.Position = 4;
			w57.Expand = false;
			w57.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.buttonDeleteReading = new global::Gtk.Button ();
			this.buttonDeleteReading.Sensitive = false;
			this.buttonDeleteReading.CanFocus = true;
			this.buttonDeleteReading.Name = "buttonDeleteReading";
			this.buttonDeleteReading.UseUnderline = true;
			this.buttonDeleteReading.Label = global::Mono.Unix.Catalog.GetString ("Удалить показание");
			global::Gtk.Image w58 = new global::Gtk.Image ();
			w58.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDeleteReading.Image = w58;
			this.vbox5.Add (this.buttonDeleteReading);
			global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.buttonDeleteReading]));
			w59.Position = 5;
			w59.Expand = false;
			w59.Fill = false;
			this.hbox4.Add (this.vbox5);
			global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.vbox5]));
			w60.Position = 1;
			w60.Expand = false;
			w60.Fill = false;
			this.notebookMain.Add (this.hbox4);
			global::Gtk.Notebook.NotebookChild w61 = ((global::Gtk.Notebook.NotebookChild)(this.notebookMain [this.hbox4]));
			w61.Position = 1;
			// Notebook tab
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString ("Счётчики");
			this.notebookMain.SetTabLabel (this.hbox4, this.label6);
			this.label6.ShowAll ();
			this.vbox2.Add (this.notebookMain);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.notebookMain]));
			w62.Position = 1;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w63.Position = 0;
			// Internal child bazar.Place.ActionArea
			global::Gtk.HButtonBox w64 = this.ActionArea;
			w64.Name = "dialog1_ActionArea";
			w64.Spacing = 10;
			w64.BorderWidth = ((uint)(5));
			w64.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString ("О_тменить");
			global::Gtk.Image w65 = new global::Gtk.Image ();
			w65.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w65;
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w66 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w64 [this.buttonCancel]));
			w66.Expand = false;
			w66.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.Sensitive = false;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = global::Mono.Unix.Catalog.GetString ("_OK");
			global::Gtk.Image w67 = new global::Gtk.Image ();
			w67.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			this.buttonOk.Image = w67;
			w64.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w68 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w64 [this.buttonOk]));
			w68.Position = 1;
			w68.Expand = false;
			w68.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 810;
			this.DefaultHeight = 519;
			this.Show ();
			this.buttonContactOpen.Clicked += new global::System.EventHandler (this.OnButtonContactOpenClicked);
			this.buttonContactClean.Clicked += new global::System.EventHandler (this.OnButtonContactCleanClicked);
			this.buttonContact.Clicked += new global::System.EventHandler (this.OnButtonContactClicked);
			this.entryNumber.Changed += new global::System.EventHandler (this.OnEntryNumberChanged);
			this.comboPType.Changed += new global::System.EventHandler (this.OnComboPTypeChanged);
			this.buttonContract.Clicked += new global::System.EventHandler (this.OnButtonContractClicked);
			this.buttonLessee.Clicked += new global::System.EventHandler (this.OnButtonLesseeClicked);
			this.buttonNewContract.Clicked += new global::System.EventHandler (this.OnButtonNewContractClicked);
			this.notebookMain.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnNotebookMainSwitchPage);
			this.treeviewHistory.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnTreeviewHistoryButtonPressEvent);
			this.treeviewHistory.PopupMenu += new global::Gtk.PopupMenuHandler (this.OnTreeviewHistoryPopupMenu);
			this.notebookMeters.SwitchPage += new global::Gtk.SwitchPageHandler (this.OnNotebookMetersSwitchPage);
			this.buttonAddMeter.Clicked += new global::System.EventHandler (this.OnButtonAddMeterClicked);
			this.buttonEditMeter.Clicked += new global::System.EventHandler (this.OnButtonEditMeterClicked);
			this.buttonDeleteMeter.Clicked += new global::System.EventHandler (this.OnButtonDeleteMeterClicked);
			this.buttonAddReading.Clicked += new global::System.EventHandler (this.OnButtonAddReadingClicked);
			this.buttonDeleteReading.Clicked += new global::System.EventHandler (this.OnButtonDeleteReadingClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
