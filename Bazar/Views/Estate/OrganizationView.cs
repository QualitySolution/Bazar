using System;
using Bazar.Domain.Estate;
using Bazar.ViewModels.Estate;
using QS.Views.GtkUI;
using QS.Widgets;

namespace Bazar.Views.Estate
{

	public partial class OrganizationView : EntityTabViewBase<OrganizationViewModel, Organization>
	{
		public OrganizationView(OrganizationViewModel viewModel) : base(viewModel)
		{
			this.Build();

			dataentryName.Binding.AddBinding(Entity, e => e.Name, w => w.Text).InitializeFromSource();
			dataentryFullName.Binding.AddBinding(Entity, e => e.PrintName, w => w.Text).InitializeFromSource();

			entryPhone.Binding.AddBinding(Entity, e => e.Phone, w => w.Text).InitializeFromSource();
			dataentryINN.ValidationMode = ValidationType.Numeric;
			dataentryINN.Binding.AddBinding(Entity, e => e.INN, w => w.Text).InitializeFromSource();
			dataentryKPP.ValidationMode = ValidationType.Numeric;
			dataentryKPP.Binding.AddBinding(Entity, e => e.KPP, w => w.Text).InitializeFromSource();

			datatextviewJurAddress.Binding.AddBinding(Entity, e => e.JurAddress, w => w.Buffer.Text).InitializeFromSource();

			yentryLeaderSign.Binding.AddBinding(Entity, e => e.LeaderSign, w => w.Text).InitializeFromSource();
			yentryBuhgalterSign.Binding.AddBinding(Entity, e => e.BuhgalterSign, w => w.Text).InitializeFromSource();

			//Банковские реквизиты
			validatedentryBankBik.ValidationMode = ValidationType.Numeric;
			validatedentryBankBik.Binding.AddBinding(Entity, e => e.BankBik, w => w.Text).InitializeFromSource();
			validatedentryBankCorAccount.ValidationMode = ValidationType.Numeric;
			validatedentryBankCorAccount.Binding.AddBinding(Entity, e => e.BankCorAccount, w => w.Text).InitializeFromSource();
			validatedentryBankAccount.ValidationMode = ValidationType.Numeric;
			validatedentryBankAccount.Binding.AddBinding(Entity, e => e.BankAccount, w => w.Text).InitializeFromSource();
			ytextviewBankName.Binding.AddBinding(Entity, e => e.BankName, w => w.Buffer.Text).InitializeFromSource();

			notebookMain.Page = 0;
			notebookMain.ShowTabs = false;

			CommonButtonSubscription();
		}

		protected void OnRadioTabInfoToggled(object sender, EventArgs e)
		{
			if(radioTabInfo.Active)
				notebookMain.CurrentPage = 0;
		}

		protected void OnRadioTabAccountsToggled(object sender, EventArgs e)
		{
			if(radioTabAccounts.Active)
				notebookMain.CurrentPage = 1;
		}
	}
}
