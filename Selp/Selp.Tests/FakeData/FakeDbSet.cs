namespace Selp.Tests.FakeData
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;

	public class FakeDbSet<T> : IDbSet<T>
		where T : class
	{
		private readonly IQueryable query;

		public FakeDbSet()
		{
			Local = new ObservableCollection<T>();
			query = Local.AsQueryable();
		}

		public virtual T Find(params object[] keyValues)
		{
			throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
		}

		public T Add(T item)
		{
			Local.Add(item);
			return item;
		}

		public T Remove(T item)
		{
			Local.Remove(item);
			return item;
		}

		public T Attach(T item)
		{
			Local.Add(item);
			return item;
		}

		public T Create()
		{
			return Activator.CreateInstance<T>();
		}

		public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
		{
			return Activator.CreateInstance<TDerivedEntity>();
		}

		public ObservableCollection<T> Local { get; set; }

		Type IQueryable.ElementType
		{
			get { return query.ElementType; }
		}

		Expression IQueryable.Expression
		{
			get { return query.Expression; }
		}

		IQueryProvider IQueryable.Provider
		{
			get { return query.Provider; }
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Local.GetEnumerator();
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return Local.GetEnumerator();
		}

		public T Detach(T item)
		{
			Local.Remove(item);
			return item;
		}
	}
}