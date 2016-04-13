namespace Selp.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using Configuration;
	using Entities;
	using Interfaces;
	using Validator;

	public abstract class SelpRepository<TModel, TEntity, TKey> : ISelpRepository<TModel, TEntity, TKey>
		where TModel : class, ISelpEntitiy<TKey>  where TEntity : class, ISelpEntitiy<TKey>
	{
		public abstract bool IsRemovingFake { get; }

		public abstract string FakeRemovingPropertyName { get; }

		public abstract DbContext DbContext { get; }

		public abstract IDbSet<TEntity> DbSet { get; }

		public abstract ISelpConfiguration Configuration { get; }

		protected abstract TModel MapEntityToModel(TEntity entity);

		protected abstract TEntity MapModelToEntity(TModel entity);

		public SelpValidator CreateValidator { get; set; }

		public SelpValidator UpdateValidator { get; set; }

		public virtual IEnumerable<TModel> GetAll()
		{
			return DbSet.Select(entity => MapEntityToModel(entity));
		}

		public virtual TModel GetById(TKey id)
		{
			if (id == null)
			{
				throw new ArgumentException("ID cannot be null");
			}

			return MapEntityToModel(DbSet.Find(id));
		}

		public virtual IEnumerable<TModel> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression)
		{
			return DbSet.Where(customExpression).Select(entity => MapEntityToModel(entity));
		}

		public virtual IEnumerable<TModel> GetByFilter(BaseFilter filter)
		{
			return ApplyFilters(DbSet, filter).Select(entity => MapEntityToModel(entity));
		}

		public virtual RepositoryModifyResult<TModel> Create(TModel item)
		{
			var result = DbSet.Add(MapModelToEntity(item));
			DbContext.SaveChanges();
			return new RepositoryModifyResult<TModel>(MapEntityToModel(result));
		}

		public virtual RepositoryModifyResult<TModel> Update(TKey id, TModel item)
		{
			//context.Entry(entity).State = EntityState.Modified;
			throw new NotImplementedException();
		}

		public virtual void Remove(TKey key)
		{
			TEntity entity = DbSet.Find(key);
			DbSet.Remove(entity);
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