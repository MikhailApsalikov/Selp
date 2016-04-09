namespace Selp.Interfaces
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;

	public interface ISelpRepository<TEntity, TKey> where TEntity : ISelpEntitiy<TKey>
	{
		IQueryable<TEntity> GetAll();
		TEntity GetById(int id);

		IQueryable<TEntity> GetByFilter(BaseFilter filter);

		IQueryable<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> filter);

		TEntity Create(TEntity item);

		TEntity Update(TEntity item);

		void Remove(TEntity item);

		void RemoveById(TKey key);
	}
}