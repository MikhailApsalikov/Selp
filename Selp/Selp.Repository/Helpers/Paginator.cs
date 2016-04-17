namespace Selp.Repository.Pagination
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;
	using Entities;

	internal static class Pagination
	{
		internal static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, BaseFilter filter, int defaultPageSize)
		{
			if (!filter.Page.HasValue || filter.Page < 1)
			{
				filter.Page = 1;
			}

			if (!filter.PageSize.HasValue || filter.PageSize < 1)
			{
				filter.PageSize = defaultPageSize;
			}

			return source.Skip((filter.Page.Value - 1)*filter.PageSize.Value).Take(filter.PageSize.Value);
		}

		internal static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, BaseFilter filter)
		{
			if (filter.SortDirection != null && !string.IsNullOrWhiteSpace(filter.SortField))
			{
				ParameterExpression entityParameter = Expression.Parameter(typeof(T));
				Expression<Func<T, object>> lambda =
					Expression.Lambda<Func<T, object>>(
						Expression.Convert(Expression.Property(entityParameter, filter.SortField), typeof(object)), entityParameter);
				switch (filter.SortDirection)
				{
					case ListSortDirection.Ascending:
						return source.OrderBy(lambda);
					case ListSortDirection.Descending:
						return source.OrderByDescending(lambda);
				}
			}

			return source;
		}
	}
}