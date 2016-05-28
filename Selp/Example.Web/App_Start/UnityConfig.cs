namespace Example.Web
{
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using System.Web.Http.Dependencies;
	using Interfaces.Repositories;
	using Microsoft.Practices.Unity;
	using Repositories;
	using Selp.Configuration;
	using Selp.Interfaces;
	using VDS.RDF;
	using VDS.RDF.Writing;

	/// <summary>
	///     Bootstrapper for the application.
	/// </summary>
	public static class UnityConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var dbContext = new ExampleDbContext();
			var container = new UnityContainer();

			container.RegisterType<IRdfWriter, RdfXmlWriter>(new InjectionConstructor());

			container.RegisterType<ISelpConfiguration, InMemoryConfiguration>();
			var efConstructorParameter = new InjectionConstructor(dbContext, container.Resolve<ISelpConfiguration>());

			container.RegisterType<IUserRepository, UserRepository>(efConstructorParameter);
			container.RegisterType<IRegionRepository, RegionRepository>(efConstructorParameter);
			container.RegisterType<IAttachmentRepository, AttachmentRepository>(efConstructorParameter);
			container.RegisterType<IPolicyRepository, PolicyRepository>(efConstructorParameter);
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
			IUnityContainer child = container.CreateChildContainer();
			return new UnityResolver(child);
		}

		public void Dispose()
		{
			container.Dispose();
		}
	}
}