using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Estate;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Rental
{
	[Appellative (Gender = GrammaticalGender.Masculine,
		NominativePlural = "договора",
		Nominative = "договор"
	)]
	public class Contract : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string number;

		[Display (Name = "Номер")]
		public virtual string Number {
			get { return number; }
			set { SetField (ref number, value); }
		}

		private Lessee lessee;

		[Display (Name = "Арендатор")]
		public virtual Lessee Lessee {
			get { return lessee; }
			set { SetField (ref lessee, value); }
		}

		private Organization organization;

		[Display (Name = "Организация")]
		public virtual Organization Organization {
			get { return organization; }
			set { SetField (ref organization, value); }
		}

		private DateTime signDate;

		[Display (Name = "Дата подписания")]
		public virtual DateTime SignDate {
			get { return signDate; }
			set { SetField (ref signDate, value); }
		}

		private DateTime beginDate;

		[Display (Name = "Начало действия")]
		public virtual DateTime BeginDate {
			get { return beginDate; }
			set { SetField (ref beginDate, value); }
		}

		private DateTime endDate;

		[Display (Name = "Окончание действия")]
		public virtual DateTime EndDate {
			get { return endDate; }
			set { SetField (ref endDate, value); }
		}

		private DateTime cancelDate;

		[Display (Name = "Дата расторжения")]
		public virtual DateTime CancelDate {
			get { return cancelDate; }
			set { SetField (ref cancelDate, value); }
		}

		private int payDay;

		[Display (Name = "День оплаты")]
		public virtual int PayDay {
			get { return payDay; }
			set { SetField (ref payDay, value); }
		}

		private string comments;

		[Display (Name = "Комментарий")]
		public virtual string Comments {
			get { return comments; }
			set { SetField (ref comments, value); }
		}

		#endregion
	}
}
