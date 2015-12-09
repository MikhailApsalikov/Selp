namespace Selp.Kernel.Interfaces
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	public interface ISelpRepository<T, in TKey> : IDisposable where T : class
	{
		IQueryable<T> GetAll();
		IQueryable<T> GetRange<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count);
		IQueryable<T> Filter(Expression<Func<T, bool>> filter);

		IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector, int offset, int count);

		T GetById(TKey key);
		T FilterFirst(Expression<Func<T, bool>> filter);
		T FilterSingle(Expression<Func<T, bool>> filter);
		T Add(T item);
		T Update(T item);
		void Remove(T item);
		void RemoveById(TKey key);


		Task<IQueryable<T>> GetAllAsync();
		Task<IQueryable<T>> GetRangeAsync<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count);
		Task<IQueryable<T>> FilterAsync(Expression<Func<T, bool>> filter);

		Task<IQueryable<T>> FilterAsync<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector, int offset, int count);
		Task<T> GetByIdAsync(TKey key);
		Task<T> FilterFirstAsync(Expression<Func<T, bool>> filter);
		Task<T> FilterSingleAsync(Expression<Func<T, bool>> filter);
		Task<T> AddAsync(T item);
		Task<T> UpdateAsync(T item);
		Task RemoveAsync(T item);
		Task RemoveByIdAsync(TKey key);
	}
}