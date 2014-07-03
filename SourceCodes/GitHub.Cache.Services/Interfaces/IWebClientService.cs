using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the WebClientService class.
    /// </summary>
    public interface IWebClientService
    {
        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="url">URL to send the request.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        HttpResponseMessage GetResponse(HttpRequestMessage request, string url);

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        HttpResponseMessage GetResponse(HttpRequestMessage request, Uri uri);

        /// <summary>
        /// Gets the basic authentication URL with username and password.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>Returns the basic authentication URL with username and password.</returns>
        Uri GetBasicAuthenticationUri(string url);

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance for error.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="ex"><c>Exception</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance for error.</returns>
        HttpResponseMessage GetErrorReponse(HttpRequestMessage request, Exception ex);

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="value">Content value.</param>
        /// <returns>Returns the string content.</returns>
        StringContent GetStringContent(string value);

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="ex">Exception instance.</param>
        /// <returns>Returns the string content.</returns>
        StringContent GetStringContent(Exception ex);
    }
}