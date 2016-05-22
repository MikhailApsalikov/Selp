namespace Selp.UnitTests.SemanticTests
{
	using System.Collections.Generic;
	using Common.Entities;
	using Fake;
	using Interfaces;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Semantic.Core;
	using VDS.RDF.Writing;

	[TestClass]
	public class SanityTests
	{
		internal FakeSemanticController Controller { get; set; }

		static SanityTests()
		{
			SemanticTypeResolver.Instance.Register<FakeEntity>("http://www.fakeentity.com");
		}

		[TestInitialize]
		public void Initialise()
		{
			var mock = new Mock<ISelpRepository < FakeEntity, int>> ();
			mock.Setup((s) => s.GetAll()).Returns(() => new List<FakeEntity>()
			{
				new FakeEntity(),
				new FakeEntity(),
			});

			mock.Setup((s) => s.GetById(It.IsAny<int>())).Returns<int>((i) => new FakeEntity()
			{
				Id = i
			});

			Controller = new FakeSemanticController(mock.Object, new RdfXmlWriter());
		}

		[TestMethod]
		public void GetShouldWork()
		{
			Controller.Get();
		}

		[TestMethod]
		public void GetByIdShouldWork()
		{
			Controller.Get(1);
		}
	}
}