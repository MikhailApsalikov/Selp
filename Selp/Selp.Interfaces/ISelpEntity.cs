namespace Selp.Interfaces
{
	public interface ISelpEntity<TId>
	{
		TId Id { get; set; }
	}
}