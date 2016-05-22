namespace Selp.Semantic.Core
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Common.Attributes;
	using Interfaces;
	using VDS.RDF;

	public static class SemanticCore<T, TKey> where T : ISelpEntity<TKey>
	{
		public static IGraph GenerateGraph(T model)
		{
			var graph = new Graph();
			string baseUri = SemanticTypeResolver.Instance.Resolve<T>();
			InitGraph(graph, baseUri);
			AddItem(model, graph, baseUri);
			return graph;
		}

		public static IGraph GenerateGraph(IEnumerable<T> models)
		{
			var graph = new Graph();
			string baseUri = SemanticTypeResolver.Instance.Resolve<T>();
			InitGraph(graph, baseUri);
			foreach (T model in models)
			{
				AddItem(model, graph, baseUri);
			}

			return graph;
		}

		private static void InitGraph(Graph graph, string baseUri)
		{
			graph.BaseUri = UriFactory.Create(baseUri);
		}

		private static Uri ConstructSubjectUrl(string baseUrl, T model)
		{
			return UriFactory.Create($"{baseUrl}/{model.Id}");
		}

		private static Uri ConstructPredicateUrl(string baseUrl, PropertyInfo property)
		{
			var attr = (PredicateAttribute[]) property.GetCustomAttributes(typeof (PredicateAttribute), false);
			return UriFactory.Create(attr.Length > 0 ? attr[0].Uri : $"{baseUrl}/predicates/{property.Name}");
		}

		private static INode ConstructObjectValue(object value, Graph graph)
		{
			if (value == null)
			{
				return graph.CreateBlankNode();
			}

			// рассмотреть IEnumerable
			string baseUrl = SemanticTypeResolver.Instance.Resolve(value.GetType());
			if (baseUrl != null)
			{
				try
				{
					return graph.CreateUriNode(ConstructSubjectUrl(baseUrl, (dynamic) value));
				}
				catch
				{
					// ignored
				}
			}

			return graph.CreateLiteralNode(value.ToString());
		}

		private static void AddItem(T model, Graph graph, string baseUrl)
		{
			Type modelType = model.GetType();
			IUriNode subjectNode = graph.CreateUriNode(ConstructSubjectUrl(baseUrl, model));
			foreach (PropertyInfo propertyInfo in GetProperties(modelType))
			{
				IUriNode predicateNode = graph.CreateUriNode(ConstructPredicateUrl(baseUrl, propertyInfo));
				INode objectNode = ConstructObjectValue(propertyInfo.GetValue(model), graph);

				graph.Assert(new Triple(subjectNode, predicateNode, objectNode));
			}
		}

		private static PropertyInfo[] GetProperties(Type modelType)
		{
			return modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
	}
}