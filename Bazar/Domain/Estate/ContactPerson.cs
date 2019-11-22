using System;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "контактные лица",
		Nominative = "контактное лицо"
	)]
	public class ContactPerson : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		#endregion
	}
}
