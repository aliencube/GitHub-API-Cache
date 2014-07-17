using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the WebClientService class.
    /// </summary>
    public interface IWebClientService : IDisposable
    {
        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        HttpResponseMessage GetResponseMessage(HttpRequestMessage request);
    }
}