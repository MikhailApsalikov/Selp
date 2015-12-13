namespace Selp.Kernel.BaseClasses
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Common.Exceptions;
	using Common.Helpers;
	using Config;
	using Interfaces;
	using Selp.Common.Extensions;

	public abstract class SelpRepository<T, TKey, TRepository> : ISelpRepository<T, TKey>
		where T : class, ISelpEntity<TKey>
		where TRepository : SelpDbContext
	{
		protected SelpRepository()
		{
			DbContext = CreateContext();
		}

		protected SelpRepository(TRepository dbContext)
		{
			DbContext = dbContext;
		}

		protected TRepository DbContext { get; private set; }

		protected abstract DbSet<T> DbSet { get; }

		public virtual T Add(T item)
		{
			ArgumentGuard.ThrowOnNull(item, "item");
			Console.WriteLine();
			OnAdding(item);
			item = DbSet.Add(item);
			DbContext.SaveChanges();
			OnAdded(item);
			return item;
		}

		public virtual async Task<T> AddAsync(T item)
		{
			ArgumentGuard.ThrowOnNull(item, "item");
			OnAdding(item);
			item = DbSet.Add(item);
			await DbContext.SaveChangesAsync();
			OnAdded(item);
			return item;
		}

		public virtual IQueryable<T> Filter(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector,
			int offset, int count)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<IQueryable<T>> FilterAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<IQueryable<T>> FilterAsync<TValue>(Expression<Func<T, bool>> filter,
			Expression<Func<T, TValue>> keySelector,
			int offset, int count)
		{
			throw new NotImplementedException();
		}

		public virtual T FilterFirst(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<T> FilterFirstAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual T FilterSingle(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<T> FilterSingleAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public virtual IQueryable<T> GetAll()
		{
			throw new NotImplementedException();
		}

		public virtual async Task<IQueryable<T>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public virtual T GetById(TKey key)
		{
			var item = DbSet.Find(key);
			return item;
		}

		public virtual async Task<T> GetByIdAsync(TKey key)
		{
			var item = await DbSet.FindAsync(key);
			return item;
		}

		public virtual IQueryable<T> GetRange<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<IQueryable<T>> GetRangeAsync<TValue>(Expression<Func<T, TValue>> keySelector, int offset,
			int count)
		{
			throw new NotImplementedException();
		}

		public virtual void Remove(T item)
		{
			throw new NotImplementedException();
		}

		public virtual async Task RemoveAsync(T item)
		{
			throw new NotImplementedException();
		}

		public virtual void RemoveById(TKey key)
		{
			throw new NotImplementedException();
		}

		public virtual async Task RemoveByIdAsync(TKey key)
		{
			throw new NotImplementedException();
		}

		public virtual T Update(T item)
		{
			ArgumentGuard.ThrowOnNull(item, "item");
			OnUpdateing(item);

			try
			{
				DbContext.Update(item);
				DbContext.SaveChanges();
			}
			catch (InvalidOperationException)
			{
				var itemInContext = GetById(item.Id);
				if (itemInContext == null)
				{
					throw new EntityNotFoundException("Entity not found");
				}

				DbContext.Update(itemInContext);
				DbContext.SaveChanges();
			}

			OnUpdated(item);
			return item;
		}

		public virtual async Task<T> UpdateAsync(T item)
		{
			ArgumentGuard.ThrowOnNull(item, "item");
			OnUpdateing(item);

			try
			{
				DbContext.Update(item);
				await DbContext.SaveChangesAsync();
			}
			catch (InvalidOperationException)
			{
				var itemInContext = await GetByIdAsync(item.Id);
				if (itemInContext == null)
				{
					throw new EntityNotFoundException("Entity not found");
				}

				DbContext.Update(itemInContext);
				await DbContext.SaveChangesAsync();
			}

			OnUpdated(item);
			return item;
		}

		protected abstract TRepository CreateContext();

		#region events

		protected virtual void OnAdding(T item)
		{
		}

		protected virtual void OnAdded(T item)
		{
		}

		protected virtual void OnUpdateing(T item)
		{
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

		#endregion

		#region IDisposable Support

		private bool disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					DbContext.Dispose();
				}

				DbContext = null;

				// TODO: set Expressions to null
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}