using System;
using System.ComponentModel.DataAnnotations;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Estate
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "организации",
		Nominative = "организация"
	)]
	public class Organization : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		private string printName;
		[Display(Name = "Название для документов")]
		public virtual string PrintName {
			get => printName;
			set => SetField(ref printName, value);
		}

		private string inn;
		[Display(Name = "ИНН")]
		public virtual string INN {
			get => inn;
			set => SetField(ref inn, value);
		}

		private string kpp;
		[Display(Name = "КПП")]
		public virtual string KPP {
			get => kpp;
			set => SetField(ref kpp, value);
		}

		private string jurAddress;
		[Display(Name = "Юридический адрес")]
		public virtual string JurAddress {
			get => jurAddress;
			set => SetField(ref jurAddress, value);
		}

		private string phone;
		[Display(Name = "Телефон")]
		public virtual string Phone {
			get => phone;
			set => SetField(ref phone, value);
		}

		private string bankBik;
		[Display(Name = "БИК банка")]
		public virtual string BankBik {
			get => bankBik;
			set => SetField(ref bankBik, value);
		}

		private string bankName;
		[Display(Name = "Имя банка")]
		public virtual string BankName {
			get => bankName;
			set => SetField(ref bankName, value);
		}

		private string bankAccount;
		[Display(Name = "Расчетный счёт")]
		public virtual string BankAccount {
			get => bankAccount;
			set => SetField(ref bankAccount, value);
		}

		private string bankCorAccount;
		[Display(Name = "Кореспондентский счет")]
		public virtual string BankCorAccount {
			get => bankCorAccount;
			set => SetField(ref bankCorAccount, value);
		}


		#endregion
	}
}
