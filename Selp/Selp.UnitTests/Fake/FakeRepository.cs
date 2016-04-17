namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;
	using System.Linq;
	using Common.Entities;
	using Configuration;
	using Interfaces;
	using Repository;

	public class FakeRepository : SelpRepository<FakeEntity, FakeEntity, int>
	{
		public FakeRepository(bool isRemovingFake, DbContext dbContext,
			IDbSet<FakeEntity> dbSet, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
			IsRemovingFake = isRemovingFake;
			DbSet = dbSet;
		}

		public override bool IsRemovingFake { get; }
		public override string FakeRemovingPropertyName => "IsDeleted";
		public override IDbSet<FakeEntity> DbSet { get; }

		public bool IsBeforeEventExecuted { get; set; }
		public bool IsAfterEventExecuted { get; set; }

		protected override void MarkAsModified(FakeEntity entity)
		{
		}

		protected override FakeEntity MapEntityToModel(FakeEntity entity)
		{
			return entity;
		}

		protected override FakeEntity MapModelToEntity(FakeEntity model)
		{
			return model;
		}

		protected override FakeEntity MapModelToEntity(FakeEntity source, FakeEntity destination)
		{
			destination.Name = source.Name;
			destination.Description = source.Description;

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