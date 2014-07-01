using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the controller entity for REF API.
    /// </summary>
    public class RefController : BaseApiController
    {
        private const string REF_URL = "https://api.github.com/repos/{0}/{1}/git/refs/heads/{2}";

        private readonly IWebClientService _webClientService;

        /// <summary>
        /// Initialises a new instance of the RefController class.
        /// </summary>
        /// <param name="webClientService"><c>WebClientService</c> instance.</param>
        public RefController(IWebClientService webClientService)
        {
            this._webClientService = webClientService;
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
            if (String.IsNullOrWhiteSpace(user))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            if (String.IsNullOrWhiteSpace(repo))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            if (String.IsNullOrWhiteSpace(branch))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var url = String.Format(REF_URL, user, repo, branch);
            var response = this._webClientService.GetResponse(Request, url);
            return response;
        }
    }
}