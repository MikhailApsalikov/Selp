namespace Selp.Repository.Helpers
{
	using System.Collections.Generic;
	using System.Data.Entity.Validation;
	using Common.Entities;

	public static class EntityFrameworkValidationConverter
	{
		public static IEnumerable<ValidatorError> ConvertToValidatorErrorList(DbEntityValidationException exception)
		{
			var errorMessages = new List<string>();
			foreach (DbEntityValidationResult validationResult in exception.EntityValidationErrors)
			{
				//string entityName = validationResult.Entry.Entity.GetType().Name;
				foreach (DbValidationError error in validationResult.ValidationErrors)
				{
					yield return new ValidatorError(error.ErrorMessage, error.PropertyName);
				}
			}
		}
	}
}