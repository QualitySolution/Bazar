using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Payments
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "кассы",
		Nominative = "касса"
	)]
	public class Cash : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		private string color;

		[Display (Name = "Окрас")]
		public virtual string Color {
			get { return color; }
			set { SetField (ref color, value); }
		}

		#endregion
	}
}
