namespace Selp.Common.Extensions
{
	using System;
	using System.Data.Entity.Validation;
	using System.Linq;

	public static class DbEntityValidationExceptionExtensions
	{
		public static string GetErrorMessage(this DbEntityValidationException exception)
		{
			var errorMessages = exception.EntityValidationErrors
				.SelectMany(x => x.ValidationErrors)
				.Select(x => x.ErrorMessage);

			// Join the list to a single string.
			return string.Join(Environment.NewLine, errorMessages);
		}
	}
}