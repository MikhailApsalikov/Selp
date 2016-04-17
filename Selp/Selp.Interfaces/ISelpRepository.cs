namespace Selp.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Common.Entities;

	public interface ISelpRepository<TModel, TEntity, in TKey> where TModel : ISelpEntity<TKey>
		where TEntity : ISelpEntity<TKey>
	{
		IEnumerable<TModel> GetAll();
		TModel GetById(TKey id);

		IEnumerable<TModel> GetByFilter(BaseFilter filter);

		IEnumerable<TModel> GetByCustomExpression(Expression<Func<TEntity, bool>> customExpression);

		RepositoryModifyResult<TModel> Create(TModel model);

		RepositoryModifyResult<TModel> Update(TKey id, TModel model);

		void Remove(TKey id);
	}
}