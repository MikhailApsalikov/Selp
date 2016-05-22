namespace Example.Web
{
	using System.Data.Entity;
	using System.Web;
	using System.Web.Http;
	using Repositories;

	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
			Database.SetInitializer(new TestDataInitializer());
			UnityConfig.Register(GlobalConfiguration.Configuration);
		}
	}
}