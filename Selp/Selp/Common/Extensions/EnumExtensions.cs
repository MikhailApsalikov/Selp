namespace Selp.Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	public static class EnumExtensions
	{
		public static string GetString(this Enum e)
		{
			var s = e.ToString();
			var fi = e.GetType().GetField(s);
			var attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(
				typeof (DescriptionAttribute), false);
			if (attributes.Length > 0)
			{
				s = attributes[0].Description;
			}

			return s;
		}

		public static IEnumerable<object> GetValues(this Enum e)
		{
			return Enum.GetValues(e.GetType()).Cast<Enum>().Select(value => value.GetString());
		}
	}
}