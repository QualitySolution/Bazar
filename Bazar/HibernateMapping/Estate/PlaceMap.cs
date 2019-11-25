using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class PlaceMap : ClassMap<Place>
	{
		public PlaceMap()
		{
			Table("places");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.PlaceNumber).Column("place_no").Not.Nullable();
			Map(x => x.Area).Column("area");
			Map(x => x.Comments).Column("comments");

			References(x => x.PlaceType).Column("type_id").Not.Nullable();
			References(x => x.ContactPerson).Column("contact_person_id");
			References(x => x.Organization).Column("org_id");
		}
	}
}
