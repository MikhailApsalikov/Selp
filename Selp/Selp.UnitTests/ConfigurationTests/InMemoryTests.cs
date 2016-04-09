namespace Selp.UnitTests.ConfigurationTests
{
	using Configuration;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class InMemoryTests
	{
		private ISelpConfiguration configuration;

		[TestInitialize]
		public void MyTestInitialize()
		{
			configuration = SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory);
		}

		[TestMethod]
		public void DefaultPageSizeTest()
		{
			configuration.DefaultPageSize = 500;
			Assert.AreEqual(500, configuration.DefaultPageSize, "Default page size hasn't saved to configuration");
		}
	}
}