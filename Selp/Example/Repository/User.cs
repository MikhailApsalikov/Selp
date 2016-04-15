namespace Example.Repository
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class User : ISelpEntity<int>
	{
		[Required]
		public string Surname { get; set; }

		[Required]
		public string Name { get; set; }

		public string Midname { get; set; }
		public int Id { get; set; }
	}
}