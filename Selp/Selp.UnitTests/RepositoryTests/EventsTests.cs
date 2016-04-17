namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Common.Entities;
	using Configuration;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using ValidatorTests.ValidatorsMocks;

	//создать
	[TestClass]
	public class EventsTests
	{
		private FakeRepository repository;

		[TestInitialize]
		public void InitRepositoryParams()
		{
			var testData = new List<FakeEntity>();
			for (var i = 1; i <= 150; i++)
			{
				testData.Add(new FakeEntity
				{
					Id = i,
					Name = "Entity " + i,
					Description = null,
					IsDeleted = i > 100
				});
			}
			IQueryable<FakeEntity> fakeList = testData.AsQueryable();

			IDbSet<FakeEntity> dbSetMock = TestsMockFactory.CreateDbSet(fakeList);

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSetMock);

			repository = new FakeRepository(false, dbContextMock.Object, dbSetMock,
				new InMemoryConfiguration());
		}

		[TestMethod]
		public void CreateShouldExecuteEvents()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsTrue(repository.IsAfterEventExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void UpdateShouldExecuteEvents()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsTrue(repository.IsAfterEventExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void RemoveShouldExecuteEvents()
		{
			repository.Remove(15);
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsTrue(repository.IsAfterEventExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void CreateShouldExecuteOnlyBeforeEventWhenFail()
		{
			repository.CreateValidator = new FailedValidator();
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsFalse(repository.IsAfterEventExecuted, "After event fired");
		}

		[TestMethod]
		public void UpdateShouldExecuteOnlyBeforeEventWhenFail()
		{
			repository.UpdateValidator = new FailedValidator();
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsFalse(repository.IsAfterEventExecuted, "After event fired");
		}

		[TestMethod]
		public void RemoveShouldExecuteOnlyBeforeEventWhenFail()
		{
			try
			{
				repository.Remove(43); // special ID for fail mock
			}
			catch
			{
				// ignored
			}
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsFalse(repository.IsAfterEventExecuted, "After event fired");
		}
	}
}