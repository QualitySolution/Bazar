using System.Collections.Generic;
using Bazar.Domain.Rental;
using QS.DomainModel.UoW;

namespace Bazar.Repositories.Rental
{
	public static class AccrualRepository
	{
		public static IList<AccrualItem> GetAccrualItems(IUnitOfWork uow, int accrualId)
		{
			return uow.Session.QueryOver<AccrualItem>().Where(x => x.Accrual.Id == accrualId).List();
		}

		public static Accrual GetAcctualByInvoice(IUnitOfWork uow, uint invoiceNumber, uint year, int exludeId)
		{
			return uow.Session.QueryOver<Accrual>()
				.Where(x => x.InvoiceNumber == invoiceNumber && x.Year == year)
				.Where(x => x.Id != exludeId)
				.SingleOrDefault();
		}
	}
}
