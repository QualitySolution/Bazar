using System;
using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class ContractItemMap : ClassMap<ContractItem>
	{
		public ContractItemMap ()
		{
			Table ("contract_pays");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Amount).Column ("count").Not.Nullable ();
			Map (x => x.Price).Column ("price").Not.Nullable ();
			Map (x => x.MinimalSum).Column ("min_sum");

			References (x => x.Contract).Column ("contract_id").Not.Nullable();
			References (x => x.Service).Column ("service_id").Not.Nullable ();
			References (x => x.Place).Column ("place_id");
			References (x => x.Cash).Column ("cash_id").Not.Nullable ();
		}
	}
}
