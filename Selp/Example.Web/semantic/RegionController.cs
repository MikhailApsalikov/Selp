namespace Example.Web.semantic
{
	using Entities;
	using Interfaces.Repositories;
	using Models;
	using Selp.Semantic;

	public class RegionController : SelpSemanticController<RegionModel, RegionModel, Region, int>
    {
        public RegionController(IRegionRepository repository) : base(repository)
        {
        }

        protected override RegionModel MapEntityToModel(Region entity)
        {
            return new RegionModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        protected override RegionModel MapEntityToShortModel(Region entity)
        {
            return MapEntityToModel(entity);
        }
    }
}