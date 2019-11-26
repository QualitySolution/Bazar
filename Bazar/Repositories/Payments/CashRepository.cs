using System;
using System.Collections.Generic;
using Bazar.Domain.Payments;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Payments
{
	public static class CashRepository
	{
		public static IList<Cash> GetActiveCashes(IUnitOfWork uow)
		{
			return uow.Session.QueryOver<Cash>().List();
		}
	}
}
