namespace Selp.Common.Exceptions
{
	using System;

	public class SelpConfigurationException : SelpException
	{
		private readonly string parameterName;
		private readonly string explanation;

		public SelpConfigurationException(string parameterName, string explanation)
			: base(explanation)
		{
			this.parameterName = parameterName;
			this.explanation = explanation;
		}

		public SelpConfigurationException(string parameterName, string explanation, Exception innerException)
			: base(explanation, innerException)
		{
			this.parameterName = parameterName;
			this.explanation = explanation;
		}

		public override string Message => $"Configuration violation. Parameter: {parameterName}. Reason: {explanation}";
	}
}