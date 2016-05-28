namespace Example.Web
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Dispatcher;
	using System.Web.Http.Routing;

	public class NamespaceHttpControllerSelector : IHttpControllerSelector
	{
		private const string NamespaceKey = "namespace";
		private const string ControllerKey = "controller";

		private readonly HttpConfiguration configuration;
		private readonly Lazy<Dictionary<string, HttpControllerDescriptor>> controllers;
		private readonly IHttpControllerSelector defaultSelector;
		private readonly HashSet<string> duplicates;

		public NamespaceHttpControllerSelector(HttpConfiguration config, IHttpControllerSelector defaultSelector)
		{
			configuration = config;
			duplicates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			controllers = new Lazy<Dictionary<string, HttpControllerDescriptor>>(InitializeControllerDictionary);
			this.defaultSelector = defaultSelector;
		}

		public HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			try
			{
				IHttpRouteData routeData = request.GetRouteData();
				if (routeData == null)
				{
					throw new HttpResponseException(HttpStatusCode.NotFound);
				}

				// Get the namespace and controller variables from the route data.
				var namespaceName = GetRouteVariable<string>(routeData, NamespaceKey);
				if (namespaceName == null)
				{
					throw new HttpResponseException(HttpStatusCode.NotFound);
				}

				var controllerName = GetRouteVariable<string>(routeData, ControllerKey);
				if (controllerName == null)
				{
					throw new HttpResponseException(HttpStatusCode.NotFound);
				}

				// Find a matching controller.
				string key = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", namespaceName, controllerName);

				HttpControllerDescriptor controllerDescriptor;
				if (controllers.Value.TryGetValue(key, out controllerDescriptor))
				{
					return controllerDescriptor;
				}
				if (duplicates.Contains(key))
				{
					throw new HttpResponseException(
						request.CreateErrorResponse(HttpStatusCode.InternalServerError,
							"Multiple controllers were found that match this request."));
				}
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			catch (HttpResponseException)
			{
				return defaultSelector.SelectController(request);
			}
		}

		public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
		{
			return controllers.Value;
		}

		private Dictionary<string, HttpControllerDescriptor> InitializeControllerDictionary()
		{
			var dictionary = new Dictionary<string, HttpControllerDescriptor>(StringComparer.OrdinalIgnoreCase);

			// Create a lookup table where key is "namespace.controller". The value of "namespace" is the last
			// segment of the full namespace. For example:
			// MyApplication.Controllers.V1.ProductsController => "V1.Products"
			IAssembliesResolver assembliesResolver = configuration.Services.GetAssembliesResolver();
			IHttpControllerTypeResolver controllersResolver = configuration.Services.GetHttpControllerTypeResolver();

			ICollection<Type> controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

			foreach (Type t in controllerTypes)
			{
				string[] segments = t.Namespace.Split(Type.Delimiter);

				// For the dictionary key, strip "Controller" from the end of the type name.
				// This matches the behavior of DefaultHttpControllerSelector.
				string controllerName = t.Name.Remove(t.Name.Length - DefaultHttpControllerSelector.ControllerSuffix.Length);

				string key = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", segments[segments.Length - 1], controllerName);

				// Check for duplicate keys.
				if (dictionary.Keys.Contains(key))
				{
					duplicates.Add(key);
				}
				else
				{
					dictionary[key] = new HttpControllerDescriptor(configuration, t.Name, t);
				}
			}

			// Remove any duplicates from the dictionary, because these create ambiguous matches. 
			// For example, "Foo.V1.ProductsController" and "Bar.V1.ProductsController" both map to "v1.products".
			foreach (string s in duplicates)
			{
				dictionary.Remove(s);
			}
			return dictionary;
		}

		// Get a value from the route data, if present.
		private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
		{
			object result = null;
			if (routeData.Values.TryGetValue(name, out result))
			{
				return (T) result;
			}
			return default(T);
		}
	}
}