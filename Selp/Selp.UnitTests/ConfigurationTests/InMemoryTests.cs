using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selp.UnitTests.ConfigurationTests
{
	using Configuration;

	[TestClass]
	public class InMemoryTests
	{
		ISelpConfiguration configuration;

		[TestInitialize()]
		public void MyTestInitialize()
		{
			configuration = SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory);
		}

		[TestMethod]
		public void DefaultPageSizeTest()
		{
			configuration.DefaultPageSize = 500;
			Assert.AreEqual(500, configuration.DefaultPageSize);
		}
	}
}
