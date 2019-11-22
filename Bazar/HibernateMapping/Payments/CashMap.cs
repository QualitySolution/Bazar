using System;
using Bazar.Domain.Payments;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Payments
{
	public class CashMap : ClassMap<Cash>
	{
		public CashMap ()
		{
			Table ("cash");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
			Map (x => x.Color).Column ("color");
		}
	}
}
