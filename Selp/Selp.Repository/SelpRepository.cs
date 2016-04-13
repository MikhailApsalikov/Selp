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
		where TModel : class, ISelpEntity<TKey>  where TEntity : class, ISelpEntity<TKey>
	{
		public abstract bool IsRemovingFake { get; }

		public abstract string FakeRemovingPropertyName { get; }

		public abstract DbContext DbContext { get; }

		public abstract IDbSet<TEntity> DbSet { get; }

		public abstract ISelpConfiguration Configuration { get; }

		protected abstract TModel MapEntityToModel(TEntity entity);

		protected abstract TEntity MapModelToEntity(TModel entity);

		protected abstract TEntity MapModelToEntity(TModel source, TEntity destination);

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
			TEntity entity = MapModelToEntity(item);
			OnCreating(entity);
			var result = DbSet.Add(entity);
			DbContext.SaveChanges();
			OnCreated(entity);
			return new RepositoryModifyResult<TModel>(MapEntityToModel(result));
		}

		public virtual RepositoryModifyResult<TModel> Update(TKey id, TModel model)
		{
			if (id == null)
			{
				throw new ArgumentException("ID cannot be null");
			}

			TEntity entity = DbSet.Find(id);
			OnUpdating(id, entity);
			MapModelToEntity(model, entity);
			DbContext.Entry(entity).State = EntityState.Modified;
			DbContext.SaveChanges();
			OnUpdated(id, entity);
			return new RepositoryModifyResult<TModel>(MapEntityToModel(entity));
		}

		public virtual void Remove(TKey id)
		{
			if (id == null)
			{
				throw new ArgumentException("ID cannot be null");
			}

			TEntity entity = DbSet.Find(id);
			OnRemoving(id, entity);
			DbSet.Remove(entity);
			DbContext.SaveChanges();
			OnRemoved(id);
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

		protected virtual void OnRemoving(TKey key, TEntity item)
		{
		}

		protected virtual void OnRemoved(TKey key)
		{
		}
		#endregion
	}
}