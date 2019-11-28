using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Rental;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "тарифы счётчика",
		Nominative = "тариф счетчика"
	)]
	public class MeterTariff : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField(ref name, value); }
		}

		private MeterType meterType;

		[Display(Name = "Тип счетчика")]
		public virtual MeterType MeterType {
			get { return meterType; }
			set { SetField(ref meterType, value); }
		}

		private Service service;

		[Display(Name = "Услуга")]
		public virtual Service Service {
			get { return service; }
			set { SetField(ref service, value); }
		}

		#endregion
	}
}
