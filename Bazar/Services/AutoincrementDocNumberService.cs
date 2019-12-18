using System;
using Bazar.Domain.Application;
using QS.DomainModel.UoW;

namespace Bazar.Services
{
	public class AutoincrementDocNumberService
	{
		public AutoincrementDocNumberService()
		{
		}

		public uint GetNewNumber(IUnitOfWork uow, DocumentType docType, uint year)
		{
			var lastnumber = uow.Session.QueryOver<DocumentLastNumber>()
				.Where(x => x.DocumentType == docType)
				.Where(x => x.Year == year)
				.SingleOrDefault();
			if(lastnumber == null)
				lastnumber = new DocumentLastNumber {
					DocumentType = docType,
					Year = year,
					LastNumber = 0
				};

			lastnumber.LastNumber++;
			uow.Save(lastnumber);
			return lastnumber.LastNumber;
		}
	}
}
