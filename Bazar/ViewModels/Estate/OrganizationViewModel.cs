using Bazar.Domain.Estate;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Services;
using QS.ViewModels;

namespace Bazar.ViewModels.Estate
{
	public class OrganizationViewModel : EntityTabViewModelBase<Organization>
	{
		public OrganizationViewModel(IEntityUoWBuilder uowBuilder, IUnitOfWorkFactory unitOfWorkFactory, ICommonServices commonServices) : base(uowBuilder, unitOfWorkFactory, commonServices)
		{
		}
	}
}
