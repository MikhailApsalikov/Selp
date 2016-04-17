namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using Configuration;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository.Validator;
	using ValidatorTests.ValidatorsMocks;

	[TestClass]
	public class ValidatorCallingTests
	{
		private FakeRepository repository;

		[TestInitialize]
		public void Initialize()
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

			IDbSet<FakeEntity> dbSetMock = TestsMockFactory.CreateDbSet(testData);
			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntities)
				.Returns(dbSetMock);
			repository = new FakeRepository(false, dbContextMock.Object, dbSetMock,
				SelpConfigurationFactory.GetConfiguration(ConfigurationTypes.InMemory));
		}

		[TestMethod]
		public void CreateCanPassWithValidator()
		{
			var mock = new Mock<SelpValidator>();
			repository.CreateValidator = mock.Object;
			repository.Create(new FakeEntity
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
			repository.Create(new FakeEntity
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
			repository.Create(new FakeEntity
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
			repository.UpdateValidator = mock.Object;
			repository.Update(1, new FakeEntity
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
			repository.UpdateValidator = mock;
			repository.Update(1, new FakeEntity
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
			repository.UpdateValidator = mock.Object;
			repository.Update(1, new FakeEntity
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(false, mock.Object.IsValid);
			Assert.AreEqual(1, mock.Object.Errors.Count);
		}
	}
}