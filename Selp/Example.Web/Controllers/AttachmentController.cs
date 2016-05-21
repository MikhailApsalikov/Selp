namespace Example.Web.Controllers
{
	using System;
	using System.Web.Http;
	using Entities;
	using Models;
	using Repositories.AdditionalInterfaces;
	using Selp.Controller;

	//NYI
	public class AttachmentController : SelpController<AttachmentModel, AttachmentModel, Attachment, Guid>
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

		protected override AttachmentModel MapEntityToModel(Attachment entity)
		{
			return new AttachmentModel
			{
				Id = entity.Id,
				Description = entity.Description,
				FileName = entity.FileName,
				FileSize = entity.FileSize,
				Uploaded = entity.Uploaded
			};
		}

		protected override Attachment MapModelToEntity(AttachmentModel model)
		{
			return new Attachment
			{
				Id = model.Id,
				Description = model.Description,
				FileName = model.FileName,
				FileSize = model.FileSize,
				Uploaded = model.Uploaded
			};
		}

		protected override AttachmentModel MapEntityToShortModel(Attachment entity)
		{
			throw new NotImplementedException();
		}

		//download
		[HttpGet]
		public override IHttpActionResult Get(Guid id)
		{
			return base.Get(id);
		}
	}
}