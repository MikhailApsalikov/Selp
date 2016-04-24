namespace Example.Models
{
	using System.Reflection;
	using Selp.Interfaces;
	using System.ComponentModel.DataAnnotations;

	public class UserModel : ISelpEntity<string>
	{
		[Required(ErrorMessage = "Введите пароль")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Введите логин")]
		public string Id { get; set; }
	}
}