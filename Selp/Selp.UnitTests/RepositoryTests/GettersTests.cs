namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data.Entity;
	using System.Linq;
	using Common.Entities;
	using Common.Exceptions;
	using Configuration;
	using Fake;
	using Interfaces;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class GettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private FakeRepository repository;


		[TestMethod]
		public void GetAllShouldReturnAllWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			Assert.AreEqual(150, repository.GetAll().Count(), "GetAll returned incorrect number of entities");
        }

		[TestMethod]
		public void GetAllShouldNotReturnAllWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
			Assert.AreEqual(100, repository.GetAll().Count(), "GetAll returned incorrect number of entities");
        }

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException), "Method didn't raise an exception when entity is not found")]
		public void GetByIdShouldThrowAnExceptionWhenEntityNotFound()
		{
			InitRepositoryParams(false);
			FakeEntity entity = repository.GetById(290);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityIsRemovedException), "Method didn't raise an exception when found deleted entity")]
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
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when expression is null")]
		public void GetByCustomExpressionShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
			List<FakeEntity> list = repository.GetByCustomExpression(null);
		}

		[TestMethod]
		public void GetByCustomExpressionShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
			List<FakeEntity> list = repository.GetByCustomExpression(a => false);
			Assert.IsNotNull(list, "Result is null");
            Assert.AreEqual(0, list.Count, "Result is not empty");
        }

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
			List<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(75, list.Count, "Result count invalid");
		}

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
			List<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(50, list.Count, "Result count invalid");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when filter is null")]
		public void GetByFilterShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
			int total;
			List<FakeEntity> list = repository.GetByFilter(null, out total);
		}

		[TestMethod]
		public void GetByFilterShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
			int total;
			BaseFilter filter = new BaseFilter
			{
				Page = 100,
				PageSize = 200
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Count, "Result is not empty");
            Assert.AreEqual(150, total, "Total incorrect");
            Assert.AreEqual(100, filter.Page, "Page incorrect");
            Assert.AreEqual(200, filter.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectCountWhenPageSizeIsSpecified()
		{
			InitRepositoryParams(true);
			int total;
			BaseFilter filter = new BaseFilter
			{
				PageSize = 20
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(20, list.Count, "Result count is wrong");
            Assert.AreEqual(100, total, "Result total is wrong");
            Assert.AreEqual(20, filter.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOffsetWhenPageIsSpecified()
		{
			InitRepositoryParams(true);
			int total;
			BaseFilter filter = new BaseFilter
			{
				PageSize = 20,
				Page = 2
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(21, list.Min(d => d.Id), "Minimal ID is wrong");
            Assert.AreEqual(2, filter.Page, "Page incorrect");
            Assert.AreEqual(20, filter.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOrderWhenItIsSpecifiedDescending()
		{
			InitRepositoryParams(true);
			int total;
			BaseFilter filter = new BaseFilter
			{
				PageSize = 20,
				Page = 3,
				SortDirection = ListSortDirection.Descending,
				SortField = "Id"
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(60, list[0].Id, "First item id is wrong");
            Assert.AreEqual(3, filter.Page, "Page incorrect");
            Assert.AreEqual(20, filter.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOrderWhenItIsSpecified()
		{
			InitRepositoryParams(true);
			int total;
			BaseFilter filter = new BaseFilter
			{
				PageSize = 20,
				Page = 3,
				SortDirection = ListSortDirection.Ascending,
				SortField = "Id"
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(41, list[0].Id, "First item id is wrong");
            Assert.AreEqual(3, filter.Page, "Page incorrect");
            Assert.AreEqual(20, filter.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetByFilterShouldThrowIfSortFieldNotFound()
		{
			InitRepositoryParams(true);
			int total;
			BaseFilter filter = new BaseFilter
			{
				SortDirection = ListSortDirection.Descending,
				SortField = "dsadsadsada"
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
		}

		[TestMethod]
		public void GetByFilterShouldUseDefaultIfPageSizeIfItIsIncorrect()
		{
			ISelpConfiguration configuration = new InMemoryConfiguration();
			configuration.DefaultPageSize = 11;
			InitRepositoryParams(true, configuration);
			int total;
			var filter = new BaseFilter
			{
				PageSize = -55
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(11, list.Count, "Result count is wrong");
			Assert.AreEqual(11, filter.PageSize, "Filter hasn't been normalized");
            Assert.AreEqual(100, total, "Total is incorrect");
            Assert.AreEqual(11, filter.PageSize, "Returned PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldUseFirstPageIfItIsIncorrect()
		{
			InitRepositoryParams(true);
			int total;
			var filter = new BaseFilter
			{
                Page = -5,
				PageSize = 20
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(1, list.Min(d => d.Id), "Minimal ID is incorrect");
			Assert.AreEqual(1, filter.Page, "Filter hasn't been normalized");
            Assert.AreEqual(20, filter.PageSize, "Returned PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByName()
		{
			InitRepositoryParams(false);
			int total;
			var filter = new BaseFilter
			{
				Search = "Entity 10"
			};
			List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(11, total, "Count is incorrect");
		}

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByNameNotIncludedDeleted()
		{
			InitRepositoryParams(true);
			int total;
			var filter = new BaseFilter
			{
				Search = "125"
			};
            List<FakeEntity> list = repository.GetByFilter(filter, out total);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, total, "Count is incorrect");
		}


		private void InitRepositoryParams(bool isRemovingFake, ISelpConfiguration configuration = null)
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

			dbSet = TestsMockFactory.CreateDbSet(testData);

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSet);

			repository = new FakeRepository(isRemovingFake, dbContextMock.Object, dbSet,
				configuration ?? new InMemoryConfiguration());
		}
	}
}