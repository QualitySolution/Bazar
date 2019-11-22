using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Estate;
using Bazar.Domain.Payments;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Rental
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "услуги договора",
		Nominative = "услуга договора"
	)]
	public class ContractItem : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private Contract contract;

		[Display (Name = "Договор")]
		public virtual Contract Contract {
			get { return contract; }
			set { SetField (ref contract, value); }
		}

		private Service service;

		[Display (Name = "Услуга")]
		public virtual Service Service {
			get { return service; }
			set { SetField (ref service, value); }
		}

		private Place place;

		[Display (Name = "Место")]
		public virtual Place Place {
			get { return place; }
			set { SetField (ref place, value); }
		}

		private Cash cash;

		[Display (Name = "Касса")]
		public virtual Cash Cash {
			get { return cash; }
			set { SetField (ref cash, value); }
		}

		private decimal amount;

		[Display (Name = "Количество")]
		public virtual decimal Amount {
			get { return amount; }
			set { SetField (ref amount, value); }
		}

		private decimal price;

		[Display (Name = "Цена")]
		public virtual decimal Price {
			get { return price; }
			set { SetField (ref price, value); }
		}

		private decimal? minimalSum;

		[Display (Name = "Минимальная сумма")]
		public virtual decimal? MinimalSum {
			get { return minimalSum; }
			set { SetField (ref minimalSum, value); }
		}

		#endregion
	}
}
