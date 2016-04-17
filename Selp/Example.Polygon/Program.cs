namespace Example.Polygon
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Models;
	using Repositories;
	using Selp.Configuration;

	internal class Program
	{
		private static void Main(string[] args)
		{
			Database.SetInitializer(new TestDataInitializer());
			ExampleDbContext dbContext = new ExampleDbContext();
			var repo = new UserRepository(dbContext, new InMemoryConfiguration());
			List<UserModel> users = repo.GetAll().ToList();
				
			Console.WriteLine();
		}
	}
}