using System;
using System.Reflection;
using QS.DBScripts.Models;
using QS.Updater.DB;

namespace Bazar.Sql
{
	public class ScriptsConfiguration
	{
		public static CreationScript MakeCreationScript()
		{
			return new CreationScript(
				Assembly.GetAssembly(typeof(ScriptsConfiguration)),
				"bazar.SQLScripts.new-2.3.sql",
				new Version(2,3)
			);
		}

		public static UpdateConfiguration MakeUpdateConfiguration()
		{
			var configuration = new UpdateConfiguration();

			//Настраиваем обновления
			configuration.AddUpdate (
				new Version (2, 2),
				new Version (2, 3),
				"bazar.SQLScripts.Update 2.2 to 2.3.sql");

			configuration.AddMicroUpdate (
				new Version (2, 3),
				new Version (2, 3, 1),
				"bazar.SQLScripts.Update 2.3.1.sql");

			configuration.AddMicroUpdate (
				new Version (2, 3, 1),
				new Version (2, 3, 4),
				"bazar.SQLScripts.2.3.4.sql");
			
			return configuration;
		}
	}
}
