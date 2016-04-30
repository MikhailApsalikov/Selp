namespace Example.Web.Controllers
{
	using System;
	using System.Web.Http;
	using Entities;
	using Models;
	using Selp.Controller;
	using Selp.Interfaces;

	public class RegionController : SelpController<RegionModel, Region, int>
	{
		public RegionController(ISelpRepository<RegionModel, Region, int> repository) : base(repository)
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
	}
}