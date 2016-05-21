namespace Selp.Repository.Helpers
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Common;
	using Common.Entities;
	using InternalEntities;

	internal static class Pagination
	{
		private static readonly MethodInfo OrderByMethodInfo;
		private static readonly MethodInfo OrderByDescendingMethodInfo;
		private static readonly char[] PropertiesSeparator = {'.'};

		private static readonly IDictionary<SortKey, LambdaExpression> _propertyAccessorCache =
			new ConcurrentDictionary<SortKey, LambdaExpression>();

		static Pagination()
		{
			MethodInfo[] methods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
			OrderByMethodInfo = methods.First(m => m.Name.Equals("OrderBy")
			                                       && m.GetParameters().Length == 2);
			OrderByDescendingMethodInfo = methods.First(m => m.Name.Equals("OrderByDescending")
			                                                 && m.GetParameters().Length == 2);
		}

		internal static List<T> ApplyPaginationAndSorting<T>(this IQueryable<T> source, BaseFilter filter, int defaultPageSize, out int total)
		{
			source.ThrowIfNull("Pagination error: source is null");
			if (!filter.Page.HasValue || filter.Page < 1)
			{
				filter.Page = 1;
			}

			if (!filter.PageSize.HasValue || filter.PageSize < 1)
			{
				filter.PageSize = defaultPageSize;
			}

			if (filter.SortDirection == null)
			{
				filter.SortDirection = ListSortDirection.Ascending;
			}

			if (string.IsNullOrWhiteSpace(filter.SortField))
			{
				filter.SortField = "Id";
			}

			IEnumerable<T> items;
			total = GetPartition(source, filter, out items);
			return items.ToList();
		}

		[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
		private static int GetPartition<TSource>(IQueryable<TSource> source, BaseFilter filter, out IEnumerable<TSource> items)
		{
			int totalCount = source.Count();
			int page = filter.Page.Value;
			int pageSize = filter.PageSize.Value;

			string orderBy = filter.SortField;

			LambdaExpression keySelector = GetProperty(source, orderBy);
			MethodInfo method = filter.SortDirection != ListSortDirection.Descending
				? OrderByMethodInfo
				: OrderByDescendingMethodInfo;
			source = source.Provider.CreateQuery<TSource>(
				Expression.Call(null, method.MakeGenericMethod(typeof(TSource), keySelector.ReturnType), new[]
				{
					source.Expression,
					Expression.Quote(keySelector)
				}));
			// navigate to necessary page
			if (page > 1)
			{
				source = source.Skip((page - 1)*pageSize);
			}
			// take page items
			items = source.Take(pageSize).ToList();

			return totalCount;
		}

		private static LambdaExpression GetProperty<T>(IEnumerable<T> source, string orderBy)
		{
			Type typeToAccess = typeof(T);
			var key = new SortKey(typeToAccess, orderBy);

			LambdaExpression cachedResult;
			if (!_propertyAccessorCache.TryGetValue(key, out cachedResult))
			{
				string[] pathes = orderBy.Split(PropertiesSeparator);

				ParameterExpression entityParameter = Expression.Parameter(typeToAccess, "inst");
				Expression orderByProperty = null;
				foreach (string path in pathes)
				{
					orderByProperty = Expression.Property(
						orderByProperty ?? entityParameter,
						orderByProperty != null ? orderByProperty.Type : typeToAccess,
						path);
				}

				Type type = typeof(Func<,>).MakeGenericType(typeof(T), orderByProperty.Type);
				cachedResult = Expression.Lambda(type, orderByProperty, entityParameter);
				_propertyAccessorCache[key] = cachedResult;
			}

			return cachedResult;
		}
	}
}