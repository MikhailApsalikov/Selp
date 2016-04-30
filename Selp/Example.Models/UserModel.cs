namespace Example.Models
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Interfaces;

	public class UserModel : ISelpEntity<string>
	{
		[Required(ErrorMessage = "Введите пароль")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Введите логин")]
		public string Id { get; set; }
	}
}