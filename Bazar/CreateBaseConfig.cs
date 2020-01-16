using Autofac;
using Bazar.Domain.Estate;
using Bazar.JournalViewModels;
using Bazar.JournalViewModels.Estate;
using Bazar.ViewModels.Estate;
using Bazar.Views.Estate;
using QS.BusinessCommon.Domain;
using QS.Dialog;
using QS.Dialog.GtkUI;
using QS.DomainModel.NotifyChange;
using QS.DomainModel.UoW;
using QS.Journal.GtkUI;
using QS.Navigation;
using QS.Permissions;
using QS.Project.DB;
using QS.Project.Journal;
using QS.Project.Search.GtkUI;
using QS.Project.Services;
using QS.Project.Services.GtkUI;
using QS.Services;
using QS.Validation;
using QS.Validation.GtkUI;
using QS.Views.Resolve;
using QSProjectsLib;

namespace Bazar
{
	partial class MainClass
	{
		static void CreateBaseConfig()
		{
			logger.Info("Настройка параметров базы...");

			// Настройка ORM
			var db = FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
				.ConnectionString(QSMain.ConnectionString)
				.ShowSql()
				.FormatSql();

			OrmConfig.ConfigureOrm(db, new System.Reflection.Assembly[] {
				System.Reflection.Assembly.GetAssembly (typeof(Place)),
				System.Reflection.Assembly.GetAssembly (typeof(MeasurementUnits)),
			});

			NotifyConfiguration.Enable();
			JournalsColumnsConfigs.RegisterColumns();
		}

		public static Autofac.IContainer AppDIContainer;

		static void AutofacClassConfig()
		{
			var builder = new ContainerBuilder();

			#region База
			builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
			builder.RegisterType<DefaultSessionProvider>().As<ISessionProvider>();
			#endregion

			#region Сервисы
			#region GtkUI
			builder.RegisterType<GtkMessageDialogsInteractive>().As<IInteractiveMessage>();
			builder.RegisterType<GtkQuestionDialogsInteractive>().As<IInteractiveQuestion>();
			builder.RegisterType<GtkInteractiveService>().As<IInteractiveService>();
			builder.RegisterType<GtkValidationViewFactory>().As<IValidationViewFactory>();
			builder.RegisterType<GtkDeleteEntityService>().As<IDeleteEntityService>();
			#endregion GtkUI
			//FIXME Нужно в конечнои итоге попытаться избавится от CommonServce вообще.
			builder.RegisterType<CommonServices>().As<ICommonServices>();
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<ValidationService>().As<IValidationService>();
			//FIXME Реализовать везде возможность отсутствия сервиса прав, чтобы не приходилось создавать то что не используется
			builder.RegisterType<DefaultAllowedPermissionService>().As<IPermissionService>();
			#endregion

			#region Навигация
			builder.RegisterType<ClassNamesHashGenerator>().As<IPageHashGenerator>();
			builder.Register((ctx) => new AutofacViewModelsGtkPageFactory(AppDIContainer)).As<IViewModelsPageFactory>();
			builder.RegisterType<GtkWindowsNavigationManager>().AsSelf().As<INavigationManager>().SingleInstance();
			builder.Register(ctx => 
				new RegisteredGtkViewResolver(
					new ClassNamesBaseGtkViewResolver(System.Reflection.Assembly.GetAssembly(typeof(OrganizationView))))
						.RegisterView<JournalViewModelBase, JournalView>()
				).As<IGtkViewResolver>();
			#endregion

			#region ViewModels
			//Estate
			builder.RegisterType<OrganizationViewModel>().AsSelf();
			#endregion

			builder.RegisterType<OneEntrySearchView>().Named<Gtk.Widget>("GtkJournalSearchView");
			#region JournalViewModels
			//Estate
			builder.RegisterType<OrganizationJournalViewModel>().AsSelf();
			builder.RegisterType<PlacesJournalViewModel>().AsSelf();
			#endregion

			AppDIContainer = builder.Build();
		}
	}
}
