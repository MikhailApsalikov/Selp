namespace Selp.Repository
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using Configuration;
	using Entities;
	using Interfaces;
	using Validator;

	public abstract class SelpRepository<TEntity, TKey> : ISelpRepository<TEntity, TKey>
		where TEntity : class, ISelpEntitiy<TKey>
	{
		public abstract bool IsRemovingFake { get; }

		public abstract string FakeRemovingPropertyName { get; }

		public abstract DbContext DbContext { get; }

		public abstract IDbSet<TEntity> DbSet { get; }

		public abstract ISelpConfiguration Configuration { get; }

		public SelpValidator CreateValidator { get; set; }

		public SelpValidator UpdateValidator { get; set; }

		public virtual IQueryable<TEntity> GetAll()
		{
			throw new NotImplementedException();
		}

		public virtual TEntity GetById(TKey id)
		{
			if (id == null)
			{
				throw new ArgumentException("ID cannot be null");
			}
			throw new NotImplementedException();
		}

		public virtual IQueryable<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual RepositoryModifyResult<TEntity> Create(TEntity item)
		{
			throw new NotImplementedException();
		}

		public virtual RepositoryModifyResult<TEntity> Update(TKey id, TEntity item)
		{
			throw new NotImplementedException();
		}

		public virtual void Remove(TKey key)
		{
			throw new NotImplementedException();
		}

		public virtual IQueryable<TEntity> GetByFilter(BaseFilter filter)
		{
			throw new NotImplementedException();
		}

		protected abstract IQueryable<TEntity> ApplyFilters(BaseFilter filter);

		protected virtual void OnCreating(TEntity item)
		{
		}

		protected virtual void OnCreated(TEntity item)
		{
		}

		protected virtual void OnUpdating(TKey key, TEntity item)
		{
		}

		protected virtual void OnUpdated(TKey key, TEntity item)
		{
		}

		protected virtual void OnRemoving(TKey key)
		{
		}

		protected virtual void OnRemoved(TKey key)
		{
		}
	}
}