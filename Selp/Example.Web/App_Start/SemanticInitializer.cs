namespace Example.Web.App_Start
{
	using Models;
	using Selp.Semantic.Core;

	public static class SemanticInitializer
	{
		public static void SemanticInitialize()
		{
			SemanticTypeResolver.Instance.Register<RegionModel>("http://localhost:33332/semantic/Region");
			SemanticTypeResolver.Instance.Register<UserModel>("http://localhost:33332/semantic/User");
			SemanticTypeResolver.Instance.Register<PolicyModel>("http://localhost:33332/semantic/Policy");
			SemanticTypeResolver.Instance.Register<AttachmentModel>("http://localhost:33332/semantic/Attachment");
			SemanticTypeResolver.Instance.Register<DocumentModel>("http://localhost:33332/semantic/Document");
			SemanticTypeResolver.Instance.Register<PartyModel>("http://localhost:33332/semantic/Party");
		}
	}
}