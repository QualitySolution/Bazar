using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Rental
{
	[Appellative (Gender = GrammaticalGender.Masculine,
		NominativePlural = "арендаторы",
		Nominative = "арендатор"
	)]
	public class Lessee : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		private string directorFio;

		[Display (Name = "ФИО директора")]
		public virtual string DirectorFio {
			get { return directorFio; }
			set { SetField (ref directorFio, value); }
		}

		private string directorPassportSeries;

		[Display (Name = "Паспорт серия")]
		public virtual string DirectorPassportSeries {
			get { return directorPassportSeries; }
			set { SetField (ref directorPassportSeries, value); }
		}

		private string directorPassportNumber;

		[Display (Name = "Паспорт номер")]
		public virtual string DirectorPassportNumber {
			get { return directorPassportNumber; }
			set { SetField (ref directorPassportNumber, value); }
		}

		private string directorPassportExit;

		[Display (Name = "Паспорт выдача")]
		public virtual string DirectorPassportExit {
			get { return directorPassportExit; }
			set { SetField (ref directorPassportExit, value); }
		}

		private string address;

		[Display (Name = "Адрес")]
		public virtual string Address {
			get { return address; }
			set { SetField (ref address, value); }
		}

		private string inn;

		[Display (Name = "ИНН")]
		public virtual string Inn {
			get { return inn; }
			set { SetField (ref inn, value); }
		}

		private string kpp;

		[Display (Name = "КПП")]
		public virtual string Kpp {
			get { return kpp; }
			set { SetField (ref kpp, value); }
		}

		private string ogrn;

		[Display (Name = "ОГРН")]
		public virtual string Ogrn {
			get { return ogrn; }
			set { SetField (ref ogrn, value); }
		}

		private bool wholesaler;

		[Display (Name = "Оптовая торговля")]
		public virtual bool Wholesaler {
			get { return wholesaler; }
			set { SetField (ref wholesaler, value); }
		}

		private bool retail;

		[Display (Name = "Розница")]
		public virtual bool Retail {
			get { return retail; }
			set { SetField (ref retail, value); }
		}

		//Goods?

		private string comments;

		[Display (Name = "Комментарий")]
		public virtual string Comments {
			get { return comments; }
			set { SetField (ref comments, value); }
		}

		#endregion
	}
}
