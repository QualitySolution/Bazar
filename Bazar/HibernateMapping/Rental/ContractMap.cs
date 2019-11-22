using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class ContractMap : ClassMap<Contract>
	{
		public ContractMap ()
		{
			Table ("contracts");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Number).Column ("number").Not.Nullable ();
			Map (x => x.SignDate).Column ("sign_date");
			Map (x => x.BeginDate).Column ("start_date");
			Map (x => x.EndDate).Column ("end_date");
			Map (x => x.CancelDate).Column ("cancel_date");
			Map (x => x.PayDay).Column ("pay_day");
			Map (x => x.Comments).Column ("comments");

			References (x => x.Organization).Column ("org_id");
			References (x => x.Lessee).Column ("lessee_id");
		}
	}
}
