namespace Example.Repositories.Validators
{
	using Models;
	using Selp.Repository.Validator;

	public class UserLoginValidator : SelpValidator
	{
		public UserLoginValidator(UserModel user)
		{
			User = user;
		}

		public UserModel User { get; set; }

		public override string EntityName => "Login";

		protected override void ValidateLogic()
		{
			if (User == null)
			{
				AddError("Неверный логин/пароль");
				return;
			}

			if (string.IsNullOrWhiteSpace(User.Id))
			{
				AddError("Введите логин", "Id");
			}

			if (string.IsNullOrWhiteSpace(User.Password))
			{
				AddError("Введите пароль", "Password");
			}
		}
	}
}