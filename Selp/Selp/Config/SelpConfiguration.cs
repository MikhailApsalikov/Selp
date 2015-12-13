namespace Selp.Config
{
	using Kernel.Enums;

	public static class SelpConfiguration
	{
		/// <summary>
		/// If enabled it caches Expressions inside Repositories by default. Doesn't affect overrided methods.
		/// </summary>
		public static bool CacheExpressions => false;

		/// <summary>
		/// If enabled it uses isDelete flag on entities instead of delete them.
		/// </summary>
		public static bool UseVirtualDelete => false;
	}
}