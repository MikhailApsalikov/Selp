namespace Selp.Kernel.BaseClasses
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Common.Helpers;
	using Config;
	using Interfaces;

	public abstract class SelpRepository<T, TKey, TRepository> : ISelpRepository<T, TKey> where T : class
		where TRepository : SelpDbContext, new()
	{
		private readonly bool isReuseRepository = SelpConfiguration.IsReuseRepositoriesByDefault;
		private TRepository dbContext;

		protected SelpRepository()
		{
			if (isReuseRepository)
			{
				dbContext = new TRepository();
			}
		}

		protected SelpRepository(bool isReuseRepository)
			: this()
		{
			this.isReuseRepository = isReuseRepository;
		}

		protected SelpRepository(TRepository dbContext)
		{
			isReuseRepository = true;
			this.dbContext = dbContext;
		}

		protected TRepository DbContext => isReuseRepository ? dbContext : new TRepository();

		public virtual T Add(T item)
		{
			throw new NotImplementedException();
		}

		public virtual async Task<T> AddAsync(T item)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public virtual async Task<T> GetByIdAsync(TKey key)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		public virtual async Task<T> UpdateAsync(T item)
		{
			throw new NotImplementedException();
		}

		protected abstract IDbSet<T> DbSet { get; }

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

		#region IDisposable Support

		private bool disposedValue; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					dbContext.Dispose();
				}

				dbContext = null;

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