
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class lessee
	{
		private global::Gtk.VBox vbox2;
		private global::Gtk.HPaned hpaned1;
		private global::Gtk.VBox vbox3;
		private global::Gtk.Frame frame1;
		private global::Gtk.Alignment GtkAlignment2;
		private global::Gtk.Table table1;
		private global::Gtk.Entry entryAddress;
		private global::Gtk.Entry entryINN;
		private global::Gtk.Entry entryName;
		private global::Gtk.Entry entryOGRN;
		private global::Gtk.Label label1;
		private global::Gtk.Label label2;
		private global::Gtk.Label label3;
		private global::Gtk.Label label4;
		private global::Gtk.Label label5;
		private global::Gtk.Label labelID;
		private global::Gtk.Label GtkLabel2;
		private global::Gtk.Frame frame3;
		private global::Gtk.Alignment GtkAlignment4;
		private global::Gtk.VBox vbox5;
		private global::Gtk.HBox hbox1;
		private global::Gtk.CheckButton checkBwholesaler;
		private global::Gtk.CheckButton checkBretail;
		private global::Gtk.HBox hbox2;
		private global::Gtk.Label label10;
		private global::Gtk.Entry entryGoods;
		private global::Gtk.Button buttonGoodsClean;
		private global::Gtk.Button buttonGoodsEdit;
		private global::Gtk.Label GtkLabel5;
		private global::Gtk.VBox vbox4;
		private global::Gtk.Frame frame2;
		private global::Gtk.Alignment GtkAlignment3;
		private global::Gtk.Table table2;
		private global::Gtk.Entry entryExit;
		private global::Gtk.Entry entryFIO;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Entry entryPassSer;
		private global::Gtk.Entry entryPassNo;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Label label9;
		private global::Gtk.Label GtkLabel6;
		private global::Gtk.Frame frame4;
		private global::Gtk.Alignment GtkAlignment5;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TextView textviewComments;
		private global::Gtk.Label GtkLabel7;
		private global::Gtk.HBox hbox4;
		private global::Gtk.Label label8;
		private global::Gtk.CheckButton checkActiveContracts;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.TreeView treeviewContracts;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.lessee
			this.Name = "bazar.lessee";
			this.Title = "Новый арендатор";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.lessee.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hpaned1 = new global::Gtk.HPaned ();
			this.hpaned1.CanFocus = true;
			this.hpaned1.Name = "hpaned1";
			this.hpaned1.Position = 388;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.vbox3 = new global::Gtk.VBox ();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame ();
			this.frame1.Name = "frame1";
			this.frame1.BorderWidth = ((uint)(3));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(5)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(3));
			// Container child table1.Gtk.Table+TableChild
			this.entryAddress = new global::Gtk.Entry ();
			this.entryAddress.CanFocus = true;
			this.entryAddress.Name = "entryAddress";
			this.entryAddress.IsEditable = true;
			this.entryAddress.InvisibleChar = '●';
			this.table1.Add (this.entryAddress);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryAddress]));
			w2.TopAttach = ((uint)(2));
			w2.BottomAttach = ((uint)(3));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryINN = new global::Gtk.Entry ();
			this.entryINN.CanFocus = true;
			this.entryINN.Name = "entryINN";
			this.entryINN.IsEditable = true;
			this.entryINN.MaxLength = 12;
			this.entryINN.InvisibleChar = '●';
			this.table1.Add (this.entryINN);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryINN]));
			w3.TopAttach = ((uint)(3));
			w3.BottomAttach = ((uint)(4));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryName = new global::Gtk.Entry ();
			this.entryName.CanFocus = true;
			this.entryName.Name = "entryName";
			this.entryName.IsEditable = true;
			this.entryName.InvisibleChar = '●';
			this.table1.Add (this.entryName);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryName]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryOGRN = new global::Gtk.Entry ();
			this.entryOGRN.CanFocus = true;
			this.entryOGRN.Name = "entryOGRN";
			this.entryOGRN.IsEditable = true;
			this.entryOGRN.MaxLength = 15;
			this.entryOGRN.InvisibleChar = '●';
			this.table1.Add (this.entryOGRN);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryOGRN]));
			w5.TopAttach = ((uint)(4));
			w5.BottomAttach = ((uint)(5));
			w5.LeftAttach = ((uint)(1));
			w5.RightAttach = ((uint)(2));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = "Код:";
			this.table1.Add (this.label1);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1 [this.label1]));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label ();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = "Название<span foreground=\"red\">*</span>:";
			this.label2.UseMarkup = true;
			this.table1.Add (this.label2);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label ();
			this.label3.Name = "label3";
			this.label3.Xalign = 1F;
			this.label3.LabelProp = "Юр. адрес:";
			this.table1.Add (this.label3);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
			w8.TopAttach = ((uint)(2));
			w8.BottomAttach = ((uint)(3));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = "ИНН:";
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w9.TopAttach = ((uint)(3));
			w9.BottomAttach = ((uint)(4));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = "ОГРН:";
			this.table1.Add (this.label5);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
			w10.TopAttach = ((uint)(4));
			w10.BottomAttach = ((uint)(5));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelID = new global::Gtk.Label ();
			this.labelID.Name = "labelID";
			this.labelID.LabelProp = "Не определён";
			this.labelID.Selectable = true;
			this.table1.Add (this.labelID);
			global::Gtk.Table.TableChild w11 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelID]));
			w11.LeftAttach = ((uint)(1));
			w11.RightAttach = ((uint)(2));
			w11.XOptions = ((global::Gtk.AttachOptions)(4));
			w11.YOptions = ((global::Gtk.AttachOptions)(4));
			this.GtkAlignment2.Add (this.table1);
			this.frame1.Add (this.GtkAlignment2);
			this.GtkLabel2 = new global::Gtk.Label ();
			this.GtkLabel2.Name = "GtkLabel2";
			this.GtkLabel2.LabelProp = "<b>Арендатор</b>";
			this.GtkLabel2.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel2;
			this.vbox3.Add (this.frame1);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame1]));
			w14.Position = 0;
			w14.Expand = false;
			w14.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.frame3 = new global::Gtk.Frame ();
			this.frame3.Name = "frame3";
			this.frame3.BorderWidth = ((uint)(3));
			// Container child frame3.Gtk.Container+ContainerChild
			this.GtkAlignment4 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment4.Name = "GtkAlignment4";
			this.GtkAlignment4.LeftPadding = ((uint)(12));
			// Container child GtkAlignment4.Gtk.Container+ContainerChild
			this.vbox5 = new global::Gtk.VBox ();
			this.vbox5.Name = "vbox5";
			this.vbox5.Spacing = 6;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.checkBwholesaler = new global::Gtk.CheckButton ();
			this.checkBwholesaler.CanFocus = true;
			this.checkBwholesaler.Name = "checkBwholesaler";
			this.checkBwholesaler.Label = "Оптовая торговля";
			this.checkBwholesaler.DrawIndicator = true;
			this.checkBwholesaler.UseUnderline = true;
			this.hbox1.Add (this.checkBwholesaler);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.checkBwholesaler]));
			w15.Position = 0;
			// Container child hbox1.Gtk.Box+BoxChild
			this.checkBretail = new global::Gtk.CheckButton ();
			this.checkBretail.CanFocus = true;
			this.checkBretail.Name = "checkBretail";
			this.checkBretail.Label = "Розница";
			this.checkBretail.DrawIndicator = true;
			this.checkBretail.UseUnderline = true;
			this.hbox1.Add (this.checkBretail);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.checkBretail]));
			w16.Position = 1;
			this.vbox5.Add (this.hbox1);
			global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.hbox1]));
			w17.Position = 0;
			w17.Expand = false;
			w17.Fill = false;
			// Container child vbox5.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			this.hbox2.BorderWidth = ((uint)(3));
			// Container child hbox2.Gtk.Box+BoxChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.LabelProp = "Группа товаров:";
			this.hbox2.Add (this.label10);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label10]));
			w18.Position = 0;
			w18.Expand = false;
			w18.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.entryGoods = new global::Gtk.Entry ();
			this.entryGoods.CanFocus = true;
			this.entryGoods.Name = "entryGoods";
			this.entryGoods.Text = "не указана";
			this.entryGoods.IsEditable = false;
			this.entryGoods.InvisibleChar = '•';
			this.hbox2.Add (this.entryGoods);
			global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.entryGoods]));
			w19.Position = 1;
			// Container child hbox2.Gtk.Box+BoxChild
			this.buttonGoodsClean = new global::Gtk.Button ();
			this.buttonGoodsClean.TooltipMarkup = "Убрать группу товаров";
			this.buttonGoodsClean.CanFocus = true;
			this.buttonGoodsClean.Name = "buttonGoodsClean";
			this.buttonGoodsClean.UseUnderline = true;
			global::Gtk.Image w20 = new global::Gtk.Image ();
			w20.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Button);
			this.buttonGoodsClean.Image = w20;
			this.hbox2.Add (this.buttonGoodsClean);
			global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.buttonGoodsClean]));
			w21.Position = 2;
			w21.Expand = false;
			w21.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.buttonGoodsEdit = new global::Gtk.Button ();
			this.buttonGoodsEdit.TooltipMarkup = "Выбрать группу товаров";
			this.buttonGoodsEdit.CanFocus = true;
			this.buttonGoodsEdit.Name = "buttonGoodsEdit";
			this.buttonGoodsEdit.UseUnderline = true;
			global::Gtk.Image w22 = new global::Gtk.Image ();
			w22.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Button);
			this.buttonGoodsEdit.Image = w22;
			this.hbox2.Add (this.buttonGoodsEdit);
			global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.buttonGoodsEdit]));
			w23.Position = 3;
			w23.Expand = false;
			w23.Fill = false;
			this.vbox5.Add (this.hbox2);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.hbox2]));
			w24.Position = 1;
			w24.Expand = false;
			w24.Fill = false;
			this.GtkAlignment4.Add (this.vbox5);
			this.frame3.Add (this.GtkAlignment4);
			this.GtkLabel5 = new global::Gtk.Label ();
			this.GtkLabel5.Name = "GtkLabel5";
			this.GtkLabel5.LabelProp = "<b>Класификация</b>";
			this.GtkLabel5.UseMarkup = true;
			this.frame3.LabelWidget = this.GtkLabel5;
			this.vbox3.Add (this.frame3);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.frame3]));
			w27.Position = 1;
			w27.Expand = false;
			w27.Fill = false;
			this.hpaned1.Add (this.vbox3);
			global::Gtk.Paned.PanedChild w28 = ((global::Gtk.Paned.PanedChild)(this.hpaned1 [this.vbox3]));
			w28.Resize = false;
			// Container child hpaned1.Gtk.Paned+PanedChild
			this.vbox4 = new global::Gtk.VBox ();
			this.vbox4.Name = "vbox4";
			this.vbox4.Spacing = 6;
			// Container child vbox4.Gtk.Box+BoxChild
			this.frame2 = new global::Gtk.Frame ();
			this.frame2.ExtensionEvents = ((global::Gdk.ExtensionMode)(2));
			this.frame2.Name = "frame2";
			this.frame2.BorderWidth = ((uint)(3));
			// Container child frame2.Gtk.Container+ContainerChild
			this.GtkAlignment3 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment3.Name = "GtkAlignment3";
			this.GtkAlignment3.LeftPadding = ((uint)(12));
			// Container child GtkAlignment3.Gtk.Container+ContainerChild
			this.table2 = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(3));
			// Container child table2.Gtk.Table+TableChild
			this.entryExit = new global::Gtk.Entry ();
			this.entryExit.CanFocus = true;
			this.entryExit.Name = "entryExit";
			this.entryExit.IsEditable = true;
			this.entryExit.InvisibleChar = '●';
			this.table2.Add (this.entryExit);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryExit]));
			w29.TopAttach = ((uint)(2));
			w29.BottomAttach = ((uint)(3));
			w29.LeftAttach = ((uint)(1));
			w29.RightAttach = ((uint)(2));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.entryFIO = new global::Gtk.Entry ();
			this.entryFIO.CanFocus = true;
			this.entryFIO.Name = "entryFIO";
			this.entryFIO.IsEditable = true;
			this.entryFIO.InvisibleChar = '●';
			this.table2.Add (this.entryFIO);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table2 [this.entryFIO]));
			w30.LeftAttach = ((uint)(1));
			w30.RightAttach = ((uint)(2));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.entryPassSer = new global::Gtk.Entry ();
			this.entryPassSer.CanFocus = true;
			this.entryPassSer.Name = "entryPassSer";
			this.entryPassSer.IsEditable = true;
			this.entryPassSer.WidthChars = 5;
			this.entryPassSer.MaxLength = 5;
			this.entryPassSer.InvisibleChar = '●';
			this.hbox3.Add (this.entryPassSer);
			global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryPassSer]));
			w31.Position = 0;
			// Container child hbox3.Gtk.Box+BoxChild
			this.entryPassNo = new global::Gtk.Entry ();
			this.entryPassNo.CanFocus = true;
			this.entryPassNo.Name = "entryPassNo";
			this.entryPassNo.IsEditable = true;
			this.entryPassNo.MaxLength = 6;
			this.entryPassNo.InvisibleChar = '●';
			this.hbox3.Add (this.entryPassNo);
			global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryPassNo]));
			w32.Position = 1;
			this.table2.Add (this.hbox3);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table2 [this.hbox3]));
			w33.TopAttach = ((uint)(1));
			w33.BottomAttach = ((uint)(2));
			w33.LeftAttach = ((uint)(1));
			w33.RightAttach = ((uint)(2));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = "Ф.И.О.:";
			this.table2.Add (this.label6);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table2 [this.label6]));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = "Паспорт:";
			this.table2.Add (this.label7);
			global::Gtk.Table.TableChild w35 = ((global::Gtk.Table.TableChild)(this.table2 [this.label7]));
			w35.TopAttach = ((uint)(1));
			w35.BottomAttach = ((uint)(2));
			w35.XOptions = ((global::Gtk.AttachOptions)(4));
			w35.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xalign = 1F;
			this.label9.LabelProp = "Выдан:";
			this.table2.Add (this.label9);
			global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table2 [this.label9]));
			w36.TopAttach = ((uint)(2));
			w36.BottomAttach = ((uint)(3));
			w36.XOptions = ((global::Gtk.AttachOptions)(4));
			w36.YOptions = ((global::Gtk.AttachOptions)(4));
			this.GtkAlignment3.Add (this.table2);
			this.frame2.Add (this.GtkAlignment3);
			this.GtkLabel6 = new global::Gtk.Label ();
			this.GtkLabel6.Name = "GtkLabel6";
			this.GtkLabel6.LabelProp = "<b>Директор</b>";
			this.GtkLabel6.UseMarkup = true;
			this.frame2.LabelWidget = this.GtkLabel6;
			this.vbox4.Add (this.frame2);
			global::Gtk.Box.BoxChild w39 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.frame2]));
			w39.Position = 0;
			w39.Expand = false;
			w39.Fill = false;
			// Container child vbox4.Gtk.Box+BoxChild
			this.frame4 = new global::Gtk.Frame ();
			this.frame4.Name = "frame4";
			this.frame4.BorderWidth = ((uint)(3));
			// Container child frame4.Gtk.Container+ContainerChild
			this.GtkAlignment5 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment5.Name = "GtkAlignment5";
			this.GtkAlignment5.LeftPadding = ((uint)(12));
			// Container child GtkAlignment5.Gtk.Container+ContainerChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			this.GtkScrolledWindow.BorderWidth = ((uint)(6));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.textviewComments = new global::Gtk.TextView ();
			this.textviewComments.CanFocus = true;
			this.textviewComments.Name = "textviewComments";
			this.textviewComments.WrapMode = ((global::Gtk.WrapMode)(2));
			this.GtkScrolledWindow.Add (this.textviewComments);
			this.GtkAlignment5.Add (this.GtkScrolledWindow);
			this.frame4.Add (this.GtkAlignment5);
			this.GtkLabel7 = new global::Gtk.Label ();
			this.GtkLabel7.Name = "GtkLabel7";
			this.GtkLabel7.LabelProp = "<b>Комментарии</b>";
			this.GtkLabel7.UseMarkup = true;
			this.frame4.LabelWidget = this.GtkLabel7;
			this.vbox4.Add (this.frame4);
			global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.frame4]));
			w43.Position = 1;
			this.hpaned1.Add (this.vbox4);
			this.vbox2.Add (this.hpaned1);
			global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hpaned1]));
			w45.Position = 0;
			w45.Expand = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox ();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.LabelProp = "<b>Договора</b>";
			this.label8.UseMarkup = true;
			this.hbox4.Add (this.label8);
			global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.label8]));
			w46.Position = 0;
			w46.Expand = false;
			w46.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.checkActiveContracts = new global::Gtk.CheckButton ();
			this.checkActiveContracts.CanFocus = true;
			this.checkActiveContracts.Name = "checkActiveContracts";
			this.checkActiveContracts.Label = "Показывать только активные";
			this.checkActiveContracts.Active = true;
			this.checkActiveContracts.DrawIndicator = true;
			this.checkActiveContracts.UseUnderline = true;
			this.hbox4.Add (this.checkActiveContracts);
			global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.hbox4 [this.checkActiveContracts]));
			w47.Position = 1;
			this.vbox2.Add (this.hbox4);
			global::Gtk.Box.BoxChild w48 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbox4]));
			w48.Position = 1;
			w48.Expand = false;
			w48.Fill = false;
			// Container child vbox2.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewContracts = new global::Gtk.TreeView ();
			this.treeviewContracts.CanFocus = true;
			this.treeviewContracts.Name = "treeviewContracts";
			this.GtkScrolledWindow1.Add (this.treeviewContracts);
			this.vbox2.Add (this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow1]));
			w50.Position = 2;
			w1.Add (this.vbox2);
			global::Gtk.Box.BoxChild w51 = ((global::Gtk.Box.BoxChild)(w1 [this.vbox2]));
			w51.Position = 0;
			// Internal child bazar.lessee.ActionArea
			global::Gtk.HButtonBox w52 = this.ActionArea;
			w52.Name = "dialog1_ActionArea";
			w52.Spacing = 10;
			w52.BorderWidth = ((uint)(5));
			w52.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "О_тменить";
			global::Gtk.Image w53 = new global::Gtk.Image ();
			w53.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w53;
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w54 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w52 [this.buttonCancel]));
			w54.Expand = false;
			w54.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "_OK";
			global::Gtk.Image w55 = new global::Gtk.Image ();
			w55.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			this.buttonOk.Image = w55;
			w52.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w56 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w52 [this.buttonOk]));
			w56.Position = 1;
			w56.Expand = false;
			w56.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 806;
			this.DefaultHeight = 575;
			this.Show ();
			this.entryName.Changed += new global::System.EventHandler (this.OnEntryNameChanged);
			this.buttonGoodsClean.Clicked += new global::System.EventHandler (this.OnButtonGoodsCleanClicked);
			this.buttonGoodsEdit.Clicked += new global::System.EventHandler (this.OnButtonGoodsEditClicked);
			this.checkActiveContracts.Toggled += new global::System.EventHandler (this.OnCheckActiveContractsToggled);
			this.treeviewContracts.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.OnTreeviewContractsButtonPressEvent);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
