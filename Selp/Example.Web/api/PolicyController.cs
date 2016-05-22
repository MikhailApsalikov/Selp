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

	public class PolicyController : SelpController<PolicyModel, PolicyModel, Policy, int>
	{
		public PolicyController(IPolicyRepository repository) : base(repository)
		{
		}

		public override string ControllerName => "Policy";

		protected override PolicyModel MapEntityToModel(Policy entity)
		{
			return new PolicyModel
			{
				Id = entity.Id,
				InsuranceSum = entity.InsuranceSum,
				RegionId = entity.RegionId,
				ExpirationDate = entity.ExpirationDate.ToString("dd.MM.yyyy"),
				StartDate = entity.StartDate.ToString("dd.MM.yyyy"),
				InsurancePremium = entity.InsurancePremium,
				Attachments = entity.Attachments?.Select(s => s.Id).ToList(),
				Status = ConvertStatus(entity.Status),
				CreatedDate = entity.CreatedDate.ToString("dd.MM.yyyy"),
				UserId = entity.UserId,
				InsuredList = entity.Parties?.Select(p => p.Id).ToList(),
				Serial = entity.Serial,
				Number = entity.Number
			};
		}

		protected override Policy MapModelToEntity(PolicyModel model)
		{
			return new Policy
			{
				ExpirationDate = DateTime.Parse(model.ExpirationDate, new CultureInfo("ru-RU")),
				CreatedDate = DateTime.Parse(model.CreatedDate, new CultureInfo("ru-RU")),
				InsurancePremium = model.InsurancePremium,
				InsuranceSum = model.InsuranceSum,
				RegionId = model.RegionId,
				StartDate = DateTime.Parse(model.StartDate, new CultureInfo("ru-RU")),
				Status = ConvertStatus(model.Status),
				Serial = model.Serial,
				Number = model.Number
			};
		}

		protected override PolicyModel MapEntityToShortModel(Policy entity)
		{
			return MapEntityToModel(entity);
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