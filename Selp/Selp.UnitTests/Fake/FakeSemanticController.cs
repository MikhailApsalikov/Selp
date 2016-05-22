namespace Selp.UnitTests.Fake
{
	using Interfaces;
	using Semantic;
	using VDS.RDF;

	public class FakeSemanticController : SelpSemanticController<FakeEntity, FakeEntity, FakeEntity, int>
	{
		public FakeSemanticController(ISelpRepository<FakeEntity, int> repository, IRdfWriter rdfWriter)
			: base(repository, rdfWriter)
		{
		}

		protected override FakeEntity MapEntityToModel(FakeEntity entity)
		{
			return entity;
		}

		protected override FakeEntity MapEntityToShortModel(FakeEntity entity)
		{
			return entity;
		}
	}
}