using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Payments
{

	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "статьи дохода",
		Nominative = "статья дохода"
	)]
	public class IncomeCategory : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		string name;

		[Required (ErrorMessage = "Название должно быть заполнено.")]
		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value, () => Name); }
		}

		#endregion
	}
}
