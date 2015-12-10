namespace Selp.Tests.FakeData
{
	using Kernel.BaseClasses;

	internal class FakeDbContext : SelpDbContext
	{
		public FakeDbContext()
		{
			FakeObjectDbSet = new FakeDbSet<FakeEntity>();
		}

		public FakeDbSet<FakeEntity> FakeObjectDbSet { get; set; }
	}
}