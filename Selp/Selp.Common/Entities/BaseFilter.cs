namespace Selp.Common.Entities
{
	using System.ComponentModel;

	public class BaseFilter
	{
		/// <summary>
		///     Simple search string
		/// </summary>
		public string Search { get; set; }

		/// <summary>
		///     Current page
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		///     Current page size
		/// </summary>
		public int? PageSize { get; set; }

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