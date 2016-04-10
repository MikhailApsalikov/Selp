namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data.Entity;
	using System.Linq;
	using Common.Exceptions;
	using Configuration;
	using Entities;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Moq.Protected;
	using Repository;

	[TestClass]
	public class GettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private SelpRepository<FakeEntity, int> repository;


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
		[ExpectedException(typeof (EntityNotFoundException), "Method didn't raise an exception when entity is not found")]
		public void GetByIdShouldThrowAnExceptionWhenEntityNotFound()
		{
			InitRepositoryParams(false);
			FakeEntity entity = repository.GetById(290);
		}

		[TestMethod]
		[ExpectedException(typeof (EntityIsRemovedException), "Method didn't raise an exception when found deleted entity")]
		public void GetByIdShouldThrowAnExceptionWhenEntityIsFakeDeleted()
		{
			InitRepositoryParams(true);
			FakeEntity entity = repository.GetById(110);
		}

		[TestMethod]
		public void GetByIdShouldReturnAnEntityWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			FakeEntity entity = repository.GetById(110);
			Assert.AreEqual("Entity 110", entity.Name, "Method didn't return entity");
		}

		[TestMethod]
		public void GetByCustomExpression()
		{
			InitRepositoryParams(false);
			FakeEntity entity = repository.GetById(110);
			Assert.AreEqual("Entity 110", entity.Name, "Method didn't return entity");
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException), "Method didn't raise an exception when expression is null")]
		public void GetByCustomExpressionShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
			IQueryable<FakeEntity> list = repository.GetByCustomExpression(null);
		}

		[TestMethod]
		public void GetByCustomExpressionShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
			IQueryable<FakeEntity> list = repository.GetByCustomExpression(a => false);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Count(), "Result is not empty");
		}

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			IQueryable<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(75, list.Count(), "Result count invalid");
		}

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
			IQueryable<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(50, list.Count(), "Result count invalid");
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException), "Method didn't raise an exception when filter is null")]
		public void GetByFilterShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
			IQueryable<FakeEntity> list = repository.GetByFilter(null);
		}

		[TestMethod]
		public void GetByFilterShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
			IQueryable<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				Page = 100,
				PageSize = 200
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Count(), "Result is not empty");
		}

		[TestMethod]
		public void GetByFilterShouldHaveCorrectCountWhenPageSizeIsSpecified()
		{
			InitRepositoryParams(true);
			IQueryable<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(20, list.Count(), "Result count is wrong");
		}

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOffsetWhenPageIsSpecified()
		{
			InitRepositoryParams(true);
			IQueryable<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20,
				Page = 2
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(21, list.Min(d => d.Id), "Minimal ID is wrong");
		}

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOrderWhenItIsSpecified()
		{
			InitRepositoryParams(true);
			IQueryable<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20,
				Page = 3,
				SortDirection = ListSortDirection.Descending,
				SortField = "Id"
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(60, list.ToList()[0], "First item id is wrong");
		}

		[TestMethod]
		public void GetByFilterShouldUseDefaultIfPageSizeIfItIsIncorrect()
		{
			ISelpConfiguration configuration = SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory);
			configuration.DefaultPageSize = 11;
			InitRepositoryParams(true, configuration);
			var filter = new BaseFilter
			{
				PageSize = -55
			};
			IQueryable<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(11, list.Count(), "Result count is wrong");
			Assert.AreEqual(11, filter.PageSize, "Filter hasn't been normalized");
		}

		[TestMethod]
		public void GetByFilterShouldUseFirstPageIfItIsIncorrect()
		{
			InitRepositoryParams(true);
			var filter = new BaseFilter
			{
				PageSize = 20
			};
			IQueryable<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(1, list.Min(d => d.Id), "Minimal ID is incorrect");
			Assert.AreEqual(1, filter.Page, "Filter hasn't been normalized");
		}

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByName()
		{
			InitRepositoryParams(false);
			var filter = new BaseFilter
			{
				Search = "Entity 1"
			};
			IQueryable<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(3, list.Count(), "Count is incorrect");
		}

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByNameNotIncludedDeleted()
		{
			InitRepositoryParams(true);
			var filter = new BaseFilter
			{
				Search = "125"
			};
			IQueryable<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Count(), "Count is incorrect");
		}


		private void InitRepositoryParams(bool isRemovingFake, ISelpConfiguration configuration = null)
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
			mockRepository.Protected()
				.Setup<IQueryable<FakeEntity>>("ApplyFilters", ItExpr.IsAny<BaseFilter>())
				.Returns<BaseFilter>(f => { return testData.Where(s => s.Name.Contains(f.Search)).AsQueryable(); });
			if (configuration != null)
			{
				mockRepository.SetupGet(d => d.Configuration).Returns(configuration);
			}

			repository = mockRepository.Object;
		}
	}
}