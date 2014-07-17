using Aliencube.AlienCache.WebApi.Interfaces;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Validators;
using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Helpers
{
    /// <summary>
    /// This represents the GitHub cache service helper entity.
    /// </summary>
    public class GitHubCacheServiceHelper : IGitHubCacheServiceHelper
    {
        private readonly IWebApiCacheConfigurationSettingsProvider _webApiCacheSettings;
        private readonly IGitHubCacheServiceSettingsProvider _gitHubCacheSettings;
        private readonly IServiceValidator _parameterValidator;

        /// <summary>
        /// Initialises a new instance of the GitHubCacheServiceHelper class.
        /// </summary>
        /// <param name="webApiCacheSettings"><c>WebApiCacheConfigurationSettingsProvider</c> instance.</param>
        /// <param name="gitHubCacheSettings"><c>GitHubCacheServiceSettingsProvider</c> instance.</param>
        /// <param name="parameterValidator"><c>ParameterValidator</c> instance.</param>
        public GitHubCacheServiceHelper(IWebApiCacheConfigurationSettingsProvider webApiCacheSettings,
                                        IGitHubCacheServiceSettingsProvider gitHubCacheSettings,
                                        IServiceValidator parameterValidator)
        {
            if (webApiCacheSettings == null)
            {
                throw new ArgumentNullException("webApiCacheSettings");
            }
            this._webApiCacheSettings = webApiCacheSettings;

            if (gitHubCacheSettings == null)
            {
                throw new ArgumentNullException("gitHubCacheSettings");
            }
            this._gitHubCacheSettings = gitHubCacheSettings;

            if (parameterValidator == null)
            {
                throw new ArgumentNullException("parameterValidator");
            }
            this._parameterValidator = parameterValidator;
        }

        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        public bool ValidateAuthentication(HttpRequestMessage request)
        {
            var validator = BaseAuthenticationValidator.CreateInstance(this._gitHubCacheSettings.AuthenticationType);
            var validated = validator.ValidateAuthentication(request);
            return validated;
        }

        /// <summary>
        /// Validates the request URL.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        public bool ValidateRequestUrl(HttpRequestMessage request)
        {
            if (!this._webApiCacheSettings.UseQueryStringAsKey)
            {
                return true;
            }

            var key = this._webApiCacheSettings.QueryStringKey;
            if (String.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            var url = request.RequestUri.ParseQueryString().Get(key);
            var validated = this._parameterValidator.ValidateAllValuesRequired(url);
            return validated;
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