using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using QS.DomainModel.Entity;
using QS.Project.Domain;

namespace Bazar.Domain.Rental
{
	[Appellative(Gender = GrammaticalGender.Neuter,
		NominativePlural = "начисления",
		Nominative = "начисление"
	)]
	public class Accrual : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private UserBase user;

		[Display(Name = "Пользователь")]
		public virtual UserBase User {
			get { return user; }
			set { SetField(ref user, value); }
		}

		private Contract contract;

		[Display(Name = "Договор")]
		public virtual Contract Contract {
			get { return contract; }
			set { SetField(ref contract, value); }
		}

		private DateTime date;
		[Display(Name = "Дата")]
		public virtual DateTime Date {
			get => date;
			set => SetField(ref date, value);
		}

		private string invoiceNumber;
		[Display(Name = "Номер счета")]
		public virtual string InvoiceNumber {
			get => invoiceNumber;
			set => SetField(ref invoiceNumber, value);
		}

		private uint month;

		[Display(Name = "Месяц")]
		public virtual uint Month {
			get { return month; }
			set { SetField(ref month, value); }
		}

		private uint year;

		[Display(Name = "Год")]
		public virtual uint Year {
			get { return year; }
			set { SetField(ref year, value); }
		}

		private bool paid;

		[Display(Name = "Оплачено")]
		public virtual bool Paid {
			get { return paid; }
			set { SetField(ref paid, value); }
		}

		[Display(Name = "Не полностью начислено")]
		public virtual bool NotComplete {
			get { return Items.Any(x => x.Total == 0); }
			set { }
		}

		private string comments;

		[Display(Name = "Комментарий")]
		public virtual string Comments {
			get { return comments; }
			set { SetField(ref comments, value); }
		}

		IList<AccrualItem> items = new List<AccrualItem>();
		[Display(Name = "Строки")]
		public virtual IList<AccrualItem> Items {
			get => items;
			set => SetField(ref items, value);
		}

		GenericObservableList<AccrualItem> observableItems;
		//FIXME Кослыль пока не разберемся как научить hibernate работать с обновляемыми списками.
		public virtual GenericObservableList<AccrualItem> ObservableItems {
			get {
				if(observableItems == null)
					observableItems = new GenericObservableList<AccrualItem>(Items);
				return observableItems;
			}
		}

		#endregion

		#region Расчетные

		public virtual decimal AccrualTotal => Items.Sum(x => x.Total);

		#endregion
	}
}
