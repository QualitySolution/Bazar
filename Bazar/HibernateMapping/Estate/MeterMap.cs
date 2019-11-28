using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class MeterMap : ClassMap<Meter>
	{
		public MeterMap()
		{
			Table("meters");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Name).Column("name").Not.Nullable();
			Map(x => x.Disabled).Column("disabled").Not.Nullable();

			References(x => x.MeterType).Column("meter_type_id");
			References(x => x.Place).Column("place_id");
			References(x => x.ParentMeter).Column("parent_meter_id");
		}
	}
}
