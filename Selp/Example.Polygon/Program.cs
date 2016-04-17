namespace Example.Polygon
{
	using System;
	using System.Data.Entity;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using Models;
	using Repositories;
	using Selp.Common.Entities;
	using Selp.Configuration;

	internal class Program
	{
		private static void Main(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
			Database.SetInitializer(new TestDataInitializer());
			var dbContext = new ExampleDbContext();
			var repo = new UserRepository(dbContext, new InMemoryConfiguration());
			RepositoryModifyResult<UserModel> result = repo.Create(new UserModel
			{
				Id = "Pr",
				Password = "Me"
			});
			var errors = result.Errors.ToList();

			Console.WriteLine();
		}
	}
}