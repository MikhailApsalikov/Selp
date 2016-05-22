namespace Example.Web.semantic
{
    using Entities;
    using Interfaces.Repositories;
    using Models;
    using Selp.Semantic;
    using VDS.RDF;

    public class UserController : SelpSemanticController<UserModel, UserModel, User, string>
    {
        public UserController(IUserRepository repository, IRdfWriter rdfWriter)
            : base(repository, rdfWriter)
        {
        }

        protected override UserModel MapEntityToModel(User entity)
        {
            return new UserModel
            {
                Id = entity.Id
            };
        }

        protected override UserModel MapEntityToShortModel(User entity)
        {
            return MapEntityToModel(entity);
        }

        protected override string GetMimeType()
        {
            return "text/txt";
        }
    }
}