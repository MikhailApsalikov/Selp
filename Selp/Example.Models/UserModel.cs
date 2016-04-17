namespace Example.Models
{
	using Selp.Interfaces;

	public class UserModel : ISelpEntity<string>
	{
		public string Password { get; set; }
		public string Id { get; set; }
	}
}