namespace Example.Web.Ninject
{
	using Entities;
	using global::Ninject.Modules;
	using Models;
	using Repositories;
	using Selp.Configuration;
	using Selp.Interfaces;

	public class ExampleModule : NinjectModule
	{
		public override void Load()
		{
			ExampleDbContext dbContext = new ExampleDbContext();
			Bind<ISelpConfiguration>().To<InMemoryConfiguration>().InSingletonScope();
			Bind<ISelpRepository<UserModel, User, string>>().To<UserRepository>().WithConstructorArgument("dbContext", dbContext);
		}
	}
}