namespace Selp.Semantic
{
	using System;
	using System.Web.Http;
	using Interfaces;

	public abstract class SelpSemanticController<TShortModel, TModel, TEntity, TKey> : ISelpSemanticController<TKey>
		where TShortModel : ISelpEntity<TKey> where TModel : ISelpEntity<TKey> where TEntity : ISelpEntity<TKey>
	{
		protected SelpSemanticController(ISelpRepository<TEntity, TKey> repository)
		{
			Repository = repository;
		}

		protected ISelpRepository<TEntity, TKey> Repository { get; }

		public IHttpActionResult Get(TKey id)
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult Get()
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult GetPredicate()
		{
			throw new NotImplementedException();
		}

		public IHttpActionResult GetSubject()
		{
			throw new NotImplementedException();
		}

		protected abstract TModel MapEntityToModel(TEntity entity);

		protected abstract TShortModel MapEntityToShortModel(TEntity entity);
	}
}