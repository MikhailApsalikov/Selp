namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Core;
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
	public class SettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private SelpRepository<FakeEntity, int> repository;

		[TestMethod]
		public void CreateShouldntFailWhenDbContextThrowsExcetion()
		{
			InitRepositoryParams(false);
			var result = repository.Create(new FakeEntity()
			{
				Name = "New entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
				Description = "Description"
			});
			Assert.IsNull(result.ModifiedEntity, "Should not pass the entity back");
			Assert.AreEqual(1, result.Errors.Count(), "Should contain an error");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when entity is null")]
		public void CreateShouldThrowWhenArgumentIsNull()
		{
			InitRepositoryParams(false);
			var result = repository.Create(null);
		}

		[TestMethod]
		public void UpdateShouldntFailWhenDbContextThrowsExcetion()
		{
			InitRepositoryParams(false);
			var result = repository.Update(10, new FakeEntity()
			{
				Name = "Existing entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
				Description = "Description"
			});
			Assert.IsNull(result.ModifiedEntity, "Should not pass the entity back");
			Assert.AreEqual(1, result.Errors.Count(), "Should contain an error");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when entity is null")]
		public void UpdateShouldThrowWhenEntityIsNull()
		{
			InitRepositoryParams(false);
			var result = repository.Update(10, null);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException), "Method didn't raise an exception when entity doesn't exist")]
		public void UpdateShouldThrowWhenEntityDoesntExist()
		{
			InitRepositoryParams(false);
			var result = repository.Update(1111, new FakeEntity()
			{
				Name = "Prison",
				Description = null
			});
		}

		[TestMethod]
		[ExpectedException(typeof(EntityIsRemovedException), "Method didn't raise an exception when entity is deleted")]
		public void UpdateShouldThrowWhenEntityDeleted()
		{
			InitRepositoryParams(true);
			var result = repository.Update(105, new FakeEntity()
			{
				Name = "Prison",
				Description = null
			});
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException), "Method didn't raise an exception when entity doesn't exist")]
		public void RemoveShouldThrowWhenEntityDoesntExist()
		{
			InitRepositoryParams(false);
			repository.Remove(1111);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityIsRemovedException), "Method didn't raise an exception when entity is deleted")]
		public void RemoveShouldThrowWhenEntityIsDeleted()
		{
			InitRepositoryParams(true);
			repository.Remove(105);
		}

		[TestMethod]
		public void FakeRemovingShouldWork()
		{
			InitRepositoryParams(true);
			repository.Remove(33);
			var entity = dbSet.FirstOrDefault(d => d.Id == 33);
			Assert.IsNotNull(entity, "Entity should be still in collection");
			Assert.AreEqual(true, entity.IsDeleted, "IsDeleted flag is not set");
		}

		[TestMethod]
		public void FakeRemovingShouldntWorkWhenItsOff()
		{
			InitRepositoryParams(false);
			repository.Remove(33);
			var entity = dbSet.FirstOrDefault(d => d.Id == 33);
			Assert.IsNull(entity, "Entity should not be in collection");
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
			mockRepository.SetupGet(d => d.FakeRemovingPropertyName).Returns("IsDeleted");
			mockRepository.Protected().Setup<IQueryable<FakeEntity>>("ApplyFilters", ItExpr.IsAny<BaseFilter>()).Returns<BaseFilter>(f =>
			{
				return testData.Where(s => s.Name.Contains(f.Search)).AsQueryable();
			});
			if (configuration != null)
			{
				mockRepository.SetupGet(d => d.Configuration).Returns(configuration);
			}

			repository = mockRepository.Object;
		}
		
	}
}