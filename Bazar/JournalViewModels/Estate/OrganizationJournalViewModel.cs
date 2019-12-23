using Bazar.Domain.Estate;
using Bazar.ViewModels.Estate;
using NHibernate;
using NHibernate.Transform;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Project.Journal;
using QS.Project.Services;
using QS.Services;

namespace Bazar.JournalViewModels.Estate
{
	public class OrganizationJournalViewModel : EntityJournalViewModelBase<Organization, OrganizationViewModel, OrganizationJournalNode>
	{
		public OrganizationJournalViewModel(IUnitOfWorkFactory unitOfWorkFactory, IInteractiveService interactiveService, INavigationManager navigationManager, IDeleteEntityService deleteEntityService, ICurrentPermissionService currentPermissionService = null) : base(unitOfWorkFactory, interactiveService, navigationManager, deleteEntityService, currentPermissionService)
		{
			UseSlider = true;
		}

		protected override IQueryOver<Organization> ItemsQuery(IUnitOfWork uow)
		{
			OrganizationJournalNode resultAlias = null;
			return uow.Session.QueryOver<Organization>()
				.Where(GetSearchCriterion<Organization>(
					x => x.Id,
					x => x.INN,
					x => x.Name,
					x => x.JurAddress
					))
				.SelectList((list) => list
					.Select(x => x.Id).WithAlias(() => resultAlias.Id)
					.Select(x => x.INN).WithAlias(() => resultAlias.INN)
					.Select(x => x.Name).WithAlias(() => resultAlias.Name)
					.Select(x => x.JurAddress).WithAlias(() => resultAlias.Address)
				).TransformUsing(Transformers.AliasToBean<OrganizationJournalNode>());
		}
	}

	public class OrganizationJournalNode
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string INN { get; set; }
		public string Address { get; set; }
	}
}