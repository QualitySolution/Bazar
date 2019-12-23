using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class OrganizationMap : ClassMap<Organization>
	{
		public OrganizationMap ()
		{
			Table ("organizations");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
			Map(x => x.PrintName).Column("print_name");
			Map(x => x.INN).Column("inn");
			Map(x => x.KPP).Column("kpp");
			Map(x => x.JurAddress).Column("jur_address");
			Map(x => x.Phone).Column("phone");
			Map(x => x.BankBik).Column("bank_bik");
			Map(x => x.BankName).Column("bank_name");
			Map(x => x.BankAccount).Column("bank_account");
			Map(x => x.BankCorAccount).Column("bank_cor_account");
		}
	}
}
