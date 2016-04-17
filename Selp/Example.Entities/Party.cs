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
		public string FirstName { get; set; }

		[MaxLength(50)]
		[MinLength(3)]
		[Required]
		public string LastName { get; set; }

		[MaxLength(50)]
		[MinLength(3)]
		public string MiddleName { get; set; }

		[MaxLength(4000)]
		public string Address { get; set; }

		[MaxLength(50)]
		public string Email { get; set; }

		[MaxLength(20)]
		public string Phone { get; set; }

		public DateTime BirthDate { get; set; }

		public virtual ICollection<Passport> Documents { get; set; }

		public virtual ICollection<Policy> Policies { get; set; }

		public int Id { get; set; }
	}
}