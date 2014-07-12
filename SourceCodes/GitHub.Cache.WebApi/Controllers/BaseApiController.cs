using Aliencube.AlienCache.WebApi;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.WebApi.RequireHttps;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the base controller entity. This must be inherited.
    /// </summary>
    [WebApiCache(CachConfigurationSettingsProviderType = typeof(CacheConfigurationSettingsProvider))]
    [RequreHttps(RequireHttpsConfigurationSettingsProviderType = typeof(RequireHttpsConfigurationSettingsProvider))]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Initialises a new instance of the BaseApiController class.
        /// </summary>
        /// <param name="validationService"><c>ValidationService</c> instance.</param>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        protected BaseApiController(IValidationService validationService, IWebClientService webClientService)
        {
            this.ValidationService = validationService;
            this.WebClientService = webClientService;
        }

        /// <summary>
        /// Gets the <c>ValidationService</c> instance.
        /// </summary>
        protected IValidationService ValidationService { get; private set; }

        /// <summary>
        /// Gets the <c>WebClientService</c> instance.
        /// </summary>
        protected IWebClientService WebClientService { get; private set; }
    }
}