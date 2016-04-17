namespace Example.Entities
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class Party : ISelpEntity<int>
	{
		[MaxLength(50)]
		[MinLength(3)]
		[Required]
		public string Surname { get; set; }

		[MaxLength(50)]
		[MinLength(3)]
		[Required]
		public string Name { get; set; }

		[MaxLength(50)]
		[MinLength(3)]
		public string Midname { get; set; }

		public DateTime BirthDate { get; set; }

		public virtual ICollection<Document> Documents { get; set; }

		public virtual ICollection<Policy> Policies { get; set; }

		public int Id { get; set; }
	}
}