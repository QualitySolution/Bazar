
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class Meter
	{
		private global::Gtk.Table table1;
		private global::Gtk.CheckButton checkDisabled;
		private global::Gtk.ComboBox comboMeterType;
		private global::Gtk.Entry entryName;
		private global::Gtk.HBox hbox2;
		private global::Gtk.ComboBox comboPlaceType;
		private global::Gtk.Label label10;
		private global::Gtk.ComboBox comboPlaceNo;
		private global::Gtk.HBox hbox3;
		private global::Gtk.Entry entryParent;
		private global::Gtk.Button buttonParentClean;
		private global::Gtk.Button buttonParentEdit;
		private global::Gtk.Label label4;
		private global::Gtk.Label label5;
		private global::Gtk.Label label6;
		private global::Gtk.Label label7;
		private global::Gtk.Label label8;
		private global::Gtk.Label labelID;
		private global::Gtk.Button buttonCancel;
		private global::Gtk.Button buttonOk;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget bazar.Meter
			this.Name = "bazar.Meter";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.Meter.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table (((uint)(6)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			this.table1.BorderWidth = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.checkDisabled = new global::Gtk.CheckButton ();
			this.checkDisabled.TooltipMarkup = "Счётчик больше не используется.";
			this.checkDisabled.CanFocus = true;
			this.checkDisabled.Name = "checkDisabled";
			this.checkDisabled.Label = "Отключен";
			this.checkDisabled.DrawIndicator = true;
			this.checkDisabled.UseUnderline = true;
			this.table1.Add (this.checkDisabled);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1 [this.checkDisabled]));
			w2.TopAttach = ((uint)(5));
			w2.BottomAttach = ((uint)(6));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			w2.XOptions = ((global::Gtk.AttachOptions)(4));
			w2.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.comboMeterType = new global::Gtk.ComboBox ();
			this.comboMeterType.Name = "comboMeterType";
			this.table1.Add (this.comboMeterType);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1 [this.comboMeterType]));
			w3.TopAttach = ((uint)(2));
			w3.BottomAttach = ((uint)(3));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.entryName = new global::Gtk.Entry ();
			this.entryName.TooltipMarkup = "Напишите понятное завание счётчека или укажите серийный номер.";
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
			this.hbox2 = new global::Gtk.HBox ();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.comboPlaceType = global::Gtk.ComboBox.NewText ();
			this.comboPlaceType.Name = "comboPlaceType";
			this.hbox2.Add (this.comboPlaceType);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.comboPlaceType]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.label10 = new global::Gtk.Label ();
			this.label10.Name = "label10";
			this.label10.LabelProp = "-";
			this.hbox2.Add (this.label10);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.label10]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.comboPlaceNo = global::Gtk.ComboBox.NewText ();
			this.comboPlaceNo.Name = "comboPlaceNo";
			this.hbox2.Add (this.comboPlaceNo);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.comboPlaceNo]));
			w7.Position = 2;
			w7.Expand = false;
			w7.Fill = false;
			this.table1.Add (this.hbox2);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox2]));
			w8.TopAttach = ((uint)(3));
			w8.BottomAttach = ((uint)(4));
			w8.LeftAttach = ((uint)(1));
			w8.RightAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox3 = new global::Gtk.HBox ();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.entryParent = new global::Gtk.Entry ();
			this.entryParent.CanFocus = true;
			this.entryParent.Name = "entryParent";
			this.entryParent.IsEditable = false;
			this.entryParent.InvisibleChar = '●';
			this.hbox3.Add (this.entryParent);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryParent]));
			w9.Position = 0;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonParentClean = new global::Gtk.Button ();
			this.buttonParentClean.TooltipMarkup = "Очистить";
			this.buttonParentClean.CanFocus = true;
			this.buttonParentClean.Name = "buttonParentClean";
			this.buttonParentClean.UseUnderline = true;
			// Container child buttonParentClean.Gtk.Container+ContainerChild
			global::Gtk.Alignment w10 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w11 = new global::Gtk.HBox ();
			w11.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w12 = new global::Gtk.Image ();
			w12.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Menu);
			w11.Add (w12);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w14 = new global::Gtk.Label ();
			w11.Add (w14);
			w10.Add (w11);
			this.buttonParentClean.Add (w10);
			this.hbox3.Add (this.buttonParentClean);
			global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonParentClean]));
			w18.Position = 1;
			w18.Expand = false;
			w18.Fill = false;
			// Container child hbox3.Gtk.Box+BoxChild
			this.buttonParentEdit = new global::Gtk.Button ();
			this.buttonParentEdit.CanFocus = true;
			this.buttonParentEdit.Name = "buttonParentEdit";
			this.buttonParentEdit.UseUnderline = true;
			// Container child buttonParentEdit.Gtk.Container+ContainerChild
			global::Gtk.Alignment w19 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w20 = new global::Gtk.HBox ();
			w20.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w21 = new global::Gtk.Image ();
			w21.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-edit", global::Gtk.IconSize.Menu);
			w20.Add (w21);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w23 = new global::Gtk.Label ();
			w20.Add (w23);
			w19.Add (w20);
			this.buttonParentEdit.Add (w19);
			this.hbox3.Add (this.buttonParentEdit);
			global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.buttonParentEdit]));
			w27.Position = 2;
			w27.Expand = false;
			w27.Fill = false;
			this.table1.Add (this.hbox3);
			global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox3]));
			w28.TopAttach = ((uint)(4));
			w28.BottomAttach = ((uint)(5));
			w28.LeftAttach = ((uint)(1));
			w28.RightAttach = ((uint)(2));
			w28.XOptions = ((global::Gtk.AttachOptions)(4));
			w28.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label ();
			this.label4.Name = "label4";
			this.label4.Xalign = 1F;
			this.label4.LabelProp = "Код:";
			this.table1.Add (this.label4);
			global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
			w29.XOptions = ((global::Gtk.AttachOptions)(4));
			w29.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label5 = new global::Gtk.Label ();
			this.label5.Name = "label5";
			this.label5.Xalign = 1F;
			this.label5.LabelProp = "Название<span foreground=\"red\">*</span>:";
			this.label5.UseMarkup = true;
			this.table1.Add (this.label5);
			global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
			w30.TopAttach = ((uint)(1));
			w30.BottomAttach = ((uint)(2));
			w30.XOptions = ((global::Gtk.AttachOptions)(4));
			w30.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label6 = new global::Gtk.Label ();
			this.label6.Name = "label6";
			this.label6.Xalign = 1F;
			this.label6.LabelProp = "Место<span foreground=\"red\">*</span>:";
			this.label6.UseMarkup = true;
			this.table1.Add (this.label6);
			global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table1 [this.label6]));
			w31.TopAttach = ((uint)(3));
			w31.BottomAttach = ((uint)(4));
			w31.XOptions = ((global::Gtk.AttachOptions)(4));
			w31.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label7 = new global::Gtk.Label ();
			this.label7.Name = "label7";
			this.label7.Xalign = 1F;
			this.label7.LabelProp = "Тип счётчика<span foreground=\"red\">*</span>:";
			this.label7.UseMarkup = true;
			this.table1.Add (this.label7);
			global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1 [this.label7]));
			w32.TopAttach = ((uint)(2));
			w32.BottomAttach = ((uint)(3));
			w32.XOptions = ((global::Gtk.AttachOptions)(4));
			w32.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label8 = new global::Gtk.Label ();
			this.label8.Name = "label8";
			this.label8.Xalign = 1F;
			this.label8.LabelProp = "Подключен к счётчику:";
			this.table1.Add (this.label8);
			global::Gtk.Table.TableChild w33 = ((global::Gtk.Table.TableChild)(this.table1 [this.label8]));
			w33.TopAttach = ((uint)(4));
			w33.BottomAttach = ((uint)(5));
			w33.XOptions = ((global::Gtk.AttachOptions)(4));
			w33.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.labelID = new global::Gtk.Label ();
			this.labelID.Name = "labelID";
			this.labelID.LabelProp = "не определён";
			this.table1.Add (this.labelID);
			global::Gtk.Table.TableChild w34 = ((global::Gtk.Table.TableChild)(this.table1 [this.labelID]));
			w34.LeftAttach = ((uint)(1));
			w34.RightAttach = ((uint)(2));
			w34.XOptions = ((global::Gtk.AttachOptions)(4));
			w34.YOptions = ((global::Gtk.AttachOptions)(4));
			w1.Add (this.table1);
			global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(w1 [this.table1]));
			w35.Position = 0;
			w35.Expand = false;
			w35.Fill = false;
			// Internal child bazar.Meter.ActionArea
			global::Gtk.HButtonBox w36 = this.ActionArea;
			w36.Name = "dialog1_ActionArea";
			w36.Spacing = 10;
			w36.BorderWidth = ((uint)(5));
			w36.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button ();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			// Container child buttonCancel.Gtk.Container+ContainerChild
			global::Gtk.Alignment w37 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w38 = new global::Gtk.HBox ();
			w38.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w39 = new global::Gtk.Image ();
			w39.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-cancel", global::Gtk.IconSize.Menu);
			w38.Add (w39);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w41 = new global::Gtk.Label ();
			w41.LabelProp = "О_тменить";
			w41.UseUnderline = true;
			w38.Add (w41);
			w37.Add (w38);
			this.buttonCancel.Add (w37);
			this.AddActionWidget (this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w45 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w36 [this.buttonCancel]));
			w45.Expand = false;
			w45.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button ();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			// Container child buttonOk.Gtk.Container+ContainerChild
			global::Gtk.Alignment w46 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w47 = new global::Gtk.HBox ();
			w47.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w48 = new global::Gtk.Image ();
			w48.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-ok", global::Gtk.IconSize.Menu);
			w47.Add (w48);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w50 = new global::Gtk.Label ();
			w50.LabelProp = "_OK";
			w50.UseUnderline = true;
			w47.Add (w50);
			w46.Add (w47);
			this.buttonOk.Add (w46);
			w36.Add (this.buttonOk);
			global::Gtk.ButtonBox.ButtonBoxChild w54 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w36 [this.buttonOk]));
			w54.Position = 1;
			w54.Expand = false;
			w54.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 412;
			this.DefaultHeight = 268;
			this.Show ();
			this.comboPlaceType.Changed += new global::System.EventHandler (this.OnComboPlaceTypeChanged);
			this.comboPlaceNo.Changed += new global::System.EventHandler (this.OnComboPlaceNoChanged);
			this.entryName.Changed += new global::System.EventHandler (this.OnEntryNameChanged);
			this.comboMeterType.Changed += new global::System.EventHandler (this.OnComboMeterTypeChanged);
			this.buttonOk.Clicked += new global::System.EventHandler (this.OnButtonOkClicked);
		}
	}
}
