namespace Selp.Interfaces
{
	public interface ISelpRepository<TEntity, TKey> where TEntity : ISelpEntitiy<TKey>
	{
	}
}