namespace Example.Controllers
{
	using Models;
	using Repository;
	using Selp.Controller;
	using Selp.Interfaces;

	public class UserController : SelpController<UserModel, User, int>
	{
		public UserController(ISelpRepository<UserModel, User, int> repository) : base(repository)
		{
		}

		public override string ControllerName => "User";
	}
}