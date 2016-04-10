namespace Selp.Entities
{
	using System.Collections.Generic;

	public class RepositoryModifyResult<TEntity>
	{
		public TEntity ModifiedEntity { get; }
		public IEnumerable<ValidatorError> Errors { get; }

		public RepositoryModifyResult(IEnumerable<ValidatorError> errors)
		{
			Errors = errors;
		}

		public RepositoryModifyResult(TEntity entity)
		{
			ModifiedEntity = entity;
		}
	}
}