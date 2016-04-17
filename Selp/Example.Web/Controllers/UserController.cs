namespace Example.Web.Controllers
{
	using System;
	using System.Web.Http;
	using Entities;
	using Models;
	using Selp.Controller;
	using Selp.Interfaces;

	public class UserController : SelpController<UserModel, User, string>
	{
		public UserController(ISelpRepository<UserModel, User, string> repository) : base(repository)
		{
		}

		public override string ControllerName => "User";

		public override IHttpActionResult Put(string id, UserModel value)
		{
			throw new NotSupportedException();
		}

		public override IHttpActionResult Delete(string id)
		{
			throw new NotSupportedException();
		}
	}
}