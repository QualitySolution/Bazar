using System;
using Bazar.Domain.Payments;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Payments
{
	public class PaymentItemMap : ClassMap<PaymentItem>
	{
		public PaymentItemMap()
		{
			Table("payment_details");

			Id(x => x.Id).Column("id").GeneratedBy.Native();

			Map(x => x.Sum).Column("sum").Not.Nullable();

			References(x => x.Payment).Column("payment_id").Not.Nullable();
			References(x => x.AccrualItem).Column("accrual_pay_id").Not.Nullable();
		}
	}
}
