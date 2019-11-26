using System;
using System.Collections.Generic;
using Bazar.Domain.Rental;
using QS.DomainModel.UoW;
using NHibernate.Criterion;
using NHibernate;

namespace Bazar.Repositories.Rental
{
	public static class ContractRepository
	{
		public static IList<ContractItem> GetContractItems(IUnitOfWork uow, int contractId)
		{
			return uow.Session.QueryOver<ContractItem>().Where(x => x.Contract.Id == contractId).List();
		}

		public static IList<ContractItem> GetContractItemsOnPlace(IUnitOfWork uow, int placeId, DateTime begin, DateTime end)
		{
			Contract contractAlias = null;
			return uow.Session.QueryOver<ContractItem>()
				.Where(x => x.Place_NhOnly.Id == placeId)
				.JoinAlias(x => x.Contract, () => contractAlias)
				.WhereNot(Restrictions.LeProperty(
					Projections.SqlFunction("COALESCE", NHibernateUtil.DateTime,
						Projections.Property(() => contractAlias.CancelDate),
						Projections.Property(() => contractAlias.EndDate)),
					Projections.Constant(begin)))
				.AndNot(() => end < contractAlias.BeginDate)
				.List();
		}

	}
}
