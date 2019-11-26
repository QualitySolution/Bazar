using Bazar.JournalViewModels.Estate;
using Gamma.ColumnConfig;
using QS.Journal.GtkUI;

namespace Bazar.JournalViewModels
{
	public static class JournalsColumnsConfigs
	{
		public static void RegisterColumns()
		{
			TreeViewColumnsConfigFactory.Register<PlacesJournalViewModel>(
				() => FluentColumnsConfig<PlaceJournalNode>.Create()
					.AddColumn("Номер").AddTextRenderer(node => node.PlaceName).SearchHighlight()
					.AddColumn("Площадь").AddTextRenderer(node => node.AreaText, useMarkup: true)
					.AddColumn("Организация").AddTextRenderer(node => node.Organization)
					.AddColumn("Даты аренды").AddTextRenderer(node => node.LeaseDates)
					.AddColumn("Арендатор").AddTextRenderer(node => node.Lessee)
					.Finish()
			);
		}
	}
}