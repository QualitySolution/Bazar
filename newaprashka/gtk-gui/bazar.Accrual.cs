
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class Accrual
	{
		private global::Gtk.HBox hbox1;
		private global::Gtk.Table table1;
		private global::Gtk.ComboBox comboContract;
		private global::Gtk.HBox hbox23;
		private global::Gtk.ComboBox comboAccrualMonth;
		private global::Gtk.ComboBox comboAccuralYear;
		private global::Gtk.Label label4;
		private global::Gtk.Label label5;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Label label8;
		private global::Gtk.Label labelLessee;
		private global::Gtk.Label labelOrg;
		private global::Gtk.Label labelPlace;
		private global::Gtk.VBox vbox2;
		private global::Gtk.Table table2;
		private global::Gtk.Entry entryNumber;
		private global::Gtk.Entry entryUser;
		private global::Gtk.Label label13;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;
		private global::Gtk.Label labelDebtName;
		private global::Gtk.Label labelDebts;
		private global::Gtk.Label labelStatus;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Button buttonMakePayment;
		private global::Gtk.Button buttonPrint;
		private global::Gtk.Notebook notebook1;
		private global::Gtk.VBox vbox3;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TreeView treeviewServices;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Button buttonAddService;
		private global::Gtk.Button buttonDelService;
		private global::Gtk.Label labelSum;
		private global::Gtk.Label label1;
		private global::Gtk.VBox vbox4;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.TreeView treeviewIncomes;
		private global::Gtk.Label labelIncomeSum;
		private global::Gtk.Label label12;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;
		
		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.Accrual
			this.Name = "bazar.Accrual";
			this.Title = "Новое начисление";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.Accrual.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(5)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(3));
			// Container child table1.Gtk.Table+TableChild
			this.comboContract = global::Gtk.ComboBox.NewText ();
			this.comboContract.Name = "comboContract";
			this.table1.Add (this.comboContract);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.comboContract]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox23 = new global::Gtk.HBox ();
			this.hbox23.Name = "hbox23";
			this.hbox23.Spacing = 6;
			// Container child hbox23.Gtk.Box+BoxChild
			this.comboAccrualMonth = global::Gtk.ComboBox.NewText ();
			this.comboAccrualMonth.AppendText ("Нет");
			this.comboAccrualMonth.AppendText ("Январь");
			this.comboAccrualMonth.AppendText ("Февраль");
			this.comboAccrualMonth.AppendText ("Март");
			this.comboAccrualMonth.AppendText ("Апрель");
			this.comboAccrualMonth.AppendText ("Май");
			this.comboAccrualMonth.AppendText ("Июнь");
			this.comboAccrualMonth.AppendText ("Июль");
			this.comboAccrualMonth.AppendText ("Август");
			this.comboAccrualMonth.AppendText ("Сентябрь");
			this.comboAccrualMonth.AppendText ("Октябрь");
			this.comboAccrualMonth.AppendText ("Ноябрь");
			this.comboAccrualMonth.AppendText ("Декабрь");
			this.comboAccrualMonth.Name = "comboAccrualMonth";
			this.hbox23.Add (this.comboAccrualMonth);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox23 [this.comboAccrualMonth]));
			w3.Position = 0;
			// Container child hbox23.Gtk.Box+BoxChild
			this.comboAccuralYear = global::Gtk.ComboBox.NewText ();
			this.comboAccuralYear.Name = "comboAccuralYear";
			this.hbox23.Add (this.comboAccuralYear);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox23 [this.comboAccuralYear]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			this.table1.Add (this.hbox23);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox23]));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = "Договор:";
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w6.TopAttach = ((uint)(1));
			w6.BottomAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = "Месяц начисления:";
			this.table1.Add (this.label5);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = "Организация:";
			this.table1.Add (this.label6);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.label6]));
			w8.TopAttach = ((uint)(2));
			w8.BottomAttach = ((uint)(3));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = "Место:";
			this.table1.Add (this.label7);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.label7]));
			w9.TopAttach = ((uint)(3));
			w9.BottomAttach = ((uint)(4));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = "Арендатор:";
			this.table1.Add (this.label8);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1 [this.label8]));
			w10.TopAttach = ((uint)(4));
			w10.BottomAttach = ((uint)(5));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelLessee = new global::Gtk.Label ();
			this.labelLessee.Name = "labelLessee";
			this.labelLessee.LabelProp = "--";
			this.table1.Add (this.labelLessee);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelLessee]));
			w11.TopAttach = ((uint)(4));
			w11.BottomAttach = ((uint)(5));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelOrg = new global::Gtk.Label ();
			this.labelOrg.Name = "labelOrg";
			this.labelOrg.LabelProp = "--";
			this.table1.Add (this.labelOrg);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelOrg]));
			w12.TopAttach = ((uint)(2));
			w12.BottomAttach = ((uint)(3));
			w12.LeftAttach = ((uint)(1));
			w12.RightAttach = ((uint)(2));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelPlace = new global::Gtk.Label ();
			this.labelPlace.Name = "labelPlace";
			this.labelPlace.LabelProp = "--";
			this.table1.Add (this.labelPlace);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelPlace]));
			w13.TopAttach = ((uint)(3));
			w13.BottomAttach = ((uint)(4));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			this.hbox1.Add (this.table1);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.table1]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.table2 = new global::Gtk.Table (((uint)(4)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(3));
			// Container child table2.Gtk.Table+TableChild
			this.entryNumber = new global::Gtk.Entry ();
			this.entryNumber.Sensitive = false;
			this.entryNumber.CanFocus = true;
			this.entryNumber.Name = "entryNumber";
			this.entryNumber.Text = "не присвоен";
			this.entryNumber.IsEditable = true;
			this.entryNumber.InvisibleChar = '●';
			this.table2.Add (this.entryNumber);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryNumber]));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.entryUser = new global::Gtk.Entry ();
			this.entryUser.Sensitive = false;
			this.entryUser.CanFocus = true;
			this.entryUser.Name = "entryUser";
			this.entryUser.IsEditable = true;
			this.entryUser.InvisibleChar = '●';
			this.table2.Add (this.entryUser);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryUser]));
			w16.TopAttach = ((uint)(1));
			w16.BottomAttach = ((uint)(2));
			w16.LeftAttach = ((uint)(1));
			w16.RightAttach = ((uint)(2));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label13 = new global::Gtk.Label ();
			this.label13.Name = "label13";
			this.label13.Xalign = 1F;
			this.label13.LabelProp = "Состояние:";
			this.table2.Add (this.label13);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table2 [this.label13]));
			w17.TopAttach = ((uint)(2));
			w17.BottomAttach = ((uint)(3));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = "Номер начисления:";
			this.table2.Add (this.label2);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table2 [this.label2]));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = "Пользователь:";
			this.table2.Add (this.label3);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table2 [this.label3]));
			w19.TopAttach = ((uint)(1));
			w19.BottomAttach = ((uint)(2));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelDebtName = new global::Gtk.Label ();
			this.labelDebtName.Name = "labelDebtName";
			this.labelDebtName.LabelProp = "Предыдущий долг:";
			this.table2.Add (this.labelDebtName);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelDebtName]));
			w20.TopAttach = ((uint)(3));
			w20.BottomAttach = ((uint)(4));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelDebts = new global::Gtk.Label ();
			this.labelDebts.Name = "labelDebts";
			this.labelDebts.LabelProp = "нет";
			this.labelDebts.UseMarkup = true;
			this.table2.Add (this.labelDebts);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelDebts]));
			w21.TopAttach = ((uint)(3));
			w21.BottomAttach = ((uint)(4));
			w21.LeftAttach = ((uint)(1));
			w21.RightAttach = ((uint)(2));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelStatus = new global::Gtk.Label ();
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.LabelProp = "Новое";
			this.labelStatus.UseMarkup = true;
			this.table2.Add (this.labelStatus);
			global::Gtk.Table.TableChild w22 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelStatus]));
			w22.TopAttach = ((uint)(2));
			w22.BottomAttach = ((uint)(3));
			w22.LeftAttach = ((uint)(1));
			w22.RightAttach = ((uint)(2));
			w22.XOptions = ((global::Gtk.AttachOptions)(4));
			w22.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add (this.table2);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.table2]));
			w23.Position = 0;
			w23.Expand = false;
			w23.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonMakePayment = new global::Gtk.Button ();
			this.buttonMakePayment.CanFocus = true;
			this.buttonMakePayment.Name = "buttonMakePayment";
			this.buttonMakePayment.UseUnderline = true;
			// Container child buttonMakePayment.Gtk.Container+ContainerChild
			global::Gtk.Alignment w24 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w25 = new global::Gtk.HBox ();
			w25.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w26 = new global::Gtk.Image ();
			w26.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dnd", global::Gtk.IconSize.Menu);
			w25.Add (w26);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w28 = new global::Gtk.Label ();
			w28.LabelProp = "Внести оплату";
			w28.UseUnderline = true;
			w25.Add (w28);
			w24.Add (w25);
			this.buttonMakePayment.Add (w24);
			this.hbox3.Add (this.buttonMakePayment);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonMakePayment]));
			w32.Position = 0;
			w32.Expand = false;
			w32.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonPrint = new global::Gtk.Button ();
			this.buttonPrint.CanFocus = true;
			this.buttonPrint.Name = "buttonPrint";
			this.buttonPrint.UseUnderline = true;
			// Container child buttonPrint.Gtk.Container+ContainerChild
			global::Gtk.Alignment w33 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w34 = new global::Gtk.HBox ();
			w34.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w35 = new global::Gtk.Image ();
			w35.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-print", global::Gtk.IconSize.Menu);
			w34.Add (w35);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w37 = new global::Gtk.Label ();
			w37.LabelProp = "Расчёт";
			w37.UseUnderline = true;
			w34.Add (w37);
			w33.Add (w34);
			this.buttonPrint.Add (w33);
			this.hbox3.Add (this.buttonPrint);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonPrint]));
			w41.Position = 1;
			w41.Expand = false;
			w41.Fill = false;
			this.vbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox3]));
			w42.Position = 1;
			w42.Expand = false;
			w42.Fill = false;
			this.hbox1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
			w43.Position = 1;
			w43.Expand = false;
			w43.Fill = false;
			w1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox1]));
			w44.Position = 0;
			w44.Expand = false;
			w44.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.notebook1 = new global::Gtk.Notebook ();
			this.notebook1.CanFocus = true;
			this.notebook1.Name = "notebook1";
			this.notebook1.CurrentPage = 0;
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeviewServices = new global::Gtk.TreeView ();
			this.treeviewServices.CanFocus = true;
			this.treeviewServices.Name = "treeviewServices";
			this.GtkScrolledWindow.Add (this.treeviewServices);
			this.vbox3.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
			w46.Position = 0;
			// Container child vbox3.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.buttonAddService = new global::Gtk.Button ();
			this.buttonAddService.CanFocus = true;
			this.buttonAddService.Name = "buttonAddService";
			this.buttonAddService.UseUnderline = true;
			// Container child buttonAddService.Gtk.Container+ContainerChild
			global::Gtk.Alignment w47 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w48 = new global::Gtk.HBox ();
			w48.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w49 = new global::Gtk.Image ();
			w49.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w48.Add (w49);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w51 = new global::Gtk.Label ();
			w51.LabelProp = "Добавить";
			w51.UseUnderline = true;
			w48.Add (w51);
			w47.Add (w48);
			this.buttonAddService.Add (w47);
			this.hbox4.Add (this.buttonAddService);
			global::Gtk.Box.BoxChild w55 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonAddService]));
			w55.Position = 0;
			w55.Expand = false;
			w55.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.buttonDelService = new global::Gtk.Button ();
			this.buttonDelService.CanFocus = true;
			this.buttonDelService.Name = "buttonDelService";
			this.buttonDelService.UseUnderline = true;
			// Container child buttonDelService.Gtk.Container+ContainerChild
			global::Gtk.Alignment w56 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w57 = new global::Gtk.HBox ();
			w57.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w58 = new global::Gtk.Image ();
			w58.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			w57.Add (w58);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w60 = new global::Gtk.Label ();
			w60.LabelProp = "Удалить";
			w60.UseUnderline = true;
			w57.Add (w60);
			w56.Add (w57);
			this.buttonDelService.Add (w56);
			this.hbox4.Add (this.buttonDelService);
			global::Gtk.Box.BoxChild w64 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonDelService]));
			w64.Position = 1;
			w64.Expand = false;
			w64.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.labelSum = new global::Gtk.Label ();
			this.labelSum.Name = "labelSum";
			this.labelSum.Xalign = 1F;
			this.labelSum.LabelProp = "Сумма: 0 руб.";
			this.hbox4.Add (this.labelSum);
			global::Gtk.Box.BoxChild w65 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.labelSum]));
			w65.Position = 2;
			this.vbox3.Add (this.hbox4);
			global::Gtk.Box.BoxChild w66 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox4]));
			w66.Position = 1;
			w66.Expand = false;
			w66.Fill = false;
			this.notebook1.Add (this.vbox3);
			// Notebook tab
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.LabelProp = "Начислено";
			this.notebook1.SetTabLabel (this.vbox3, this.label1);
			this.label1.ShowAll ();
			// Container child notebook1.Gtk.Notebook+NotebookChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewIncomes = new global::Gtk.TreeView ();
			this.treeviewIncomes.CanFocus = true;
			this.treeviewIncomes.Name = "treeviewIncomes";
			this.GtkScrolledWindow1.Add (this.treeviewIncomes);
			this.vbox4.Add (this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w69 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.GtkScrolledWindow1]));
			w69.Position = 0;
			// Container child vbox4.Gtk.Box+BoxChild
			this.labelIncomeSum = new global::Gtk.Label ();
			this.labelIncomeSum.Name = "labelIncomeSum";
			this.labelIncomeSum.Xalign = 1F;
			this.labelIncomeSum.LabelProp = "Сумма: 0 р.";
			this.vbox4.Add (this.labelIncomeSum);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.labelIncomeSum]));
			w70.Position = 1;
			w70.Expand = false;
			w70.Fill = false;
			this.notebook1.Add (this.vbox4);
			global::Gtk.Notebook.NotebookChild w71 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.vbox4]));
			w71.Position = 1;
			// Notebook tab
			this.label12 = new global::Gtk.Label ();
			this.label12.Name = "label12";
			this.label12.LabelProp = "Оплачено";
			this.notebook1.SetTabLabel (this.vbox4, this.label12);
			this.label12.ShowAll ();
			w1.Add (this.notebook1);
			global::Gtk.Box.BoxChild w72 = ((global::Gtk.Box.BoxChild)(w1 [this.notebook1]));
			w72.Position = 1;
			// Internal child bazar.Accrual.ActionArea
			global::Gtk.HButtonBox w73 = this.ActionArea;
			w73.Name = "dialog1_ActionArea";
			w73.Spacing = 10;
			w73.BorderWidth = ((uint)(5));
			w73.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w74 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w75 = new global::Gtk.HBox ();
			w75.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w76 = new global::Gtk.Image ();
			w76.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w75.Add (w76);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w78 = new global::Gtk.Label ();
			w78.LabelProp = "О_тменить";
			w78.UseUnderline = true;
			w75.Add (w78);
			w74.Add (w75);
			this.buttonCancel.Add (w74);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w82 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w73 [this.buttonCancel]));
			w82.Expand = false;
			w82.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w83 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w84 = new global::Gtk.HBox ();
			w84.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w85 = new global::Gtk.Image ();
			w85.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w84.Add (w85);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w87 = new global::Gtk.Label ();
			w87.LabelProp = "_OK";
			w87.UseUnderline = true;
			w84.Add (w87);
			w83.Add (w84);
			this.buttonOk.Add (w83);
			w73.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w91 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w73 [this.buttonOk]));
			w91.Position = 1;
			w91.Expand = false;
			w91.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 650;
			this.DefaultHeight = 423;
			this.Show ();
			this.comboAccrualMonth.Changed += new global::System.EventHandler (this.OnComboAccrualMonthChanged);
			this.comboAccuralYear.Changed += new global::System.EventHandler (this.OnComboAccuralYearChanged);
			this.comboContract.Changed += new global::System.EventHandler (this.OnComboContractChanged);
			this.buttonMakePayment.Clicked += new global::System.EventHandler (this.OnButtonMakePaymentClicked);
			this.buttonPrint.Clicked += new global::System.EventHandler (this.OnButtonPrintClicked);
			this.treeviewServices.CursorChanged += new global::System.EventHandler (this.OnTreeviewServicesCursorChanged);
			this.buttonAddService.Clicked += new global::System.EventHandler (this.OnButtonAddServiceClicked);
			this.buttonDelService.Clicked += new global::System.EventHandler (this.OnButtonDelServiceClicked);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
