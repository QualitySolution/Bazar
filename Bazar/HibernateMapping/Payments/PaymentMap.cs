using System;
using Bazar.Domain.Payments;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Payments
{
	public class PaymentMap : ClassMap<Payment>
	{
		public PaymentMap()
		{
			Table("payments");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.CreateDate).Column("createdate");

			References(x => x.IncomeSlip).Column("credit_slip_id");
			References(x => x.Accrual).Column("accrual_id");
		}
	}
}
