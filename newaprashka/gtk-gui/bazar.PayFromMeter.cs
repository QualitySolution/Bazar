
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class PayFromMeter
	{
		private global::Gtk.HBox hboxMessage;
		private global::Gtk.Image image499;
		private global::Gtk.Label label9;
		private global::Gtk.Label label5;
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		private global::Gtk.TreeView treeviewMeters;
		private global::Gtk.HBox hbox2;
		private global::Gtk.VBox vboxChilds;
		private global::Gtk.Label label1;
		private global::Gtk.ScrolledWindow GtkScrolledWindow1;
		private global::Gtk.TreeView treeviewChilds;
		private global::Gtk.Frame frame1;
		private global::Gtk.Alignment GtkAlignment2;
		private global::Gtk.Table table2;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Label label8;
		private global::Gtk.Label labelChilds;
		private global::Gtk.Label labelChildText;
		private global::Gtk.Label labelCount;
		private global::Gtk.Label labelSum;
		private global::Gtk.Label labelTotal;
		private global::Gtk.Label labelTotalText;
		private global::Gtk.SpinButton spinPrice;
		private global::Gtk.Label GtkLabel2;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.PayFromMeter
			this.Name = "bazar.PayFromMeter";
			this.Title = "Расчет из показаний счетчиков";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			this.Modal = true;
			// Internal child bazar.PayFromMeter.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hboxMessage = new global::Gtk.HBox ();
			this.hboxMessage.Name = "hboxMessage";
			this.hboxMessage.Spacing = 6;
			// Container child hboxMessage.Gtk.Box+BoxChild
			this.image499 = new global::Gtk.Image ();
			this.image499.Name = "image499";
			this.image499.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-warning", global::Gtk.IconSize.Dnd);
			this.hboxMessage.Add (this.image499);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hboxMessage [this.image499]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hboxMessage.Gtk.Box+BoxChild
			this.label9 = new global::Gtk.Label ();
			this.label9.Name = "label9";
			this.label9.Xalign = 0F;
			this.label9.LabelProp = "Вы не можете именять предыдущие значения счетчиков. Расчет доступен только\nдля просмотра. В этом режиме подключенные счетчики не учитываются.";
			this.hboxMessage.Add (this.label9);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hboxMessage [this.label9]));
			w3.Position = 1;
			w1.Add (this.hboxMessage);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1 [this.hboxMessage]));
			w4.Position = 0;
			w4.Expand = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 0F;
			this.label5.LabelProp = "<b>Показания счётчиков</b>";
			this.label5.UseMarkup = true;
			w1.Add (this.label5);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(w1 [this.label5]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeviewMeters = new global::Gtk.TreeView ();
			this.treeviewMeters.CanFocus = true;
			this.treeviewMeters.Name = "treeviewMeters";
			this.GtkScrolledWindow.Add (this.treeviewMeters);
			w1.Add (this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(w1 [this.GtkScrolledWindow]));
			w7.Position = 2;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.vboxChilds = new global::Gtk.VBox ();
			this.vboxChilds.Name = "vboxChilds";
			this.vboxChilds.Spacing = 6;
			// Container child vboxChilds.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label ();
			this.label1.Name = "label1";
			this.label1.Xalign = 0F;
			this.label1.LabelProp = "<b>Подключенные счетчики</b>";
			this.label1.UseMarkup = true;
			this.vboxChilds.Add (this.label1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vboxChilds [this.label1]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child vboxChilds.Gtk.Box+BoxChild
			this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
			this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
			this.treeviewChilds = new global::Gtk.TreeView ();
			this.treeviewChilds.CanFocus = true;
			this.treeviewChilds.Name = "treeviewChilds";
			this.GtkScrolledWindow1.Add (this.treeviewChilds);
			this.vboxChilds.Add (this.GtkScrolledWindow1);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vboxChilds [this.GtkScrolledWindow1]));
			w10.Position = 1;
			this.hbox2.Add (this.vboxChilds);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vboxChilds]));
			w11.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame ();
			this.frame1.Name = "frame1";
			this.frame1.BorderWidth = ((uint)(3));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment2.Name = "GtkAlignment2";
			this.GtkAlignment2.LeftPadding = ((uint)(12));
			// Container child GtkAlignment2.Gtk.Container+ContainerChild
			this.table2 = new global::Gtk.Table (((uint)(5)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			// Container child table2.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = "Расход по счетчикам:";
			this.table2.Add (this.label6);
			global::Gtk.Table.TableChild w12 = ((global::Gtk.Table.TableChild)(this.table2 [this.label6]));
			w12.XOptions = ((global::Gtk.AttachOptions)(4));
			w12.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = "Цена за единицу:";
			this.table2.Add (this.label7);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table2 [this.label7]));
			w13.TopAttach = ((uint)(3));
			w13.BottomAttach = ((uint)(4));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = "Сумма:";
			this.table2.Add (this.label8);
			global::Gtk.Table.TableChild w14 = ((global::Gtk.Table.TableChild)(this.table2 [this.label8]));
			w14.TopAttach = ((uint)(4));
			w14.BottomAttach = ((uint)(5));
			w14.XOptions = ((global::Gtk.AttachOptions)(4));
			w14.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelChilds = new global::Gtk.Label ();
			this.labelChilds.Name = "labelChilds";
			this.labelChilds.LabelProp = "label4";
			this.table2.Add (this.labelChilds);
			global::Gtk.Table.TableChild w15 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelChilds]));
			w15.TopAttach = ((uint)(1));
			w15.BottomAttach = ((uint)(2));
			w15.LeftAttach = ((uint)(1));
			w15.RightAttach = ((uint)(2));
			w15.XOptions = ((global::Gtk.AttachOptions)(4));
			w15.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelChildText = new global::Gtk.Label ();
			this.labelChildText.Name = "labelChildText";
			this.labelChildText.Xalign = 1F;
			this.labelChildText.LabelProp = "Расход подключенных:";
			this.table2.Add (this.labelChildText);
			global::Gtk.Table.TableChild w16 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelChildText]));
			w16.TopAttach = ((uint)(1));
			w16.BottomAttach = ((uint)(2));
			w16.XOptions = ((global::Gtk.AttachOptions)(4));
			w16.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelCount = new global::Gtk.Label ();
			this.labelCount.Name = "labelCount";
			this.labelCount.LabelProp = "label10";
			this.labelCount.UseMarkup = true;
			this.table2.Add (this.labelCount);
			global::Gtk.Table.TableChild w17 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelCount]));
			w17.LeftAttach = ((uint)(1));
			w17.RightAttach = ((uint)(2));
			w17.XOptions = ((global::Gtk.AttachOptions)(4));
			w17.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelSum = new global::Gtk.Label ();
			this.labelSum.Name = "labelSum";
			this.labelSum.LabelProp = "label9";
			this.labelSum.UseMarkup = true;
			this.table2.Add (this.labelSum);
			global::Gtk.Table.TableChild w18 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelSum]));
			w18.TopAttach = ((uint)(4));
			w18.BottomAttach = ((uint)(5));
			w18.LeftAttach = ((uint)(1));
			w18.RightAttach = ((uint)(2));
			w18.XOptions = ((global::Gtk.AttachOptions)(4));
			w18.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelTotal = new global::Gtk.Label ();
			this.labelTotal.Name = "labelTotal";
			this.labelTotal.LabelProp = "label5";
			this.table2.Add (this.labelTotal);
			global::Gtk.Table.TableChild w19 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelTotal]));
			w19.TopAttach = ((uint)(2));
			w19.BottomAttach = ((uint)(3));
			w19.LeftAttach = ((uint)(1));
			w19.RightAttach = ((uint)(2));
			w19.XOptions = ((global::Gtk.AttachOptions)(4));
			w19.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.labelTotalText = new global::Gtk.Label ();
			this.labelTotalText.Name = "labelTotalText";
			this.labelTotalText.Xalign = 1F;
			this.labelTotalText.LabelProp = "Итоговый расход:";
			this.table2.Add (this.labelTotalText);
			global::Gtk.Table.TableChild w20 = ((global::Gtk.Table.TableChild)(this.table2 [this.labelTotalText]));
			w20.TopAttach = ((uint)(2));
			w20.BottomAttach = ((uint)(3));
			w20.XOptions = ((global::Gtk.AttachOptions)(4));
			w20.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table2.Gtk.Table+TableChild
			this.spinPrice = new global::Gtk.SpinButton (0, 100, 0.1);
			this.spinPrice.CanFocus = true;
			this.spinPrice.Name = "spinPrice";
			this.spinPrice.Adjustment.PageIncrement = 10;
			this.spinPrice.ClimbRate = 1;
			this.spinPrice.Digits = ((uint)(2));
			this.spinPrice.Numeric = true;
			this.table2.Add (this.spinPrice);
			global::Gtk.Table.TableChild w21 = ((global::Gtk.Table.TableChild)(this.table2 [this.spinPrice]));
			w21.TopAttach = ((uint)(3));
			w21.BottomAttach = ((uint)(4));
			w21.LeftAttach = ((uint)(1));
			w21.RightAttach = ((uint)(2));
			w21.XOptions = ((global::Gtk.AttachOptions)(4));
			w21.YOptions = ((global::Gtk.AttachOptions)(4));
			this.GtkAlignment2.Add (this.table2);
			this.frame1.Add (this.GtkAlignment2);
			this.GtkLabel2 = new global::Gtk.Label ();
			this.GtkLabel2.Name = "GtkLabel2";
			this.GtkLabel2.LabelProp = "<b>Итого по счётчикам:</b>";
			this.GtkLabel2.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel2;
			this.hbox2.Add (this.frame1);
			global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.frame1]));
			w24.PackType = ((global::Gtk.PackType)(1));
			w24.Position = 1;
			w24.Expand = false;
			w24.Fill = false;
			w1.Add (this.hbox2);
			global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(w1 [this.hbox2]));
			w25.Position = 3;
			w25.Expand = false;
			w25.Fill = false;
			// Internal child bazar.PayFromMeter.ActionArea
			global::Gtk.HButtonBox w26 = this.ActionArea;
			w26.Name = "dialog1_ActionArea";
			w26.Spacing = 10;
			w26.BorderWidth = ((uint)(5));
			w26.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w27 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w28 = new global::Gtk.HBox ();
			w28.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w29 = new global::Gtk.Image ();
			w29.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w28.Add (w29);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w31 = new global::Gtk.Label ();
			w31.LabelProp = "О_тменить";
			w31.UseUnderline = true;
			w28.Add (w31);
			w27.Add (w28);
			this.buttonCancel.Add (w27);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w35 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w26 [this.buttonCancel]));
			w35.Expand = false;
			w35.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w36 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w37 = new global::Gtk.HBox ();
			w37.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w38 = new global::Gtk.Image ();
			w38.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w37.Add (w38);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w40 = new global::Gtk.Label ();
			w40.LabelProp = "_OK";
			w40.UseUnderline = true;
			w37.Add (w40);
			w36.Add (w37);
			this.buttonOk.Add (w36);
			w26.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w44 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w26 [this.buttonOk]));
			w44.Position = 1;
			w44.Expand = false;
			w44.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 619;
			this.DefaultHeight = 377;
			this.Show ();
			this.spinPrice.ValueChanged += new global::System.EventHandler (this.OnSpinPriceValueChanged);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
