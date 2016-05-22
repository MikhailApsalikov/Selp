namespace Example.Repositories
{
	using System;
	using System.Data.Entity;
	using System.Linq;
	using Entities;
	using Entities.Enums;
	using Interfaces.Repositories;
	using Selp.Common.Entities;
	using Selp.Interfaces;
	using Selp.Repository;

	public class PolicyRepository : SelpRepository<Policy, int>, IPolicyRepository
	{
		public PolicyRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
		{
		}

		public override bool IsRemovingFake => false;
		public override string FakeRemovingPropertyName => null;
		public override IDbSet<Policy> DbSet => ((ExampleDbContext) DbContext).Policies;

		protected override Policy Merge(Policy source, Policy destination)
		{
			destination.ExpirationDate = source.ExpirationDate;
			destination.InsurancePremium = source.InsurancePremium;
			destination.RegionId = source.RegionId;
			destination.StartDate = source.StartDate;
			destination.Status = source.Status;
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
	}
}