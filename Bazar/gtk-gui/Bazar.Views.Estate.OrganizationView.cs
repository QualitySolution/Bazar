
// This file has been generated by the GUI designer. Do not modify.
namespace Bazar.Views.Estate
{
	public partial class OrganizationView
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Button buttonSave;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.VSeparator vseparator1;

		private global::Gtk.RadioButton radioTabInfo;

		private global::Gtk.RadioButton radioTabAccounts;

		private global::Gtk.Notebook notebookMain;

		private global::Gtk.ScrolledWindow scrolledwindow1;

		private global::Gtk.Table datatableMain;

		private global::Gamma.GtkWidgets.yEntry dataentryFullName;

		private global::QS.Widgets.ValidatedEntry dataentryINN;

		private global::QS.Widgets.ValidatedEntry dataentryKPP;

		private global::Gamma.GtkWidgets.yEntry dataentryName;

		private global::QS.Widgets.GtkUI.PhoneEntry entryPhone;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gamma.GtkWidgets.yTextView datatextviewJurAddress;

		private global::Gtk.Label label2;

		private global::Gtk.Label label3;

		private global::Gtk.Label label4;

		private global::Gtk.Label label5;

		private global::Gtk.Label label6;

		private global::Gtk.Label label7;

		private global::Gtk.Label label8;

		private global::Gtk.Label label9;

		private global::Gamma.GtkWidgets.yEntry yentryBuhgalterSign;

		private global::Gamma.GtkWidgets.yEntry yentryLeaderSign;

		private global::Gtk.Label label1;

		private global::Gtk.VBox vbox2;

		private global::Gtk.Table table1;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTextView ytextviewBankName;

		private global::QS.Widgets.ValidatedEntry validatedentryBankAccount;

		private global::QS.Widgets.ValidatedEntry validatedentryBankBik;

		private global::QS.Widgets.ValidatedEntry validatedentryBankCorAccount;

		private global::Gamma.GtkWidgets.yLabel ylabel1;

		private global::Gamma.GtkWidgets.yLabel ylabel2;

		private global::Gamma.GtkWidgets.yLabel ylabel3;

		private global::Gamma.GtkWidgets.yLabel ylabel4;

