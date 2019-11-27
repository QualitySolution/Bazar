using System;
using FluentNHibernate.Mapping;
using QS.Project.Domain;

namespace Bazar.HibernateMapping.Application
{
	public class UserMap : ClassMap<UserBase>
	{
		public UserMap()
		{
			Table("users");

			Id(x => x.Id).Column("id").GeneratedBy.Native();
			Map(x => x.Name).Column("name");
			Map(x => x.Login).Column("login");
		}
	}
}
