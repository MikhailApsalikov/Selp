namespace Example.Models
{
	using Selp.Interfaces;

	public class RegionModel : ISelpEntity<int>
	{
		public string Name { get; set; }

		public int Id { get; set; }
	}
}