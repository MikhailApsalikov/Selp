namespace Example.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using Entities;
	using Interfaces.Repositories;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;

	public class RegionRepository : SelpRepository<Region, int>, IRegionRepository
	{
		public RegionRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => false;
		public override string FakeRemovingPropertyName => null;
		public override IDbSet<Region> DbSet => ((ExampleDbContext) DbContext).Regions;

		protected override Region Merge(Region source, Region destination)
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