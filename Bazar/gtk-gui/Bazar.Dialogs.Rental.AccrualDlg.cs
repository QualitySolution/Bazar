
// This file has been generated by the GUI designer. Do not modify.
namespace Bazar.Dialogs.Rental
{
	public partial class AccrualDlg
	{
		private global::Gtk.HBox hbox1;

		private global::Gtk.Table table1;

		private global::QS.Widgets.GtkUI.DatePicker dateAccrual;

		private global::Gtk.HBox hbox23;

		private global::Gtk.ComboBox comboAccrualMonth;

		private global::Gtk.ComboBox comboAccuralYear;

		private global::Gtk.HBox hbox5;

		private global::Gtk.ComboBox comboContract;

		private global::Gtk.Button buttonOpenContract;

		private global::Gtk.HBox hbox7;

		private global::Gamma.GtkWidgets.yEntry yentryInvoceNumber;

		private global::Gamma.GtkWidgets.yCheckButton ycheckInvoiceAuto;

		private global::Gtk.Label label2;

		private global::Gtk.Label label4;

		private global::Gtk.Label label5;

		private global::Gtk.Label label6;

		private global::Gtk.Label label7;

		private global::Gtk.Label label8;

		private global::Gtk.Label labelLessee;

		private global::Gtk.Label labelOrg;

		private global::Gtk.VBox vbox2;

		private global::Gtk.Table table2;

		private global::Gtk.Entry entryUser;

		private global::Gtk.ScrolledWindow GtkScrolledWindow2;

		private global::Gtk.TextView textviewComments;

		private global::Gtk.Label label13;

		private global::Gtk.Label label3;

		private global::Gtk.Label label9;

		private global::Gtk.Label labelDebtName;

		private global::Gtk.Label labelDebts;

		private global::Gtk.Label labelStatus;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Button buttonMakePayment;

		private global::Gtk.Button buttonFillService;

		private global::Gtk.Notebook notebook1;

		private global::Gtk.VBox vbox3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gamma.GtkWidgets.yTreeView treeviewServices;

		private global::Gtk.HBox hbox4;

		private global::Gtk.VBox vbox5;

		private global::Gtk.HBox hbox6;

		private global::Gtk.Button buttonAddService;

		private global::Gtk.Button buttonDelService;

		private global::Gtk.VSeparator vseparator1;

		private global::Gtk.Button buttonPlaceSet;

		private global::Gtk.Button buttonPlaceClean;

		private global::Gtk.VSeparator vseparator2;

		private global::Gtk.Button buttonFromMeter;

		private global::Gtk.Label labelSum;

		private global::Gtk.Label label1;

		private global::Gtk.VBox vbox4;

		private global::Gtk.ScrolledWindow GtkScrolledWindow1;

		private global::Gtk.TreeView treeviewIncomes;

		private global::Gtk.Label labelIncomeSum;

		private global::Gtk.Label label12;

