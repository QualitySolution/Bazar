using System;
using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class AccrualItemMap : ClassMap<AccrualItem>
	{
		public AccrualItemMap()
		{
			Table ("accrual_pays");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Amount).Column ("count").Not.Nullable ();
			Map (x => x.Price).Column ("price").Not.Nullable ();

			References (x => x.Accrual).Column ("accrual_id").Not.Nullable();
			References (x => x.Service_NhOnly).Column ("service_id").Not.Nullable ();
			References (x => x.Place_NhOnly).Column ("place_id");
			References (x => x.Cash).Column ("cash_id").Not.Nullable ();
		}
	}
}
