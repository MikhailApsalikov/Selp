namespace Example.Interfaces.Repositories
{
	using Entities;
	using Selp.Interfaces;

	public interface IUserRepository : ISelpRepository<User, string>
	{
	}
}