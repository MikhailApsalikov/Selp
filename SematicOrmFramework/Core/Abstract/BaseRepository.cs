namespace SematicOrmFramework.Core.Abstract
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using SematicOrmFramework.Common;
	using SematicOrmFramework.Core.Extensions;
	using SematicOrmFramework.Exceptions;
	using SematicOrmFramework.Interfaces;

	public abstract class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class
	{
		private static readonly Expression<Func<T, bool>> isNotDeletedExpression;
		private static readonly Expression<Func<T, bool>> isNotBuiltInExpression;
		protected readonly IDbContext dbContext;
		private volatile Holder<int, string[]> includedPathes;

		static BaseRepository()
		{
			if (IsLogicalRemovable())
			{
				var parameter = Expression.Parameter(typeof (T));
				isNotDeletedExpression =
					Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Property(parameter, "IsDeleted")),
						parameter);
			}
			if (IsBuiltInSupported())
			{
				var parameter = Expression.Parameter(typeof (T));
				isNotBuiltInExpression =
					Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Property(parameter, "IsBuiltIn")),
						parameter);
			}
		}

		protected BaseRepository(IDbContext dbContext)
		{
			ArgumentGuard.ThrowOnNull(dbContext, nameof(dbContext));
			this.dbContext = dbContext;
		}

		private IDbSet<T> DbSet => GetDbSet(dbContext);

		private IQueryable<T> DataSet
		{
			get
			{
				IQueryable<T> result = DbSet;
				if (IsLogicalRemovable())
				{
					result = result.Where(isNotDeletedExpression);
				}
				if (IsBuiltInSupported())
				{
					result = result.Where(isNotBuiltInExpression);
				}
				return ApplyIncludedPathes(result);
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public IQueryable<T> GetAll()
		{
			return DataSet;
		}

		public IQueryable<T> GetAsNoTracking()
		{
			return DataSet.AsNoTracking();
		}

		public IQueryable<T> GetRange<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count)
		{
			ArgumentGuard.ThrowOnNull(keySelector, nameof(keySelector));
			ArgumentGuard.ThrowOnOutOfRange(offset, nameof(offset), x => x >= 0);
			ArgumentGuard.ThrowOnOutOfRange(count, nameof(count), x => x > 0);

			return DataSet.OrderBy(keySelector).Skip(offset).Take(count);
		}

		public IQueryable<T> Filter(Expression<Func<T, bool>> filter)
		{
			ArgumentGuard.ThrowOnNull(filter, "filter");
			return DataSet.Where(filter);
		}

		public IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector,
			int offset, int count)
		{
			ArgumentGuard.ThrowOnNull(filter, nameof(filter));
			ArgumentGuard.ThrowOnNull(keySelector, nameof(keySelector));
			ArgumentGuard.ThrowOnOutOfRange(offset, nameof(offset), x => x >= 0);
			ArgumentGuard.ThrowOnOutOfRange(count, nameof(count), x => x > 0);

			return DataSet.Where(filter).OrderBy(keySelector).Skip(offset).Take(count);
		}

		public T FilterSingle(Expression<Func<T, bool>> filter)
		{
			ArgumentGuard.ThrowOnNull(filter, nameof(filter));

			return DataSet.SingleOrDefault(filter);
		}

		public T FilterFirst(Expression<Func<T, bool>> filter)
		{
			ArgumentGuard.ThrowOnNull(filter, nameof(filter));

			return DataSet.FirstOrDefault(filter);
		}

		public T GetById(TKey key)
		{
			var item = DbSet.FindOrThrow(key);

			var logicalRemove = item as ILogicalRemove;
			if (logicalRemove != null && logicalRemove.IsDeleted)
				throw new EntityNotFoundException();

			var builtIn = item as IBuiltIn;
			if (builtIn != null && builtIn.IsBuiltIn)
				throw new InvalidOperationException();

			return item;
		}

		public T GetByIdAsNoTracking(TKey key)
		{
			var item = DbSet.FindOrThrow(key);

			var logicalRemove = item as ILogicalRemove;
			if (logicalRemove != null && logicalRemove.IsDeleted)
				throw new EntityNotFoundException();

			var builtIn = item as IBuiltIn;
			if (builtIn != null && builtIn.IsBuiltIn)
				throw new InvalidOperationException();

			dbContext.Detach(item);

			return item;
		}

		public T Add(T item)
		{
			ArgumentGuard.ThrowOnNull(item, nameof(item));

			OnAdding(item);

			item = DbSet.Add(item);
			SaveChanges();

			OnAdded(item);

			return item;
		}

		public T Update(T item)
		{
			ArgumentGuard.ThrowOnNull(item, nameof(item));

			OnUpdating(item);

			var logicalRemove = item as ILogicalRemove;
			if (logicalRemove != null)
			{
				logicalRemove.IsDeleted = false;
			}
			dbContext.Update(item);
			SaveChanges();

			OnUpdated(item);

			return item;
		}

		public void Remove(T item)
		{
			ArgumentGuard.ThrowOnNull(item, nameof(item));

			OnRemoving(item);

			var builtIn = item as IBuiltIn;
			if (builtIn != null && builtIn.IsBuiltIn)
				throw new InvalidOperationException();

			var logicalRemove = item as ILogicalRemove;
			if (logicalRemove != null)
			{
				logicalRemove.IsDeleted = true;
				dbContext.Update(item);
			}
			else
			{
				DbSet.Remove(item);
			}
			SaveChanges();

			OnRemoved(item);
		}

		public void RemoveById(TKey key)
		{
			Remove(GetById(key));
		}

		public void SetIncludedPathes(params string[] pathes)
		{
			includedPathes = new Holder<int, string[]>(0, pathes?.ToArray());
		}

		public void SaveChanges()
		{
			dbContext.SaveChanges();
		}

		protected abstract IDbSet<T> GetDbSet(IDbContext dbContext);

		protected virtual void OnAdding(T item)
		{
			CheckForBuiltIn(item);
		}

		protected virtual void OnAdded(T item)
		{
		}

		protected virtual void OnUpdating(T item)
		{
			CheckForBuiltIn(item);
		}

		protected virtual void OnUpdated(T item)
		{
		}

		protected virtual void OnRemoving(T item)
		{
		}

		protected virtual void OnRemoved(T item)
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			dbContext.Dispose();
		}

		protected virtual IQueryable<T> ApplyIncludedPathes(IQueryable<T> source)
		{
			var pathes = includedPathes;
			if (pathes != null && Interlocked.CompareExchange(ref pathes.item1, 1, 0) == 0)
			{
				source = pathes.item2.Aggregate(source, (current, path) => current.Include(path));
			}
			return source;
		}

		private static void CheckForBuiltIn(T item)
		{
			var builtIn = item as IBuiltIn;
			if (builtIn != null)
			{
				builtIn.IsBuiltIn = false;
			}
		}

		internal static bool IsLogicalRemovable()
		{
			return typeof (ILogicalRemove).IsAssignableFrom(typeof (T));
		}

		internal static bool IsBuiltInSupported()
		{
			return typeof (IBuiltIn).IsAssignableFrom(typeof (T));
		}
	}
}