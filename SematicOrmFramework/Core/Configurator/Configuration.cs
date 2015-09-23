namespace SematicOrmFramework.Core.Configurator
{
	internal class Configuration
	{
		private static readonly Configuration instance;

		static Configuration()
		{
			instance = new Configuration();
		}

		private Configuration()
		{
		}

		public Configuration Instance => instance;
	}
}