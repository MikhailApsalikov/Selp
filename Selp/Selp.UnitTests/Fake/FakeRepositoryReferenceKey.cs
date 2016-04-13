namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;
	using System.Linq;
	using Configuration;
	using Entities;
	using Repository;

	public class FakeRepositoryReferenceKey : SelpRepository<FakeEntityReferenceKey, string>
	{
		public FakeRepositoryReferenceKey(DbContext dbContext,
			IDbSet<FakeEntityReferenceKey> dbSet, ISelpConfiguration configuration)
		{
			IsRemovingFake = false;
			DbContext = dbContext;
			DbSet = dbSet;
			Configuration = configuration;
		}

		public override bool IsRemovingFake { get; }
		public override string FakeRemovingPropertyName => "IsDeleted";
		public override DbContext DbContext { get; }
		public override IDbSet<FakeEntityReferenceKey> DbSet { get; }
		public override ISelpConfiguration Configuration { get; }

		public bool IsBeforeEventExecuted { get; set; }
		public bool IsAfterEventExecuted { get; set; }

		protected override IQueryable<FakeEntityReferenceKey> ApplyFilters(IQueryable<FakeEntityReferenceKey> dbSet, BaseFilter filter)
		{
			return dbSet.Where(s => s.Name.Contains(filter.Search)).AsQueryable();
		}
	}
}