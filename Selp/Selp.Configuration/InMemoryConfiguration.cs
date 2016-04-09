namespace Selp.Configuration
{
	internal sealed class InMemoryConfiguration : ISelpConfiguration
	{
		#region Singleton
		public static InMemoryConfiguration Instance { get; }

		static InMemoryConfiguration()
		{
			Instance = new InMemoryConfiguration();
		}

		private InMemoryConfiguration()
		{
			DefaultPageSize = 25;
		}
		#endregion

		public int DefaultPageSize { get; set; }
	}
}