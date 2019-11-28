using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class MeterReadingMap : ClassMap<MeterReading>
	{
		public MeterReadingMap()
		{
			Table("meter_reading");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Date).Column("date").Not.Nullable();
			Map(x => x.ReadingValue).Column("value").Not.Nullable();

			References(x => x.Meter).Column("meter_id");
			References(x => x.MeterTariff).Column("meter_tariff_id");
			References(x => x.AccrualItem).Column("accrual_pay_id");
		}
	}
}
