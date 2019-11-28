using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class MeterTariffMap : ClassMap<MeterTariff>
	{
		public MeterTariffMap()
		{
			Table("meter_tariffs");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Name).Column("name").Not.Nullable();

			References(x => x.MeterType).Column("meter_type_id");
			References(x => x.Service).Column("service_id");
		}
	}
}
