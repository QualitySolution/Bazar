using System;
using System.Collections.Generic;
using Bazar.Domain.Rental;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Rental
{
	public static class ServiceRepository
	{
		public static IList<Service> GetActiveServices(IUnitOfWork uow)
		{
			return uow.Session.QueryOver<Service>().List();
		}
	}
}
