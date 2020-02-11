using System;
using System.Globalization;
using System.Linq;
using Bazar.ViewModels.ReportParameters;
using QS.DomainModel.UoW;
using QS.Utilities.Text;
using QS.Views.Dialog;
using QSProjectsLib;

namespace Bazar.Views.ReportParameters
{
	[WindowSize(200, 500)]
	public partial class LesseeDebtsView : DialogViewBase<LesseeDebtsViewModel>
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		public LesseeDebtsView(LesseeDebtsViewModel viewModel) : base(viewModel)
		{
			this.Build();

			ComboWorks.ComboFillReference(comboCash, "cash", ComboWorks.ListMode.WithAll);
			ComboWorks.ComboFillReference(comboOrg, "organizations", ComboWorks.ListMode.WithAll, OrderBy: "name");

			IUnitOfWork uow = UnitOfWorkFactory.CreateWithoutRoot();

			entityLessee.ViewModel = viewModel.LesseeEntryViewModel;
			dateReport.Binding.AddBinding(ViewModel, vm => vm.Date, w => w.Date).InitializeFromSource(); 

			ytreeServices.CreateFluentColumnsConfig<ServiceNode>()
				.AddColumn("Выб.").AddToggleRenderer(x => x.Active).Editing()
				.AddColumn("Услуги").AddTextRenderer(x => x.Service.Name)
				.Finish();

			ytreeServices.ItemsDataSource = ViewModel.Services;

			var df = CultureInfo.CurrentUICulture.DateTimeFormat;
			comboMonthFrom.SetRenderTextFunc<uint>(x => df.MonthNames[x - 1].StringToTitleCase());
			comboMonthFrom.ItemsList = Enumerable.Range(1, 12).Select(x => (uint)x).ToList();
			comboMonthFrom.Binding.AddBinding(ViewModel, e => e.MonthFrom, w => w.SelectedItem).InitializeFromSource();

			sYearFrom.Binding.AddBinding(viewModel, vm => vm.YearFrom, w => w.ValueAsInt).InitializeFromSource();

			comboMonthTo.SetRenderTextFunc<uint>(x => df.MonthNames[x - 1].StringToTitleCase());
			comboMonthTo.ItemsList = Enumerable.Range(1, 12).Select(x => (uint)x).ToList();
			comboMonthTo.Binding.AddBinding(ViewModel, e => e.MonthTo, w => w.SelectedItem).InitializeFromSource();

			sYearTo.Binding.AddBinding(viewModel, vm => vm.YearTo, w => w.ValueAsInt).InitializeFromSource();

			comboCash.Active = 0;
			comboOrg.Active = 0;
		}

		protected void OnCheckDetailClicked(object sender, EventArgs e)
		{
			ViewModel.UseServiceDetail = checkDetail.Active; 
		}

		protected void OnComboCashChanged(object sender, EventArgs e)
		{
			ViewModel.CashId = ComboWorks.GetActiveId(comboCash);
		}

		protected void OnRadioSetMonthsToggled(object sender, EventArgs e)
		{
			tableMonth.Visible = radioSetMonths.Active;
			ViewModel.UseAnyMonth = radioAnyMonth.Active;
		}

		protected void OnComboOrgChanged(object sender, EventArgs e)
		{
			ViewModel.OrgId = ComboWorks.GetActiveId(comboOrg);
		}

		protected void OnButtonReportClicked(object sender, EventArgs e)
		{
			ViewModel.RunReport();
		}

		protected void OnRadioAnyMonthToggled(object sender, EventArgs e)
		{
			tableMonth.Visible = radioSetMonths.Active;
			ViewModel.UseAnyMonth = radioAnyMonth.Active;
		}
	}
}
