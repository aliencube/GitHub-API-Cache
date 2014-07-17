using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services
{
    /// <summary>
    /// This represents the web client service entity.
    /// </summary>
    public class WebClientService : IWebClientService
    {
        private readonly IGitHubCacheServiceSettingsProvider _gitHubCacheSettings;
        private readonly IGitHubCacheServiceHelper _gitHubCacheServiceHelper;
        private readonly IEmailHelper _emailHelper;

        /// <summary>
        /// Initialises a new instance of the WebClientService class.
        /// </summary>
        /// <param name="gitHubCacheSettings"><c>GitHubCacheServiceSettingsProvider</c> instance.</param>
        /// <param name="gitHubCacheServiceHelper"><c>GitHubCacheServiceHelper</c> instance.</param>
        /// <param name="emailHelper"><c>EmailHelper</c> instance.</param>
        public WebClientService(IGitHubCacheServiceSettingsProvider gitHubCacheSettings,
                                IGitHubCacheServiceHelper gitHubCacheServiceHelper,
                                IEmailHelper emailHelper)
        {
            if (gitHubCacheSettings == null)
            {
                throw new ArgumentNullException("gitHubCacheSettings");
            }
            this._gitHubCacheSettings = gitHubCacheSettings;

            if (gitHubCacheServiceHelper == null)
            {
                throw new ArgumentNullException("gitHubCacheServiceHelper");
            }
            this._gitHubCacheServiceHelper = gitHubCacheServiceHelper;

            if (emailHelper == null)
            {
                throw new ArgumentNullException("emailHelper");
            }
            this._emailHelper = emailHelper;
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage GetResponseMessage(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpResponseMessage response;
            try
            {
                var validated = this._gitHubCacheServiceHelper.ValidateAuthentication(request);
                if (!validated)
                {
                    throw new InvalidOperationException("Unauthorised");
                }

                validated = this._gitHubCacheServiceHelper.ValidateRequestUrl(request);
                if (!validated)
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }

                response = this._gitHubCacheServiceHelper.GetResponseMessage(request);
            }
            catch (Exception ex)
            {
                response = this._gitHubCacheServiceHelper.GetErrorReponseMessage(request, ex);

                if (!this._gitHubCacheSettings.UseErrorLogEmail)
                {
                    return response;
                }

                var from = this._gitHubCacheSettings.ErrorLogEmailFrom;
                var to = this._gitHubCacheSettings.ErrorLogEmailTo;
                var subject = String.Format("GitHub API Cache - {0}", ex.Message);
                var body = this._emailHelper.GetBodyContent(request, ex);
                this._emailHelper.Send(from, to, subject, body);
            }
            return response;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}