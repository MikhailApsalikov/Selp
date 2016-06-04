namespace Example.Interfaces
{
	using System.Data.Entity;
	using Entities;

	public interface IExampleDbContext
	{
		DbSet<User> Users { get; set; }
		DbSet<Policy> Policies { get; set; }

		DbSet<Party> Parties { get; set; }
		DbSet<Region> Regions { get; set; }
	}
}