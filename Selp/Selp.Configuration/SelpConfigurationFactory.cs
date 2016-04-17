namespace Selp.Configuration
{
	using System;

	public static class SelpConfigurationFactory
	{
		public static ISelpConfiguration GetConfiguration(ConfigurationTypes configuration)
		{
			switch (configuration)
			{
				case ConfigurationTypes.InMemory:
					return InMemoryConfiguration.Instance;
				default:
					throw new ArgumentException("Invalid configuration type");
			}
		}
	}
}