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

		[NonAction]
		protected virtual TModel MapEntityToModel(TEntity entity)
		{
			throw new NotImplementedException();
		}

		[NonAction]
		protected virtual TEntity MapModelToEntity(TModel entity)
		{
			throw new NotImplementedException();
		}

		public virtual IHttpActionResult Delete(TKey id)
		{
			throw new NotImplementedException();
		}

		public virtual IHttpActionResult Get(TKey id)
		{
			throw new NotImplementedException();
		}

		public virtual IHttpActionResult Get([FromUri] BaseFilter query)
		{
			throw new NotImplementedException();
		}

		public virtual IHttpActionResult Post([FromBody] TModel value)
		{
			throw new NotImplementedException();
		}

		public virtual IHttpActionResult Put(TKey id, [FromBody] TModel value)
		{
			throw new NotImplementedException();
		}
	}
}