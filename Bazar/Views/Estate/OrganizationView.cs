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

			notebookMain.Page = 0;
			notebookMain.ShowTabs = false;
			//accountsview1.ParentReference = new ParentReferenceGeneric<Organization, Account>(UoWGeneric, o => o.Accounts);

			CommonButtonSubscription();
		}
	}
}
