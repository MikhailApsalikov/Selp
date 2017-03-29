namespace Selp.Common.Entities
{
	using System.Collections.Generic;

	public class RepositoryModifyResult<TEntity>
	{
		public RepositoryModifyResult(IEnumerable<ValidatorError> errors)
		{
			errors.ThrowIfNull("RepositoryModifyResult: errors cannot be null");
			Errors = errors;
		}

		public RepositoryModifyResult(TEntity entity)
		{
			entity.ThrowIfNull("RepositoryModifyResult: entity cannot be null");
			ModifiedEntity = entity;
		}

		public TEntity ModifiedEntity { get; }
		public IEnumerable<ValidatorError> Errors { get; }

		public bool IsValid => ModifiedEntity != null;
	}
}