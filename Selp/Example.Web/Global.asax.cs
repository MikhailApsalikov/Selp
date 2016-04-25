namespace Example.Web
{
	using System.Data.Entity;
	using System.Web;
	using System.Web.Http;
	using Repositories;
	using SampleApplication.App_Start;

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