namespace SematicOrmFramework.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Data;

	public interface IDbContext : IDisposable
	{
		int SaveChanges();
		void Update<TEntity>(TEntity entity) where TEntity : class;
		void Detach<TEntity>(TEntity entity) where TEntity : class;
		ITransaction BeginTransaction();
		ITransaction BeginTransaction(IsolationLevel level);
		IEnumerable<T> SelectQuery<T>(string query, params object[] parameters);
	}
}