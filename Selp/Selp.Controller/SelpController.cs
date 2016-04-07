namespace Selp.Controller
{
	using System;
	using System.Web.Http;
	using Entities;
	using Interfaces;

	public abstract class SelpController<TModel, TEntity, TKey> : ISelpController<TModel, TEntity, TKey>
		where TModel : ISelpEntitiy<TKey> where TEntity : ISelpEntitiy<TKey>
	{
		public abstract string ControllerName { get; }

		public IHttpActionResult Delete(TKey id)
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult Get(TKey id)
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult Get([FromUri] BaseFilter query)
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult Post([FromBody] TModel value)
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult Put(TKey id, [FromBody] TModel value)
		{
			throw new NotImplementedException();
		}
	}
}