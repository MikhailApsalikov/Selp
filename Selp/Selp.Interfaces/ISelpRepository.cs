namespace Selp.Interfaces
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;

	public interface ISelpRepository<TEntity, TKey> where TEntity : ISelpEntitiy<TKey>
	{
		IQueryable<TEntity> GetAll();
		TEntity GetById(TKey id);

		IQueryable<TEntity> GetByFilter(BaseFilter filter);

		IQueryable<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression);

		RepositoryModifyResult<TEntity> Create(TEntity item);

		RepositoryModifyResult<TEntity> Update(TKey id, TEntity item);

		void Remove(TKey key);
	}
}