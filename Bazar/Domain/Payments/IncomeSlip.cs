using System;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Payments
{
	[Appellative(Gender = GrammaticalGender.Feminine,
		NominativePlural = "приходные ордера",
		Nominative = "приходный ордер"
	)]
	public class IncomeSlip : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		//FIXME Дозаполнить

		#endregion
	}
}
