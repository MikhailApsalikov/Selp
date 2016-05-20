using System.Collections.Generic;
using Selp.Common.Entities;

namespace Selp.Interfaces
{
	public interface ISelpValidator
	{
		bool IsValid { get; }
		List<ValidatorError> Errors { get; }
		void Validate();
	}
}