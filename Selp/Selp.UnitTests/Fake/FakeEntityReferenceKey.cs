namespace Selp.UnitTests.Fake
{
	using Interfaces;

	public class FakeEntityReferenceKey : ISelpEntity<string>
	{
		public string Name { get; set; }
		public string Id { get; set; }
	}
}