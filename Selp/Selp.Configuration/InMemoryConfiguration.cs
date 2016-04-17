namespace Selp.Configuration
{
	public sealed class InMemoryConfiguration : ISelpConfiguration
	{
		public InMemoryConfiguration()
		{
			DefaultPageSize = 25;
		}

		public int DefaultPageSize { get; set; }
	}
}