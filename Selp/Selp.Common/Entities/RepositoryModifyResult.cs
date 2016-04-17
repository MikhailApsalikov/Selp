namespace Selp.Common.Entities
{
	using System.Collections.Generic;

	public class RepositoryModifyResult<TEntity>
	{
		public RepositoryModifyResult(IEnumerable<ValidatorError> errors)
		{
			Errors = errors;
		}

		public RepositoryModifyResult(TEntity entity)
		{
			ModifiedEntity = entity;
		}

		public TEntity ModifiedEntity { get; }
		public IEnumerable<ValidatorError> Errors { get; }
	}
}