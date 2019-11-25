using System;
using Bazar.Domain.Estate;
using QS.BusinessCommon.Domain;
using QS.DomainModel.NotifyChange;
using QS.Project.DB;
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
		}
	}
}
