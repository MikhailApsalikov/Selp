namespace Example.Web.api
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using Entities;
    using Entities.Enums;
    using Interfaces.Repositories;
    using Models;
    using Selp.Controller;

    public class PolicyController : SelpController<PolicyShortModel, PolicyModel, Policy, int>
    {
        private readonly IPartyRepository partyRepository;

        public PolicyController(IPolicyRepository repository, IPartyRepository partyRepository) : base(repository)
        {
            this.partyRepository = partyRepository;
        }

        public override string ControllerName => "Policy";

        [Route("api/policy/generateNumber")]
        [HttpGet]
        public IHttpActionResult GenerateNumber()
        {
            return Ok(new
            {
                Serial = "XXX",
                Number = new Random().Next(1000000000, 2000000000).ToString()
            });
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
                Insured = entity.Parties?.Select(p=>p.Id).ToList(),
                RegionId = entity.RegionId,
                Region = entity.Region.Name
            };
        }

        protected override Policy MapModelToEntity(PolicyModel model)
        {
            return new Policy
            {
                Id = model.Id,
                Serial = model.Serial,
                Number = model.Number,
                RegionId = model.RegionId,
                CreatedDate = DateTime.Parse(model.CreatedDate, new CultureInfo("ru-RU")),
                StartDate = DateTime.Parse(model.StartDate, new CultureInfo("ru-RU")),
                ExpirationDate = DateTime.Parse(model.ExpirationDate, new CultureInfo("ru-RU")),
                InsurancePremium = model.InsurancePremium,
                InsuranceSum = model.InsuranceSum,
                Status = ConvertStatus(model.Status),
                UserId = model.UserId
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