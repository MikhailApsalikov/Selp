﻿namespace Selp.UnitTests.Fake
{
	using System.Data.Entity;
	using System.Linq;
	using Configuration;
	using Entities;
	using Repository;

	public class FakeRepository : SelpRepository<FakeEntity, FakeEntity, int>
	{
		public FakeRepository(bool isRemovingFake, DbContext dbContext,
			IDbSet<FakeEntity> dbSet, ISelpConfiguration configuration)
		{
			IsRemovingFake = isRemovingFake;
			DbContext = dbContext;
			DbSet = dbSet;
			Configuration = configuration;
		}

		public override bool IsRemovingFake { get; }
		public override string FakeRemovingPropertyName => "IsDeleted";
		public override DbContext DbContext { get; }
		public override IDbSet<FakeEntity> DbSet { get; }
		public override ISelpConfiguration Configuration { get; }

		public bool IsBeforeEventExecuted { get; set; }
		public bool IsAfterEventExecuted { get; set; }

		protected override FakeEntity MapEntityToModel(FakeEntity entity)
		{
			return entity;
		}

		protected override FakeEntity MapModelToEntity(FakeEntity entity)
		{
			return entity;
		}

		protected override IQueryable<FakeEntity> ApplyFilters(IQueryable<FakeEntity> dbSet, BaseFilter filter)
		{
			return dbSet.Where(s => s.Name.Contains(filter.Search)).AsQueryable();
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

		protected override void OnRemoving(int key)
		{
			IsBeforeEventExecuted = true;
		}

		protected override void OnRemoved(int key)
		{
			IsAfterEventExecuted = true;
		}
	}
}