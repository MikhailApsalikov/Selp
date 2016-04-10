namespace Selp.Repository
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;
	using Interfaces;
	using Validator;

	public abstract class SelpRepository<TEntity, TKey> : ISelpRepository<TEntity, TKey> where TEntity : class, ISelpEntitiy<TKey>
	{
		protected abstract IQueryable<TEntity> ApplyFilters(BaseFilter filter);

		public abstract bool IsRemovingFake { get; }

		public abstract bool FakeRemovingPropertyName { get; }

		public abstract DbContext DbContext { get; }

		public abstract IDbSet<TEntity> DbSet { get; }

		public SelpValidator CreateValidator { get; set; }

		public SelpValidator UpdateValidator { get; set; }

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