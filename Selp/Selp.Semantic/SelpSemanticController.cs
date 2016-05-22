namespace Selp.Semantic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using Core;
    using Interfaces;
    using VDS.RDF;
    using VDS.RDF.Writing;

    public abstract class SelpSemanticController<TShortModel, TModel, TEntity, TKey> : ApiController,
        ISelpSemanticController<TKey>
        where TShortModel : ISelpEntity<TKey> where TModel : ISelpEntity<TKey> where TEntity : ISelpEntity<TKey>
    {
        protected SelpSemanticController(ISelpRepository<TEntity, TKey> repository, IRdfWriter rdfWriter)
        {
            Repository = repository;
            RdfWriter = rdfWriter;
        }

        protected ISelpRepository<TEntity, TKey> Repository { get; }

        protected IRdfWriter RdfWriter { get; }

        public HttpResponseMessage Get(TKey id)
        {
            TEntity entity = Repository.GetById(id);
            TModel model = MapEntityToModel(entity);
            IGraph graph = SemanticCore<TModel, TKey>.GenerateGraph(model);
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    StringWriter.Write(graph, RdfWriter),
                    Encoding.Unicode,
                    GetMimeType()
                    )
            };
        }

        public HttpResponseMessage Get()
        {
            List<TEntity> entities = Repository.GetAll();
            List<TShortModel> model = entities.Select(MapEntityToShortModel).ToList();
            IGraph graph = SemanticCore<TShortModel, TKey>.GenerateGraph(model);
            return new HttpResponseMessage
            {
                Content = new StringContent(
                    StringWriter.Write(graph, RdfWriter),
                    Encoding.Unicode,
                    GetMimeType()
                    )
            };
        }

        protected virtual string GetMimeType()
        {
            return "application/rdf+xml";
        }

        protected abstract TModel MapEntityToModel(TEntity entity);

        protected abstract TShortModel MapEntityToShortModel(TEntity entity);
    }
}