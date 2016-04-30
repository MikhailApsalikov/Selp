﻿namespace Example.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using Entities;
	using Models;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;

	public class RegionRepository : SelpRepository<RegionModel, Region, int>
	{
		public RegionRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => false;
		public override string FakeRemovingPropertyName => null;
		public override IDbSet<Region> DbSet => ((ExampleDbContext) DbContext).Regions;

		protected override RegionModel MapEntityToModel(Region entity)
		{
			return new RegionModel
			{
				Id = entity.Id,
				Name = entity.Name
			};
		}

		protected override Region MapModelToEntity(RegionModel model)
		{
			return new Region
			{
				Id = model.Id,
				Name = model.Name
			};
		}

		protected override Region MapModelToEntity(RegionModel source, Region destination)
		{
			destination.Name = source.Name;
			return destination;
		}

		protected override IQueryable<Region> ApplyFilters(IQueryable<Region> entities, BaseFilter filter)
		{
			if (string.IsNullOrWhiteSpace(filter.Search))
			{
				return entities;
			}
			return entities.Where(e => e.Name.Contains(filter.Search));
		}
	}
}