namespace Selp.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Common.Entities;

    public interface ISelpRepository<TEntity, in TKey> where TEntity : ISelpEntity<TKey>
    {
        List<TEntity> GetAll();
		TEntity GetById(TKey id);

		List<TEntity> GetByFilter(BaseFilter filter, out int total);

		List<TEntity> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression);

        RepositoryModifyResult<TEntity> Create(TEntity entity);

        RepositoryModifyResult<TEntity> Update(TKey id, TEntity entity);

        void Remove(TKey id);
    }
}