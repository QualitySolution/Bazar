using System;
using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class ServiceMap : ClassMap<Service>
	{
		public ServiceMap ()
		{
			Table ("services");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
			Map (x => x.DependOnArea).Column ("by_area");
			Map (x => x.IncompleteMonth).Column ("incomplete_month");
			Map(x => x.PlaceSet).Column("place_set").CustomType<PlaceSetForService>();
			Map(x => x.PlaceOccupy).Column("place_occupy");

			References (x => x.Units).Column ("units_id");
			References (x => x.IncomeCategory).Column ("income_id");
			References (x => x.ServiceProvider).Column ("service_provider_id");
		}
	}
}
