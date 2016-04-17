namespace Example.Entities
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class Region : ISelpEntity<int>
	{
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public int Id { get; set; }
	}
}