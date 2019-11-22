using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class PlaceTypeMap : ClassMap<PlaceType>
	{
		public PlaceTypeMap ()
		{
			Table ("place_types");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
			Map (x => x.Description).Column ("description");
		}
	}
}
