using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "типы счётчиков",
		Nominative = "тип счётчика"
	)]
	public class MeterType : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display(Name = "Наименование")]
		public virtual string Name {
			get { return name; }
			set { SetField(ref name, value); }
		}

		private double readingRatio = 1;

		[Display(Name = "Коэффециент показаний")]
		public virtual double ReadingRatio {
			get { return readingRatio; }
			set { SetField(ref readingRatio, value); }
		}

		#endregion
	}
}
