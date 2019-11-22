using System;
using Bazar.Domain.Payments;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Payments
{
	public class IncomeCategoryMap : ClassMap<IncomeCategory>
	{
		public IncomeCategoryMap ()
		{
			Table ("income_items");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
		}
	}
}
