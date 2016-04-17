namespace Example.Entities
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class User : ISelpEntity<string>
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Password { get; set; }

		public int? PartyId { get; set; }

		public virtual Party Party { get; set; }

		public bool IsInactive { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Id { get; set; }
	}
}