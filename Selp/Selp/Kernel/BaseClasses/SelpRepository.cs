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

	public abstract class SelpRepository<T, TKey, TRepository> : ISelpRepository<T, TKey> where T : class
		where TRepository : SelpDbContext, new()
	{
		private readonly bool isReuseRepository = SelpConfiguration.IsReuseRepositoriesByDefault;
		protected TRepository DbContext { get; private set; }

		protected SelpRepository()
		{
			if (isReuseRepository)
			{
				DbContext = new TRepository();
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
			this.DbContext = dbContext;
		}

		protected abstract IDbSet<T> DbSet { get; }

		public virtual T Add(T item)
		{
			ArgumentGuard.ThrowOnNull(item, "item");
			RecreateDbContextIfRequired();
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
			RecreateDbContextIfRequired();
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

		private void RecreateDbContextIfRequired()
		{
			if (!isReuseRepository)
			{
				DbContext = RecreateContext();
			}
		}

		protected virtual TRepository RecreateContext()
		{
			throw new RepositoryException("If you are not using isReuseRepository property. You need to override and implement RecreateContext method!");
		}

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