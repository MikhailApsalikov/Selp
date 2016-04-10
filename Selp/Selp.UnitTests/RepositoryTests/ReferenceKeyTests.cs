namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Data.Entity;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository;

	[TestClass]
	public class ReferenceKeyTests
	{
		private SelpRepository<FakeEntityReferenceKey, string> repositoryReferenceKey;

		[TestMethod]
		[ExpectedException(typeof (ArgumentException), "Method didn't raise an exception when key is null")]
		public void GetByIdShouldThrowAnExceptionWhenKeyIsNull()
		{
			InitRepositoryParamsReferenceType();
			FakeEntityReferenceKey entity = repositoryReferenceKey.GetById(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void UpdateShouldThrowAnExceptionWhenKeyIsNull()
		{
			InitRepositoryParamsReferenceType();
			var result = repositoryReferenceKey.Update(null, new FakeEntityReferenceKey()
			{
				Name = "Preved medved!"
			});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void RemoveShouldThrowAnExceptionWhenKeyIsNull()
		{
			InitRepositoryParamsReferenceType();
			repositoryReferenceKey.Remove(null);
		}

		private void InitRepositoryParamsReferenceType()
		{
			var dbSetMock = new Mock<IDbSet<FakeEntityReferenceKey>>();

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntitiesReferenceKey)
				.Returns(dbSetMock.Object);

			var mockRepository = new Mock<SelpRepository<FakeEntityReferenceKey, string>>();
			mockRepository.SetupGet(d => d.DbContext).Returns(dbContextMock.Object);
			mockRepository.SetupGet(d => d.DbSet).Returns(dbSetMock.Object);
			mockRepository.SetupGet(d => d.IsRemovingFake).Returns(false);
			repositoryReferenceKey = mockRepository.Object;
		}
	}
}