using Aliencube.AlienCache.WebApi;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    [WebApiCache(TimeSpan = 60, AuthenticationType = AuthenticationType.Anonymous, UseAbsoluteUrl = false)]
    public class BaseApiController : ApiController
    {
    }
}