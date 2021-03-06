
// This file has been generated by the GUI designer. Do not modify.
namespace bazar
{
	public partial class ServiceProvidersPaymentReport
	{
		private global::Gtk.Table table1;

		private global::Gtk.HBox hbox1;

		private global::Gamma.GtkWidgets.ySpinButton yspinLastReadingDay;

		private global::Gamma.GtkWidgets.yLabel ylabel2;

		private global::Gtk.HBox hbox2;

		private global::Gtk.ComboBox comboPeriod;

		private global::Gtk.ComboBox comboYear;

		private global::Gtk.Label label1;

		private global::Gtk.Label label2;

		private global::Gtk.Label label3;

		private global::Gtk.VBox vbox3;

		private global::Gtk.RadioButton radioButtonMonth;

		private global::Gtk.RadioButton radioButtonQuarter;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget bazar.ServiceProvidersPaymentReport
			this.Name = "bazar.ServiceProvidersPaymentReport";
			this.Title = global::Mono.Unix.Catalog.GetString("Отчет по поставщикам");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child bazar.ServiceProvidersPaymentReport.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.yspinLastReadingDay = new global::Gamma.GtkWidgets.ySpinButton(1D, 28D, 1D);
			this.yspinLastReadingDay.CanFocus = true;
			this.yspinLastReadingDay.Name = "yspinLastReadingDay";
			this.yspinLastReadingDay.Adjustment.PageIncrement = 10D;
			this.yspinLastReadingDay.ClimbRate = 1D;
			this.yspinLastReadingDay.Numeric = true;
			this.yspinLastReadingDay.Value = 15D;
			this.yspinLastReadingDay.ValueAsDecimal = 0m;
			this.yspinLastReadingDay.ValueAsInt = 0;
			this.hbox1.Add(this.yspinLastReadingDay);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.yspinLastReadingDay]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.ylabel2 = new global::Gamma.GtkWidgets.yLabel();
			this.ylabel2.Name = "ylabel2";
			this.ylabel2.LabelProp = global::Mono.Unix.Catalog.GetString("следующего месяца");
			this.hbox1.Add(this.ylabel2);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.ylabel2]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			this.table1.Add(this.hbox1);
			global::Gtk.Table.TableChild w4 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox1]));
			w4.TopAttach = ((uint)(2));
			w4.BottomAttach = ((uint)(3));
			w4.LeftAttach = ((uint)(1));
			w4.RightAttach = ((uint)(2));
			w4.XOptions = ((global::Gtk.AttachOptions)(4));
			w4.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.comboPeriod = global::Gtk.ComboBox.NewText();
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Январь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Февраль"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Март"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Апрель"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Май"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Июнь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Июль"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Август"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Сентябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Октябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Ноябрь"));
			this.comboPeriod.AppendText(global::Mono.Unix.Catalog.GetString("Декабрь"));
			this.comboPeriod.Name = "comboPeriod";
			this.hbox2.Add(this.comboPeriod);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.comboPeriod]));
			w5.Position = 0;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.comboYear = global::Gtk.ComboBox.NewText();
			this.comboYear.Name = "comboYear";
			this.hbox2.Add(this.comboYear);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.comboYear]));
			w6.Position = 1;
			w6.Expand = false;
			w6.Fill = false;
			this.table1.Add(this.hbox2);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox2]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			w7.XOptions = ((global::Gtk.AttachOptions)(4));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Отчет за:");
			this.table1.Add(this.label1);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1[this.label1]));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.Xalign = 1F;
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Период:");
			this.table1.Add(this.label2);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.label2]));
			w9.TopAttach = ((uint)(1));
			w9.BottomAttach = ((uint)(2));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Показания снимаются до:");
			this.table1.Add(this.label3);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1[this.label3]));
			w10.TopAttach = ((uint)(2));
			w10.BottomAttach = ((uint)(3));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.vbox3 = new global::Gtk.VBox();
			this.vbox3.Name = "vbox3";
			this.vbox3.Spacing = 6;
			// Container child vbox3.Gtk.Box+BoxChild
			this.radioButtonMonth = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Месяц"));
			this.radioButtonMonth.CanFocus = true;
			this.radioButtonMonth.Name = "radioButtonMonth";
			this.radioButtonMonth.DrawIndicator = true;
			this.radioButtonMonth.UseUnderline = true;
			this.radioButtonMonth.Group = new global::GLib.SList(global::System.IntPtr.Zero);
			this.vbox3.Add(this.radioButtonMonth);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioButtonMonth]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox3.Gtk.Box+BoxChild
			this.radioButtonQuarter = new global::Gtk.RadioButton(global::Mono.Unix.Catalog.GetString("Квартал"));
			this.radioButtonQuarter.CanFocus = true;
			this.radioButtonQuarter.Name = "radioButtonQuarter";
			this.radioButtonQuarter.DrawIndicator = true;
			this.radioButtonQuarter.UseUnderline = true;
			this.radioButtonQuarter.Group = this.radioButtonMonth.Group;
			this.vbox3.Add(this.radioButtonQuarter);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox3[this.radioButtonQuarter]));
			w12.Position = 1;
			w12.Expand = false;
			w12.Fill = false;
			this.table1.Add(this.vbox3);
			global::Gtk.Table.TableChild w13 = ((global::Gtk.Table.TableChild)(this.table1[this.vbox3]));
			w13.LeftAttach = ((uint)(1));
			w13.RightAttach = ((uint)(2));
			w13.XOptions = ((global::Gtk.AttachOptions)(4));
			w13.YOptions = ((global::Gtk.AttachOptions)(4));
			w1.Add(this.table1);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(w1[this.table1]));
			w14.PackType = ((global::Gtk.PackType)(1));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			// Internal child bazar.ServiceProvidersPaymentReport.ActionArea
			global::Gtk.HButtonBox w15 = this.ActionArea;
			w15.Name = "dialog1_ActionArea";
			w15.Spacing = 10;
			w15.BorderWidth = ((uint)(5));
			w15.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w16 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w15[this.buttonCancel]));
			w16.Expand = false;
			w16.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w17 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w15[this.buttonOk]));
			w17.Position = 1;
			w17.Expand = false;
			w17.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 412;
			this.DefaultHeight = 190;
			this.Show();
			this.radioButtonQuarter.Toggled += new global::System.EventHandler(this.OnRadiobuttonQuarterToggled);
			this.yspinLastReadingDay.ValueChanged += new global::System.EventHandler(this.OnYspinLastReadingDayValueChanged);
			this.buttonOk.Clicked += new global::System.EventHandler(this.OnButtonOkClicked);
		}
	}
}
