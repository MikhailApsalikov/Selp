namespace Example.Models
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class UserModel : ISelpEntity<int>
	{
		[Required]
		public string Surname { get; set; }

		[Required]
		public string Name { get; set; }

		public string Midname { get; set; }
		public int Id { get; set; }
	}
}