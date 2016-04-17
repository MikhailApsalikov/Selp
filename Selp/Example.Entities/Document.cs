namespace Example.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class Document : ISelpEntity<int>
	{
		[Required]
		[MaxLength(50)]
		public string Seria { get; set; }

		[Required]
		[MaxLength(50)]
		public string Number { get; set; }

		[Required]
		[MaxLength(4000)]
		public string IssuedBy { get; set; }

		public DateTime IssuedDate { get; set; }

		public int PartyId { get; set; }
		public virtual Party Party { get; set; }
		public int Id { get; set; }
	}
}