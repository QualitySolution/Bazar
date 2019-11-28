using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative(Gender = GrammaticalGender.Masculine,
		NominativePlural = "счётчики",
		Nominative = "счётчик"
	)]
	public class Meter : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField(ref name, value); }
		}

		private Place place;

		[Display(Name = "Место")]
		public virtual Place Place {
			get { return place; }
			set { SetField(ref place, value); }
		}

		private MeterType meterType;

		[Display(Name = "Тип счётчика")]
		public virtual MeterType MeterType {
			get { return meterType; }
			set { SetField(ref meterType, value); }
		}

		private Meter parentMeter;

		[Display(Name = "Вышестоящий счётчик")]
		public virtual Meter ParentMeter {
			get { return parentMeter; }
			set { SetField(ref parentMeter, value); }
		}

		private bool disabled;

		[Display(Name = "Отключен")]
		public virtual bool Disabled {
			get { return disabled; }
			set { SetField(ref disabled, value); }
		}

		#endregion
	}
}
