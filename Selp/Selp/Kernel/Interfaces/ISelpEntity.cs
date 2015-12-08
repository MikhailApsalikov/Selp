namespace Selp.Kernel.Interfaces
{
	public interface ISelpEntity<TKey>
	{
		TKey Id { get; set; }
	}
}