namespace Selp.UnitTests.RepositoryTests
{
	using Configuration;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository.Validator;
	using System.Collections.Generic;   //создать
	using System.Data.Entity;
	using System.Linq;
	using ValidatorTests.ValidatorsMocks;
	[TestClass]
	public class ValidatorCallingTests
	{
		FakeRepository repository;

		[TestInitialize]
		public void Initialize()
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

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSetMock.Object);

			repository = new FakeRepository(false, dbContextMock.Object, dbSetMock.Object, SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory));
		}

		[TestMethod]
		public void CreateCanPassWithValidator()
		{
			var mock = new Mock<SelpValidator>();
			repository.CreateValidator = mock.Object;
			repository.Create(new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(true, mock.Object.IsValid);
		}

		[TestMethod]
		public void CreateEntityCanBeFailedByValidator()
		{
			var mock = new FailedValidator();
			repository.CreateValidator = mock;
			repository.Create(new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(false, mock.IsValid);
			Assert.AreEqual(1, mock.Errors.Count);
		}

		[TestMethod]
		public void CreateEntityCanBeFailedByNestedValidator()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.AddNestedValidator(new FailedValidator());
			repository.CreateValidator = mock.Object;
			repository.Create(new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(false, mock.Object.IsValid);
			Assert.AreEqual(1, mock.Object.Errors.Count);
		}

		[TestMethod]
		public void UpdateCanPassWithValidator()
		{
			var mock = new Mock<SelpValidator>();
			repository.CreateValidator = mock.Object;
			repository.Update(1, new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(true, mock.Object.IsValid);
		}

		[TestMethod]
		public void UpdateEntityCanBeFailedByValidator()
		{
			var mock = new FailedValidator();
			repository.CreateValidator = mock;
			repository.Update(1, new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(false, mock.IsValid);
			Assert.AreEqual(1, mock.Errors.Count);
		}

		[TestMethod]
		public void UpdateEntityCanBeFailedByNestedValidator()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.AddNestedValidator(new FailedValidator());
			repository.CreateValidator = mock.Object;
			repository.Update(1, new FakeEntity()
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(false, mock.Object.IsValid);
			Assert.AreEqual(1, mock.Object.Errors.Count);
		}
	}
}