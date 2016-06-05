namespace Example.Web.api
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using Entities;
	using Interfaces.Repositories;
	using Models;
	using Selp.Common.Entities;
	using Selp.Controller;

	public class RegionController : SelpController<RegionModel, RegionModel, Region, int>
	{
		public RegionController(IRegionRepository repository) : base(repository)
		{
		}

		public override string ControllerName => "Region";

		[HttpPost]
		public override IHttpActionResult Post(RegionModel value)
		{
			throw new NotSupportedException();
		}

		[HttpPut]
		public override IHttpActionResult Put(int id, RegionModel value)
		{
			throw new NotSupportedException();
		}

		public override IHttpActionResult Delete(int id)
		{
			throw new NotSupportedException();
		}

	    public override IHttpActionResult Get(BaseFilter filter)
	    {
            try
            {
                List<RegionModel> data = Repository.GetAll().Select(MapEntityToShortModel).ToList();
                return Ok(data);
            }
            catch (Exception e)
            {
                return HandleException(e);
            }
        }

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

		protected override RegionModel MapEntityToShortModel(Region entity)
		{
			return MapEntityToModel(entity);
		}
	}
}