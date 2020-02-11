using Bazar.JournalViewModels.Estate;
using Bazar.JournalViewModels.Rental;
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

			TreeViewColumnsConfigFactory.Register<OrganizationJournalViewModel>(
				() => FluentColumnsConfig<OrganizationJournalNode>.Create()
					.AddColumn("Код").AddTextRenderer(node => node.Id.ToString()).SearchHighlight()
					.AddColumn("ИНН").AddTextRenderer(node => node.INN).SearchHighlight()
					.AddColumn("Название").AddTextRenderer(node => node.Name).SearchHighlight()
					.AddColumn("Адрес").AddTextRenderer(node => node.Address).SearchHighlight()
					.Finish()
			);

			TreeViewColumnsConfigFactory.Register<LesseeJournalViewModel>(
				() => FluentColumnsConfig<LesseeJournalNode>.Create()
					.AddColumn("Код").AddTextRenderer(node => node.Id.ToString()).SearchHighlight()
					.AddColumn("ИНН").AddTextRenderer(node => node.Inn).SearchHighlight()
					.AddColumn("Название").AddTextRenderer(node => node.Name).SearchHighlight()
					.Finish()
			);
		}
	}
}