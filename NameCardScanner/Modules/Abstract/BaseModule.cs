using Nancy;

/// <summary>
/// Abstract base module
/// </summary>
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
