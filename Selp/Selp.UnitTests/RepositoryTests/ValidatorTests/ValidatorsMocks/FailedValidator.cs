namespace Selp.UnitTests.RepositoryTests.ValidatorTests.ValidatorsMocks
{
	using Repository.Validator;

	internal class FailedValidator : SelpValidator
	{
		public FailedValidator()
		{
		}

		public FailedValidator(SelpValidator parentValidor) : base(parentValidor)
		{
		}

		public override string EntityName => "Failed";

		protected override void ValidateLogic()
		{
			AddError("Text", "FieldName");
		}
	}
}