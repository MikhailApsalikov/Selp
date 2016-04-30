﻿namespace Example.Repositories
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using AdditionalInterfaces;
	using Entities;
	using Models;
	using Selp.Common;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;

	public class AttachmentRepository : SelpRepository<AttachmentModel, Attachment, Guid>, IAttachmentRepository
	{
		public AttachmentRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => false;
		public override string FakeRemovingPropertyName => null;
		public override IDbSet<Attachment> DbSet => ((ExampleDbContext) DbContext).Attachments;

		public void Upload(Guid id, byte[] content)
		{
			Attachment entity = FindById(id);
			entity.Content = content;
			MarkAsModified(entity);
			DbContext.SaveChanges();
		}

		public byte[] Download(Guid id)
		{
			id.ThrowIfNull("ID cannot be null");
			return FindById(id).Content;
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

		protected override Attachment MapModelToEntity(AttachmentModel source, Attachment destination)
		{
			destination.Description = source.Description;
			destination.FileName = source.FileName;
			destination.FileSize = source.FileSize;
			destination.Uploaded = source.Uploaded;
			return destination;
		}

		protected override IQueryable<Attachment> ApplyFilters(IQueryable<Attachment> entities, BaseFilter filter)
		{
			if (string.IsNullOrWhiteSpace(filter.Search))
			{
				return entities;
			}
			return entities.Where(e => e.FileName.Contains(filter.Search));
		}
	}
}