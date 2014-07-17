using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the controller entity for REF API.
    /// </summary>
    [Route("api/ref/{user}/{repo}/{branch}")]
    [Obsolete("Try GitHub original API URLs instead.")]
    public class RefController : BaseApiController
    {
        private const string REF_URL = "https://api.github.com/repos/{0}/{1}/git/refs/heads/{2}";

        /// <summary>
        /// Initialises a new instance of the RefController class.
        /// </summary>
        /// <param name="parameterValidator"><c>ServiceValidator</c> instance.</param>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        public RefController(IServiceValidator parameterValidator, IWebClientService webClientService)
            : base(parameterValidator, webClientService)
        {
        }

        /// <summary>
        /// OPTIONS:    /api/ref/{user}/{repo}/{branch}
        /// </summary>
        /// <param name="user">GitHub username or organisation name.</param>
        /// <param name="repo">GitHub repository name. (Case sensitive)</param>
        /// <param name="branch">Branch name.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Options(string user, string repo, string branch)
        {
            return this.GetResponseMessage(user, repo, branch);
        }

        /// <summary>
        /// GET:    /api/ref/{user}/{repo}/{branch}
        /// </summary>
        /// <param name="user">GitHub username or organisation name.</param>
        /// <param name="repo">GitHub repository name. (Case sensitive)</param>
        /// <param name="branch">Branch name.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage Get(string user, string repo, string branch)
        {
            return this.GetResponseMessage(user, repo, branch);
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="user">GitHub username or organisation name.</param>
        /// <param name="repo">GitHub repository name. (Case sensitive)</param>
        /// <param name="branch">Branch name.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        private HttpResponseMessage GetResponseMessage(string user, string repo, string branch)
        {
            if (!this.ParameterValidator.ValidateAllValuesRequired(user, repo, branch))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var url = String.Format(REF_URL, user, repo, branch);
            var response = this.WebClientService.GetResponse(Request, url);
            return response;
        }
    }
}