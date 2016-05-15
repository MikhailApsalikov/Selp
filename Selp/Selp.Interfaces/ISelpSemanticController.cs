using System.Web.Http;

namespace Selp.Interfaces
{
    internal interface ISelpSemanticController<in TModel, in TKey> where TModel : ISelpEntity<TKey>
    {
        IHttpActionResult Get();

        IHttpActionResult GetPredicate();

        IHttpActionResult GetSubject();
    }
}