		private global::Gtk.Label label12;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Bazar.Views.Estate.OrganizationView
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Bazar.Views.Estate.OrganizationView";
			// Container child Bazar.Views.Estate.OrganizationView.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonSave = new global::Gtk.Button();
			this.buttonSave.CanFocus = true;
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.UseUnderline = true;
			this.buttonSave.Label = global::Mono.Unix.Catalog.GetString("Сохранить");
			global::Gtk.Image w1 = new global::Gtk.Image();
			w1.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-save", global::Gtk.IconSize.Menu);
			this.buttonSave.Image = w1;
			this.hbox1.Add(this.buttonSave);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonSave]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("Отменить");
			global::Gtk.Image w3 = new global::Gtk.Image();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-revert-to-saved", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w3;
			this.hbox1.Add(this.buttonCancel);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonCancel]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator();
			this.vseparator1.Name = "vseparator1";
			this.hbox1.Add(this.vseparator1);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vseparator1]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioTabInfo = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Основное"));
			this.radioTabInfo.CanFocus = true;
			this.radioTabInfo.Name = "radioTabInfo";
			this.radioTabInfo.DrawIndicator = false;
			this.radioTabInfo.UseUnderline = true;
			this.radioTabInfo.Group = new global::GLib.SList(global::System.IntPtr.Zero);
			this.hbox1.Add(this.radioTabInfo);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.radioTabInfo]));
			w6.Position = 3;
			// Container child hbox1.Gtk.Box+BoxChild
			this.radioTabAccounts = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Счета"));
			this.radioTabAccounts.CanFocus = true;
			this.radioTabAccounts.Name = "radioTabAccounts";
			this.radioTabAccounts.DrawIndicator = false;
			this.radioTabAccounts.UseUnderline = true;
			this.radioTabAccounts.Group = this.radioTabInfo.Group;
			this.hbox1.Add(this.radioTabAccounts);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.radioTabAccounts]));
			w7.Position = 4;
			this.vbox1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.notebookMain = new global::Gtk.Notebook();
			this.notebookMain.CanFocus = true;
			this.notebookMain.Name = "notebookMain";
			this.notebookMain.CurrentPage = 0;
			// Container child notebookMain.Gtk.Notebook+NotebookChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			global::Gtk.Viewport w9 = new global::Gtk.Viewport();
			w9.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child GtkViewport.Gtk.Container+ContainerChild
			this.datatableMain = new global::Gtk.Table(((uint)(8)), ((uint)(2)), false);
			this.datatableMain.Name = "datatableMain";
			this.datatableMain.RowSpacing = ((uint)(6));
			this.datatableMain.ColumnSpacing = ((uint)(6));
			this.datatableMain.BorderWidth = ((uint)(2));
			// Container child datatableMain.Gtk.Table+TableChild
			this.dataentryFullName = new global::Gamma.GtkWidgets.yEntry();
			this.dataentryFullName.CanFocus = true;
			this.dataentryFullName.Name = "dataentryFullName";
			this.dataentryFullName.IsEditable = true;
			this.dataentryFullName.MaxLength = 200;
			this.dataentryFullName.InvisibleChar = '●';
			this.datatableMain.Add(this.dataentryFullName);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.dataentryFullName]));
			w10.TopAttach = ((uint)(1));
			w10.BottomAttach = ((uint)(2));
			w10.LeftAttach = ((uint)(1));
			w10.RightAttach = ((uint)(2));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.dataentryINN = new global::QS.Widgets.ValidatedEntry();
			this.dataentryINN.CanFocus = true;
			this.dataentryINN.Name = "dataentryINN";
			this.dataentryINN.IsEditable = true;
			this.dataentryINN.MaxLength = 12;
			this.dataentryINN.InvisibleChar = '●';
			this.datatableMain.Add(this.dataentryINN);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.dataentryINN]));
			w11.TopAttach = ((uint)(3));
			w11.BottomAttach = ((uint)(4));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.dataentryKPP = new global::QS.Widgets.ValidatedEntry();
			this.dataentryKPP.CanFocus = true;
			this.dataentryKPP.Name = "dataentryKPP";
			this.dataentryKPP.IsEditable = true;
			this.dataentryKPP.MaxLength = 9;
			this.dataentryKPP.InvisibleChar = '●';
			this.datatableMain.Add(this.dataentryKPP);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.dataentryKPP]));
			w12.TopAttach = ((uint)(4));
			w12.BottomAttach = ((uint)(5));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.dataentryName = new global::Gamma.GtkWidgets.yEntry();
			this.dataentryName.CanFocus = true;
			this.dataentryName.Name = "dataentryName";
			this.dataentryName.IsEditable = true;
			this.dataentryName.MaxLength = 100;
			this.dataentryName.InvisibleChar = '●';
			this.datatableMain.Add(this.dataentryName);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.dataentryName]));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.entryPhone = new global::QS.Widgets.GtkUI.PhoneEntry();
			this.entryPhone.CanFocus = true;
			this.entryPhone.Name = "entryPhone";
			this.entryPhone.IsEditable = true;
			this.entryPhone.InvisibleChar = '●';
			this.datatableMain.Add(this.entryPhone);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.entryPhone]));
			w14.TopAttach = ((uint)(2));
			w14.BottomAttach = ((uint)(3));
			w14.LeftAttach = ((uint)(1));
			w14.RightAttach = ((uint)(2));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.datatextviewJurAddress = new global::Gamma.GtkWidgets.yTextView();
			this.datatextviewJurAddress.CanFocus = true;
			this.datatextviewJurAddress.Name = "datatextviewJurAddress";
			this.datatextviewJurAddress.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow1.Add(this.datatextviewJurAddress);
			this.datatableMain.Add(this.GtkScrolledWindow1);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.GtkScrolledWindow1]));
			w16.TopAttach = ((uint)(5));
			w16.BottomAttach = ((uint)(6));
			w16.LeftAttach = ((uint)(1));
			w16.RightAttach = ((uint)(2));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("ФИО руководителя:");
			this.datatableMain.Add(this.label2);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label2]));
			w17.TopAttach = ((uint)(6));
			w17.BottomAttach = ((uint)(7));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Название на документах:");
			this.datatableMain.Add(this.label3);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label3]));
			w18.TopAttach = ((uint)(1));
			w18.BottomAttach = ((uint)(2));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Название<span foreground=\"red\">*</span>:");
			this.label4.UseMarkup = true;
			this.datatableMain.Add(this.label4);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label4]));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("ФИО бухгалтера:");
			this.datatableMain.Add(this.label5);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label5]));
			w20.TopAttach = ((uint)(7));
			w20.BottomAttach = ((uint)(8));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.Yalign = 0F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("Юридический адрес:");
			this.datatableMain.Add(this.label6);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label6]));
			w21.TopAttach = ((uint)(5));
			w21.BottomAttach = ((uint)(6));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("ИНН:");
			this.datatableMain.Add(this.label7);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label7]));
			w22.TopAttach = ((uint)(3));
			w22.BottomAttach = ((uint)(4));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString("КПП:");
			this.datatableMain.Add(this.label8);
			global::Gtk.Table.TableChild w23 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label8]));
			w23.TopAttach = ((uint)(4));
			w23.BottomAttach = ((uint)(5));
			w23.XOptions = ((global::Gtk.AttachOptions)(4));
			w23.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.Yalign = 0F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString("Телефон:");
			this.datatableMain.Add(this.label9);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.label9]));
			w24.TopAttach = ((uint)(2));
			w24.BottomAttach = ((uint)(3));
			w24.XOptions = ((global::Gtk.AttachOptions)(4));
			w24.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.yentryBuhgalterSign = new global::Gamma.GtkWidgets.yEntry();
			this.yentryBuhgalterSign.CanFocus = true;
			this.yentryBuhgalterSign.Name = "yentryBuhgalterSign";
			this.yentryBuhgalterSign.IsEditable = true;
			this.yentryBuhgalterSign.MaxLength = 60;
			this.yentryBuhgalterSign.InvisibleChar = '●';
			this.datatableMain.Add(this.yentryBuhgalterSign);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.yentryBuhgalterSign]));
			w25.TopAttach = ((uint)(7));
			w25.BottomAttach = ((uint)(8));
			w25.LeftAttach = ((uint)(1));
			w25.RightAttach = ((uint)(2));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child datatableMain.Gtk.Table+TableChild
			this.yentryLeaderSign = new global::Gamma.GtkWidgets.yEntry();
			this.yentryLeaderSign.CanFocus = true;
			this.yentryLeaderSign.Name = "yentryLeaderSign";
			this.yentryLeaderSign.IsEditable = true;
			this.yentryLeaderSign.MaxLength = 60;
			this.yentryLeaderSign.InvisibleChar = '●';
			this.datatableMain.Add(this.yentryLeaderSign);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.datatableMain[this.yentryLeaderSign]));
			w26.TopAttach = ((uint)(6));
			w26.BottomAttach = ((uint)(7));
			w26.LeftAttach = ((uint)(1));
			w26.RightAttach = ((uint)(2));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			w9.Add(this.datatableMain);
			this.scrolledwindow1.Add(w9);
			this.notebookMain.Add(this.scrolledwindow1);
			// Notebook tab
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Информация");
			this.notebookMain.SetTabLabel(this.scrolledwindow1, this.label1);
			this.label1.ShowAll();
			// Container child notebookMain.Gtk.Notebook+NotebookChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(4)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.ytextviewBankName = new global::Gamma.GtkWidgets.yTextView();
			this.ytextviewBankName.CanFocus = true;
			this.ytextviewBankName.Name = "ytextviewBankName";
			this.GtkScrolledWindow.Add(this.ytextviewBankName);
			this.table1.Add(this.GtkScrolledWindow);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table1[this.GtkScrolledWindow]));
			w31.TopAttach = ((uint)(1));
			w31.BottomAttach = ((uint)(2));
			w31.LeftAttach = ((uint)(1));
			w31.RightAttach = ((uint)(2));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.validatedentryBankAccount = new global::QS.Widgets.ValidatedEntry();
			this.validatedentryBankAccount.CanFocus = true;
			this.validatedentryBankAccount.Name = "validatedentryBankAccount";
			this.validatedentryBankAccount.IsEditable = true;
			this.validatedentryBankAccount.MaxLength = 25;
			this.validatedentryBankAccount.InvisibleChar = '●';
			this.table1.Add(this.validatedentryBankAccount);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1[this.validatedentryBankAccount]));
			w32.TopAttach = ((uint)(3));
			w32.BottomAttach = ((uint)(4));
			w32.LeftAttach = ((uint)(1));
			w32.RightAttach = ((uint)(2));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.validatedentryBankBik = new global::QS.Widgets.ValidatedEntry();
			this.validatedentryBankBik.CanFocus = true;
			this.validatedentryBankBik.Name = "validatedentryBankBik";
			this.validatedentryBankBik.IsEditable = true;
			this.validatedentryBankBik.MaxLength = 9;
			this.validatedentryBankBik.InvisibleChar = '●';
			this.table1.Add(this.validatedentryBankBik);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table1[this.validatedentryBankBik]));
			w33.LeftAttach = ((uint)(1));
			w33.RightAttach = ((uint)(2));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.validatedentryBankCorAccount = new global::QS.Widgets.ValidatedEntry();
			this.validatedentryBankCorAccount.CanFocus = true;
			this.validatedentryBankCorAccount.Name = "validatedentryBankCorAccount";
			this.validatedentryBankCorAccount.IsEditable = true;
			this.validatedentryBankCorAccount.MaxLength = 25;
			this.validatedentryBankCorAccount.InvisibleChar = '●';
			this.table1.Add(this.validatedentryBankCorAccount);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table1[this.validatedentryBankCorAccount]));
			w34.TopAttach = ((uint)(2));
			w34.BottomAttach = ((uint)(3));
			w34.LeftAttach = ((uint)(1));
			w34.RightAttach = ((uint)(2));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabel1 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel1.Name = "ylabel1";
			this.ylabel1.Xalign = 1F;
			this.ylabel1.LabelProp = global::Mono.Unix.Catalog.GetString("БИК банка:");
			this.table1.Add(this.ylabel1);
			global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabel1]));
			w35.XOptions = ((global::Gtk.AttachOptions)(4));
			w35.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabel2 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel2.Name = "ylabel2";
			this.ylabel2.Xalign = 1F;
			this.ylabel2.Yalign = 0F;
			this.ylabel2.LabelProp = global::Mono.Unix.Catalog.GetString("Название банка:");
			this.table1.Add(this.ylabel2);
			global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabel2]));
			w36.TopAttach = ((uint)(1));
			w36.BottomAttach = ((uint)(2));
			w36.XOptions = ((global::Gtk.AttachOptions)(4));
			w36.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabel3 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel3.Name = "ylabel3";
			this.ylabel3.Xalign = 1F;
			this.ylabel3.LabelProp = global::Mono.Unix.Catalog.GetString("Корреспондентский счёт:");
			this.table1.Add(this.ylabel3);
			global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabel3]));
			w37.TopAttach = ((uint)(2));
			w37.BottomAttach = ((uint)(3));
			w37.XOptions = ((global::Gtk.AttachOptions)(4));
			w37.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.ylabel4 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel4.Name = "ylabel4";
			this.ylabel4.Xalign = 1F;
			this.ylabel4.LabelProp = global::Mono.Unix.Catalog.GetString("Расчётный счёт:");
			this.table1.Add(this.ylabel4);
			global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table1[this.ylabel4]));
			w38.TopAttach = ((uint)(3));
			w38.BottomAttach = ((uint)(4));
			w38.XOptions = ((global::Gtk.AttachOptions)(4));
			w38.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.table1);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.table1]));
			w39.Position = 0;
			w39.Expand = false;
			w39.Fill = false;
			this.notebookMain.Add(this.vbox2);
			global::Gtk.Notebook.NotebookChild w40 = ((global::Gtk.Notebook.NotebookChild)(this.notebookMain[this.vbox2]));
			w40.Position = 1;
			// Notebook tab
			this.label12 = new global::Gtk.Label();
			this.label12.Name = "label12";
			this.label12.LabelProp = global::Mono.Unix.Catalog.GetString("Счет");
			this.notebookMain.SetTabLabel(this.vbox2, this.label12);
			this.label12.ShowAll();
			this.vbox1.Add(this.notebookMain);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.notebookMain]));
			w41.Position = 1;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
			this.radioTabInfo.Toggled += new global::System.EventHandler(this.OnRadioTabInfoToggled);
			this.radioTabAccounts.Toggled += new global::System.EventHandler(this.OnRadioTabAccountsToggled);
		}
	}
}
