using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Rental;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Payments
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "оплаты",
		Nominative = "оплата"
	)]
	public class Payment : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private DateTime createDate;

		[Display(Name = "Дата создания")]
		public virtual DateTime CreateDate {
			get { return createDate; }
			set { SetField(ref createDate, value); }
		}

		private Accrual accrual;

		[Display(Name = "Начисление")]
		public virtual Accrual Accrual {
			get { return accrual; }
			set { SetField(ref accrual, value); }
		}

		private IncomeSlip incomeSlip;

		[Display(Name = "Приходный ордер")]
		public virtual IncomeSlip IncomeSlip {
			get { return incomeSlip; }
			set { SetField(ref incomeSlip, value); }
		}

		#endregion
	}
}
