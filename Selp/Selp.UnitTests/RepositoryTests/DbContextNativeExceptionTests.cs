namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Data.Entity.Validation;
	using Common.Entities;
	using Configuration;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class DbContextNativeExceptionTests
	{
		private FakeRepository repository;

		[TestInitialize]
		public void InitializeRepository()
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

			IDbSet<FakeEntity> dbSet = TestsMockFactory.CreateDbSet(testData);

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSet);

			dbContextMock
				.Setup(s => s.SaveChanges())
				.Throws<DbEntityValidationException>();

			repository = new FakeRepository(true, dbContextMock.Object, dbSet,
				new InMemoryConfiguration());
		}

		[TestMethod]
		public void CreateShouldntFailWhenDbContextThrowsException()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name =
					"New entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
				Description = "Description"
			});
			Assert.IsNull(result.ModifiedEntity, "Should not pass the entity back");
		}

		[TestMethod]
		public void UpdateShouldntFailWhenDbContextThrowsException()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(10, new FakeEntity
			{
				Name =
					"Existing entity with invalid too long as motherfucker's name. It is more than 50 symbols. Here you are.....................",
				Description = "Description"
			});
			Assert.IsNull(result.ModifiedEntity, "Should not pass the entity back");
		}
	}
}