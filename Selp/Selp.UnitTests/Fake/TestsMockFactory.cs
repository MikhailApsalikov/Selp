namespace Selp.UnitTests.Fake
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Interfaces;
	using Moq;

	public static class TestsMockFactory
	{
		public static IDbSet<T> CreateDbSet<T, TKey>(IEnumerable<T> source) where T : class, ISelpEntity<TKey>
		{
			IQueryable<T> queryable = source.AsQueryable();

			var dbSetMock = new Mock<IDbSet<T>>();
			dbSetMock.Setup(m => m.Provider).Returns(queryable.Provider);
			dbSetMock.Setup(m => m.Expression).Returns(queryable.Expression);
			dbSetMock.Setup(m => m.ElementType).Returns(queryable.ElementType);
			dbSetMock.Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
			dbSetMock.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>((ids) => queryable.FirstOrDefault(a => ids.Contains(a.Id)));
			return dbSetMock.Object;
		}
	}
}