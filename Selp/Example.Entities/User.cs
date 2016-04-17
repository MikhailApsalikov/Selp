using System.ComponentModel.DataAnnotations;
using Selp.Interfaces;

namespace Example.Entities
{
	public class User : ISelpEntity<string>
	{
		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Password { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(50)]
		public string Id { get; set; }

		public int? PartyId { get; set; }

		public virtual Party Party { get; set; }
	}
}