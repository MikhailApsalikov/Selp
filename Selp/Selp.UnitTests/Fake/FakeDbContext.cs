namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;

	public class FakeDbContext : DbContext
	{
		public virtual IDbSet<FakeEntity> FakeEntities { get; set; }

		public virtual IDbSet<FakeEntityReferenceKey> FakeEntitiesReferenceKey { get; set; }
	}
}