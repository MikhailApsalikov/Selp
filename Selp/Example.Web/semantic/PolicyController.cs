namespace Example.Web.semantic
{
	using System;
	using System.Linq;
	using Entities;
	using Entities.Enums;
	using Interfaces.Repositories;
	using Models;
	using Selp.Semantic;
	using VDS.RDF;

	public class PolicyController : SelpSemanticController<PolicyShortModel, PolicyModel, Policy, int>
	{
		public PolicyController(IPolicyRepository repository, IRdfWriter rdfWriter) : base(repository, rdfWriter)
		{
		}

		protected override PolicyModel MapEntityToModel(Policy entity)
		{
			return new PolicyModel
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
				Number = entity.Number,
				Insured = entity.Parties?.Select(p => p.Id).ToList(),
				RegionId = entity.RegionId,
				Region = entity.Region.Name
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

		protected override string GetMimeType()
		{
			return "text/txt";
		}
	}
}