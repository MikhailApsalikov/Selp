namespace Example.Repositories
{
	using System.Data.Entity;
	using Entities;

	public class ExampleDbContext : DbContext
	{
		public IDbSet<User> Users { get; set; }
		public IDbSet<Policy> Policies { get; set; }

		public IDbSet<Party> Parties { get; set; }

		public IDbSet<Document> Documents { get; set; }
		public IDbSet<Region> Regions { get; set; }
		public IDbSet<Attachment> Attachments { get; set; }
	}
}