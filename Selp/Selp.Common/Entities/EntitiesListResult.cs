namespace Selp.Common.Entities
{
	using System;
	using System.Collections.Generic;

	public class EntitiesListResult<T>
	{
		public List<T> Data { get; set; }

		public int Page { get; set; }
		public int PageSize { get; set; }
		public int Total { get; set; }

		public int Pages => (int) Math.Ceiling(Total/(double) PageSize);
	}
}