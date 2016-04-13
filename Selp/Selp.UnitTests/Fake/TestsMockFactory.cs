namespace Selp.UnitTests.Fake
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Interfaces;
	using Moq;

	public static class TestsMockFactory
	{
		private static readonly Random Random = new Random();

		public static IDbSet<T> CreateDbSet<T>(IEnumerable<T> source) where T : class, ISelpEntity<int>
		{
			IQueryable<T> queryable = source.AsQueryable();

			var dbSetMock = new Mock<IDbSet<T>>();
			dbSetMock.Setup(m => m.Provider).Returns(() => queryable.Provider);
			dbSetMock.Setup(m => m.Expression).Returns(() => queryable.Expression);
			dbSetMock.Setup(m => m.ElementType).Returns(() => queryable.ElementType);
			dbSetMock.Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
			dbSetMock.Setup(m => m.Find(It.IsAny<object[]>()))
				.Returns<object[]>(ids => queryable.FirstOrDefault(a => ids.Contains(a.Id)));
			dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Returns<T>(e =>
			{
				e.Id = Random.Next(500, 50000000);
				var list = queryable.ToList();
				list.Add(e);
				queryable = list.AsQueryable();
				return e;
			});

			dbSetMock.Setup(m => m.Remove(It.IsAny<T>())).Returns<T>(e =>
			{
				var list = queryable.ToList();
				list.Remove(e);
				queryable = list.AsQueryable();
				return e;
			});
			return dbSetMock.Object;
		}
	}
}