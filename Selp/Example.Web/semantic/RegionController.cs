namespace Example.Web.semantic
{
	using Entities;
	using Interfaces.Repositories;
	using Models;
	using Selp.Semantic;
	using VDS.RDF;

    public class RegionController : SelpSemanticController<RegionModel, RegionModel, Region, int>
    {
        public RegionController(IRegionRepository repository, IRdfWriter rdfWriter) : base(repository, rdfWriter)
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

        protected override string GetMimeType()
        {
            return "text/txt";
        }
    }
}