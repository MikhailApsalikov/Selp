namespace Selp.Common.Helpers
{
	using System;

	public static class ArgumentGuard
	{
		public static void ThrowOnNull<T>(T argument, string argumentName) where T : class
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);
		}

		public static void ThrowOnNull<T>(T[] argument, string argumentName) where T : class
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			foreach (var item in argument)
			{
				ThrowOnNull(item, argumentName);
			}
		}

		public static void ThrowOnStringIsNullOrEmpty(string argument, string argumentName)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			// TODO: move texts to resources
			if (string.IsNullOrEmpty(argument))
				throw new ArgumentException("Argument shouldn't be an empty string", argumentName);
		}

		public static void ThrowOnStringIsNullOrEmpty(string[] argument, string argumentName)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			foreach (var item in argument)
			{
				ThrowOnStringIsNullOrEmpty(item, argumentName);
			}
		}

		public static void ThrowOnOutOfRange<T>(T argument, string argumentName, Predicate<T> rangeFilter)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			if (!rangeFilter(argument))
				throw new ArgumentOutOfRangeException(argumentName);
		}
	}
}