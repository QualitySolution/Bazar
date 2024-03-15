using System;
using System.Data.Common;
using System.IO;
using Autofac;
using Bazar.Sql;
using QS.BaseParameters;
using QS.Configuration;
using QS.Deletion.Views;
using QS.Deletion;
using QS.Dialog.GtkUI;
using QS.Dialog;
using QS.DomainModel.UoW;
using QS.Navigation;
using QS.Permissions;
using QS.Project.DB;
using QS.Project.Dialogs.GtkUI.ServiceDlg;
using QS.Project.Domain;
using QS.Project.Search.GtkUI;
using QS.Project.Services.GtkUI;
using QS.Project.Services;
using QS.Project.Versioning;
using QS.Project.ViewModels;
using QS.Project.Views;
using QS.Services;
using QS.Tdi;
using QS.Updater.DB.Views;
using QS.Updater;
using QS.Validation;
using QS.Views.Resolve;
using QSProjectsLib;

namespace bazar
{
	static partial class MainClass
	{
		public static Autofac.IContainer AppDIContainer;

		static void CreateBaseConfig ()
		{
			logger.Info ("Настройка параметров базы...");

			// Настройка ORM
			var db = FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
				.Dialect<MySQL57ExtendedDialect>()
				.ConnectionString (QSMain.ConnectionString)
				.AdoNetBatchSize(100)
				.ShowSql ()
				.FormatSql ();

			OrmConfig.ConfigureOrm (db, new System.Reflection.Assembly[] {
				System.Reflection.Assembly.GetAssembly (typeof(MainClass)),
				System.Reflection.Assembly.GetAssembly (typeof(UserBase)),
			});

#if DEBUG
			NLog.LogManager.Configuration.RemoveRuleByName("HideNhibernate");
#endif
		}
		
		static void AutofacClassConfig()
		{
			var builder = new ContainerBuilder();

			#region База
			builder.RegisterType<DefaultUnitOfWorkFactory>().As<IUnitOfWorkFactory>();
			builder.RegisterType<DefaultSessionProvider>().As<ISessionProvider>();
			builder.Register(c => new MySqlConnectionFactory(QSMain.ConnectionString)).As<IConnectionFactory>();
			builder.Register<DbConnection>(c => c.Resolve<IConnectionFactory>().OpenConnection()).AsSelf().InstancePerLifetimeScope();
			builder.RegisterType<ParametersService>().AsSelf();
			builder.Register(c => QSProjectsLib.QSMain.ConnectionStringBuilder).AsSelf().ExternallyOwned();
			builder.RegisterType<NhDataBaseInfo>().As<IDataBaseInfo>();
			builder.RegisterType<MySQLProvider>().As<IMySQLProvider>();
			builder.RegisterType<UserService>().As<IUserService>();
			#endregion

			#region Сервисы
			#region GtkUI
			builder.RegisterType<GtkMessageDialogsInteractive>().As<IInteractiveMessage>();
			builder.RegisterType<GtkQuestionDialogsInteractive>().As<IInteractiveQuestion>();
			builder.RegisterType<GtkInteractiveService>().As<IInteractiveService>();
			builder.RegisterType<GtkValidationViewFactory>().As<IValidationViewFactory>();
			builder.RegisterType<GtkGuiDispatcher>().As<IGuiDispatcher>();
			builder.RegisterType<GtkRunOperationService>().As<IRunOperationService>();
			#endregion GtkUI
			#region Удаление
			builder.RegisterModule(new DeletionAutofacModule());
			builder.RegisterType<DeleteEntityGUIService>().As<IDeleteEntityService>();
			builder.Register(x => DeleteConfig.Main).AsSelf().ExternallyOwned();
 			#endregion
			//FIXME Нужно в конечно и итоге попытаться избавится от CommonService вообще.
			builder.RegisterType<CommonServices>().As<ICommonServices>();
			builder.RegisterType<UserService>().As<IUserService>();
			builder.RegisterType<ObjectValidator>().As<IValidator>();
			//FIXME Реализовать везде возможность отсутствия сервиса прав, чтобы не приходилось создавать то что не используется
			builder.RegisterType<DefaultAllowedPermissionService>().As<IPermissionService>();
			builder.RegisterType<CommonMessages>().AsSelf();
			#endregion

			#region Навигация
			builder.Register((ctx) => new AutofacViewModelsTdiPageFactory(AppDIContainer)).As<IViewModelsPageFactory>();
			builder.Register((ctx) => new AutofacTdiPageFactory(AppDIContainer)).As<ITdiPageFactory>();
			builder.Register((ctx) => new AutofacViewModelsGtkPageFactory(AppDIContainer)).AsSelf();
			builder.RegisterType<GtkWindowsNavigationManager>().AsSelf().As<INavigationManager>().SingleInstance();
			builder.RegisterType<BasedOnNameTDIResolver>().As<ITDIWidgetResolver>();
			builder.Register(cc => new ClassNamesBaseGtkViewResolver(
				typeof(DeletionView), 
				typeof(UpdateProcessView)
			)).As<IGtkViewResolver>();
			#endregion

			#region Отдельные диалоги
			builder.RegisterType<AboutView>().AsSelf();
			builder.RegisterType<AboutViewModel>().AsSelf();
			#endregion

			#region Журналы
			builder.RegisterType<OneEntrySearchView>().Named<Gtk.Widget>("GtkJournalSearchView");
			#endregion

			#region Обновления и версии
			builder.RegisterType<ApplicationVersionInfo>().As<IApplicationInfo>();
			builder.RegisterModule(new UpdaterAutofacModule());
			builder.Register(c => ScriptsConfiguration.MakeUpdateConfiguration()).AsSelf();
			builder.Register(c => ScriptsConfiguration.MakeCreationScript()).AsSelf();
			#endregion
			
			#region Настройка
			builder.Register(c => new IniFileConfiguration(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "workwear.ini"))).As<IChangeableConfiguration>().AsSelf();
			#endregion
			AppDIContainer = builder.Build();
		}
	}
}
