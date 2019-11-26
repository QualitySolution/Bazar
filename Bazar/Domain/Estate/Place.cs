using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative (Gender = GrammaticalGender.Neuter,
		NominativePlural = "места",
		Nominative = "место"
	)]
	public class Place : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private PlaceType placeType;

		[Display (Name = "Тип места")]
		public virtual PlaceType PlaceType {
			get { return placeType; }
			set { SetField (ref placeType, value); }
		}

		private string placeNumber;

		[Display (Name = "Номер места")]
		public virtual string PlaceNumber {
			get { return placeNumber; }
			set { SetField (ref placeNumber, value); }
		}

		private decimal area;

		[Display (Name = "Площадь")]
		public virtual decimal Area {
			get { return area; }
			set { SetField (ref area, value); }
		}

		private ContactPerson contactPerson;

		[Display (Name = "Контактное лицо")]
		public virtual ContactPerson ContactPerson {
			get { return contactPerson; }
			set { SetField (ref contactPerson, value); }
		}

		private Organization organization;

		[Display (Name = "Организация")]
		public virtual Organization Organization {
			get { return organization; }
			set { SetField (ref organization, value); }
		}

		private string comments;

		[Display (Name = "Комментарий")]
		public virtual string Comments {
			get { return comments; }
			set { SetField (ref comments, value); }
		}

		#endregion

		#region Расчетные

		public virtual string Title => PlaceType != null ? $"{PlaceType.Name}-{PlaceNumber}" : PlaceNumber;

		#endregion
	}
}
