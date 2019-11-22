using Bazar.Domain.Rental;
using FluentNHibernate.Mapping;

namespace Bazar.HibernateMapping.Rental
{
	public class LesseeMap : ClassMap<Lessee>
	{
		public LesseeMap ()
		{
			Table ("lessees");

			Id (x => x.Id).Column ("id").GeneratedBy.Native ();

			Map (x => x.Name).Column ("name").Not.Nullable ();
			Map (x => x.DirectorFio).Column ("FIO_dir");
			Map (x => x.DirectorPassportSeries).Column ("passport_ser");
			Map (x => x.DirectorPassportNumber).Column ("passport_no");
			Map (x => x.DirectorPassportExit).Column ("passport_exit");
			Map (x => x.Address).Column ("address");
			Map (x => x.Inn).Column ("INN");
			Map (x => x.Kpp).Column ("KPP");
			Map (x => x.Ogrn).Column ("OGRN");
			Map (x => x.Wholesaler).Column ("wholesaler");
			Map (x => x.Retail).Column ("retail");
			Map (x => x.Comments).Column ("comments");
		}
	}
}
