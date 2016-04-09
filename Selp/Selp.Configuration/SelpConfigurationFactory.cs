using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selp.Configuration
{
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
