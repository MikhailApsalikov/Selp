namespace SematicOrmFramework.Common
{
	using System;

	internal static class ArgumentGuard
	{
		internal static void ThrowOnNull<T>(T argument, string argumentName) where T : class
		{
			if (ReferenceEquals(null, argument))
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		internal static void ThrowOnNull<T>(T[] argument, string argumentName) where T : class
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			foreach (var item in argument)
			{
				ThrowOnNull(item, argumentName);
			}
		}

		internal static void ThrowOnStringIsNullOrEmpty(string argument, string argumentName)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			// TODO: move texts to resources
			if (String.IsNullOrEmpty(argument))
				throw new ArgumentException("Argument shouldn't be an empty string", argumentName);
		}

		internal static void ThrowOnStringIsNullOrEmpty(string[] argument, string argumentName)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			foreach (var item in argument)
			{
				ThrowOnStringIsNullOrEmpty(item, argumentName);
			}
		}

		internal static void ThrowOnOutOfRange<T>(T argument, string argumentName, Predicate<T> rangeFilter)
		{
			if (ReferenceEquals(null, argument))
				throw new ArgumentNullException(argumentName);

			if (!rangeFilter(argument))
				throw new ArgumentOutOfRangeException(argumentName);
		}
	}
}