using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class MeterTypeMap : ClassMap<MeterType>
	{
		public MeterTypeMap()
		{
			Table("meter_types");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Name).Column("name").Not.Nullable();
			Map(x => x.ReadingRatio).Column("reading_ratio").Not.Nullable();
		}
	}
}
