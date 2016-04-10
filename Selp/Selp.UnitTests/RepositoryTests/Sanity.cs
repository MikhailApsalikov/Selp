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
	public class Sanity
	{
		SelpRepository<FakeEntity, int> repository;
		[TestInitialize]
		public void Initialize()
		{
			var fakeList = new List<FakeEntity>
			{
				new FakeEntity { Id = 1, Name = "Entity 1", IsDeleted = false, Description = "Description 1"},
				new FakeEntity { Id = 2, Name = "Entity 2", IsDeleted = false, Description = "Description 2" },
				new FakeEntity { Id = 3, Name = "Entity 3", IsDeleted = true, Description = "Description 3" },
				new FakeEntity { Id = 4, Name = "Entity 4", IsDeleted = false, Description = null }
			}.AsQueryable();

			var dbSetMock = new Mock<IDbSet<FakeEntity>>();
			dbSetMock.Setup(m => m.Provider).Returns(fakeList.Provider);
			dbSetMock.Setup(m => m.Expression).Returns(fakeList.Expression);
			dbSetMock.Setup(m => m.ElementType).Returns(fakeList.ElementType);
			dbSetMock.Setup(m => m.GetEnumerator()).Returns(fakeList.GetEnumerator());

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSetMock.Object);

			var mock = new Mock<SelpRepository<FakeEntity, int>>();
			mock.SetupGet(d => d.IsRemovingFake).Returns(false);
			mock.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mock.SetupGet(d => d.DbSet).Returns(dbSetMock.Object);
			repository = mock.Object;
		}

		[TestMethod]
		public void GetWorks()
		{
			var list = repository.GetAll();
			Assert.AreEqual(4, list.Count(), "GetAll doesn't work");
        }
	}
}