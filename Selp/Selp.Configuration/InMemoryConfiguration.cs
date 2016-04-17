namespace Selp.Configuration
{
	internal sealed class InMemoryConfiguration : ISelpConfiguration
	{
		public int DefaultPageSize { get; set; }

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
	}
}