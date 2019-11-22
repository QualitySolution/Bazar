using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative (Gender = GrammaticalGender.Masculine,
		NominativePlural = "типы мест",
		Nominative = "тип места"
	)]
	public class PlaceType : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Наименование")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		private string description;

		[Display (Name = "Описание")]
		public virtual string Description {
			get { return description; }
			set { SetField (ref description, value); }
		}

		#endregion
	}
}
