using Nancy;

namespace NamecardScanner.Http
{
    public static class ResponseEx
    {
        public static Response AsBadRequest(this IResponseFormatter response, string errormsg)
        {
            return response.AsJson(errormsg).WithStatusCode(HttpStatusCode.BadRequest);
        }
    }
}
