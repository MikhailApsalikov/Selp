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
			return DbSet;
		}

		public virtual TEntity GetById(TKey id)
		{
			if (id == null)
			{
				throw new ArgumentException("ID cannot be null");
			}

			return DbSet.Find(id);
		}

		public virtual IQueryable<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression)
		{
			return DbSet.Where(customExpression);
		}

		public virtual RepositoryModifyResult<TEntity> Create(TEntity item)
		{
			var result = DbSet.Add(item);
			DbContext.SaveChanges();
			return new RepositoryModifyResult<TEntity>(result);
		}

		public virtual RepositoryModifyResult<TEntity> Update(TKey id, TEntity item)
		{
			//context.Entry(entity).State = EntityState.Modified;
			throw new NotImplementedException();
		}

		public virtual void Remove(TKey key)
		{
			TEntity entity = DbSet.Find(key);
			DbSet.Remove(entity);
		}

		public virtual IQueryable<TEntity> GetByFilter(BaseFilter filter)
		{
			return ApplyFilters(DbSet, filter);
		}

		protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> entities, BaseFilter filter);

		#region events for overriding
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
		#endregion
	}
}