using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Selp.UnitTests.ConfigurationTests
{
	[TestClass]
	public class Sanity
	{

		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext) { }

		[ClassCleanup()]
		public static void MyClassCleanup() { }

		[TestInitialize()]
		public void MyTestInitialize() { }

		[TestCleanup()]
		public void MyTestCleanup() { }

		[TestMethod]
		public void isFakeDeleteEnabledTest()
		{
		}
	}
}
