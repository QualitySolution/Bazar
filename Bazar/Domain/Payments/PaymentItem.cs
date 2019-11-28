using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Rental;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Payments
{

	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "строки оплаты",
		Nominative = "строка оплаты"
	)]
	public class PaymentItem : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private Payment payment;

		[Display(Name = "Оплата")]
		public virtual Payment Payment {
			get { return payment; }
			set { SetField(ref payment, value); }
		}

		private AccrualItem accrualItem;

		[Display(Name = "Строка начисления")]
		public virtual AccrualItem AccrualItem {
			get { return accrualItem; }
			set { SetField(ref accrualItem, value); }
		}

		private IncomeCategory incomeCategory;

		[Display(Name = "Статья дохода")]
		public virtual IncomeCategory IncomeCategory {
			get { return incomeCategory; }
			set { SetField(ref incomeCategory, value); }
		}

		private decimal sum;

		[Display(Name = "Сумма оплаты")]
		public virtual decimal Sum {
			get { return sum; }
			set { SetField(ref sum, value); }
		}

		#endregion
	}
}
