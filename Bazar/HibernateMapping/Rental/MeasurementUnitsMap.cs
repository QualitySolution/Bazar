using System;
namespace Bazar.HibernateMapping.Rental
{
	public class MeasurementUnitsMap : QS.BusinessCommon.HMap.MeasurementUnitsMap
	{
		public MeasurementUnitsMap () : base()
		{
			Table ("units");
		}
	}
}
