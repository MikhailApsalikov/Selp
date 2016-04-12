// ReSharper disable InconsistentNaming

namespace Selp.Controller.Entities
{
	using System.Collections.Generic;
	using Selp.Entities;

	internal class ErrorList
	{
		public ErrorList(IEnumerable<ValidatorError> errors)
		{
			this.errors = errors;
		}

		public IEnumerable<ValidatorError> errors { get; set; }
		public bool isValid => false;
	}
}