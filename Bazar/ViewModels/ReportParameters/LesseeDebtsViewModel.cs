using System;
using System.Collections.Generic;
using System.Linq;
using Bazar.Domain.Rental;
using Bazar.JournalViewModels.Rental;
using Bazar.Repositories.Rental;
using QS.DomainModel.Entity;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.ViewModels.Control.EEVM;
using QS.ViewModels.Dialog;
using QSProjectsLib;

namespace Bazar.ViewModels.ReportParameters
{
	public class LesseeDebtsViewModel : UowDialogViewModelBase
	{
		public EntityEntryViewModel<Lessee> LesseeEntryViewModel;

		private DateTime? date = DateTime.Today;
		[PropertyChangedAlso("ButtonRunSensetive")]
		public virtual DateTime? Date {
			get => date;
			set => SetField(ref date, value);
		}

		private uint monthFrom = (uint)DateTime.Today.AddMonths(-3).Month;
		public virtual uint MonthFrom {
			get => monthFrom;
			set => SetField(ref monthFrom, value);
		}

		private int yearFrom = DateTime.Today.AddMonths(-3).Year;
		public virtual int YearFrom {
			get => yearFrom;
			set => SetField(ref yearFrom, value);
		}

		private uint monthTo = (uint)DateTime.Today.Month;
		public virtual uint MonthTo {
			get => monthTo;
			set => SetField(ref monthTo, value);
		}

		private int yearTo = DateTime.Today.Year;
		public virtual int YearTo {
			get => yearTo;
			set => SetField(ref yearTo, value);
		}

		public int CashId = -1;
		public int OrgId = -1;

		public List<ServiceNode> Services;

		public bool UseServiceDetail;

		public bool UseAnyMonth = true;

		public LesseeDebtsViewModel(IUnitOfWorkFactory unitOfWorkFactory, INavigationManager navigation) : base(unitOfWorkFactory, navigation)
		{
			IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();

			Title = "Параметры отчета по долгам арендаторов";

			LesseeEntryViewModel = new EntityEntryViewModel<Lessee> {
				EntitySelector = new JournalViewModelSelector<Lessee, LesseeJournalViewModel>(this, uow, MainClass.MainWin.NavigationManager),
				AutocompleteSelector = new JournalViewModelAutocompleteSelector<Lessee, LesseeJournalViewModel>(uow, MainClass.AppDIContainer.BeginLifetimeScope())
			};

			var services = ServiceRepository.GetActiveServices(UoW);
			Services = services.Select(x => new ServiceNode(x)).ToList();
		}

		public bool ButtonRunSensetive => Date.HasValue;

		public void RunReport()
		{
			string param = "Day=" + Date.Value.Day.ToString() +
						   "&Month=" + Date.Value.Month.ToString() +
						   "&Year=" + Date.Value.Year.ToString() +
						   "&Cash=" + CashId + 
						   "&Org=" + OrgId +
						   "&Lessee=" + (LesseeEntryViewModel.Entity?.Id ?? -1);

			if(UseAnyMonth) 
				param += "&MonthFrom=-1&YearFrom=-1&MonthTo=-1&YearTo=-1";
			else
				param += "&MonthFrom=" + monthFrom +
					"&YearFrom=" + YearFrom +
					"&MonthTo=" + MonthTo +
					"&YearTo=" + YearTo;

			if(UseServiceDetail) {
				param += "&Services=";
				foreach(var row in Services) {
					if(row.Active)
						param += String.Format("{0},", row.Service.Id);
				}
				ViewReportExt.Run("LesseeDebtsDetail", param.TrimEnd(','));
			} else
				ViewReportExt.Run("LesseeDebts", param);
		}
	}

	public class ServiceNode : PropertyChangedBase
	{
		private bool active;
		public virtual bool Active {
			get => active;
			set => SetField(ref active, value);
		}

		public Service Service;

		public ServiceNode(Service service)
		{
			Service = service ?? throw new ArgumentNullException(nameof(service));
		}
	}
}
