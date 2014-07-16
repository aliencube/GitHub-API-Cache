using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the GitHubCacheServiceHelper class.
    /// </summary>
    public interface IGitHubCacheServiceHelper : IDisposable
    {
        /// <summary>
        /// Validates whether the request comes with proper values or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        bool ValidateRequest(HttpRequestMessage request);
    }
}