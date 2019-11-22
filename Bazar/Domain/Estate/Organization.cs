using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "организации",
		Nominative = "организация"
	)]
	public class Organization : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		#endregion
	}
}
