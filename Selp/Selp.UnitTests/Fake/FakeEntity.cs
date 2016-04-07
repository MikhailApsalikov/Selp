namespace Selp.UnitTests.Fake
{
	using Interfaces;

	public class FakeEntity : ISelpEntitiy<int>
	{
		public string Name { get; set; }
		public bool IsDeleted { get; set; }

		public string Description { get; set; }
		public int Id { get; set; }
	}
}