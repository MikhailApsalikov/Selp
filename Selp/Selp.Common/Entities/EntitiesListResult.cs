﻿namespace Selp.Common.Entities
{
	using System.Collections.Generic;

	public class EntitiesListResult<T>
	{
		public IEnumerable<T> Data { get; set; }

		public int Page { get; set; }
		public int PageSize { get; set; }
		public int Total { get; set; }
	}
}