namespace Example.Models
{
	using System.ComponentModel.DataAnnotations;
	using Selp.Common.Attributes;
	using Selp.Interfaces;

    public class UserModel : ISelpEntity<string>
	{
		[Required(ErrorMessage = "Введите пароль")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Введите логин")]
        [Predicate("http://xmlns.com/foaf/0.1/Name")]
		public string Id { get; set; }
	}
}