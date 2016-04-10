namespace Selp.UnitTests.RepositoryTests.ValidatorTests
{
	using System.Collections.Generic;
	using Common.Exceptions;
	using Entities;
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
			mock.Object.AddNestedValidator(new FailedValidator());
			mock.Object.AddNestedValidator(new FailedValidator());
			Assert.AreEqual(2, mock.Object.NestedValidators.Count, "Validators haven't been added to the collection");
		}

		[TestMethod]
		public void ValidatorCanPassNullNesterValidators()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.AddNestedValidator(null);
			Assert.AreEqual(0, mock.Object.NestedValidators.Count, "Validators list is not empty");
		}

		[TestMethod]
		public void ValidatorCanCreateNestedValidatorUsingConstructorWithParentValidator()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.AddNestedValidator(new FailedValidator(mock.Object));
			Assert.AreEqual(1, mock.Object.NestedValidators.Count, "Validator hasn't been added to the collection");
		}

		[TestMethod]
		public void NestedValidatorWorks()
		{
			var mock = new Mock<SelpValidator>();
			mock.SetupGet(d => d.EntityName).Returns("Mock");
			mock.Object.AddNestedValidator(new FailedValidator());
			mock.Object.Validate();
			Assert.AreEqual(1, mock.Object.NestedValidators.Count, "Validator hasn't been added to the collection");
			Assert.AreEqual(false, mock.Object.IsValid, "Validator hasn't recognized an error");
			Assert.AreEqual(1, mock.Object.Errors.Count, "Validator didn't add error to the list");
			Assert.AreEqual("Text", mock.Object.Errors[0].Text, "Validator didn't add content to the error");
			Assert.AreEqual("FieldName", mock.Object.Errors[0].FieldName, "Validator didn't save fieldname");
			Assert.AreEqual(1, mock.Object.Errors[0].ParentEntities.Count, "Parent entities should contain 1 element");
			Assert.AreEqual("Mock", mock.Object.Errors[0].ParentEntities[0], "Parent entities should contain 1 element: Mock");
		}

		[TestMethod]
		public void NestedValidatorLevel2Works()
		{
			var mock = new Mock<SelpValidator>();
			mock.SetupGet(d => d.EntityName).Returns("Mock");
			var nestedLevel1 = new FailedValidator();
			mock.Object.AddNestedValidator(nestedLevel1);
			nestedLevel1.AddNestedValidator(new FailedValidatorLevel2());
			mock.Object.Validate();
			Assert.AreEqual(1, mock.Object.NestedValidators.Count, "Validator hasn't been added to the collection");
			Assert.AreEqual(false, mock.Object.IsValid, "Validator hasn't recognized an error");
			Assert.AreEqual(2, mock.Object.Errors.Count, "Both nested validators should raise an error. They actually didn't.");
			Assert.AreEqual("Text", mock.Object.Errors[0].Text, "Level1 validator text failed");
			Assert.AreEqual("FieldName", mock.Object.Errors[0].FieldName, "Level1 validator fieldname failed");
			Assert.AreEqual(1, mock.Object.Errors[0].ParentEntities.Count, "Parent entities level 1 should contain 1 element");
			Assert.AreEqual("Mock", mock.Object.Errors[0].ParentEntities[0],
				"Parent entities level 1 should contain 1 element: Mock");
			Assert.AreEqual("Text level 2", mock.Object.Errors[1].Text, "Level2 validator text failed");
			Assert.IsNull(mock.Object.Errors[1].FieldName, "Level2 validator fieldname failed (should contains nothing)");
			Assert.AreEqual(2, mock.Object.Errors[1].ParentEntities.Count, "Parent entities level 2 should contain 2 elements");
			Assert.AreEqual("Mock", mock.Object.Errors[1].ParentEntities[0],
				"Parent entities level 2 should contain element Mock on 1st position");
			Assert.AreEqual("Failed", mock.Object.Errors[1].ParentEntities[1],
				"Parent entities level 2 should contain element Failed on 2nd position");
		}

		[TestMethod]
		[ExpectedException(typeof (WorkflowException))]
		public void DoubleValidationShouldRaiseAnException()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.Validate();
			mock.Object.Validate();
		}

		[TestMethod]
		[ExpectedException(typeof (WorkflowException))]
		public void AddingNestedAfterValidationShouldRaiseAnException()
		{
			var mock = new Mock<SelpValidator>();
			mock.Object.Validate();
			mock.Object.AddNestedValidator(new Mock<SelpValidator>().Object);
		}

		[TestMethod]
		[ExpectedException(typeof (WorkflowException))]
		public void GettingErrorsBeforeValidationShouldRaiseAnException()
		{
			var mock = new Mock<SelpValidator>();
			List<ValidatorError> errors = mock.Object.Errors;
		}

		[TestMethod]
		[ExpectedException(typeof (WorkflowException))]
		public void CheckingIsValidBeforeValidationShouldRaiseAnException()
		{
			var mock = new Mock<SelpValidator>();
			bool isValid = mock.Object.IsValid;
		}
	}
}