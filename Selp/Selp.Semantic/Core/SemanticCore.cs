namespace Selp.Semantic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using VDS.RDF;

    public static class SemanticCore<T>
    {
        public static IGraph GenerateGraph(T model)
        {
            var graph = new Graph();
            AddItem(model, graph);
            return graph;
        }

        public static IGraph GenerateGraph(IEnumerable<T> models)
        {
            var graph = new Graph();
            foreach (T model in models)
            {
                AddItem(model, graph);
            }

            return graph;
        }

        private static Uri ConstructSubjectUrl(T model)
        {
            // имя сайта/каррент юрл
            return UriFactory.Create("http://localhost/subjectUrl");
        }

        private static Uri ConstructPredicateUrl(PropertyInfo property)
        {
            return UriFactory.Create("http://localhost/predicate/" + property.Name);
        }

        private static INode ConstructObjectValue(object value, Graph graph)
        {
            return graph.CreateUriNode(UriFactory.Create("http://localhost/objectUrl" + value));
        }

        private static void AddItem(T model, Graph graph)
        {
            Type modelType = model.GetType();
            IUriNode subjectNode = graph.CreateUriNode(ConstructSubjectUrl(model));
            foreach (PropertyInfo propertyInfo in GetProperties(modelType))
            {
                IUriNode predicateNode = graph.CreateUriNode(ConstructPredicateUrl(propertyInfo));
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