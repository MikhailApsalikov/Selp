namespace Selp.UnitTests.Fake
{
	using System.ComponentModel.DataAnnotations;
	using Interfaces;

	public class FakeEntity : ISelpEntity<int>
	{
		[MaxLength(50)]
		public string Name { get; set; }

		public bool IsDeleted { get; set; }

		public string Description { get; set; }
		public int Id { get; set; }
	}
}