		private global::QSWidgetLib.MenuButton buttonPrint;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Bazar.Dialogs.Rental.AccrualDlg
			this.Name = "Bazar.Dialogs.Rental.AccrualDlg";
			this.Title = global::Mono.Unix.Catalog.GetString("Новое начисление");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child Bazar.Dialogs.Rental.AccrualDlg.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(6)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(3));
			// Container child table1.Gtk.Table+TableChild
			this.dateAccrual = new global::QS.Widgets.GtkUI.DatePicker();
			this.dateAccrual.Events = ((global::Gdk.EventMask)(256));
			this.dateAccrual.Name = "dateAccrual";
			this.dateAccrual.WithTime = false;
			this.dateAccrual.HideCalendarButton = false;
			this.dateAccrual.Date = new global::System.DateTime(0);
			this.dateAccrual.IsEditable = true;
			this.dateAccrual.AutoSeparation = true;
			this.table1.Add(this.dateAccrual);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.dateAccrual]));
			w2.TopAttach = ((uint)(5));
			w2.BottomAttach = ((uint)(6));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox23 = new global::Gtk.HBox();
			this.hbox23.Name = "hbox23";
			this.hbox23.Spacing = 6;
			// Container child hbox23.Gtk.Box+BoxChild
			this.comboAccrualMonth = global::Gtk.ComboBox.NewText();
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Нет"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Январь"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Февраль"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Март"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Апрель"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Май"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Июнь"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Июль"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Август"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Сентябрь"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Октябрь"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Ноябрь"));
			this.comboAccrualMonth.AppendText(global::Mono.Unix.Catalog.GetString("Декабрь"));
			this.comboAccrualMonth.Name = "comboAccrualMonth";
			this.hbox23.Add(this.comboAccrualMonth);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox23[this.comboAccrualMonth]));
			w3.Position = 0;
			// Container child hbox23.Gtk.Box+BoxChild
			this.comboAccuralYear = global::Gtk.ComboBox.NewText();
			this.comboAccuralYear.Name = "comboAccuralYear";
			this.hbox23.Add(this.comboAccuralYear);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox23[this.comboAccuralYear]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			this.table1.Add(this.hbox23);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox23]));
			w5.TopAttach = ((uint)(1));
			w5.BottomAttach = ((uint)(2));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox5 = new global::Gtk.HBox();
			this.hbox5.Name = "hbox5";
			this.hbox5.Spacing = 6;
			// Container child hbox5.Gtk.Box+BoxChild
			this.comboContract = new global::Gtk.ComboBox();
			this.comboContract.Name = "comboContract";
			this.hbox5.Add(this.comboContract);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.comboContract]));
			w6.Position = 0;
			// Container child hbox5.Gtk.Box+BoxChild
			this.buttonOpenContract = new global::Gtk.Button();
			this.buttonOpenContract.TooltipMarkup = "Открыть договор";
			this.buttonOpenContract.Sensitive = false;
			this.buttonOpenContract.CanFocus = true;
			this.buttonOpenContract.Name = "buttonOpenContract";
			this.buttonOpenContract.UseUnderline = true;
			global::Gtk.Image w7 = new global::Gtk.Image();
			w7.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-file", global::Gtk.IconSize.Menu);
			this.buttonOpenContract.Image = w7;
			this.hbox5.Add(this.buttonOpenContract);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox5[this.buttonOpenContract]));
			w8.Position = 1;
			w8.Expand = false;
			w8.Fill = false;
			this.table1.Add(this.hbox5);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox5]));
			w9.TopAttach = ((uint)(2));
			w9.BottomAttach = ((uint)(3));
			w9.LeftAttach = ((uint)(1));
			w9.RightAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox7 = new global::Gtk.HBox();
			this.hbox7.Name = "hbox7";
			this.hbox7.Spacing = 6;
			// Container child hbox7.Gtk.Box+BoxChild
			this.yentryInvoceNumber = new global::Gamma.GtkWidgets.yEntry();
			this.yentryInvoceNumber.CanFocus = true;
			this.yentryInvoceNumber.Name = "yentryInvoceNumber";
			this.yentryInvoceNumber.IsEditable = true;
			this.yentryInvoceNumber.InvisibleChar = '●';
			this.hbox7.Add(this.yentryInvoceNumber);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.yentryInvoceNumber]));
			w10.Position = 0;
			w10.Expand = false;
			w10.Fill = false;
			// Container child hbox7.Gtk.Box+BoxChild
			this.ycheckInvoiceAuto = new global::Gamma.GtkWidgets.yCheckButton();
			this.ycheckInvoiceAuto.TooltipMarkup = "Устанавливать автоматически следующий номер счета при сохранении.";
			this.ycheckInvoiceAuto.CanFocus = true;
			this.ycheckInvoiceAuto.Name = "ycheckInvoiceAuto";
			this.ycheckInvoiceAuto.Label = global::Mono.Unix.Catalog.GetString("Авто");
			this.ycheckInvoiceAuto.DrawIndicator = true;
			this.ycheckInvoiceAuto.UseUnderline = true;
			this.hbox7.Add(this.ycheckInvoiceAuto);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox7[this.ycheckInvoiceAuto]));
			w11.Position = 1;
			w11.Expand = false;
			w11.Fill = false;
			this.table1.Add(this.hbox7);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox7]));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Номер счета:");
			this.table1.Add(this.label2);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1[this.label2]));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Договор:");
			this.table1.Add(this.label4);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table1[this.label4]));
			w14.TopAttach = ((uint)(2));
			w14.BottomAttach = ((uint)(3));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = global::Mono.Unix.Catalog.GetString("Месяц начисления:");
			this.table1.Add(this.label5);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table1[this.label5]));
			w15.TopAttach = ((uint)(1));
			w15.BottomAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = global::Mono.Unix.Catalog.GetString("Организация:");
			this.table1.Add(this.label6);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table1[this.label6]));
			w16.TopAttach = ((uint)(3));
			w16.BottomAttach = ((uint)(4));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = global::Mono.Unix.Catalog.GetString("Дата начисления:");
			this.table1.Add(this.label7);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table1[this.label7]));
			w17.TopAttach = ((uint)(5));
			w17.BottomAttach = ((uint)(6));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = global::Mono.Unix.Catalog.GetString("Арендатор:");
			this.table1.Add(this.label8);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table1[this.label8]));
			w18.TopAttach = ((uint)(4));
			w18.BottomAttach = ((uint)(5));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelLessee = new global::Gtk.Label();
			this.labelLessee.Name = "labelLessee";
			this.labelLessee.LabelProp = global::Mono.Unix.Catalog.GetString("--");
			this.table1.Add(this.labelLessee);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table1[this.labelLessee]));
			w19.TopAttach = ((uint)(4));
			w19.BottomAttach = ((uint)(5));
			w19.LeftAttach = ((uint)(1));
			w19.RightAttach = ((uint)(2));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelOrg = new global::Gtk.Label();
			this.labelOrg.Name = "labelOrg";
			this.labelOrg.LabelProp = global::Mono.Unix.Catalog.GetString("--");
			this.table1.Add(this.labelOrg);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table1[this.labelOrg]));
			w20.TopAttach = ((uint)(3));
			w20.BottomAttach = ((uint)(4));
			w20.LeftAttach = ((uint)(1));
			w20.RightAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			this.hbox1.Add(this.table1);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.table1]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table2 = new global::Gtk.Table(((uint)(4)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(3));
			// Container child table2.Gtk.Table+TableChild
			this.entryUser = new global::Gtk.Entry();
			this.entryUser.Sensitive = false;
			this.entryUser.CanFocus = true;
			this.entryUser.Name = "entryUser";
			this.entryUser.IsEditable = true;
			this.entryUser.InvisibleChar = '●';
			this.table2.Add(this.entryUser);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table2[this.entryUser]));
			w22.LeftAttach = ((uint)(1));
			w22.RightAttach = ((uint)(2));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
			this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
			this.textviewComments = new global::Gtk.TextView();
			this.textviewComments.CanFocus = true;
			this.textviewComments.Name = "textviewComments";
			this.textviewComments.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow2.Add(this.textviewComments);
			this.table2.Add(this.GtkScrolledWindow2);
			global::Gtk.Table.TableChild w24 = ((global::Gtk.Table.TableChild)(this.table2[this.GtkScrolledWindow2]));
			w24.TopAttach = ((uint)(1));
			w24.BottomAttach = ((uint)(2));
			w24.LeftAttach = ((uint)(1));
			w24.RightAttach = ((uint)(2));
			// Container child table2.Gtk.Table+TableChild
			this.label13 = new global::Gtk.Label();
			this.label13.Name = "label13";
			this.label13.Xalign = 1F;
			this.label13.LabelProp = global::Mono.Unix.Catalog.GetString("Состояние:");
			this.table2.Add(this.label13);
			global::Gtk.Table.TableChild w25 = ((global::Gtk.Table.TableChild)(this.table2[this.label13]));
			w25.TopAttach = ((uint)(2));
			w25.BottomAttach = ((uint)(3));
			w25.XOptions = ((global::Gtk.AttachOptions)(4));
			w25.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Пользователь:");
			this.table2.Add(this.label3);
			global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table2[this.label3]));
			w26.XOptions = ((global::Gtk.AttachOptions)(4));
			w26.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.Yalign = 0F;
			this.label9.LabelProp = global::Mono.Unix.Catalog.GetString("Комментарии:");
			this.table2.Add(this.label9);
			global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table2[this.label9]));
			w27.TopAttach = ((uint)(1));
			w27.BottomAttach = ((uint)(2));
			w27.XOptions = ((global::Gtk.AttachOptions)(4));
			w27.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelDebtName = new global::Gtk.Label();
			this.labelDebtName.Name = "labelDebtName";
			this.labelDebtName.Xalign = 1F;
			this.labelDebtName.LabelProp = global::Mono.Unix.Catalog.GetString("Предыдущий долг:");
			this.table2.Add(this.labelDebtName);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table2[this.labelDebtName]));
			w28.TopAttach = ((uint)(3));
			w28.BottomAttach = ((uint)(4));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelDebts = new global::Gtk.Label();
			this.labelDebts.Name = "labelDebts";
			this.labelDebts.LabelProp = global::Mono.Unix.Catalog.GetString("нет");
			this.labelDebts.UseMarkup = true;
			this.table2.Add(this.labelDebts);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table2[this.labelDebts]));
			w29.TopAttach = ((uint)(3));
			w29.BottomAttach = ((uint)(4));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelStatus = new global::Gtk.Label();
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.LabelProp = global::Mono.Unix.Catalog.GetString("Новое");
			this.labelStatus.UseMarkup = true;
			this.table2.Add(this.labelStatus);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table2[this.labelStatus]));
			w30.TopAttach = ((uint)(2));
			w30.BottomAttach = ((uint)(3));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add(this.table2);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.table2]));
			w31.Position = 0;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonMakePayment = new global::Gtk.Button();
			this.buttonMakePayment.CanFocus = true;
			this.buttonMakePayment.Name = "buttonMakePayment";
			this.buttonMakePayment.UseUnderline = true;
			this.buttonMakePayment.Label = global::Mono.Unix.Catalog.GetString("Внести оплату");
			global::Gtk.Image w32 = new global::Gtk.Image();
			w32.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-dnd", global::Gtk.IconSize.Menu);
			this.buttonMakePayment.Image = w32;
			this.hbox3.Add(this.buttonMakePayment);
			global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.buttonMakePayment]));
			w33.Position = 0;
			w33.Expand = false;
			w33.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonFillService = new global::Gtk.Button();
			this.buttonFillService.Sensitive = false;
			this.buttonFillService.CanFocus = true;
			this.buttonFillService.Name = "buttonFillService";
			this.buttonFillService.UseUnderline = true;
			this.buttonFillService.Label = global::Mono.Unix.Catalog.GetString("Заполнить услуги");
			global::Gtk.Image w34 = new global::Gtk.Image();
			w34.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-go-down", global::Gtk.IconSize.Menu);
			this.buttonFillService.Image = w34;
			this.hbox3.Add(this.buttonFillService);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.buttonFillService]));
			w35.Position = 1;
			w35.Expand = false;
			w35.Fill = false;
			this.vbox2.Add(this.hbox3);
			global::Gtk.Box.BoxChild w36 = ((global::Gtk.Box.BoxChild)(this.vbox2[this.hbox3]));
			w36.Position = 1;
			w36.Expand = false;
			w36.Fill = false;
			this.hbox1.Add(this.vbox2);
			global::Gtk.Box.BoxChild w37 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.vbox2]));
			w37.Position = 1;
			w1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w38 = ((global::Gtk.Box.BoxChild)(w1[this.hbox1]));
			w38.Position = 0;
			w38.Expand = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.notebook1 = new global::Gtk.Notebook();
			this.notebook1.CanFocus = true;
			this.notebook1.Name = "notebook1";
			this.notebook1.CurrentPage = 0;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeviewServices = new global::Gamma.GtkWidgets.yTreeView();
			this.treeviewServices.CanFocus = true;
			this.treeviewServices.Name = "treeviewServices";
			this.GtkScrolledWindow.Add(this.treeviewServices);
			this.vbox3.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.GtkScrolledWindow]));
			w40.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.vbox5 = new global::Gtk.VBox();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbox6 = new global::Gtk.HBox();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			// Container child hbox6.Gtk.Box+BoxChild
			this.buttonAddService = new global::Gtk.Button();
			this.buttonAddService.CanFocus = true;
			this.buttonAddService.Name = "buttonAddService";
			this.buttonAddService.UseUnderline = true;
			this.buttonAddService.Label = global::Mono.Unix.Catalog.GetString("Добавить");
			global::Gtk.Image w41 = new global::Gtk.Image();
			w41.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonAddService.Image = w41;
			this.hbox6.Add(this.buttonAddService);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.buttonAddService]));
			w42.Position = 0;
			w42.Expand = false;
			w42.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.buttonDelService = new global::Gtk.Button();
			this.buttonDelService.CanFocus = true;
			this.buttonDelService.Name = "buttonDelService";
			this.buttonDelService.UseUnderline = true;
			this.buttonDelService.Label = global::Mono.Unix.Catalog.GetString("Удалить");
			global::Gtk.Image w43 = new global::Gtk.Image();
			w43.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDelService.Image = w43;
			this.hbox6.Add(this.buttonDelService);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.buttonDelService]));
			w44.Position = 1;
			w44.Expand = false;
			w44.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.vseparator1 = new global::Gtk.VSeparator();
			this.vseparator1.Name = "vseparator1";
			this.hbox6.Add(this.vseparator1);
			global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.vseparator1]));
			w45.Position = 2;
			w45.Expand = false;
			w45.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.buttonPlaceSet = new global::Gtk.Button();
			this.buttonPlaceSet.CanFocus = true;
			this.buttonPlaceSet.Name = "buttonPlaceSet";
			this.buttonPlaceSet.UseUnderline = true;
			this.buttonPlaceSet.Label = global::Mono.Unix.Catalog.GetString("Указать место");
			global::Gtk.Image w46 = new global::Gtk.Image();
			w46.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-edit", global::Gtk.IconSize.Menu);
			this.buttonPlaceSet.Image = w46;
			this.hbox6.Add(this.buttonPlaceSet);
			global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.buttonPlaceSet]));
			w47.Position = 3;
			w47.Expand = false;
			w47.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.buttonPlaceClean = new global::Gtk.Button();
			this.buttonPlaceClean.CanFocus = true;
			this.buttonPlaceClean.Name = "buttonPlaceClean";
			this.buttonPlaceClean.UseUnderline = true;
			this.buttonPlaceClean.Label = global::Mono.Unix.Catalog.GetString("Очистить место");
			global::Gtk.Image w48 = new global::Gtk.Image();
			w48.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-clear", global::Gtk.IconSize.Menu);
			this.buttonPlaceClean.Image = w48;
			this.hbox6.Add(this.buttonPlaceClean);
			global::Gtk.Box.BoxChild w49 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.buttonPlaceClean]));
			w49.Position = 4;
			w49.Expand = false;
			w49.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.vseparator2 = new global::Gtk.VSeparator();
			this.vseparator2.Name = "vseparator2";
			this.hbox6.Add(this.vseparator2);
			global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.vseparator2]));
			w50.Position = 5;
			w50.Expand = false;
			w50.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.buttonFromMeter = new global::Gtk.Button();
			this.buttonFromMeter.CanFocus = true;
			this.buttonFromMeter.Name = "buttonFromMeter";
			this.buttonFromMeter.UseUnderline = true;
			this.buttonFromMeter.Label = global::Mono.Unix.Catalog.GetString("По счётчику");
			global::Gtk.Image w51 = new global::Gtk.Image();
			w51.Pixbuf = global::Gdk.Pixbuf.LoadFromResource("Bazar.Icons.meter.png");
			this.buttonFromMeter.Image = w51;
			this.hbox6.Add(this.buttonFromMeter);
			global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.hbox6[this.buttonFromMeter]));
			w52.Position = 6;
			w52.Expand = false;
			w52.Fill = false;
			this.vbox5.Add(this.hbox6);
			global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.vbox5[this.hbox6]));
			w53.Position = 0;
			w53.Expand = false;
			w53.Fill = false;
			this.hbox4.Add(this.vbox5);
			global::Gtk.Box.BoxChild w54 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.vbox5]));
			w54.Position = 0;
			w54.Expand = false;
			w54.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.labelSum = new global::Gtk.Label();
			this.labelSum.Name = "labelSum";
			this.labelSum.Xalign = 1F;
			this.labelSum.LabelProp = global::Mono.Unix.Catalog.GetString("Сумма: 0 руб.");
			this.labelSum.Justify = ((global::Gtk.Justification)(1));
			this.hbox4.Add(this.labelSum);
			global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.labelSum]));
			w55.Position = 1;
			this.vbox3.Add(this.hbox4);
			global::Gtk.Box.BoxChild w56 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.hbox4]));
			w56.Position = 1;
			w56.Expand = false;
			w56.Fill = false;
			this.notebook1.Add(this.vbox3);
			// Notebook tab
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Начислено");
			this.notebook1.SetTabLabel(this.vbox3, this.label1);
			this.label1.ShowAll();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox4 = new global::Gtk.VBox();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewIncomes = new global::Gtk.TreeView();
			this.treeviewIncomes.CanFocus = true;
			this.treeviewIncomes.Name = "treeviewIncomes";
			this.GtkScrolledWindow1.Add(this.treeviewIncomes);
			this.vbox4.Add(this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.GtkScrolledWindow1]));
			w59.Position = 0;
			// Container child vbox4.Gtk.Box+BoxChild
			this.labelIncomeSum = new global::Gtk.Label();
			this.labelIncomeSum.Name = "labelIncomeSum";
			this.labelIncomeSum.Xalign = 1F;
			this.labelIncomeSum.LabelProp = global::Mono.Unix.Catalog.GetString("Сумма: 0 р.");
			this.vbox4.Add(this.labelIncomeSum);
			global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.vbox4[this.labelIncomeSum]));
			w60.Position = 1;
			w60.Expand = false;
			w60.Fill = false;
			this.notebook1.Add(this.vbox4);
			global::Gtk.Notebook.NotebookChild w61 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1[this.vbox4]));
			w61.Position = 1;
			// Notebook tab
			this.label12 = new global::Gtk.Label();
			this.label12.Name = "label12";
			this.label12.LabelProp = global::Mono.Unix.Catalog.GetString("Платежи");
			this.notebook1.SetTabLabel(this.vbox4, this.label12);
			this.label12.ShowAll();
			w1.Add(this.notebook1);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(w1[this.notebook1]));
			w62.Position = 1;
			// Internal child Bazar.Dialogs.Rental.AccrualDlg.ActionArea
			global::Gtk.HButtonBox w63 = this.ActionArea;
			w63.Name = "dialog1_ActionArea";
			w63.Spacing = 10;
			w63.BorderWidth = ((uint)(5));
			w63.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonPrint = new global::QSWidgetLib.MenuButton();
			this.buttonPrint.CanFocus = true;
			this.buttonPrint.Name = "buttonPrint";
			this.buttonPrint.UseUnderline = true;
			this.buttonPrint.UseMarkup = false;
			this.buttonPrint.LabelXAlign = 0F;
			this.buttonPrint.Label = global::Mono.Unix.Catalog.GetString("Печать");
			global::Gtk.Image w64 = new global::Gtk.Image();
			w64.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-print", global::Gtk.IconSize.Button);
			this.buttonPrint.Image = w64;
			this.AddActionWidget(this.buttonPrint, 0);
			global::Gtk.ButtonBox.ButtonBoxChild w65 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w63[this.buttonPrint]));
			w65.Expand = false;
			w65.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("О_тменить");
			global::Gtk.Image w66 = new global::Gtk.Image();
			w66.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-cancel", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w66;
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w67 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w63[this.buttonCancel]));
			w67.Position = 1;
			w67.Expand = false;
			w67.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = global::Mono.Unix.Catalog.GetString("_OK");
			global::Gtk.Image w68 = new global::Gtk.Image();
			w68.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-ok", global::Gtk.IconSize.Menu);
			this.buttonOk.Image = w68;
			w63.Add(this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w69 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w63[this.buttonOk]));
			w69.Position = 2;
			w69.Expand = false;
			w69.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 777;
			this.DefaultHeight = 512;
			this.Show();
			this.comboContract.Changed += new global::System.EventHandler(this.OnComboContractChanged);
			this.buttonOpenContract.Clicked += new global::System.EventHandler(this.OnButtonOpenContractClicked);
			this.comboAccrualMonth.Changed += new global::System.EventHandler(this.OnComboAccrualMonthChanged);
			this.comboAccuralYear.Changed += new global::System.EventHandler(this.OnComboAccuralYearChanged);
			this.buttonMakePayment.Clicked += new global::System.EventHandler(this.OnButtonMakePaymentClicked);
			this.buttonFillService.Clicked += new global::System.EventHandler(this.OnButtonFillServiceClicked);
			this.treeviewServices.RowActivated += new global::Gtk.RowActivatedHandler(this.OnTreeviewServicesRowActivated);
			this.buttonAddService.Clicked += new global::System.EventHandler(this.OnButtonAddServiceClicked);
			this.buttonDelService.Clicked += new global::System.EventHandler(this.OnButtonDelServiceClicked);
			this.buttonPlaceSet.Clicked += new global::System.EventHandler(this.OnButtonPlaceSetClicked);
			this.buttonPlaceClean.Clicked += new global::System.EventHandler(this.OnButtonPlaceCleanClicked);
			this.buttonFromMeter.Clicked += new global::System.EventHandler(this.OnButtonFromMeterClicked);
			this.treeviewIncomes.RowActivated += new global::Gtk.RowActivatedHandler(this.OnTreeviewIncomesRowActivated);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
