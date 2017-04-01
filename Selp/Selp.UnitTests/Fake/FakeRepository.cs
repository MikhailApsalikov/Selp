namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;
	using System.Linq;
	using Common.Entities;
	using Interfaces;
	using Repository;
	using VDS.Common.Tries;

	public class FakeRepository : SelpRepository<FakeEntity, int>
	{
		public FakeRepository(bool isRemovingFake, DbContext dbContext,
			IDbSet<FakeEntity> dbSet, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
			IsRemovingFake = isRemovingFake;
			DbSet = dbSet;
			IsMergeCalled = false;
		}

		public override bool IsRemovingFake { get; }

		public bool IsMergeCalled { get; set; }
		public override string FakeRemovingPropertyName => "IsDeleted";
		public override IDbSet<FakeEntity> DbSet { get; }

		public bool IsBeforeEventExecuted { get; set; }
		public bool IsAfterEventExecuted { get; set; }

		protected override void MarkAsModified(FakeEntity entity)
		{
		}

		protected override FakeEntity Merge(FakeEntity source, FakeEntity destination)
		{
			destination.Name = source.Name;
			destination.Description = source.Description;
			IsMergeCalled = true;
			return destination;
		}

		protected override IQueryable<FakeEntity> ApplyFilters(IQueryable<FakeEntity> dbSet, BaseFilter filter)
		{
			if (string.IsNullOrWhiteSpace(filter.Search))
			{
				return dbSet;
			}

			return dbSet.Where(s => s.Name.Contains(filter.Search));
		}

		protected override void OnCreating(FakeEntity item)
		{
			IsBeforeEventExecuted = true;
		}

		protected override void OnCreated(FakeEntity item)
		{
			IsAfterEventExecuted = true;
		}

		protected override void OnUpdating(int key, FakeEntity item)
		{
			IsBeforeEventExecuted = true;
		}

		protected override void OnUpdated(int key, FakeEntity item)
		{
			IsAfterEventExecuted = true;
		}

		protected override void OnRemoving(int key, FakeEntity item)
		{
			IsBeforeEventExecuted = true;
		}

		protected override void OnRemoved(int key)
		{
			IsAfterEventExecuted = true;
		}
	}
}