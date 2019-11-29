using System.Collections.Generic;
using Bazar.Domain.Rental;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Rental
{
	public static class AccrualRepository
	{
		public static IList<AccrualItem> GetAccrualItems(IUnitOfWork uow, int accrualId)
		{
			return uow.Session.QueryOver<AccrualItem>().Where(x => x.Accrual.Id == accrualId).List();
		}
	}
}
