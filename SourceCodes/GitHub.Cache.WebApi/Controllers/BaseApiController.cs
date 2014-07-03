using Aliencube.AlienCache.WebApi;
using Aliencube.GitHub.Cache.Services.Interfaces;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the base controller entity. This must be inherited.
    /// </summary>
    [WebApiCache(CachConfigurationSettingsProviderType = typeof(CacheConfigurationSettingsProvider))]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Initialises a new instance of the BaseApiController class.
        /// </summary>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        protected BaseApiController(IWebClientService webClientService)
        {
            this.WebClientService = webClientService;
        }

        /// <summary>
        /// Gets the <c>WebClientService</c> instance.
        /// </summary>
        protected IWebClientService WebClientService { get; private set; }
    }
}