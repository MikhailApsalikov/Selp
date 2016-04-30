namespace Example.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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

		[Route("api/user/login")]
		[HttpPost]
		public IHttpActionResult Login([FromBody] UserModel model)
		{
			if (model?.Id == null || model.Password == null)
			{
				return Ok(new {valid = false, error = "Введите логин/пароль"});
			}

			IEnumerable<UserModel> result =
				Repository.GetByCustomExpression(d => d.Id == model.Id && d.Password == model.Password);
			if (result.Any())
			{
				return Ok(new {valid = true});
			}

			return NotFound();
		}
	}
}