using System.Collections.Generic;
using Bazar.Domain.Payments;
using QS.DomainModel.UoW;
using NHibernate.Criterion;

namespace Bazar.Repositories.Payments
{
	public static class PaymentRepository
	{
		public static IList<PaymentItem> GetPaymentItemsByAccrualItems(IUnitOfWork uow, int[] accrualItemIds)
		{
			return uow.Session.QueryOver<PaymentItem>()
				.Where(x => x.AccrualItem.IsIn(accrualItemIds)).List();
		}

		public static IList<PaymentItem> GetPaymentItemsForAccrual(IUnitOfWork uow, int accrualId)
		{
			return uow.Session.QueryOver<PaymentItem>()
				.JoinQueryOver(x => x.AccrualItem)
				.Where(x => x.Accrual.Id == accrualId).List();
		}
	}
}
