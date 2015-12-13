namespace Selp.Kernel.BaseClasses
{
	using System.Data.Entity;

	public class SelpDbContext : DbContext
	{
		public void Update<TEntity>(TEntity entity) where TEntity : class
		{
			Entry(entity).State = EntityState.Modified;
		}
	}
}