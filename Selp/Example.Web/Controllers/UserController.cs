namespace Example.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using Entities;
	using Models;
	using Selp.Common.Entities;
	using Selp.Controller;
	using Selp.Interfaces;
	using Validators;

	public class UserController : SelpController<UserModel, UserModel, User, string>
	{
		public UserController(ISelpRepository<User, string> repository) : base(repository)
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

			List<User> result =
				Repository.GetByCustomExpression(d => d.Id == model.Id && d.Password == model.Password);
			if (result.Count == 1)
			{
				return Ok(new {valid = true});
			}

			return NotFound();
		}

		[Route("api/user/signup")]
		[HttpPost]
		public IHttpActionResult Signup([FromBody] UserModel model)
		{
			RepositoryModifyResult<User> result = Repository.Create(MapModelToEntity(model));
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

		protected override UserModel MapEntityToModel(User entity)
		{
			return new UserModel
			{
				Id = entity.Id
			};
		}

		protected override User MapModelToEntity(UserModel model)
		{
			return new User
			{
				Id = model.Id,
				Password = model.Password
			};
		}

		protected override UserModel MapEntityToShortModel(User entity)
		{
			return MapEntityToModel(entity);
		}
	}
}