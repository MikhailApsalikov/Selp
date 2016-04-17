namespace Selp.UnitTests.RepositoryTests
{
	using System;
	using System.Data.Entity;
	using Common.Entities;
	using Configuration;
	using Fake;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;

	[TestClass]
	public class ReferenceKeyTests
	{
		private FakeRepositoryReferenceKey repositoryReferenceKey;


		[TestInitialize]
		public void InitRepositoryParamsReferenceType()
		{
			var dbSetMock = new Mock<IDbSet<FakeEntityReferenceKey>>();

			var dbContextMock = new Mock<FakeDbContext>();
			dbContextMock
				.Setup(x => x.FakeEntitiesReferenceKey)
				.Returns(dbSetMock.Object);

			repositoryReferenceKey = new FakeRepositoryReferenceKey(dbContextMock.Object, dbSetMock.Object,
				new InMemoryConfiguration());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void GetByIdShouldThrowAnExceptionWhenKeyIsNull()
		{
			FakeEntityReferenceKey entity = repositoryReferenceKey.GetById(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void UpdateShouldThrowAnExceptionWhenKeyIsNull()
		{
			RepositoryModifyResult<FakeEntityReferenceKey> result = repositoryReferenceKey.Update(null,
				new FakeEntityReferenceKey
				{
					Name = "Preved medved!"
				});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "Method didn't raise an exception when key is null")]
		public void RemoveShouldThrowAnExceptionWhenKeyIsNull()
		{
			repositoryReferenceKey.Remove(null);
		}
	}
}