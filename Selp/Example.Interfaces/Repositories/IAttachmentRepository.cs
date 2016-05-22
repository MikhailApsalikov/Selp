namespace Example.Interfaces.Repositories
{
	using System;
	using Entities;
	using Selp.Interfaces;

	public interface IAttachmentRepository : ISelpRepository<Attachment, Guid>
	{
		void Upload(Guid id, byte[] content);
		byte[] Download(Guid id);
	}
}