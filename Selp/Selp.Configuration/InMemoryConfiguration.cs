namespace Selp.Configuration
{
	using Interfaces;

	public sealed class InMemoryConfiguration : ISelpConfiguration
	{
		public InMemoryConfiguration()
		{
			DefaultPageSize = 25;
		}

		public int DefaultPageSize { get; set; }
	}
}