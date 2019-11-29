using System;
using System.Collections.Generic;
using System.Linq;
using Bazar.Domain.Estate;
using NHibernate;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Estate
{
	public class CachedMetersRepository
	{
		List<MeterForPlace> cashedResults = new List<MeterForPlace>();
		IUnitOfWork UoW;

		public CachedMetersRepository(IUnitOfWork uoW)
		{
			UoW = uoW;
		}

		public int MeterCount(int serviceId, int placeId)
		{
			var cache = cashedResults.FirstOrDefault(x => x.ServiceId == serviceId && x.PlaceId == placeId);
			if(cache != null)
				return cache.MeterCount;

			MeterType meterTypeAlias = null;
			MeterTariff meterTariffAlias = null;

			var meterCount = UoW.Session.QueryOver<Meter>()
				.Where(x => x.Place.Id == placeId)
				.Where(x => !x.Disabled)
				.JoinAlias(x => x.MeterType, () => meterTypeAlias)
				.JoinEntityAlias(() => meterTariffAlias, () => meterTariffAlias.MeterType.Id == meterTypeAlias.Id && meterTariffAlias.Service.Id == serviceId)
				.ToRowCountQuery().SingleOrDefault<int>();

			var answer = new MeterForPlace {
				ServiceId = serviceId,
				PlaceId = placeId,
				MeterCount = meterCount
			};
			cashedResults.Add(answer);
			return answer.MeterCount;
		}
	}

	internal class MeterForPlace
	{
		public int ServiceId;
		public int PlaceId;
		public int MeterCount;
	}
}
