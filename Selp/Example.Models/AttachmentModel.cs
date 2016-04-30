namespace Example.Models
{
	using System;
	using Selp.Interfaces;

	public class AttachmentModel : ISelpEntity<Guid>
	{
		public string FileName { get; set; }

		public string Description { get; set; }

		public int FileSize { get; set; }

		public DateTime Uploaded { get; set; }

		public int PolicyId { get; set; }
		public Guid Id { get; set; }
	}
}