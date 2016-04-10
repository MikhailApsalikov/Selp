namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Common.Exceptions;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository;

	[TestClass]
	public class GettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private SelpRepository<FakeEntity, int> repository;
		private SelpRepository<FakeEntityReferenceKey, string> repositoryReferenceKey;


		[TestMethod]
		public void GetAllShouldReturnAllWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			Assert.AreEqual(150, repository.GetAll(), "GetAll returned incorrect number of entities");
		}

		[TestMethod]
		public void GetAllShouldNotReturnAllWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
			Assert.AreEqual(100, repository.GetAll(), "GetAll returned incorrect number of entities");
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException), "Method didn't raise an exception when entity is not found")]
		public void GetByIdShouldThrowAnExceptionWhenEntityNotFound()
		{
			InitRepositoryParams(false);
			var entity = repository.GetById(290);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityIsDeletedException), "Method didn't raise an exception when found deleted entity")]
		public void GetByIdShouldThrowAnExceptionWhenEntityIsFakeDeleted()
		{
			InitRepositoryParams(true);
			var entity = repository.GetById(110);
		}

		[TestMethod]
		public void GetByIdShouldReturnAnEntityWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			var entity = repository.GetById(110);
			Assert.AreEqual("Entity 110", entity.Name, "Method didn't return entity");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void GetByIdShouldThrowAnExceptionWhenKeyIsNull()
		{
			InitRepositoryParamsReferenceType();
			var entity = repositoryReferenceKey.GetById(null);
		}

		[TestMethod]
		public void GetByCustomExpression()
		{
			InitRepositoryParams(false);
			var entity = repository.GetById(110);
			Assert.AreEqual("Entity 110", entity.Name, "Method didn't return entity");
		}






		private void InitRepositoryParams(bool isRemovingFake)
		{
			var testData = new List<FakeEntity>();
			for (var i = 1; i <= 150; i++)
			{
				testData.Add(new FakeEntity
				{
					Id = i,
					Name = "Entity " + i.ToString(),
					Description = null,
					IsDeleted = (i > 100)
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

			var mockRepository = new Mock<SelpRepository<FakeEntity, int>>();
			mockRepository.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mockRepository.SetupGet(d => d.DbSet).Returns(dbSet);
			mockRepository.SetupGet(d => d.IsRemovingFake).Returns(isRemovingFake);
			repository = mockRepository.Object;
		}

		private void InitRepositoryParamsReferenceType()
		{
			var dbSetMock = new Mock<IDbSet<FakeEntityReferenceKey>>();

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntitiesReferenceKey)
				.Returns(dbSetMock.Object);

			var mockRepository = new Mock<SelpRepository<FakeEntityReferenceKey, string>>();
			mockRepository.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mockRepository.SetupGet(d => d.DbSet).Returns(dbSetMock.Object);
			mockRepository.SetupGet(d => d.IsRemovingFake).Returns(false);
			repositoryReferenceKey = mockRepository.Object;
		}

	}
}