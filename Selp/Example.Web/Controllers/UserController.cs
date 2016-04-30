namespace Example.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Http;
	using Entities;
	using Models;
	using Repositories.Validators;
	using Selp.Common.Entities;
	using Selp.Controller;
	using Selp.Interfaces;

	public class UserController : SelpController<UserModel, User, string>
	{
		public UserController(ISelpRepository<UserModel, User, string> repository) : base(repository)
		{
		}

		public override string ControllerName => "User";

		[HttpPost]
		public override IHttpActionResult Post(UserModel value)
		{
			throw new NotSupportedException();
		}

		[HttpPut]
		public override IHttpActionResult Put(string id, UserModel value)
		{
			throw new NotSupportedException();
		}

		[HttpDelete]
		public override IHttpActionResult Delete(string id)
		{
			throw new NotSupportedException();
		}

		[Route("api/user/login")]
		[HttpPost]
		public IHttpActionResult Login([FromBody] UserModel model)
		{
			var validator = new UserLoginValidator(model);
			validator.Validate();
			if (!validator.IsValid)
			{
				return Ok(new
				{
					valid = false,
					errors = validator.Errors
				});
			}

			IEnumerable<UserModel> result =
				Repository.GetByCustomExpression(d => d.Id == model.Id && d.Password == model.Password);
			if (result.Any())
			{
				return Ok(new {valid = true});
			}

			return NotFound();
		}

		[Route("api/user/signup")]
		[HttpPost]
		public IHttpActionResult Signup([FromBody] UserModel model)
		{
			RepositoryModifyResult<UserModel> result = Repository.Create(model);
			if (result.IsValid)
			{
				return Ok(new {valid = true});
			}
			return Ok(new
			{
				valid = false,
				errors = result.Errors
			});
		}
	}
}