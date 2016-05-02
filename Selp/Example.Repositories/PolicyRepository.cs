namespace Example.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using Entities;
    using Entities.Enums;
    using Models;
    using Selp.Common.Entities;
    using Selp.Interfaces;
    using Selp.Repository;

    public class PolicyRepository : SelpRepository<PolicyModel, Policy, int>
    {
        public PolicyRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public override bool IsRemovingFake => false;
        public override string FakeRemovingPropertyName => null;
        public override IDbSet<Policy> DbSet => ((ExampleDbContext) DbContext).Policies;

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

        protected override Policy MapModelToEntity(PolicyModel source, Policy destination)
        {
            destination.ExpirationDate = DateTime.Parse(source.ExpirationDate, new CultureInfo("ru-RU"));
            destination.InsurancePremium = source.InsurancePremium;
            destination.RegionId = source.RegionId;
            destination.StartDate = DateTime.Parse(source.StartDate, new CultureInfo("ru-RU"));
            destination.Status = ConvertStatus(source.Status);
            destination.InsuranceSum = source.InsuranceSum;
            destination.Serial = source.Serial;
            destination.Number = source.Number;

            return destination;
        }

        protected override IQueryable<Policy> ApplyFilters(IQueryable<Policy> entities, BaseFilter filter)
        {
            return entities;
        }

        protected override void OnCreating(Policy item)
        {
            item.UserId = "ds"; // вытащить UserId
            item.CreatedDate = DateTime.Now;
            item.Status = PolicyStatus.Project;
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