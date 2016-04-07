namespace Selp.UnitTests
{
	using Entities;
	using Fake;
	using Interfaces;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class ControllerTests
	{
		public ISelpController<FakeEntity, FakeEntity, int> Controller { get; set; }

		[TestInitialize]
		public void Initialise()
		{
			Controller = new FakeController();
		}

		[TestMethod]
		public void ControllerShouldGetAllWithoutErrors()
		{
			Controller.Get(new BaseFilter());
		}

		[TestMethod]
		public void ControllerShouldGetByIdWithoutErrors()
		{
			Controller.Get(1);
		}

		[TestMethod]
		public void ControllerShouldCreateWithoutErrors()
		{
			Controller.Post(new FakeEntity());
		}

		[TestMethod]
		public void ControllerShouldUpdateWithoutErrors()
		{
			Controller.Put(1, new FakeEntity());
		}

		[TestMethod]
		public void ControllerShouldDeleteWithoutErrors()
		{
			Controller.Delete(1);
		}
	}
}