namespace Selp.Kernel.BaseClasses
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Common.Exceptions;
	using Config;
	using Enums;
	using Interfaces;

	public abstract class SelpRepository<T, TKey> : ISelpRepository<T, TKey> where T : class
	{
		private SelpDbContext dbContext;

		protected SelpRepository()
		{
			switch (SelpConfiguration.DbContextUsage)
			{
				case DbContextUsages.FromConstructor:
				{
					throw new SelpConfigurationException("DbContextUsage",
						"You cannot use constructor without dbContext parameter using DbContextUsages.FromConstructor option");
				}
				case DbContextUsages.OnePerRepository:
				{
					dbContext = new SelpDbContext();
					break;
				}
			}
		}

		protected SelpRepository(SelpDbContext dbContext)
		{
			if (SelpConfiguration.DbContextUsage != DbContextUsages.FromConstructor)
			{
				throw new SelpConfigurationException("DbContextUsage",
					"You can use constructor with dbContext parameter using only DbContextUsages.FromConstructor option");
			}
			this.dbContext = dbContext;
		}

		public T Add(T item)
		{
			throw new NotImplementedException();
		}

		public async Task<T> AddAsync(T item)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> Filter(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector,
			int offset, int count)
		{
			throw new NotImplementedException();
		}

		public async Task<IQueryable<T>> FilterAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public async Task<IQueryable<T>> FilterAsync<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector,
			int offset, int count)
		{
			throw new NotImplementedException();
		}

		public T FilterFirst(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public async Task<T> FilterFirstAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public T FilterSingle(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public async Task<T> FilterSingleAsync(Expression<Func<T, bool>> filter)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GetAll()
		{
			throw new NotImplementedException();
		}

		public async Task<IQueryable<T>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public T GetById(TKey key)
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetByIdAsync(TKey key)
		{
			throw new NotImplementedException();
		}

		public IQueryable<T> GetRange<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public async Task<IQueryable<T>> GetRangeAsync<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public void Remove(T item)
		{
			throw new NotImplementedException();
		}

		public async Task RemoveAsync(T item)
		{
			throw new NotImplementedException();
		}

		public void RemoveById(TKey key)
		{
			throw new NotImplementedException();
		}

		public async Task RemoveByIdAsync(TKey key)
		{
			throw new NotImplementedException();
		}

		public void SetIncludedPathes(params string[] pathes)
		{
			throw new NotImplementedException();
		}

		public async Task SetIncludedPathesAsync(params string[] pathes)
		{
			throw new NotImplementedException();
		}

		public T Update(T item)
		{
			throw new NotImplementedException();
		}

		public async Task<T> UpdateAsync(T item)
		{
			throw new NotImplementedException();
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