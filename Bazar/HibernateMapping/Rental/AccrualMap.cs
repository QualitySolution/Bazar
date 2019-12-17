using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class AccrualMap : ClassMap<Accrual>
	{
		public AccrualMap()
		{
			Table ("accrual");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map(x => x.Date).Column("date");
			Map(x => x.InvoiceNumber).Column("invoice_number");
			Map (x => x.Month).Column ("month");
			Map (x => x.Year).Column ("year");
			Map (x => x.Paid).Column ("paid");
			Map (x => x.NotComplete).Column ("no_complete");
			Map (x => x.Comments).Column ("comments");

			References (x => x.Contract).Column ("contract_id");
			References (x => x.User).Column ("user_id");

			HasMany(x => x.Items)
				.Inverse()
				.KeyColumn("accrual_id").Not.KeyNullable()
				.Cascade.AllDeleteOrphan().Inverse()
				.LazyLoad();
		}
	}
}
