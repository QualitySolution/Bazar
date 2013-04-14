using System;
using Gtk;

namespace bazar
{
	public partial class SelectAccrualRow : Gtk.Dialog
	{
		public ListStore AccrualRows;
		private TreeIter SelectedIter;

		public SelectAccrualRow (TreeModel AccrualRows)
		{
			this.Build ();
			
			AccrualTreeView.AppendColumn ("Услуга", new Gtk.CellRendererText (), "text", 1);
			AccrualTreeView.AppendColumn ("Начислено", new Gtk.CellRendererText (), "text", 3);
			AccrualTreeView.AppendColumn ("Уже оплачено", new Gtk.CellRendererText (), "text", 5);

			AccrualTreeView.Model = AccrualRows;
			AccrualTreeView.ShowAll();
		}

		public bool GetResult(out TreeIter iter)
		{
			bool Result = false;
			this.Show ();
			if((ResponseType) this.Run () == ResponseType.Ok)
			{
				iter = SelectedIter;
				Result = true;
			}
			else
				iter = new TreeIter();
			this.Destroy ();

			return Result;
		}

		protected void OnAccrualTreeViewCursorChanged (object sender, EventArgs e)
		{
			bool isSelect = AccrualTreeView.Selection.CountSelectedRows() == 1;
			buttonOk.Sensitive = isSelect;
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			AccrualTreeView.Selection.GetSelected(out SelectedIter);
		}

		protected void OnAccrualTreeViewRowActivated (object o, RowActivatedArgs args)
		{
			buttonOk.Click ();
		}
	}
}

