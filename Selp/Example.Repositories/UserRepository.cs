namespace Example.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using Entities;
	using Models;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;

	public class UserRepository : SelpRepository<UserModel, User, string>
	{
		public UserRepository(ExampleDbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => true;
		public override string FakeRemovingPropertyName => "IsInactive";
		public override IDbSet<User> DbSet => ((ExampleDbContext) DbContext).Users;

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

		protected override User MapModelToEntity(UserModel source, User destination)
		{
			destination.Password = source.Password;
			return destination;
		}

		protected override IQueryable<User> ApplyFilters(IQueryable<User> entities, BaseFilter filter)
		{
			if (string.IsNullOrWhiteSpace(filter.Search))
			{
				return entities;
			}
			return entities.Where(e => e.Id.Contains(filter.Search));
		}
	}
}