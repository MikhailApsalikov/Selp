namespace Example.Validators
{
	using Entities;
	using Models;
	using Selp.Common.Exceptions;
	using Selp.Interfaces;
	using Selp.Validator;

	public class UserSignupValidator : SelpValidator
	{
		public UserSignupValidator(User user, ISelpRepository<User, string> repository)
		{
			User = user;
			Repository = repository;
		}

		public User User { get; set; }

		public ISelpRepository<User, string> Repository { get; set; }

		public override string EntityName => "UserModel";

		protected override void ValidateLogic()
		{
			if (User == null)
			{
				AddError("Неверный логин/пароль");
				return;
			}

			if (string.IsNullOrWhiteSpace(User.Password))
			{
				AddError("Введите пароль", "Password");
			}

			if (string.IsNullOrWhiteSpace(User.Id))
			{
				AddError("Введите логин", "Id");
				return;
			}

			try
			{
				Repository.GetById(User.Id);
				AddError("Пользователь с таким логином уже существует", "Id");
			}
			catch (EntityNotFoundException)
			{
				// ignored
			}
		}
	}
}