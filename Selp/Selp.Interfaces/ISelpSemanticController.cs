namespace Selp.Interfaces
{
	using System.Web.Http;

	public interface ISelpSemanticController<in TKey>
	{
		IHttpActionResult Get(TKey id);
		IHttpActionResult Get();

		IHttpActionResult GetPredicate();

		IHttpActionResult GetSubject();
	}
}