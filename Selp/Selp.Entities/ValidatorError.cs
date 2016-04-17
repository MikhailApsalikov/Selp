namespace Selp.Entities
{
	using System.Collections.Generic;

	public class ValidatorError
	{
		public ValidatorError(string text)
		{
			ParentEntities = new List<string>();
			Text = text;
		}

		public ValidatorError(string text, string fieldName) : this(text)
		{
			FieldName = fieldName;
		}

		public string FieldName { get; }

		public string Text { get; }

		public List<string> ParentEntities { get; private set; }
	}
}