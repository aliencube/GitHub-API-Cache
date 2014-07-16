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
        private readonly IGitHubCacheServiceSettingsProvider _settings;

        /// <summary>
        /// Initialises a new instance of the GitHubCacheServiceHelper class.
        /// </summary>
        /// <param name="settings"><c>GitHubCacheServiceSettingsProvider</c> instance.</param>
        public GitHubCacheServiceHelper(IGitHubCacheServiceSettingsProvider settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            this._settings = settings;
        }

        /// <summary>
        /// Validates whether the request comes with proper values or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        public bool ValidateRequest(HttpRequestMessage request, Uri uri)
        {
            var validator = BaseAuthenticationValidator.CreateInstance(this._settings.AuthenticationType);
            var validated = validator.ValidateAuthentication(request, uri);
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