﻿namespace Selp.UnitTests.ValidatorTests.ValidatorsMocks
{
	using Validator;

	internal class FailedValidatorLevel2 : SelpValidator
	{
		public override string EntityName => "FailedLevel2";

		protected override void ValidateLogic()
		{
			AddError("Text level 2");
		}
	}
}