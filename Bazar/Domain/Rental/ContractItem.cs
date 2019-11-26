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
			set { 
				if (SetField (ref service, value)) {
					if(Service != null && Service.DependOnArea && Place != null)
						Amount = Place.Area;
				}
			}
		}

		private Place place;

		[Display (Name = "Место")]
		public virtual Place Place {
			get { return place; }
			set { 
				if(SetField (ref place, value)) {
					if(Service != null && Service.DependOnArea && Place != null)
						Amount = Place.Area;
				}
			}
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

		#region Расчетные

		public virtual decimal Total => Amount * Price;

		#endregion

		#region Только для установки из Hibernate

		[Display(Name = "Услуга")]
		public virtual Service Service_NhOnly {
			get => service;
			set => service = value;
		}

		[Display(Name = "Место")]
		public virtual Place Place_NhOnly {
			get => place;
			set => place = value;
		}

		#endregion

	}
}
