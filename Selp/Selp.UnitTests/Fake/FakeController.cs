namespace Selp.UnitTests.Fake
{
	using Controller;
	using Interfaces;

	internal class FakeController : SelpController<FakeEntity, FakeEntity, int>
	{
		public FakeController(ISelpRepository<FakeEntity, FakeEntity, int> repository) : base(repository)
		{
		}

		public override string ControllerName => "FakeController";
	}
}