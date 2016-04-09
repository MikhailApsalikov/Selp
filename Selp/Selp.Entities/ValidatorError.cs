namespace Selp.Entities
{
	using System.Collections.Generic;

	public class ValidatorError
	{
		public string FieldName { get; }

		public string Text { get; }

		public List<string> ParentEntities { get; private set; }

		public ValidatorError(string text)
		{
			ParentEntities = new List<string>();
			Text = text;
		}

		public ValidatorError(string text, string fieldName) : this(text)
		{
			FieldName = fieldName;
		}
	}
}