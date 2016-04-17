namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using Common.Exceptions;
	using Configuration;
	using Entities;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class SettersTests
	{
		private IDbSet<FakeEntity> dbSet;
		private FakeRepository repository;

		[TestMethod]
		public void CreateShouldntFailWhenDbContextThrowsExcetion()
		{
			InitRepositoryParams(false);
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name =
					"New entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
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
			RepositoryModifyResult<FakeEntity> result = repository.Create(null);
		}

		[TestMethod]
		public void UpdateShouldntFailWhenDbContextThrowsExcetion()
		{
			InitRepositoryParams(false);
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name =
					"Existing entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
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
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, null);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException), "Method didn't raise an exception when entity doesn't exist")]
		public void UpdateShouldThrowWhenEntityDoesntExist()
		{
			InitRepositoryParams(false);
			RepositoryModifyResult<FakeEntity> result = repository.Update(1111, new FakeEntity
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
			RepositoryModifyResult<FakeEntity> result = repository.Update(105, new FakeEntity
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
			FakeEntity entity = dbSet.FirstOrDefault(d => d.Id == 33);
			Assert.IsNotNull(entity, "Entity should be still in collection");
			Assert.AreEqual(true, entity.IsDeleted, "IsDeleted flag is not set");
		}

		[TestMethod]
		public void FakeRemovingShouldntWorkWhenItsOff()
		{
			InitRepositoryParams(false);
			repository.Remove(33);
			FakeEntity entity = dbSet.FirstOrDefault(d => d.Id == 33);
			Assert.IsNull(entity, "Entity should not be in collection");
		}

		private void InitRepositoryParams(bool isRemovingFake)
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
				new InMemoryConfiguration());
		}
	}
}