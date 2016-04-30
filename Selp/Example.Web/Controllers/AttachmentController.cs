namespace Example.Web.Controllers
{
	using System;
	using System.Web.Http;
	using Entities;
	using Models;
	using Repositories.AdditionalInterfaces;
	using Selp.Controller;

	public class AttachmentController : SelpController<AttachmentModel, Attachment, Guid>
	{
		public AttachmentController(IAttachmentRepository repository) : base(repository)
		{
		}

		public override string ControllerName => "Attachment";

		//upload
		[HttpPost]
		public override IHttpActionResult Post(AttachmentModel value)
		{
			return base.Post(value);
		}

		//download
		[HttpGet]
		public override IHttpActionResult Get(Guid id)
		{
			return base.Get(id);
		}
	}
}