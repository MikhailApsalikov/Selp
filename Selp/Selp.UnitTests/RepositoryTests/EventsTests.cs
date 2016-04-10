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
	using Moq.Protected;
	using Repository;

	//создать
	[TestClass]
	public class EventsTests
	{
		private SelpRepository<FakeEntity, int> repository;
		private bool isBeforeExecuted;
		private bool isAfterExecuted;

		[TestMethod]
		public void CreateShouldExecuteEvents()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsTrue(isAfterExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void UpdateShouldExecuteEvents()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsTrue(isAfterExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void RemoveShouldExecuteEvents()
		{
			repository.Remove(15);
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsTrue(isAfterExecuted, "After event isn't fired");
		}

		[TestMethod]
		public void CreateShouldExecuteOnlyBeforeEventWhenFail()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name = "New entity dasd sa das d asd sa da sd sad sad asd as das da sd asd asd as das d asd as das das sd as asd sa das das ",
				Description = "Description"
			});
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsFalse(isAfterExecuted, "After event fired");
		}

		[TestMethod]
		public void UpdateShouldExecuteOnlyBeforeEventWhenFail()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name = "New entity dasd sa das d asd sa da sd sad sad asd as das da sd asd asd as das d asd as das das sd as asd sa das das ",
				Description = "Description"
			});
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsFalse(isAfterExecuted, "After event fired");
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
			Assert.IsTrue(isBeforeExecuted, "Before event isn't fired");
			Assert.IsFalse(isAfterExecuted, "After event fired");
		}



		[TestInitialize]
		private void InitRepositoryParams()
		{
			isBeforeExecuted = false;
			isAfterExecuted = false;
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

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSetMock.Object);

			var mockRepository = new Mock<SelpRepository<FakeEntity, int>>();
			mockRepository.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mockRepository.SetupGet(d => d.DbSet).Returns(dbSetMock.Object);
			mockRepository.SetupGet(d => d.IsRemovingFake).Returns(true);
			mockRepository.SetupGet(d => d.FakeRemovingPropertyName).Returns("IsDeleted");
			mockRepository.Protected().Setup<IQueryable<FakeEntity>>("ApplyFilters", ItExpr.IsAny<BaseFilter>()).Returns<BaseFilter>(f =>
			{
				return testData.Where(s => s.Name.Contains(f.Search)).AsQueryable();
			});
			mockRepository.Protected().Setup("OnCreating", ItExpr.IsAny<int>()).Callback(()=> isBeforeExecuted = true);
			mockRepository.Protected().Setup("OnCreated", ItExpr.IsAny<int>()).Callback(() => isAfterExecuted = true);
			mockRepository.Protected().Setup("OnUpdating", ItExpr.IsAny<int>()).Callback(() => isBeforeExecuted = true);
			mockRepository.Protected().Setup("OnUpdated", ItExpr.IsAny<int>()).Callback(() => isAfterExecuted = true);
			mockRepository.Protected().Setup("OnRemoving", ItExpr.IsAny<int>()).Callback(() => isBeforeExecuted = true);
			mockRepository.Protected().Setup("OnRemoved", ItExpr.IsAny<int>()).Callback(() => isAfterExecuted = true);

			repository = mockRepository.Object;
		}
	}
}