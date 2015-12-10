namespace Selp.Tests
{
	using System.Linq;
	using System.Threading.Tasks;
	using Common.Exceptions;
	using FakeData;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class RepositoryTests
	{
		[TestMethod]
		public void AddTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.Add(new FakeEntity
				{
					Name = "Name"
				});
			}
		}

		[TestMethod]
		public async Task AddAsyncTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				await repository.AddAsync(new FakeEntity
				{
					Name = "Name"
				});
			}
		}

		[TestMethod]
		public void AddNewDbContextTest()
		{
			var repository = new FakeRepository();
			repository.Add(new FakeEntity
			{
				Name = "Name"
			});
		}

		[TestMethod]
		public async Task AddNewDbContextAsyncTest()
		{
			var repository = new FakeRepository();
			await repository.AddAsync(new FakeEntity
			{
				Name = "Name"
			});
		}

		[TestMethod]
		[ExpectedException(typeof (EntityNotFoundException))]
		public void UpdateInexistentTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.Update(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
			}
		}

		[TestMethod]
		public void UpdateTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				dbContext.FakeObjectDbSet.Local.Add(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
				repository.Update(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
			}
		}

		[TestMethod]
		[ExpectedException(typeof (EntityNotFoundException))]
		public async Task UpdateInexistentAsyncTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				await repository.UpdateAsync(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
			}
		}

		[TestMethod]
		public async Task UpdateAsyncTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				dbContext.FakeObjectDbSet.Local.Add(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
				await repository.UpdateAsync(new FakeEntity
				{
					Id = 1,
					Name = "Name"
				});
			}
		}

		[TestMethod]
		public void UpdateNewDbContextTest()
		{
			var repository = new FakeRepository();
			repository.Update(new FakeEntity
			{
				Id = 1,
				Name = "Name"
			});
		}

		[TestMethod]
		public async Task UpdateNewDbContextAsyncTest()
		{
			var repository = new FakeRepository();
			await repository.UpdateAsync(new FakeEntity
			{
				Id = 1,
				Name = "Name"
			});
		}

		[TestMethod]
		public void GetAllTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				Assert.AreEqual(7, repository.GetAll().Count());
				Assert.AreEqual("Name2", repository.GetAll().ToList()[1].Name);
			}
		}

		[TestMethod]
		public async Task GetAllAsyncTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				var all = (await repository.GetAllAsync()).ToList();
				Assert.AreEqual(7, all.Count);
				Assert.AreEqual("Name2", all[1].Name);
			}
		}

		[TestMethod]
		public void GetAllNewDbContextTest()
		{
			var repository = new FakeRepository();
			repository.CreateFakeEntityList();
			Assert.AreEqual(7, repository.GetAll().Count());
			Assert.AreEqual("Name2", repository.GetAll().ToList()[1].Name);
		}

		[TestMethod]
		public async Task GetAllNewDbContextAsyncTest()
		{
			var repository = new FakeRepository();
			repository.CreateFakeEntityList();
			var all = (await repository.GetAllAsync()).ToList();
			Assert.AreEqual(7, all.Count);
			Assert.AreEqual("Name2", all[1].Name);
		}

		[TestMethod]
		public void GetRangeCountTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				Assert.AreEqual(3, repository.GetRange(e => true, 0, 3).Count());
			}
		}

		[TestMethod]
		public void GetRangeOffsetTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				var range = repository.GetRange(e => true, 2, 1);
				Assert.AreEqual(3, range.ToList()[0].Id);
				range = repository.GetRange(e => true, 4, 5);
				Assert.AreEqual(3, range.Count());
				Assert.AreEqual(true, range.Any(e => e.Id == 5));
				Assert.AreEqual(true, range.Any(e => e.Id == 7));
				Assert.AreEqual(true, range.Any(e => e.Id == 10));
			}
		}

		[TestMethod]
		public void GetRangeExpressionTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				var range = repository.GetRange(e => e.Id < 3, 0, 10);
				Assert.AreEqual(2, range.Count());
				range = repository.GetRange(e => e.Id > 3, 0, 10);
				Assert.AreEqual(4, range.Count());
				range = repository.GetRange(e => e.Name == "Ватафак", 0, 10);
				Assert.AreEqual(0, range.Count());
			}
		}

		[TestMethod]
		public async Task GetRangeAsyncTest()
		{
			using (var dbContext = new FakeDbContext())
			{
				var repository = new FakeRepository(dbContext);
				repository.CreateFakeEntityList();
				var all = (await repository.GetRangeAsync(e => true, 0, 3)).ToList();
				Assert.AreEqual(3, all.Count);
			}
		}

		[TestMethod]
		public void GetRangeNewDbContextTest()
		{
			var repository = new FakeRepository();
			repository.CreateFakeEntityList();
			Assert.AreEqual(3, repository.GetRange(e => true, 0, 3).Count());
		}

		[TestMethod]
		public async Task GetRangeNewDbContextAsyncTest()
		{
			var repository = new FakeRepository();
			repository.CreateFakeEntityList();
			var all = (await repository.GetRangeAsync(e => true, 0, 3)).ToList();
			Assert.AreEqual(3, all.Count);
		}

		/*
		IQueryable<T> Filter(Expression<Func<T, bool>> filter);

		IQueryable<T> Filter<TValue>(Expression<Func<T, bool>> filter, Expression<Func<T, TValue>> keySelector, int offset, int count);

		T GetById(TKey key);
		T FilterFirst(Expression<Func<T, bool>> filter);
		T FilterSingle(Expression<Func<T, bool>> filter);
		void Remove(T item);
		void RemoveById(TKey key);
		*/
	}
}