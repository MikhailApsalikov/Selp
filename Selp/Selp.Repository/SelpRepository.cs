namespace Selp.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using Common;
	using Common.Exceptions;
	using Configuration;
	using Entities;
	using ExpressionConstructor;
	using Interfaces;
	using Pagination;
	using Validator;

	public abstract class SelpRepository<TModel, TEntity, TKey> : ISelpRepository<TModel, TEntity, TKey>
		where TModel : class, ISelpEntity<TKey> where TEntity : class, ISelpEntity<TKey>
	{
		private readonly ISelpConfiguration configuration;

		private readonly FakeRemovingExpressionContainer<TEntity> fakeRemoving =
			FakeRemovingExpressionContainer<TEntity>.Instance;

		protected SelpRepository(DbContext dbContext, ISelpConfiguration configuration)
		{
			DbContext = dbContext;
			this.configuration = configuration;
		}

		protected DbContext DbContext { get; }

		public abstract bool IsRemovingFake { get; }

		public abstract string FakeRemovingPropertyName { get; }

		public abstract IDbSet<TEntity> DbSet { get; }

		public SelpValidator CreateValidator { get; set; }

		public SelpValidator UpdateValidator { get; set; }


		public virtual IEnumerable<TModel> GetAll()
		{
			return FilterDeleted(DbSet).Select(entity => MapEntityToModel(entity));
		}

		public virtual TModel GetById(TKey id)
		{
			id.ThrowIfNull("ID cannot be null");
			TEntity entity = FindById(id);
			return MapEntityToModel(entity);
		}

		public virtual IEnumerable<TModel> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression)
		{
			customExpression.ThrowIfNull("Custom expression cannot be null");
			return FilterDeleted(DbSet).Where(customExpression).Select(entity => MapEntityToModel(entity));
		}

		public virtual IEnumerable<TModel> GetByFilter(BaseFilter filter)
		{
			filter.ThrowIfNull("Filter cannot be null");
			return ApplyFilters(FilterDeleted(DbSet), filter)
				.ApplySorting(filter)
				.ApplyPagination(filter, configuration.DefaultPageSize)
				.Select(entity => MapEntityToModel(entity));
		}

		public virtual RepositoryModifyResult<TModel> Create(TModel model)
		{
			model.ThrowIfNull("Model cannot be null");
			TEntity entity = MapModelToEntity(model);
			OnCreating(entity);
			if (CreateValidator != null)
			{
				CreateValidator.Validate();
				if (!CreateValidator.IsValid)
				{
					return new RepositoryModifyResult<TModel>(CreateValidator.Errors);
				}
			}

			TEntity result = DbSet.Add(entity);
			DbContext.SaveChanges();
			OnCreated(entity);
			return new RepositoryModifyResult<TModel>(MapEntityToModel(result));
		}

		public virtual RepositoryModifyResult<TModel> Update(TKey id, TModel model)
		{
			id.ThrowIfNull("ID cannot be null");
			model.ThrowIfNull("Model cannot be null");
			TEntity entity = FindById(id);
			OnUpdating(id, entity);
			MapModelToEntity(model, entity);
			if (UpdateValidator != null)
			{
				UpdateValidator.Validate();
				if (!UpdateValidator.IsValid)
				{
					return new RepositoryModifyResult<TModel>(UpdateValidator.Errors);
				}
			}


			MarkAsModified(entity);
			DbContext.SaveChanges();
			OnUpdated(id, entity);
			return new RepositoryModifyResult<TModel>(MapEntityToModel(entity));
		}

		public virtual void Remove(TKey id)
		{
			id.ThrowIfNull("ID cannot be null");
			TEntity entity = FindById(id);
			OnRemoving(id, entity);
			if (IsRemovingFake)
			{
				fakeRemoving.GetFakeDeleteCompiledFunction(FakeRemovingPropertyName)(entity);
				MarkAsModified(entity);
			}
			else
			{
				DbSet.Remove(entity);
			}
			DbContext.SaveChanges();
			OnRemoved(id);
		}

		protected virtual void MarkAsModified(TEntity entity)
		{
			DbContext.Entry(entity).State = EntityState.Modified;
		}

		private TEntity FindById(TKey id)
		{
			TEntity entity = DbSet.Find(id);
			if (entity == null)
			{
				throw new EntityNotFoundException();
			}

			CheckForFakeRemoved(entity);
			return entity;
		}

		protected abstract TModel MapEntityToModel(TEntity entity);

		protected abstract TEntity MapModelToEntity(TModel model);

		protected abstract TEntity MapModelToEntity(TModel source, TEntity destination);

		protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> entities, BaseFilter filter);

		private IQueryable<TEntity> FilterDeleted(IQueryable<TEntity> entities)
		{
			if (!IsRemovingFake)
			{
				return entities;
			}

			return entities.Where(fakeRemoving.GetIsRemovedExpression(FakeRemovingPropertyName));
		}

		private void CheckForFakeRemoved(TEntity entity)
		{
			if (!IsRemovingFake)
			{
				return;
			}

			if (!fakeRemoving.GetIsRemovedCompiledFunction(FakeRemovingPropertyName)(entity))
			{
				throw new EntityIsRemovedException();
			}
		}

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