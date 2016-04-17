using System.ComponentModel.DataAnnotations;
using Selp.Interfaces;

namespace Example.Entities
{
	public class Region : ISelpEntity<int>
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public int Id { get; set; }
	}
}