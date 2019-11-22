using System;
using Bazar.Domain.Estate;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Estate
{
	public class ContactPersonMap : ClassMap<ContactPerson>
	{
		public ContactPersonMap ()
		{
			Table ("contact_persons");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

		}
	}
}
