using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Repositories
{
	using System.Data.Entity;

	public class TestDataInitializer : DropCreateDatabaseIfModelChanges<ExampleDbContext>
	{
		protected override void Seed(ExampleDbContext context)
		{
			InitializeRegions(context);
			InitializeTestParties(context);
		}

		private void InitializeTestParties(ExampleDbContext context)
		{
			
		}

		private void InitializeRegions(ExampleDbContext context)
		{
			
		}
	}
}
