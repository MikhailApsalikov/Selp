namespace Example.Web.Controllers
{
    using Entities;
    using Models;
    using Selp.Controller;
    using Selp.Interfaces;

    public class PolicyController : SelpController<PolicyModel, Policy, int>
    {
        public PolicyController(ISelpRepository<PolicyModel, Policy, int> repository) : base(repository)
        {
        }

        public override string ControllerName => "Policy";
    }
}