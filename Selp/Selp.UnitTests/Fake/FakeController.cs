namespace Selp.UnitTests.Fake
{
	using Controller;
	using Interfaces;

	internal class FakeController : SelpController<FakeEntity, FakeEntity, FakeEntity, int>
	{
		public FakeController(ISelpRepository<FakeEntity, int> repository) : base(repository)
		{
		}

		public override string ControllerName => "FakeController";
		protected override FakeEntity MapEntityToModel(FakeEntity entity)
		{
			return entity;
		}

		protected override FakeEntity MapModelToEntity(FakeEntity model)
		{
			return model;
		}

		protected override FakeEntity MapEntityToShortModel(FakeEntity entity)
		{
			return entity;
		}
	}
}