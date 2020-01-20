using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Estate;
using Bazar.Domain.Payments;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Rental
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "строки начисления",
		Nominative = "строка начисления"
	)]
	public class AccrualItem : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private Accrual accrual;

		[Display(Name = "Начисление")]
		public virtual Accrual Accrual {
			get { return accrual; }
			set { SetField(ref accrual, value); }
		}

		private Service service;

		[Display(Name = "Услуга")]
		public virtual Service Service {
			get { return service; }
			set {
				if(SetField(ref service, value)) {
					if(Service != null && Service.DependOnArea && Place != null)
						Amount = Service.CalculateIncompleteAmount(Place.Area, Accrual.Month, Accrual.Year, Accrual.Contract);
				}
			}
		}

		private Place place;

		[Display(Name = "Место")]
		public virtual Place Place {
			get { return place; }
			set {
				if(SetField(ref place, value)) {
					if(Service != null && Service.DependOnArea && Place != null)
						Amount = Service.CalculateIncompleteAmount(Place.Area, Accrual.Month, Accrual.Year, Accrual.Contract);
				}
			}
		}

		private Cash cash;

		[Display(Name = "Касса")]
		public virtual Cash Cash {
			get { return cash; }
			set { SetField(ref cash, value); }
		}

		private decimal amount;

		[Display(Name = "Количество")]
		public virtual decimal Amount {
			get { return amount; }
			set { SetField(ref amount, value); }
		}

		private decimal price;

		[Display(Name = "Цена")]
		public virtual decimal Price {
			get { return price; }
			set { SetField(ref price, value); }
		}

		#endregion

		#region Расчетные

		public virtual decimal Total => Amount * Price;

		#endregion

		public AccrualItem(Accrual accrual)
		{
			Accrual = accrual ?? throw new ArgumentNullException(nameof(accrual));
		}

		public AccrualItem()
		{
		}

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
