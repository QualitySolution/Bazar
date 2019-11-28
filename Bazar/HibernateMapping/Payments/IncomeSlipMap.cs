using System;
using Bazar.Domain.Payments;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Payments
{
	public class IncomeSlipMap : ClassMap<IncomeSlip>
	{
		public IncomeSlipMap()
		{
			Table("credit_slips");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
		}
	}
}
