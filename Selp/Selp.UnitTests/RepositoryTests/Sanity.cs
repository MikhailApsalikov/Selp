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

	[TestClass]
	public class Sanity
	{
		private IDbSet<FakeEntity> dbSet;
		private FakeRepository repository;


		[TestInitialize]
		public void Initialize()
		{
			IEnumerable<FakeEntity> fakeList = new List<FakeEntity>
			{
				new FakeEntity {Id = 1, Name = "Entity 1", IsDeleted = false, Description = "Description 1"},
				new FakeEntity {Id = 2, Name = "Entity 2", IsDeleted = false, Description = "Description 2"},
				new FakeEntity {Id = 3, Name = "Entity 3", IsDeleted = true, Description = "Description 3"},
				new FakeEntity {Id = 4, Name = "Entity 4", IsDeleted = false, Description = null}
			};

			dbSet = TestsMockFactory.CreateDbSet(fakeList);

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSet);

			repository = new FakeRepository(false, dbContextMock.Object, dbSet,
				SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory));
			;
		}

		[TestMethod]
		public void GetAllWorks()
		{
			IEnumerable<FakeEntity> list = repository.GetAll();
			Assert.AreEqual(4, list.Count(), "GetAll doesn't work");
		}

		[TestMethod]
		public void GetByIdWorks()
		{
			FakeEntity entity = repository.GetById(1);
			Assert.AreEqual("Entity 1", entity.Name, "GetById doesn't work");
		}

		[TestMethod]
		public void GetByFilterWorks()
		{
			IEnumerable<FakeEntity> list = repository.GetByFilter(new BaseFilter());
			Assert.AreEqual(4, list.Count(), "GetByFilter doesn't work");
		}

		[TestMethod]
		public void GetByCustomExpressionWorks()
		{
			IEnumerable<FakeEntity> list = repository.GetByCustomExpression(d => true);
			Assert.AreEqual(4, list.Count(), "GetByCustomExpression doesn't work");
		}

		[TestMethod]
		public void CreateWorks()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Create(new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsNotNull(result.ModifiedEntity, "Create doesn't work");
			Assert.AreEqual(5, dbSet.Count(), "Create doesn't work");
		}

		[TestMethod]
		public void UpdateWorks()
		{
			RepositoryModifyResult<FakeEntity> result = repository.Update(2, new FakeEntity
			{
				Name = "New entity",
				Description = "Description"
			});
			Assert.IsNotNull(result.ModifiedEntity, "Update doesn't work");
			Assert.AreEqual(4, dbSet.Count(), "Update doesn't work");
			Assert.AreEqual("New entity", result.ModifiedEntity.Name, "Update doesn't work");
		}

		[TestMethod]
		public void Remove()
		{
			repository.Remove(2);
			Assert.AreEqual(3, dbSet.Count(), "Remove doesn't work");
			Assert.IsNull(dbSet.Find(2), "Update doesn't work");
		}
	}
}