namespace Selp.Interfaces
{
    using System;
    using System.Linq.Expressions;
    using Common.Entities;

    public interface ISelpRepository<TModel, TEntity, in TKey> where TModel : ISelpEntity<TKey>
        where TEntity : ISelpEntity<TKey>
    {
        EntitiesListResult<TModel> GetAll();
        TModel GetById(TKey id);

        EntitiesListResult<TModel> GetByFilter(BaseFilter filter);

        EntitiesListResult<TModel> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression);

        RepositoryModifyResult<TModel> Create(TModel model);

        RepositoryModifyResult<TModel> Update(TKey id, TModel model);

        void Remove(TKey id);
    }
}