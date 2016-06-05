namespace Example.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Interfaces.Repositories;
    using Selp.Common.Entities;
    using Selp.Interfaces;
    using Selp.Repository;

    public class PartyRepository : SelpRepository<Party, int>, IPartyRepository
    {
        public PartyRepository(DbContext dbContext, ISelpConfiguration configuration) : base(dbContext, configuration)
        {
        }

        public override bool IsRemovingFake => false;
        public override string FakeRemovingPropertyName => null;
        public override IDbSet<Party> DbSet => ((ExampleDbContext) DbContext).Parties;

        protected override Party Merge(Party source, Party destination)
        {
            destination.Address = source.Address;
            destination.BirthDate = source.BirthDate;
            destination.Email = source.Email;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.MiddleName = source.MiddleName;
            destination.Phone = source.Phone;
            return destination;
        }

        protected override IQueryable<Party> ApplyFilters(IQueryable<Party> entities, BaseFilter filter)
        {
            return entities;
        }
    }
}