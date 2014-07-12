using Aliencube.GitHub.Cache.Services.Interfaces;
using System.Net.Http;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the controller entity for REF API.
    /// </summary>
    [Route("api/v3")]
    public class V3Controller : BaseApiController
    {
        /// <summary>
        /// Initialises a new instance of the RefController class.
        /// </summary>
        /// <param name="validationService"><c>ValidationService</c> instance.</param>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        public V3Controller(IValidationService validationService, IWebClientService webClientService)
            : base(validationService, webClientService)
        {
        }

        /// <summary>
        /// OPTIONS:    /api/v3/{url}
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Options()
        {
            return this.WebClientService.GetResponseMessage(Request, this.ValidationService);
        }

        /// <summary>
        /// GET:    /api/v3/{url}
        /// </summary>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Get()
        {
            return this.WebClientService.GetResponseMessage(Request, this.ValidationService);
        }
    }
}