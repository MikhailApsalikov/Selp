namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;
	using System.Linq;
	using Configuration;
	using Entities;
	using Repository;

	public class FakeRepositoryReferenceKey : SelpRepository<FakeEntityReferenceKey, FakeEntityReferenceKey, string>
	{
		public FakeRepositoryReferenceKey(DbContext dbContext,
			IDbSet<FakeEntityReferenceKey> dbSet, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
			IsRemovingFake = false;
			DbSet = dbSet;
		}

		public override bool IsRemovingFake { get; }
		public override string FakeRemovingPropertyName => "IsDeleted";
		public override IDbSet<FakeEntityReferenceKey> DbSet { get; }

		public bool IsBeforeEventExecuted { get; set; }
		public bool IsAfterEventExecuted { get; set; }

		protected override void MarkAsModified(FakeEntityReferenceKey entity)
		{
		}

		protected override FakeEntityReferenceKey MapEntityToModel(FakeEntityReferenceKey entity)
		{
			return entity;
		}

		protected override FakeEntityReferenceKey MapModelToEntity(FakeEntityReferenceKey model)
		{
			return model;
		}

		protected override FakeEntityReferenceKey MapModelToEntity(FakeEntityReferenceKey source,
			FakeEntityReferenceKey destination)
		{
			destination.Name = source.Name;

			return destination;
		}

		protected override IQueryable<FakeEntityReferenceKey> ApplyFilters(IQueryable<FakeEntityReferenceKey> dbSet,
			BaseFilter filter)
		{
			return dbSet.Where(s => s.Name.Contains(filter.Search)).AsQueryable();
		}
	}
}