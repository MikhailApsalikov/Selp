namespace Selp.Semantic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Common.Helpers;

	public class SemanticTypeResolver
    {
        private readonly Dictionary<Type, string> uriDictionary = new Dictionary<Type, string>();

        public void Register<T>(string baseUri)
        {
            Type type = typeof (T);
            uriDictionary.Add(type, baseUri);
        }

        public string Resolve(Type type)
        {
            return uriDictionary.ContainsKey(type) ? UriHelper.UrlCombine(HttpContext.Current.Request.GetApplicationPath(), uriDictionary[type]) : null;
        }

        public string Resolve<T>()
        {
            return Resolve(typeof (T));
        }

        #region Singleton

        public static SemanticTypeResolver Instance { get; }

        static SemanticTypeResolver()
        {
            Instance = new SemanticTypeResolver();
        }

        private SemanticTypeResolver()
        {
        }

        #endregion
    }
}