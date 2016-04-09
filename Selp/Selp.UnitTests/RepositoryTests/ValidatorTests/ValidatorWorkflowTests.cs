namespace Selp.UnitTests.RepositoryTests.ValidatorTests
{
	using System.Web.Http;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using Repository.Validator;
	using ValidatorsMocks;

	[TestClass]
	public class ValidatorWorkflowTests
	{
		[TestMethod]
		public void ValidatorWorks()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.Validate();
			Assert.AreEqual(true, mock.Object.IsValid, "Validator doesn't work in empty way");
		}

		[TestMethod]
		public void ValidatorWorksOnFails()
		{
			var mock = new FailedValidator();
			mock.Validate();
			Assert.AreEqual(false, mock.IsValid, "Validator hasn't recognized an error");
			Assert.AreEqual(1, mock.Errors.Count, "Validator didn't add error to the list");
			Assert.AreEqual("Text", mock.Errors[0].Text, "Validator didn't add content to the error");
			Assert.AreEqual("FieldName", mock.Errors[0].FieldName, "Validator didn't save fieldname");
			Assert.AreEqual(0, mock.Errors[0].ParentEntities.Count, "Parent entities should be empty");
		}

		[TestMethod]
		public void ValidatorCanAddNestedValidators()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.NestedValidators.Add(new FailedValidator());
			mock.Object.NestedValidators.Add(new FailedValidator());
			Assert.AreEqual(2, mock.Object.NestedValidators.Count, "Validators haven't been added to the collection");
		}

		[TestMethod]
		public void NestedValidatorWorks()
		{
			var mock = new Mock<SelpValidator>();
			mock.SetupGet(d => d.EntityName).Returns("Mock");
			mock.Object.NestedValidators.Add(new FailedValidator());
			mock.Object.Validate();
			Assert.AreEqual(1, mock.Object.NestedValidators.Count, "Validator hasn't been added to the collection");
			Assert.AreEqual(false, mock.Object.IsValid, "Validator hasn't recognized an error");
			Assert.AreEqual(1, mock.Object.Errors.Count, "Validator didn't add error to the list");
			Assert.AreEqual("Text", mock.Object.Errors[0].Text, "Validator didn't add content to the error");
			Assert.AreEqual("FieldName", mock.Object.Errors[0].FieldName, "Validator didn't save fieldname");
			Assert.AreEqual(1, mock.Object.Errors[0].ParentEntities.Count, "Parent entities should contain 1 element");
			Assert.AreEqual("Mock", mock.Object.Errors[0].ParentEntities[0], "Parent entities should contain 1 element: Mock");
        }
	}
}