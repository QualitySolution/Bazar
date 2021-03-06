﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Bindings.Collections.Generic;
using System.Linq;
using Bazar.Repositories.Payments;
using Bazar.Repositories.Rental;
using QS.Dialog;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Project.Domain;
using QS.Services;

namespace Bazar.Domain.Rental
{
	[Appellative(Gender = GrammaticalGender.Neuter,
		NominativePlural = "начисления",
		Nominative = "начисление"
	)]
	public class Accrual : BusinessObjectBase<Accrual>, IDomainObject, IValidatableObject
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

		private DateTime? date = DateTime.Today;
		[Display(Name = "Дата")]
		public virtual DateTime? Date {
			get => date;
			set => SetField(ref date, value);
		}

		private uint? invoiceNumber;
		[Display(Name = "Номер счета")]
		public virtual uint? InvoiceNumber {
			get => invoiceNumber;
			set => SetField(ref invoiceNumber, value);
		}

		private uint month;

		[Display(Name = "Месяц")]
		public virtual uint Month {
			get { return month; }
			set { SetField(ref month, value); }
		}

		private uint year = (uint)DateTime.Today.Year;

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

		#region Публичные методы

		public virtual void FillItemsFromContract(IUnitOfWork uow, IInteractiveService interactive)
		{
			if(Items.Count > 0 && !interactive.Question("Список услуг не пустой. При заполнени с услугами из договора, текущий услуги будут удалены. Продолжить?"))
				return;

			var savedIds = Items.Select(x => x.Id).Where(x => x > 0).ToArray();
			if(savedIds.Length > 0 && PaymentRepository.GetPaymentItemsByAccrualItems(uow, savedIds).Count > 0) {
				interactive.ShowMessage(ImportanceLevel.Error, "По некоторым услугам уже были проведены оплаты, их нельзя очистить.");
				return;
			}

			ObservableItems.Clear();
			foreach(var contractItem in ContractRepository.GetContractItems(uow, Contract.Id)) {
				var accrualItem = new AccrualItem() {
					Accrual = this,
					Amount = contractItem.Service.CalculateIncompleteAmount(contractItem.Amount, Month, Year, contractItem.Contract),
					Cash = contractItem.Cash,
					Place_NhOnly = contractItem.Place, //Чтобы не пресчитывало количество
					Price = contractItem.Price,
					Service_NhOnly = contractItem.Service //Чтобы не пресчитывало количество
				};
				ObservableItems.Add(accrualItem);
			}
		}

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(Items.Any(x => x.Service == null))
				yield return new ValidationResult($"Для некоторых строк начисления не указана услуга.", new[] { nameof(Items) });

			foreach(var item in Items) {
				if(item.Service?.PlaceSet == PlaceSetForService.Required && item.Place == null)
					yield return new ValidationResult($"Для услуги {item.Service.Name} необходимо заполнить место.", new[] { nameof(Items) });
				if(item.Service?.PlaceSet == PlaceSetForService.Prohibited && item.Place != null)
					yield return new ValidationResult($"Для услуги {item.Service.Name} запрещено указывать место.", new[] { nameof(Items) });
			}

			if(Date.HasValue) {
				var existAccrual = AccrualRepository.GetAccrual(UoW, Contract.Id, Date.Value);
				if(existAccrual != null && existAccrual.Id != Id)
					yield return new ValidationResult($"На дату {Date:d} для договора {Contract.Number} уже есть начисление.", new[] { nameof(Date) });
			}

		}

		#endregion
	}
}
