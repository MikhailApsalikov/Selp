using System;
using System.ComponentModel.DataAnnotations;
using Selp.Interfaces;

namespace Example.Entities
{
	public class Attachment : ISelpEntity<Guid>
	{
		[Required]
		[MaxLength(255)]
		public string FileName { get; set; }

		[MaxLength(4000)]
		public string Description { get; set; }

		public int FileSize { get; set; }

		public DateTime Uploaded { get; set; }

		public int PolicyId { get; set; }

		public virtual Policy Policy { get; set; }

		public byte[] Content { get; set; }
		public Guid Id { get; set; }
	}
}