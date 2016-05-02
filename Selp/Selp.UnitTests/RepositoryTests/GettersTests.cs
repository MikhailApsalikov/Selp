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
			Assert.AreEqual(150, repository.GetAll().Data.Count(), "GetAll returned incorrect number of entities");
            Assert.AreEqual(150, repository.GetAll().Total, "GetAll returned incorrect number of entities");
        }

		[TestMethod]
		public void GetAllShouldNotReturnAllWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
			Assert.AreEqual(100, repository.GetAll().Data.Count(), "GetAll returned incorrect number of entities");
            Assert.AreEqual(100, repository.GetAll().Total, "GetAll returned incorrect number of entities");
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
			EntitiesListResult<FakeEntity> list = repository.GetByCustomExpression(null);
		}

		[TestMethod]
		public void GetByCustomExpressionShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
            EntitiesListResult<FakeEntity> list = repository.GetByCustomExpression(a => false);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Total, "Total is not empty");
            Assert.AreEqual(0, list.Data.Count(), "Result is not empty");
        }

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOff()
		{
			InitRepositoryParams(false);
            EntitiesListResult<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(75, list.Total, "Result count invalid");
		}

		[TestMethod]
		public void GetByCustomExpressionShouldWorkWhenFakeRemovingIsOn()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByCustomExpression(a => a.Id%2 == 0);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(50, list.Total, "Result count invalid");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when filter is null")]
		public void GetByFilterShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(null);
		}

		[TestMethod]
		public void GetByFilterShouldReturnEmptyListWhenNoEntitiesFound()
		{
			InitRepositoryParams(false);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				Page = 100,
				PageSize = 200
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Data.Count(), "Result is not empty");
            Assert.AreEqual(150, list.Total, "Total incorrect");
            Assert.AreEqual(100, list.Page, "Page incorrect");
            Assert.AreEqual(200, list.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectCountWhenPageSizeIsSpecified()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(20, list.Data.Count(), "Result count is wrong");
            Assert.AreEqual(100, list.Total, "Result total is wrong");
            Assert.AreEqual(20, list.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOffsetWhenPageIsSpecified()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20,
				Page = 2
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(21, list.Data.Min(d => d.Id), "Minimal ID is wrong");
            Assert.AreEqual(2, list.Page, "Page incorrect");
            Assert.AreEqual(20, list.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOrderWhenItIsSpecifiedDescending()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20,
				Page = 3,
				SortDirection = ListSortDirection.Descending,
				SortField = "Id"
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(60, list.Data.ToList()[0].Id, "First item id is wrong");
            Assert.AreEqual(3, list.Page, "Page incorrect");
            Assert.AreEqual(20, list.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldHaveCorrectOrderWhenItIsSpecified()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				PageSize = 20,
				Page = 3,
				SortDirection = ListSortDirection.Ascending,
				SortField = "Id"
			});
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(41, list.Data.ToList()[0].Id, "First item id is wrong");
            Assert.AreEqual(3, list.Page, "Page incorrect");
            Assert.AreEqual(20, list.PageSize, "PageSize incorrect");
        }

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetByFilterShouldThrowIfSortFieldNotFound()
		{
			InitRepositoryParams(true);
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(new BaseFilter
			{
				SortDirection = ListSortDirection.Descending,
				SortField = "dsadsadsada"
			});
		}

		[TestMethod]
		public void GetByFilterShouldUseDefaultIfPageSizeIfItIsIncorrect()
		{
			ISelpConfiguration configuration = new InMemoryConfiguration();
			configuration.DefaultPageSize = 11;
			InitRepositoryParams(true, configuration);
			var filter = new BaseFilter
			{
				PageSize = -55
			};
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(11, list.Data.Count(), "Result count is wrong");
			Assert.AreEqual(11, filter.PageSize, "Filter hasn't been normalized");
            Assert.AreEqual(100, list.Total, "Total is incorrect");
            Assert.AreEqual(11, list.PageSize, "Returned PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldUseFirstPageIfItIsIncorrect()
		{
			InitRepositoryParams(true);
			var filter = new BaseFilter
			{
                Page = -5,
				PageSize = 20
			};
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(1, list.Data.Min(d => d.Id), "Minimal ID is incorrect");
			Assert.AreEqual(1, filter.Page, "Filter hasn't been normalized");
            Assert.AreEqual(1, list.Page, "Returned Page incorrect");
            Assert.AreEqual(20, list.PageSize, "Returned PageSize incorrect");
        }

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByName()
		{
			InitRepositoryParams(false);
			var filter = new BaseFilter
			{
				Search = "Entity 10"
			};
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(11, list.Total, "Count is incorrect");
		}

		[TestMethod]
		public void GetByFilterShouldFilterEntitiesByNameNotIncludedDeleted()
		{
			InitRepositoryParams(true);
			var filter = new BaseFilter
			{
				Search = "125"
			};
            EntitiesListResult<FakeEntity> list = repository.GetByFilter(filter);
			Assert.IsNotNull(list, "Result is null");
			Assert.AreEqual(0, list.Total, "Count is incorrect");
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