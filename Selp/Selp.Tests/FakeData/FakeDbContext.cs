namespace Selp.Tests.FakeData
{
	using Kernel.BaseClasses;

	internal class FakeDbContext : SelpDbContext
	{
		public FakeDbSet<FakeEntity> FakeObjectDbSet { get; set; }
	}
}