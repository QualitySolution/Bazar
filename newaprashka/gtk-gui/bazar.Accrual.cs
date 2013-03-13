
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
			this.table2 = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
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
			this.labelStatus = new global::Gtk.Label ();
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.LabelProp = "Новое";
			this.labelStatus.UseMarkup = true;
			this.table2.Add (this.labelStatus);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelStatus]));
			w20.TopAttach = ((uint)(2));
			w20.BottomAttach = ((uint)(3));
			w20.LeftAttach = ((uint)(1));
			w20.RightAttach = ((uint)(2));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			this.vbox2.Add (this.table2);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.table2]));
			w21.Position = 0;
			w21.Expand = false;
			w21.Fill = false;
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
			global::Gtk.Alignment w22 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w23 = new global::Gtk.HBox ();
			w23.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w24 = new global::Gtk.Image ();
			w24.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dnd", global::Gtk.IconSize.Menu);
			w23.Add (w24);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w26 = new global::Gtk.Label ();
			w26.LabelProp = "Внести оплату";
			w26.UseUnderline = true;
			w23.Add (w26);
			w22.Add (w23);
			this.buttonMakePayment.Add (w22);
			this.hbox3.Add (this.buttonMakePayment);
			global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonMakePayment]));
			w30.Position = 0;
			w30.Expand = false;
			w30.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonPrint = new global::Gtk.Button ();
			this.buttonPrint.CanFocus = true;
			this.buttonPrint.Name = "buttonPrint";
			this.buttonPrint.UseUnderline = true;
			// Container child buttonPrint.Gtk.Container+ContainerChild
			global::Gtk.Alignment w31 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w32 = new global::Gtk.HBox ();
			w32.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w33 = new global::Gtk.Image ();
			w33.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-print", global::Gtk.IconSize.Menu);
			w32.Add (w33);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w35 = new global::Gtk.Label ();
			w35.LabelProp = "Расчёт";
			w35.UseUnderline = true;
			w32.Add (w35);
			w31.Add (w32);
			this.buttonPrint.Add (w31);
			this.hbox3.Add (this.buttonPrint);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonPrint]));
			w39.Position = 1;
			w39.Expand = false;
			w39.Fill = false;
			this.vbox2.Add (this.hbox3);
			global::Gtk.Box.BoxChild w40 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox3]));
			w40.Position = 1;
			w40.Expand = false;
			w40.Fill = false;
			this.hbox1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
			w41.Position = 1;
			w41.Expand = false;
			w41.Fill = false;
			w1.Add (this.hbox1);
			global::Gtk.Box.BoxChild w42 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox1]));
			w42.Position = 0;
			w42.Expand = false;
			w42.Fill = false;
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
			global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
			w44.Position = 0;
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
			global::Gtk.Alignment w45 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w46 = new global::Gtk.HBox ();
			w46.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w47 = new global::Gtk.Image ();
			w47.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-add", global::Gtk.IconSize.Menu);
			w46.Add (w47);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w49 = new global::Gtk.Label ();
			w49.LabelProp = "Добавить";
			w49.UseUnderline = true;
			w46.Add (w49);
			w45.Add (w46);
			this.buttonAddService.Add (w45);
			this.hbox4.Add (this.buttonAddService);
			global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonAddService]));
			w53.Position = 0;
			w53.Expand = false;
			w53.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.buttonDelService = new global::Gtk.Button ();
			this.buttonDelService.CanFocus = true;
			this.buttonDelService.Name = "buttonDelService";
			this.buttonDelService.UseUnderline = true;
			// Container child buttonDelService.Gtk.Container+ContainerChild
			global::Gtk.Alignment w54 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w55 = new global::Gtk.HBox ();
			w55.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w56 = new global::Gtk.Image ();
			w56.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-remove", global::Gtk.IconSize.Menu);
			w55.Add (w56);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w58 = new global::Gtk.Label ();
			w58.LabelProp = "Удалить";
			w58.UseUnderline = true;
			w55.Add (w58);
			w54.Add (w55);
			this.buttonDelService.Add (w54);
			this.hbox4.Add (this.buttonDelService);
			global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.buttonDelService]));
			w62.Position = 1;
			w62.Expand = false;
			w62.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.labelSum = new global::Gtk.Label ();
			this.labelSum.Name = "labelSum";
			this.labelSum.Xalign = 1F;
			this.labelSum.LabelProp = "Сумма: 0 руб.";
			this.hbox4.Add (this.labelSum);
			global::Gtk.Box.BoxChild w63 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.labelSum]));
			w63.Position = 2;
			this.vbox3.Add (this.hbox4);
			global::Gtk.Box.BoxChild w64 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox4]));
			w64.Position = 1;
			w64.Expand = false;
			w64.Fill = false;
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
			global::Gtk.Box.BoxChild w67 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.GtkScrolledWindow1]));
			w67.Position = 0;
			// Container child vbox4.Gtk.Box+BoxChild
			this.labelIncomeSum = new global::Gtk.Label ();
			this.labelIncomeSum.Name = "labelIncomeSum";
			this.labelIncomeSum.Xalign = 1F;
			this.labelIncomeSum.LabelProp = "Сумма: 0 р.";
			this.vbox4.Add (this.labelIncomeSum);
			global::Gtk.Box.BoxChild w68 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.labelIncomeSum]));
			w68.Position = 1;
			w68.Expand = false;
			w68.Fill = false;
			this.notebook1.Add (this.vbox4);
			global::Gtk.Notebook.NotebookChild w69 = ((global::Gtk.Notebook.NotebookChild)(this.notebook1 [this.vbox4]));
			w69.Position = 1;
			// Notebook tab
			this.label12 = new global::Gtk.Label ();
			this.label12.Name = "label12";
			this.label12.LabelProp = "Оплачено";
			this.notebook1.SetTabLabel (this.vbox4, this.label12);
			this.label12.ShowAll ();
			w1.Add (this.notebook1);
			global::Gtk.Box.BoxChild w70 = ((global::Gtk.Box.BoxChild)(w1 [this.notebook1]));
			w70.Position = 1;
			// Internal child bazar.Accrual.ActionArea
			global::Gtk.HButtonBox w71 = this.ActionArea;
			w71.Name = "dialog1_ActionArea";
			w71.Spacing = 10;
			w71.BorderWidth = ((uint)(5));
			w71.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w72 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w73 = new global::Gtk.HBox ();
			w73.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w74 = new global::Gtk.Image ();
			w74.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w73.Add (w74);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w76 = new global::Gtk.Label ();
			w76.LabelProp = "О_тменить";
			w76.UseUnderline = true;
			w73.Add (w76);
			w72.Add (w73);
			this.buttonCancel.Add (w72);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w80 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w71 [this.buttonCancel]));
			w80.Expand = false;
			w80.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w81 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w82 = new global::Gtk.HBox ();
			w82.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w83 = new global::Gtk.Image ();
			w83.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w82.Add (w83);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w85 = new global::Gtk.Label ();
			w85.LabelProp = "_OK";
			w85.UseUnderline = true;
			w82.Add (w85);
			w81.Add (w82);
			this.buttonOk.Add (w81);
			w71.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w89 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w71 [this.buttonOk]));
			w89.Position = 1;
			w89.Expand = false;
			w89.Fill = false;
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
