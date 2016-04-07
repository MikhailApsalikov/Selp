namespace Selp.Entities
{
	using System.ComponentModel;

	public class BaseFilter
	{
		private const int DefaultPage = 1;
		private const int DefaultPageSize = 25; // TODO: в конфиг

		private int? page;
		private int? pageSize;

		/// <summary>
		///     Simple search string
		/// </summary>
		public string Search { get; set; }

		/// <summary>
		///     Current page
		/// </summary>
		public int? Page
		{
			get { return page; }
			set
			{
				if (value.HasValue && value <= 0)
				{
					page = DefaultPage;
					return;
				}

				page = value;
			}
		}

		/// <summary>
		///     Current page size
		/// </summary>
		public int? PageSize {
			get
			{
				return pageSize;
			}
			set
			{
				if (!value.HasValue || value <= 0)
				{
					pageSize = DefaultPageSize;
					return;
				}

				pageSize = value;
			} }

		/// <summary>
		///     SortDirection
		/// </summary>
		public ListSortDirection? SortDirection { get; set; }

		/// <summary>
		///     Sort field
		/// </summary>
		public string SortField { get; set; }
	}
}