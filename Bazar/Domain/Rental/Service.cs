using System;
using System.ComponentModel.DataAnnotations;
using Bazar.Domain.Payments;
using QS.BusinessCommon.Domain;
using QS.DomainModel.Entity;

namespace Bazar.Domain.Rental
{
	[Appellative (Gender = GrammaticalGender.Feminine,
		NominativePlural = "услуги",
		Nominative = "услуга"
	)]
	public class Service : PropertyChangedBase, IDomainObject
	{
		#region Свойства

		public virtual int Id { get; set; }

		private string name;

		[Display (Name = "Название")]
		public virtual string Name {
			get { return name; }
			set { SetField (ref name, value); }
		}

		private MeasurementUnits units;

		[Display (Name = "Единица измерения")]
		public virtual MeasurementUnits Units {
			get { return units; }
			set { SetField (ref units, value); }
		}

		private IncomeCategory incomeCategory;

		[Display (Name = "Статья дохода")]
		public virtual IncomeCategory IncomeCategory {
			get { return incomeCategory; }
			set { SetField (ref incomeCategory, value); }
		}

		private ServiceProvider serviceProvider;

		[Display (Name = "Поставщик")]
		public virtual ServiceProvider ServiceProvider {
			get { return serviceProvider; }
			set { SetField (ref serviceProvider, value); }
		}

		private bool dependOnArea;

		[Display (Name = "Зависит от площади")]
		public virtual bool DependOnArea {
			get { return dependOnArea; }
			set { SetField (ref dependOnArea, value); }
		}

		private bool incompleteMonth;

		[Display (Name = "Рассчитывать не полный месяц")]
		public virtual bool IncompleteMonth {
			get { return incompleteMonth; }
			set { SetField (ref incompleteMonth, value); }
		}

		#endregion
	}
}
