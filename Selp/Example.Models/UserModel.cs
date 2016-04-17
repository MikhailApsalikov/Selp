using Selp.Interfaces;

namespace Example.Models
{
	public class UserModel : ISelpEntity<string>
	{
		public string Password { get; set; }
		public string Id { get; set; }
	}
}