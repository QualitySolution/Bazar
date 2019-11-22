using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class OrganizationMap : ClassMap<Organization>
	{
		public OrganizationMap ()
		{
			Table ("organizations");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
		}
	}
}
