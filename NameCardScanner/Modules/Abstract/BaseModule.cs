using Nancy;

namespace NamecardScanner.Modules.Abstract
{
    public abstract class BaseModule : NancyModule
    {
        protected BaseModule()
        {
            Get["/"] = _ => this.Response.AsJson("alive");
        }
    }
}
