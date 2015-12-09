namespace Selp.Tests.FakeData
{
	using Kernel.Interfaces;

	internal class FakeEntity : ISelpEntity<int>
	{
		public int Id { get; set; }

		public string Name { get; set; }
	}
}