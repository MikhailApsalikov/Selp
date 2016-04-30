namespace Example.Repositories.AdditionalInterfaces
{
	using System;
	using Entities;
	using Models;
	using Selp.Interfaces;

	public interface IAttachmentRepository : ISelpRepository<AttachmentModel, Attachment, Guid>
	{
		void Upload(Guid id, byte[] content);
		byte[] Download(Guid id);
	}
}