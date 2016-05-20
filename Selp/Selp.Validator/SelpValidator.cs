namespace Selp.Validator
{
	using System.Collections.Generic;
	using System.Linq;
	using Common.Entities;
	using Common.Exceptions;
	using Interfaces;

	public abstract class SelpValidator : ISelpValidator
	{
		private readonly List<ValidatorError> errors;

		private ValidatorStatus status;

		protected SelpValidator()
		{
			NestedValidators = new List<SelpValidator>();
			status = ValidatorStatus.Created;
			errors = new List<ValidatorError>();
		}

		protected SelpValidator(SelpValidator parentValidator) : this()
		{
			ParentValidator = parentValidator;
		}

		public List<SelpValidator> NestedValidators { get; }

		public abstract string EntityName { get; }

		public SelpValidator ParentValidator { get; set; }

		public bool IsValid
		{
			get
			{
				if (status != ValidatorStatus.Validated)
				{
					throw new WorkflowException(
						$"Validator {EntityName} didn't validate the entity yet. Please, run Validate() and wait for its completion.");
				}

				return !errors.Any() && NestedValidators.All(nv => nv.IsValid);
			}
		}

		public List<ValidatorError> Errors
		{
			get
			{
				if (status != ValidatorStatus.Validated)
				{
					throw new WorkflowException(
						$"Validator {EntityName} didn't validate the entity yet. Please, run Validate() and wait for its completion.");
				}

				var result = new List<ValidatorError>(errors);
				foreach (SelpValidator nestedValidator in NestedValidators)
				{
					result.AddRange(nestedValidator.Errors);
				}

				return result;
			}
		}

		public void Validate()
		{
			if (status != ValidatorStatus.Created)
			{
				throw new WorkflowException(
					$"Validator {EntityName} has already been executed. You cannot use validators more than one time.");
			}
			status = ValidatorStatus.InProgress;
			ValidateLogic();
			foreach (SelpValidator nestedValidator in NestedValidators)
			{
				nestedValidator.Validate();
			}
			status = ValidatorStatus.Validated;
		}

		public void AddNestedValidator(SelpValidator validator)
		{
			if (status != ValidatorStatus.Created)
			{
				throw new WorkflowException(
					$"Validator {EntityName} has already been executed. There is no sense to add a nested validator.");
			}

			if (validator == null)
			{
				return;
			}

			validator.ParentValidator = this;
			NestedValidators.Add(validator);
		}

		protected void AddError(string text)
		{
			var error = new ValidatorError(text);
			error.ParentEntities.AddRange(InitParentEntitiesList(ParentValidator));
			errors.Add(error);
		}

		protected void AddError(string text, string fieldName)
		{
			var error = new ValidatorError(text, fieldName);
			error.ParentEntities.AddRange(InitParentEntitiesList(ParentValidator));
			errors.Add(error);
		}

		private List<string> InitParentEntitiesList(SelpValidator parentValidator)
		{
			if (parentValidator == null)
			{
				return new List<string>();
			}

			List<string> result = InitParentEntitiesList(parentValidator.ParentValidator);
			result.Add(parentValidator.EntityName);
			return result;
		}

		protected abstract void ValidateLogic();
	}
}