namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository;

	[TestClass]
	public class GettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private SelpRepository<FakeEntity, int> repository;


		[TestInitialize]
		public void Initialize()
		{
			var testData = new List<FakeEntity>();
			for (var i = 0; i < 150; i++)
			{
				testData.Add(new FakeEntity
				{
					Id = i,
					Name = "Entity " + i.ToString(),
					Description = null,
					IsDeleted = false
				});
			}
			IQueryable<FakeEntity> fakeList = testData.AsQueryable();

			var dbSetMock = new Mock<IDbSet<FakeEntity>>();
			dbSetMock.Setup(m => m.Provider).Returns(fakeList.Provider);
			dbSetMock.Setup(m => m.Expression).Returns(fakeList.Expression);
			dbSetMock.Setup(m => m.ElementType).Returns(fakeList.ElementType);
			dbSetMock.Setup(m => m.GetEnumerator()).Returns(fakeList.GetEnumerator());
			dbSet = dbSetMock.Object;

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSet);

			var mock = new Mock<SelpRepository<FakeEntity, int>>();
			mock.SetupGet(d => d.IsRemovingFake).Returns(false);
			mock.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mock.SetupGet(d => d.DbSet).Returns(dbSet);
			repository = mock.Object;
		}
	}
}