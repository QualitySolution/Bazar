using System;
using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class ServiceProviderMap : ClassMap<ServiceProvider>
	{
		public ServiceProviderMap ()
		{
			Table ("service_providers");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
		}
	}
}
