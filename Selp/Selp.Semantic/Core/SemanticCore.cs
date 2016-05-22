namespace Selp.Semantic.Core
{
    using System;
    using System.Reflection;
    using VDS.RDF;

    public static class SemanticCore<T>
    {
        public static IGraph GenerateGraph(T model)
        {
            var graph = new Graph();
            Type modelType = model.GetType();
            var subjectNode = graph.CreateUriNode(ConstructSubjectUrl(model));
            foreach (PropertyInfo propertyInfo in modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var predicateNode = graph.CreateUriNode(ConstructPredicateUrl(propertyInfo));
                var objectNode = ConstructObjectValue(propertyInfo.GetValue(model), graph);

                graph.Assert(new Triple(subjectNode, predicateNode, objectNode));
            }

            return graph;
        }

        public static Uri ConstructSubjectUrl(T model)
        {
            return UriFactory.Create("http://localhost/subjectUrl");
        }

        public static Uri ConstructPredicateUrl(PropertyInfo property)
        {
            return UriFactory.Create("http://localhost/predicate/" + property.Name);
        }

        public static INode ConstructObjectValue(object value, Graph graph)
        {
            return graph.GetUriNode(UriFactory.Create("http://localhost/objectUrl"+value.ToString()));
        }
    }
}