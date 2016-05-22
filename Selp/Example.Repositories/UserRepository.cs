namespace Example.Repositories
{
	using System.Data.Entity;
	using System.Linq;
	using Entities;
	using Interfaces.Repositories;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;
	using Validators;

	public class UserRepository : SelpRepository<User, string>, IUserRepository
	{
		public UserRepository(ExampleDbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => true;
		public override string FakeRemovingPropertyName => "IsInactive";
		public override IDbSet<User> DbSet => ((ExampleDbContext) DbContext).Users;

		protected override User Merge(User source, User destination)
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

		protected override void OnCreating(User item)
		{
			base.OnCreating(item);
			CreateValidator = new UserSignupValidator(item, this);
		}
	}
}