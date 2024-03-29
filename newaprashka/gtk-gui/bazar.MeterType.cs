
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class MeterType
	{
		private global::Gtk.Table table1;

		private global::Gtk.Entry entryName;

		private global::Gtk.Label label1;

		private global::Gtk.Label label2;

		private global::Gtk.Label label4;

		private global::Gtk.Label labelID;

		private global::Gamma.GtkWidgets.ySpinButton yspinKilowattFactor;

		private global::Gtk.Label label3;

		private global::Gtk.ScrolledWindow GtkScrolledWindow;

		private global::Gtk.TreeView treeviewTariff;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Button buttonAdd;

		private global::Gtk.Button buttonDelete;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget bazar.MeterType
			this.Name = "bazar.MeterType";
			this.Title = global::Mono.Unix.Catalog.GetString("Тип счетчика");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.MeterType.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.entryName = new global::Gtk.Entry();
			this.entryName.CanFocus = true;
			this.entryName.Name = "entryName";
			this.entryName.IsEditable = true;
			this.entryName.MaxLength = 45;
			this.entryName.InvisibleChar = '●';
			this.table1.Add(this.entryName);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.entryName]));
			w2.TopAttach = ((uint)(1));
			w2.BottomAttach = ((uint)(2));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.Xalign = 1F;
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Код:");
			this.table1.Add(this.label1);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.label1]));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Название<span foreground=\"red\">*</span>:");
			this.label2.UseMarkup = true;
			this.table1.Add(this.label2);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.label2]));
			w4.TopAttach = ((uint)(1));
			w4.BottomAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Коэффициент показаний счетчика:");
			this.table1.Add(this.label4);
			global::Gtk.Table.TableChild w5 = ((global::Gtk.Table.TableChild)(this.table1[this.label4]));
			w5.TopAttach = ((uint)(2));
			w5.BottomAttach = ((uint)(3));
			w5.XOptions = ((global::Gtk.AttachOptions)(4));
			w5.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelID = new global::Gtk.Label();
			this.labelID.Name = "labelID";
			this.labelID.LabelProp = global::Mono.Unix.Catalog.GetString("не определен");
			this.table1.Add(this.labelID);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table1[this.labelID]));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.yspinKilowattFactor = new global::Gamma.GtkWidgets.ySpinButton(0D, 100D, 1D);
			this.yspinKilowattFactor.CanFocus = true;
			this.yspinKilowattFactor.Name = "yspinKilowattFactor";
			this.yspinKilowattFactor.Adjustment.PageIncrement = 10D;
			this.yspinKilowattFactor.ClimbRate = 1D;
			this.yspinKilowattFactor.Digits = ((uint)(3));
			this.yspinKilowattFactor.Numeric = true;
			this.yspinKilowattFactor.Value = 1D;
			this.yspinKilowattFactor.ValueAsDecimal = 0m;
			this.yspinKilowattFactor.ValueAsInt = 1;
			this.table1.Add(this.yspinKilowattFactor);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.yspinKilowattFactor]));
			w7.TopAttach = ((uint)(2));
			w7.BottomAttach = ((uint)(3));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			w1.Add(this.table1);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(w1[this.table1]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.Xalign = 0F;
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Тарифы счётчика</b>");
			this.label3.UseMarkup = true;
			w1.Add(this.label3);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(w1[this.label3]));
			w9.Position = 1;
			w9.Expand = false;
			w9.Fill = false;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			this.GtkScrolledWindow.BorderWidth = ((uint)(3));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.treeviewTariff = new global::Gtk.TreeView();
			this.treeviewTariff.CanFocus = true;
			this.treeviewTariff.Name = "treeviewTariff";
			this.GtkScrolledWindow.Add(this.treeviewTariff);
			w1.Add(this.GtkScrolledWindow);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(w1[this.GtkScrolledWindow]));
			w11.Position = 2;
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.hbox1.BorderWidth = ((uint)(3));
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonAdd = new global::Gtk.Button();
			this.buttonAdd.CanFocus = true;
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.UseUnderline = true;
			this.buttonAdd.Label = global::Mono.Unix.Catalog.GetString("Добавить");
			global::Gtk.Image w12 = new global::Gtk.Image();
			w12.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-add", global::Gtk.IconSize.Menu);
			this.buttonAdd.Image = w12;
			this.hbox1.Add(this.buttonAdd);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonAdd]));
			w13.Position = 0;
			w13.Expand = false;
			w13.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.buttonDelete = new global::Gtk.Button();
			this.buttonDelete.CanFocus = true;
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.UseUnderline = true;
			this.buttonDelete.Label = global::Mono.Unix.Catalog.GetString("Удалить");
			global::Gtk.Image w14 = new global::Gtk.Image();
			w14.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-remove", global::Gtk.IconSize.Menu);
			this.buttonDelete.Image = w14;
			this.hbox1.Add(this.buttonDelete);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.buttonDelete]));
			w15.Position = 1;
			w15.Expand = false;
			w15.Fill = false;
			w1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(w1[this.hbox1]));
			w16.Position = 3;
			w16.Expand = false;
			w16.Fill = false;
			// Internal child bazar.MeterType.ActionArea
			global::Gtk.HButtonBox w17 = this.ActionArea;
			w17.Name = "dialog1_ActionArea";
			w17.Spacing = 10;
			w17.BorderWidth = ((uint)(5));
			w17.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = global::Mono.Unix.Catalog.GetString("О_тменить");
			global::Gtk.Image w18 = new global::Gtk.Image();
			w18.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-cancel", global::Gtk.IconSize.Menu);
			this.buttonCancel.Image = w18;
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w19 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w17[this.buttonCancel]));
			w19.Expand = false;
			w19.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = global::Mono.Unix.Catalog.GetString("_OK");
			global::Gtk.Image w20 = new global::Gtk.Image();
			w20.Pixbuf = global::Stetic.IconLoader.LoadIcon(this, "gtk-ok", global::Gtk.IconSize.Menu);
			this.buttonOk.Image = w20;
			w17.Add(this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w21 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w17[this.buttonOk]));
			w21.Position = 1;
			w21.Expand = false;
			w21.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 490;
			this.DefaultHeight = 334;
			this.Show();
			this.entryName.Changed += new global::System.EventHandler(this.OnEntryNameChanged);
			this.treeviewTariff.CursorChanged += new global::System.EventHandler(this.OnTreeviewTariffCursorChanged);
			this.buttonAdd.Clicked += new global::System.EventHandler(this.OnButtonAddClicked);
			this.buttonDelete.Clicked += new global::System.EventHandler(this.OnButtonDeleteClicked);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
