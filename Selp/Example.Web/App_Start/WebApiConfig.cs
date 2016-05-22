namespace Example.Web
{
	using System.Web.Http;
	using System.Web.Http.Dispatcher;
	using App_Start;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.Routes.MapHttpRoute(
				"DefaultApi",
				"{namespace}/{controller}/{id}",
				new {id = RouteParameter.Optional}
				);
			config.Services.Replace(typeof (IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
			SemanticInitializer.SemanticInitialize();
		}
	}
}