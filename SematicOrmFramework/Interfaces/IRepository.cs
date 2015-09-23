namespace SematicOrmFramework.Interfaces
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	public interface IRepository<T, in TKey> : IDisposable where T : class
	{
		IQueryable<T> GetAll();
		IQueryable<T> GetAsNoTracking();
		IQueryable<T> GetRange<TValue>(Expression<Func<T, TValue>> keySelector, int offset, int count);
		IQueryable<T> Filter(Expression<Func<T, bool>> filter);

		IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector,
			int offset, int count);

		T GetById(TKey key);
		T GetByIdAsNoTracking(TKey key);
		T FilterFirst(Expression<Func<T, bool>> filter);
		T FilterSingle(Expression<Func<T, bool>> filter);
		T Add(T item);
		T Update(T item);
		void Remove(T item);
		void RemoveById(TKey key);
		void SetIncludedPathes(params string[] pathes);
	}
}