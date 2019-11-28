using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Rental;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative(Gender = GrammaticalGender.Masculine,
		NominativePlural = "показания счётчика",
		Nominative = "показание счётчика"
	)]
	public class MeterReading : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private DateTime date;

		[Display(Name = "Дата")]
		public virtual DateTime Date {
			get { return date; }
			set { SetField(ref date, value); }
		}

		private Meter meter;

		[Display(Name = "Счётчик")]
		public virtual Meter Meter {
			get { return meter; }
			set { SetField(ref meter, value); }
		}

		private MeterTariff meterTariff;

		[Display(Name = "Тариф")]
		public virtual MeterTariff MeterTariff {
			get { return meterTariff; }
			set { SetField(ref meterTariff, value); }
		}

		private int readingValue;

		[Display(Name = "Показания")]
		public virtual int ReadingValue {
			get { return readingValue; }
			set { SetField(ref readingValue, value); }
		}

		private AccrualItem accrualItem;

		[Display(Name = "Строка начисления")]
		public virtual AccrualItem AccrualItem {
			get { return accrualItem; }
			set { SetField(ref accrualItem, value); }
		}

		#endregion
	}
}
