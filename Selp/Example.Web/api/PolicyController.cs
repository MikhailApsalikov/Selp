namespace Example.Web.api
{
	using System;
	using System.Globalization;
	using System.Linq;
	using Entities;
	using Entities.Enums;
	using Interfaces.Repositories;
	using Models;
	using Selp.Controller;

	public class PolicyController : SelpController<PolicyShortModel, PolicyModel, Policy, int>
	{
		public PolicyController(IPolicyRepository repository) : base(repository)
		{
		}

		public override string ControllerName => "Policy";

		protected override PolicyModel MapEntityToModel(Policy entity)
		{
			return new PolicyModel()
			{
				Id = entity.Id
			};
		}

		protected override Policy MapModelToEntity(PolicyModel model)
		{
			return new Policy
			{
				Id = model.Id
			};
		}

		protected override PolicyShortModel MapEntityToShortModel(Policy entity)
		{
			return new PolicyShortModel
			{
				Id = entity.Id,
				InsuranceSum = entity.InsuranceSum,
				ExpirationDate = entity.ExpirationDate.ToString("dd.MM.yyyy"),
				StartDate = entity.StartDate.ToString("dd.MM.yyyy"),
				InsurancePremium = entity.InsurancePremium,
				Status = ConvertStatus(entity.Status),
				CreatedDate = entity.CreatedDate.ToString("dd.MM.yyyy"),
				UserId = entity.UserId,
				Serial = entity.Serial,
				Number = entity.Number
			};
		}

		private string ConvertStatus(PolicyStatus status)
		{
			switch (status)
			{
				case PolicyStatus.Actual:
					return "Действующий";
				case PolicyStatus.Annulated:
					return "Аннулирован";
				case PolicyStatus.Project:
					return "Проект";
				default:
					throw new ArgumentException();
			}
		}

		private PolicyStatus ConvertStatus(string status)
		{
			switch (status)
			{
				case "Действующий":
					return PolicyStatus.Actual;
				case "Аннулирован":
					return PolicyStatus.Annulated;
				case "Проект":
					return PolicyStatus.Project;
				default:
					throw new ArgumentException();
			}
		}
	}
}