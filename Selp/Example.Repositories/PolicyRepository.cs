namespace Example.Repositories
{
	using System;
	using System.Data.Entity;
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
			return new PolicyModel()
			{
				Id = entity.Id,
				InsuranceSum = entity.InsuranceSum,
				RegionId = entity.RegionId,
				ExpirationDate = entity.ExpirationDate,
				StartDate = entity.StartDate,
				InsurancePremium = entity.InsurancePremium,
				Attachments = entity.Attachments.Select(s=>s.Id).ToList(),
				Status = entity.Status,
				CreatedDate = entity.CreatedDate,
				UserId = entity.UserId,
				InsuredList = entity.Parties.Select(p=>p.Id).ToList()
			};
		}

		protected override Policy MapModelToEntity(PolicyModel model)
		{
			return new Policy()
			{
				ExpirationDate = model.ExpirationDate,
				CreatedDate = model.CreatedDate,
				InsurancePremium = model.InsurancePremium,
				InsuranceSum = model.InsuranceSum,
				RegionId = model.RegionId,
				StartDate = model.StartDate,
				Status = model.Status,
			};
		}

		protected override Policy MapModelToEntity(PolicyModel source, Policy destination)
		{
			destination.ExpirationDate = source.ExpirationDate;
			destination.InsurancePremium = source.InsurancePremium;
			destination.RegionId = source.RegionId;
			destination.StartDate = source.StartDate;
			destination.Status = source.Status;
			destination.InsuranceSum = source.InsuranceSum;

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
	}
}