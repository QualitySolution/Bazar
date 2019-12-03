using System;
using Bazar.Domain.Estate;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Estate
{
	public static class PlaceRepository
	{
		public static int? GetPlaceId(IUnitOfWork uow, int type_id, string number)
		{
			return uow.Session.QueryOver<Place>()
				.Where(x => x.PlaceType.Id == type_id && x.PlaceNumber == number)
				.Select(x => x.Id).SingleOrDefault<int?>();
		}
	}
}
