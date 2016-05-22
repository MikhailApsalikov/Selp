namespace Selp.Interfaces
{
    using System.Net.Http;

    public interface ISelpSemanticController<in TKey>
    {
        HttpResponseMessage Get(TKey id);
        HttpResponseMessage Get();
    }
}