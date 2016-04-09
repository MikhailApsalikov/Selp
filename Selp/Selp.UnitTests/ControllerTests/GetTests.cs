namespace Selp.UnitTests
{
	using System.Web.Http;
	using Entities;
	using Fake;
	using Interfaces;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class GetTests
	{
		public ISelpController<FakeEntity, FakeEntity, int> Controller { get; set; }

		[TestInitialize]
		public void Initialise()
		{
			Controller = new FakeController();
		}

		[TestMethod]
		public void ControllerShouldGetByIdWithoutErrors()
		{
			IHttpActionResult result = Controller.Get(1);
		}
	}
}