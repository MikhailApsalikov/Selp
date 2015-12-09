namespace Selp.Tests.FakeData
{
	using System.Data.Entity;
	using Kernel.BaseClasses;

	internal class FakeRepository : SelpRepository<FakeEntity, int, FakeDbContext>
	{
		protected override IDbSet<FakeEntity> GetDbSet()
		{
			return DbContext.FakeObjectDbSet;
		}
	}
}