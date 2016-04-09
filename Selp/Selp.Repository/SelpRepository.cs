namespace Selp.Repository
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;
	using Interfaces;

	public class SelpRepository<TEntity, TKey> : ISelpRepository<TEntity, TKey> where TEntity : ISelpEntitiy<TKey>
	{
		public IQueryable<TEntity> GetAll()
		{
			throw new NotImplementedException();
		}

		public TEntity GetById(int id)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public TEntity Create(TEntity item)
		{
			throw new NotImplementedException();
		}

		public TEntity Update(TEntity item)
		{
			throw new NotImplementedException();
		}

		public void Remove(TEntity item)
		{
			throw new NotImplementedException();
		}

		public void RemoveById(TKey key)
		{
			throw new NotImplementedException();
		}

		public IQueryable<TEntity> GetByFilter(BaseFilter filter)
		{
			throw new NotImplementedException();
		}
	}
}