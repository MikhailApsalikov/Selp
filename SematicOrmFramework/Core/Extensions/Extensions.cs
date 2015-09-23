namespace SematicOrmFramework.Core.Extensions
{
	using System.Data.Entity;
	using SematicOrmFramework.Exceptions;
	using SematicOrmFramework.Interfaces;

	internal static class Extensions
	{
		internal static T FindOrThrow<T>(this IDbSet<T> dbSet, params object[] key) where T : class
		{
			var item = dbSet.Find(key);
			if (item == null)
			{
				throw new EntityNotFoundException();
			}

			if (typeof (ILogicalRemove).IsAssignableFrom(typeof (T)) && ((ILogicalRemove) item).IsDeleted)
			{
				throw new EntityRemovedException();
			}

			return item;
		}
	}
}