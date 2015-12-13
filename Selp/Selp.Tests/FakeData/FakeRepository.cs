namespace Selp.Tests.FakeData
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Entity;
	using Kernel.BaseClasses;

	internal class FakeRepository : SelpRepository<FakeEntity, int, FakeDbContext>
	{
		public FakeRepository(FakeDbContext dbContext)
			: base(dbContext)
		{
		}

		public FakeRepository() : base(false)
		{
		}

		protected override IDbSet<FakeEntity> DbSet => DbContext.FakeObjectDbSet;

		internal IEnumerable<FakeEntity> CreateFakeEntityList()
		{
			var fakeDbSet = DbSet as FakeDbSet<FakeEntity>;
			if (fakeDbSet != null)
			{
				fakeDbSet.Local = new ObservableCollection<FakeEntity>(new List<FakeEntity>
				{
					new FakeEntity
					{
						Id = 1,
						Name = "Name"
					},
					new FakeEntity
					{
						Id = 2,
						Name = "Name2"
					},
					new FakeEntity
					{
						Id = 3,
						Name = "Name3"
					},
					new FakeEntity
					{
						Id = 4,
						Name = "Name4"
					},
					new FakeEntity
					{
						Id = 5,
						Name = "Name5"
					},
					new FakeEntity
					{
						Id = 7,
						Name = "Name7"
					},
					new FakeEntity
					{
						Id = 10,
						Name = "Name10"
					}
				});
				return fakeDbSet.Local;
			}
			return null;
		}

		protected override FakeDbContext RecreateContext()
		{
			return new FakeDbContext();
		}
	}
}