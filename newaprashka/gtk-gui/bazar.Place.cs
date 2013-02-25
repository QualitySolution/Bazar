
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
		private global::Gtk.Button buttonContact;
		private global::Gtk.Button buttonContactClean;
		private global::Gtk.Button buttonContactOpen;
		private global::Gtk.ComboBox comboOrg;
		private global::Gtk.ComboBox comboPType;
		private global::Gtk.Entry entryContact;
		private global::Gtk.Entry entryNumber;
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
		private global::Gtk.Label label9;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.TreeView treeviewHistory;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.Place
			this.Name = "bazar.Place";
			this.Title = "Новое место";
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
			this.table2 = new global::Gtk.Table (((uint)(5)), ((uint)(5)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(3));
			// Container child table2.Gtk.Table+TableChild
			this.buttonContact = new global::Gtk.Button ();
			this.buttonContact.TooltipMarkup = "Выбрать контактное лицо";
			this.buttonContact.CanFocus = true;
			this.buttonContact.Name = "buttonContact";
			this.buttonContact.UseUnderline = true;
			// Container child buttonContact.Gtk.Container+ContainerChild
			global::Gtk.Alignment w2 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w3 = new global::Gtk.HBox ();
			w3.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w4 = new global::Gtk.Image ();
			w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Menu);
			w3.Add (w4);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w6 = new global::Gtk.Label ();
			w3.Add (w6);
			w2.Add (w3);
			this.buttonContact.Add (w2);
			this.table2.Add (this.buttonContact);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table2 [this.buttonContact]));
			w10.TopAttach = ((uint)(4));
			w10.BottomAttach = ((uint)(5));
			w10.LeftAttach = ((uint)(4));
			w10.RightAttach = ((uint)(5));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.buttonContactClean = new global::Gtk.Button ();
			this.buttonContactClean.TooltipMarkup = "Убрать контактное лицо";
			this.buttonContactClean.CanFocus = true;
			this.buttonContactClean.Name = "buttonContactClean";
			this.buttonContactClean.UseUnderline = true;
			// Container child buttonContactClean.Gtk.Container+ContainerChild
			global::Gtk.Alignment w11 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w12 = new global::Gtk.HBox ();
			w12.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w13 = new global::Gtk.Image ();
			w13.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Menu);
			w12.Add (w13);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w15 = new global::Gtk.Label ();
			w12.Add (w15);
			w11.Add (w12);
			this.buttonContactClean.Add (w11);
			this.table2.Add (this.buttonContactClean);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table2 [this.buttonContactClean]));
			w19.TopAttach = ((uint)(4));
			w19.BottomAttach = ((uint)(5));
			w19.LeftAttach = ((uint)(3));
			w19.RightAttach = ((uint)(4));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.buttonContactOpen = new global::Gtk.Button ();
			this.buttonContactOpen.TooltipMarkup = "Открыть текущее контактное лицо";
			this.buttonContactOpen.Sensitive = false;
			this.buttonContactOpen.CanFocus = true;
			this.buttonContactOpen.Name = "buttonContactOpen";
			this.buttonContactOpen.UseUnderline = true;
			// Container child buttonContactOpen.Gtk.Container+ContainerChild
			global::Gtk.Alignment w20 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w21 = new global::Gtk.HBox ();
			w21.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w22 = new global::Gtk.Image ();
			w22.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-file", global::Gtk.IconSize.Menu);
			w21.Add (w22);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w24 = new global::Gtk.Label ();
			w21.Add (w24);
			w20.Add (w21);
			this.buttonContactOpen.Add (w20);
			this.table2.Add (this.buttonContactOpen);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table2 [this.buttonContactOpen]));
			w28.TopAttach = ((uint)(4));
			w28.BottomAttach = ((uint)(5));
			w28.LeftAttach = ((uint)(2));
			w28.RightAttach = ((uint)(3));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.comboOrg = new global::Gtk.ComboBox ();
			this.comboOrg.Name = "comboOrg";
			this.table2.Add (this.comboOrg);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table2 [this.comboOrg]));
			w29.TopAttach = ((uint)(2));
			w29.BottomAttach = ((uint)(3));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.comboPType = new global::Gtk.ComboBox ();
			this.comboPType.Name = "comboPType";
			this.table2.Add (this.comboPType);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table2 [this.comboPType]));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.entryContact = new global::Gtk.Entry ();
			this.entryContact.CanFocus = true;
			this.entryContact.Name = "entryContact";
			this.entryContact.IsEditable = false;
			this.entryContact.InvisibleChar = '●';
			this.table2.Add (this.entryContact);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryContact]));
			w31.TopAttach = ((uint)(4));
			w31.BottomAttach = ((uint)(5));
			w31.LeftAttach = ((uint)(1));
			w31.RightAttach = ((uint)(2));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.entryNumber = new global::Gtk.Entry ();
			this.entryNumber.CanFocus = true;
			this.entryNumber.Name = "entryNumber";
			this.entryNumber.IsEditable = true;
			this.entryNumber.InvisibleChar = '●';
			this.table2.Add (this.entryNumber);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryNumber]));
			w32.TopAttach = ((uint)(1));
			w32.BottomAttach = ((uint)(2));
			w32.LeftAttach = ((uint)(1));
			w32.RightAttach = ((uint)(2));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = "Тип места<span foreground=\"red\">*</span>:";
			this.label1.UseMarkup = true;
			this.table2.Add (this.label1);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table2 [this.label1]));
			w33.XOptions = ((global::Gtk.AttachOptions)(4));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.Xalign = 1F;
			this.label10.LabelProp = "Организация:";
			this.table2.Add (this.label10);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table2 [this.label10]));
			w34.TopAttach = ((uint)(2));
			w34.BottomAttach = ((uint)(3));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = "Номер места<span foreground=\"red\">*</span>:";
			this.label2.UseMarkup = true;
			this.table2.Add (this.label2);
			global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table2 [this.label2]));
			w35.TopAttach = ((uint)(1));
			w35.BottomAttach = ((uint)(2));
			w35.XOptions = ((global::Gtk.AttachOptions)(4));
			w35.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = "Площадь (кв.м.):";
			this.table2.Add (this.label3);
			global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table2 [this.label3]));
			w36.TopAttach = ((uint)(3));
			w36.BottomAttach = ((uint)(4));
			w36.XOptions = ((global::Gtk.AttachOptions)(4));
			w36.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = "Контактное лицо:";
			this.table2.Add (this.label7);
			global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.table2 [this.label7]));
			w37.TopAttach = ((uint)(4));
			w37.BottomAttach = ((uint)(5));
			w37.XOptions = ((global::Gtk.AttachOptions)(4));
			w37.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.spinArea = new global::Gtk.SpinButton (0, 100000, 1);
			this.spinArea.CanFocus = true;
			this.spinArea.Name = "spinArea";
			this.spinArea.Adjustment.PageIncrement = 100;
			this.spinArea.ClimbRate = 1;
			this.spinArea.Digits = ((uint)(2));
			this.spinArea.Numeric = true;
			this.table2.Add (this.spinArea);
			global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table2 [this.spinArea]));
			w38.TopAttach = ((uint)(3));
			w38.BottomAttach = ((uint)(4));
			w38.LeftAttach = ((uint)(1));
			w38.RightAttach = ((uint)(2));
			w38.XOptions = ((global::Gtk.AttachOptions)(4));
			w38.YOptions = ((global::Gtk.AttachOptions)(4));
			this.GtkAlignment2.Add (this.table2);
			this.frame2.Add (this.GtkAlignment2);
			this.GtkLabel8 = new global::Gtk.Label ();
			this.GtkLabel8.Name = "GtkLabel8";
			this.GtkLabel8.LabelProp = "<b>Сдаваемое место</b>";
			this.GtkLabel8.UseMarkup = true;
			this.frame2.LabelWidget = this.GtkLabel8;
			this.hbox2.Add (this.frame2);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.frame2]));
			w41.Position = 0;
			w41.Expand = false;
			w41.Fill = false;
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
			this.GtkLabel9.LabelProp = "<b>Комментарии</b>";
			this.GtkLabel9.UseMarkup = true;
			this.frame3.LabelWidget = this.GtkLabel9;
			this.vbox3.Add (this.frame3);
			global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame3]));
			w45.Position = 0;
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
			this.label11.LabelProp = "Арендатор:";
			this.table1.Add (this.label11);
			global::Gtk.Table.TableChild w46 = ((global::Gtk.Table.TableChild)(this.table1 [this.label11]));
			w46.TopAttach = ((uint)(2));
			w46.BottomAttach = ((uint)(3));
			w46.XOptions = ((global::Gtk.AttachOptions)(4));
			w46.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = "Номер договора:";
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w47 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w47.XOptions = ((global::Gtk.AttachOptions)(4));
			w47.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = "Период аренды:";
			this.table1.Add (this.label8);
			global::Gtk.Table.TableChild w48 = ((global::Gtk.Table.TableChild)(this.table1 [this.label8]));
			w48.TopAttach = ((uint)(1));
			w48.BottomAttach = ((uint)(2));
			w48.XOptions = ((global::Gtk.AttachOptions)(4));
			w48.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelContractDates = new global::Gtk.Label ();
			this.labelContractDates.Name = "labelContractDates";
			this.labelContractDates.Xalign = 0F;
			this.table1.Add (this.labelContractDates);
			global::Gtk.Table.TableChild w49 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelContractDates]));
			w49.TopAttach = ((uint)(1));
			w49.BottomAttach = ((uint)(2));
			w49.LeftAttach = ((uint)(1));
			w49.RightAttach = ((uint)(2));
			w49.XOptions = ((global::Gtk.AttachOptions)(4));
			w49.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelContractNumber = new global::Gtk.Label ();
			this.labelContractNumber.Name = "labelContractNumber";
			this.labelContractNumber.Xalign = 0F;
			this.labelContractNumber.LabelProp = "Нет активного договора";
			this.table1.Add (this.labelContractNumber);
			global::Gtk.Table.TableChild w50 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelContractNumber]));
			w50.LeftAttach = ((uint)(1));
			w50.RightAttach = ((uint)(2));
			w50.XOptions = ((global::Gtk.AttachOptions)(4));
			w50.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelLessee = new global::Gtk.Label ();
			this.labelLessee.Name = "labelLessee";
			this.labelLessee.Xalign = 0F;
			this.labelLessee.LabelProp = "<span background=\"green\">Свободно</span>";
			this.labelLessee.UseMarkup = true;
			this.table1.Add (this.labelLessee);
			global::Gtk.Table.TableChild w51 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelLessee]));
			w51.TopAttach = ((uint)(2));
			w51.BottomAttach = ((uint)(3));
			w51.LeftAttach = ((uint)(1));
			w51.RightAttach = ((uint)(2));
			w51.XOptions = ((global::Gtk.AttachOptions)(4));
			w51.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox4.Add (this.table1);
			global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.table1]));
			w52.Position = 0;
			w52.Expand = false;
			w52.Fill = false;
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
			// Container child buttonContract.Gtk.Container+ContainerChild
			global::Gtk.Alignment w53 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w54 = new global::Gtk.HBox ();
			w54.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w55 = new global::Gtk.Image ();
			w55.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-file", global::Gtk.IconSize.Menu);
			w54.Add (w55);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w57 = new global::Gtk.Label ();
			w57.LabelProp = "Договор";
			w57.UseUnderline = true;
			w54.Add (w57);
			w53.Add (w54);
			this.buttonContract.Add (w53);
			this.hbox1.Add (this.buttonContract);
			global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonContract]));
			w61.Position = 0;
			w61.Expand = false;
			w61.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonLessee = new global::Gtk.Button ();
			this.buttonLessee.Sensitive = false;
			this.buttonLessee.CanFocus = true;
			this.buttonLessee.Name = "buttonLessee";
			this.buttonLessee.UseUnderline = true;
			// Container child buttonLessee.Gtk.Container+ContainerChild
			global::Gtk.Alignment w62 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w63 = new global::Gtk.HBox ();
			w63.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w64 = new global::Gtk.Image ();
			w64.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-open", global::Gtk.IconSize.Menu);
			w63.Add (w64);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w66 = new global::Gtk.Label ();
			w66.LabelProp = "Арендатор";
			w66.UseUnderline = true;
			w63.Add (w66);
			w62.Add (w63);
			this.buttonLessee.Add (w62);
			this.hbox1.Add (this.buttonLessee);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonLessee]));
			w70.Position = 1;
			w70.Expand = false;
			w70.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonNewContract = new global::Gtk.Button ();
			this.buttonNewContract.Sensitive = false;
			this.buttonNewContract.CanFocus = true;
			this.buttonNewContract.Name = "buttonNewContract";
			this.buttonNewContract.UseUnderline = true;
			// Container child buttonNewContract.Gtk.Container+ContainerChild
			global::Gtk.Alignment w71 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w72 = new global::Gtk.HBox ();
			w72.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w73 = new global::Gtk.Image ();
			w73.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-new", global::Gtk.IconSize.Menu);
			w72.Add (w73);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w75 = new global::Gtk.Label ();
			w75.LabelProp = "Новый договор";
			w75.UseUnderline = true;
			w72.Add (w75);
			w71.Add (w72);
			this.buttonNewContract.Add (w71);
			this.hbox1.Add (this.buttonNewContract);
			global::Gtk.Box.BoxChild w79 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonNewContract]));
			w79.Position = 3;
			w79.Expand = false;
			w79.Fill = false;
			this.vbox4.Add (this.hbox1);
			global::Gtk.Box.BoxChild w80 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox1]));
			w80.Position = 1;
			w80.Expand = false;
			w80.Fill = false;
			this.GtkAlignment10.Add (this.vbox4);
			this.frame1.Add (this.GtkAlignment10);
			this.GtkLabel10 = new global::Gtk.Label ();
			this.GtkLabel10.Name = "GtkLabel10";
			this.GtkLabel10.LabelProp = "<b>Текущий договор аренды</b>";
			this.GtkLabel10.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel10;
			this.vbox3.Add (this.frame1);
			global::Gtk.Box.BoxChild w83 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame1]));
			w83.Position = 1;
			w83.Expand = false;
			w83.Fill = false;
			this.hbox2.Add (this.vbox3);
			global::Gtk.Box.BoxChild w84 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox3]));
			w84.Position = 1;
			this.vbox2.Add (this.hbox2);
			global::Gtk.Box.BoxChild w85 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox2]));
			w85.Position = 0;
			w85.Expand = false;
			w85.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xalign = 0F;
			this.label9.LabelProp = "  <b>История арендаторов:</b>";
			this.label9.UseMarkup = true;
			this.vbox2.Add (this.label9);
			global::Gtk.Box.BoxChild w86 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.label9]));
			w86.Position = 1;
			w86.Expand = false;
			w86.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			this.GtkScrolledWindow1.BorderWidth = ((uint)(3));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewHistory = new global::Gtk.TreeView ();
			this.treeviewHistory.CanFocus = true;
			this.treeviewHistory.Name = "treeviewHistory";
			this.GtkScrolledWindow1.Add (this.treeviewHistory);
			this.vbox2.Add (this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w88 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow1]));
			w88.Position = 2;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w89 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w89.Position = 0;
			// Internal child bazar.Place.ActionArea
			global::Gtk.HButtonBox w90 = this.ActionArea;
			w90.Name = "dialog1_ActionArea";
			w90.Spacing = 10;
			w90.BorderWidth = ((uint)(5));
			w90.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w91 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w92 = new global::Gtk.HBox ();
			w92.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w93 = new global::Gtk.Image ();
			w93.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w92.Add (w93);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w95 = new global::Gtk.Label ();
			w95.LabelProp = "О_тменить";
			w95.UseUnderline = true;
			w92.Add (w95);
			w91.Add (w92);
			this.buttonCancel.Add (w91);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w99 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w90 [this.buttonCancel]));
			w99.Expand = false;
			w99.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.Sensitive = false;
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w100 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w101 = new global::Gtk.HBox ();
			w101.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w102 = new global::Gtk.Image ();
			w102.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w101.Add (w102);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w104 = new global::Gtk.Label ();
			w104.LabelProp = "_OK";
			w104.UseUnderline = true;
			w101.Add (w104);
			w100.Add (w101);
			this.buttonOk.Add (w100);
			w90.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w108 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w90 [this.buttonOk]));
			w108.Position = 1;
			w108.Expand = false;
			w108.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 833;
			this.DefaultHeight = 477;
			this.Show ();
			this.entryNumber.Changed += new global::System.EventHandler (this.OnEntryNumberChanged);
			this.comboPType.Changed += new global::System.EventHandler (this.OnComboPTypeChanged);
			this.buttonContactOpen.Clicked += new global::System.EventHandler (this.OnButtonContactOpenClicked);
			this.buttonContactClean.Clicked += new global::System.EventHandler (this.OnButtonContactCleanClicked);
			this.buttonContact.Clicked += new global::System.EventHandler (this.OnButtonContactClicked);
			this.buttonContract.Clicked += new global::System.EventHandler (this.OnButtonContractClicked);
			this.buttonLessee.Clicked += new global::System.EventHandler (this.OnButtonLesseeClicked);
			this.buttonNewContract.Clicked += new global::System.EventHandler (this.OnButtonNewContractClicked);
			this.treeviewHistory.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnTreeviewHistoryButtonPressEvent);
			this.treeviewHistory.PopupMenu += new global::Gtk.PopupMenuHandler (this.OnTreeviewHistoryPopupMenu);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
