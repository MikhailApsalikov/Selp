namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Configuration;
	using Entities;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

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
					Name = "Entity " + i.ToString(),
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

			repository = new FakeRepository(true, dbContextMock.Object, dbSetMock,
				SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory));
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
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name =
					"New entity dasd sa das d asd sa da sd sad sad asd as das da sd asd asd as das d asd as das das sd as asd sa das das ",
				Description = "Description"
			});
			Assert.IsTrue(repository.IsBeforeEventExecuted, "Before event isn't fired");
			Assert.IsFalse(repository.IsAfterEventExecuted, "After event fired");
		}

		[TestMethod]
		public void UpdateShouldExecuteOnlyBeforeEventWhenFail()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name =
					"New entity dasd sa das d asd sa da sd sad sad asd as das da sd asd asd as das d asd as das das sd as asd sa das das ",
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
				repository.Remove(1000);
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