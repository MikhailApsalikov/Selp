namespace Selp.UnitTests.RepositoryTests
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using Common.Entities;
	using Configuration;
	using Fake;
	using Interfaces;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Validator;
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
				new InMemoryConfiguration());
		}

		[TestMethod]
		public void CreateRaiseValidator()
		{
			var flag = false;
			var mock = new Mock<ISelpValidator>();
			mock.Setup(s => s.Validate()).Callback(() => flag = true);
			mock.Setup(s => s.Errors).Returns(new List<ValidatorError>());
			repository.CreateValidator = mock.Object;
			repository.Create(new FakeEntity
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(true, flag);
		}

		[TestMethod]
		public void UpdateRaiseValidator()
		{
			var flag = false;
			var mock = new Mock<ISelpValidator>();
			mock.Setup(s => s.Validate()).Callback(() => flag = true);
			mock.Setup(s => s.Errors).Returns(new List<ValidatorError>());
			repository.UpdateValidator = mock.Object;
			repository.Update(1, new FakeEntity
			{
				Name = "Pass",
				Description = null
			});
			Assert.AreEqual(true, flag);
		}
	}
}