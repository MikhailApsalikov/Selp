namespace Selp.Common
{
	using System;

	public static class ArgumentGuard
	{
		public static void ThrowIfNull<T>(this T obj, string message = null)
		{
			if (obj == null)
			{
				throw new ArgumentException(message ?? string.Empty);
			}
		}
	}
}