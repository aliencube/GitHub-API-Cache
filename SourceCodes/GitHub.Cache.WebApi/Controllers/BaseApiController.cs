using Aliencube.AlienCache.WebApi;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.WebApi.RequireHttps;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the base controller entity. This must be inherited.
    /// </summary>
    [WebApiCache(WebApiCacheConfigurationSettingsProviderType = typeof(WebApiCacheConfigurationSettingsProvider))]
    [RequireHttps(RequireHttpsConfigurationSettingsProviderType = typeof(RequireHttpsConfigurationSettingsProvider))]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Initialises a new instance of the BaseApiController class.
        /// </summary>
        /// <param name="parameterValidator"><c>ServiceValidator</c> instance.</param>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        protected BaseApiController(IServiceValidator parameterValidator, IWebClientService webClientService)
        {
            this.ParameterValidator = parameterValidator;
            this.WebClientService = webClientService;
        }

        /// <summary>
        /// Gets the <c>ServiceValidator</c> instance.
        /// </summary>
        protected IServiceValidator ParameterValidator { get; private set; }

        /// <summary>
        /// Gets the <c>WebClientService</c> instance.
        /// </summary>
        protected IWebClientService WebClientService { get; private set; }
    }
}