namespace Example.Web.App_Start
{
	using Models;
	using Selp.Semantic.Core;

	public static class SemanticInitializer
	{
		public static void SemanticInitialize()
		{
			SemanticTypeResolver.Instance.Register<RegionModel>("semantic/Region");
			SemanticTypeResolver.Instance.Register<UserModel>("semantic/User");
			SemanticTypeResolver.Instance.Register<PolicyShortModel>("semantic/Policy");
			SemanticTypeResolver.Instance.Register<PartyModel>("semantic/Party");
		}
	}
}