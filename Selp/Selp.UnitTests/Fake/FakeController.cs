namespace Selp.UnitTests.Fake
{
	using Controller;

	internal class FakeController : SelpController<FakeEntity, FakeEntity, int>
	{
		public override string ControllerName => "FakeController";
	}
}