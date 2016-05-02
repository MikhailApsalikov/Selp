namespace SampleApplication.App_Start
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using System.Web.Http.Dependencies;
	using Example.Entities;
	using Example.Models;
	using Example.Repositories;
	using Example.Repositories.AdditionalInterfaces;
	using Microsoft.Practices.Unity;
	using Selp.Configuration;
	using Selp.Interfaces;

	/// <summary>
	///     Bootstrapper for the application.
	/// </summary>
	public static class UnityConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var dbContext = new ExampleDbContext();
			var container = new UnityContainer();
			container.RegisterType<ISelpConfiguration, InMemoryConfiguration>();
			container.RegisterType<ISelpRepository<UserModel, User, string>, UserRepository>(new InjectionConstructor(dbContext,
				container.Resolve<ISelpConfiguration>()));
			container.RegisterType<ISelpRepository<RegionModel, Region, int>, RegionRepository>(new InjectionConstructor(dbContext,
				container.Resolve<ISelpConfiguration>()));
			container.RegisterType<IAttachmentRepository, AttachmentRepository>(new InjectionConstructor(dbContext,
				container.Resolve<ISelpConfiguration>()));
            container.RegisterType<ISelpRepository<PolicyModel, Policy, int>, PolicyRepository>(new InjectionConstructor(dbContext,
                container.Resolve<ISelpConfiguration>()));
            config.DependencyResolver = new UnityResolver(container);
		}
	}

	public class UnityResolver : IDependencyResolver
	{
		protected IUnityContainer container;

		public UnityResolver(IUnityContainer container)
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			this.container = container;
		}

		public object GetService(Type serviceType)
		{
			try
			{
				return container.Resolve(serviceType);
			}
			catch (ResolutionFailedException)
			{
				return null;
			}
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return container.ResolveAll(serviceType);
			}
			catch (ResolutionFailedException)
			{
				return new List<object>();
			}
		}

		public IDependencyScope BeginScope()
		{
			var child = container.CreateChildContainer();
			return new UnityResolver(child);
		}

		public void Dispose()
		{
			container.Dispose();
		}
	}
}