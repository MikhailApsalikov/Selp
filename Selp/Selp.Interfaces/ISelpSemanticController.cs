using System.Web.Http;

namespace Selp.Interfaces
{
    public interface ISelpSemanticController<in TModel, in TKey> where TModel : ISelpEntity<TKey>
    {
        IHttpActionResult Get();

        IHttpActionResult GetPredicate();

        IHttpActionResult GetSubject();
    }
}