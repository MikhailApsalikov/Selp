namespace Selp.UnitTests.Fake
{
	using Interfaces;

	public class FakeEntityReferenceKey : ISelpEntitiy<string>
	{
		public string Name { get; set; }
		public string Id { get; set; }
	}
}