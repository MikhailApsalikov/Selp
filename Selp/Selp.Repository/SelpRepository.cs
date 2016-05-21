namespace Selp.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    using Common;
    using Common.Entities;
    using Common.Exceptions;
    using ExpressionConstructor;
    using Helpers;
    using Interfaces;

    public abstract class SelpRepository<TEntity, TKey> : ISelpRepository<TEntity, TKey>
        where TEntity : class, ISelpEntity<TKey>
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

        public ISelpValidator CreateValidator { get; set; }

        public ISelpValidator UpdateValidator { get; set; }


        public virtual List<TEntity> GetAll()
        {
            return FilterDeleted(DbSet).ToList();
        }

        public virtual TEntity GetById(TKey id)
        {
            id.ThrowIfNull("ID cannot be null");
            return FindById(id);
        }

        public virtual List<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression)
        {
            customExpression.ThrowIfNull("Custom expression cannot be null");
            return FilterDeleted(DbSet).Where(customExpression).ToList();
        }

        public virtual List<TEntity> GetByFilter(BaseFilter filter, out int total)
        {
            filter.ThrowIfNull("Filter cannot be null");
            return ApplyFilters(FilterDeleted(DbSet), filter)
                .ApplyPaginationAndSorting(filter, configuration.DefaultPageSize, out total);
        }

        public virtual RepositoryModifyResult<TEntity> Create(TEntity entity)
        {
			entity.ThrowIfNull("Entity cannot be null");
            OnCreating(entity);
            if (CreateValidator != null)
            {
                CreateValidator.Validate();
                if (!CreateValidator.IsValid)
                {
                    return new RepositoryModifyResult<TEntity>(CreateValidator.Errors);
                }
            }

            TEntity result = DbSet.Add(entity);
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return
                    new RepositoryModifyResult<TEntity>(EntityFrameworkValidationConverter.ConvertToValidatorErrorList(ex));
            }


            OnCreated(entity);
            return new RepositoryModifyResult<TEntity>(result);
        }

        public virtual RepositoryModifyResult<TEntity> Update(TKey id, TEntity entity)
        {
            id.ThrowIfNull("ID cannot be null");
            entity.ThrowIfNull("Entity cannot be null");
            TEntity entityInBase = FindById(id);
            OnUpdating(id, entityInBase);
            Merge(entity, entityInBase);
            if (UpdateValidator != null)
            {
                UpdateValidator.Validate();
                if (!UpdateValidator.IsValid)
                {
                    return new RepositoryModifyResult<TEntity>(UpdateValidator.Errors);
                }
            }


            MarkAsModified(entityInBase);
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return
                    new RepositoryModifyResult<TEntity>(EntityFrameworkValidationConverter.ConvertToValidatorErrorList(ex));
            }
            OnUpdated(id, entityInBase);
            return new RepositoryModifyResult<TEntity>(entityInBase);
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

        protected TEntity FindById(TKey id)
        {
            TEntity entity = DbSet.Find(id);
            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            CheckForFakeRemoved(entity);
            return entity;
        }

        protected abstract TEntity Merge(TEntity source, TEntity destination);

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