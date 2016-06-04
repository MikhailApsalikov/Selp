namespace Example.Repositories
{
	using System.Data.Entity;
	using Entities;

	public class ExampleDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Policy> Policies { get; set; }

		public DbSet<Party> Parties { get; set; }
		public DbSet<Region> Regions { get; set; }
	}
}