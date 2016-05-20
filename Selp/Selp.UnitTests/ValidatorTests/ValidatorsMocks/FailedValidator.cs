namespace Selp.UnitTests.ValidatorTests.ValidatorsMocks
{
	using Validator;

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