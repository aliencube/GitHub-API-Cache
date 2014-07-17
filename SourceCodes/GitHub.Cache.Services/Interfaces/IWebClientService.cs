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
        /// Gets the <c>HttpResponseMessage</c> instance for error.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="ex"><c>Exception</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance for error.</returns>
        HttpResponseMessage GetErrorReponse(HttpRequestMessage request, Exception ex);

        /// <summary>
        /// Checks whether the request contains JSONP callback or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request contains JSONP callback; otherwise returns <c>False</c>.</returns>
        bool IsJsonpRequest(HttpRequestMessage request);

        /// <summary>
        /// Wraps the response message with the callback function specified.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="value">Content value.</param>
        /// <returns>Returns the response message with the callback function specified.</returns>
        string WrapJsonpCallback(HttpRequestMessage request, string value);

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="value">Content value.</param>
        /// <param name="mediaType">Media type.</param>
        /// <returns>Returns the string content.</returns>
        StringContent GetStringContent(string value, string mediaType = null);

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="ex">Exception instance.</param>
        /// <param name="mediaType">Media type.</param>
        /// <returns>Returns the string content.</returns>
        StringContent GetStringContent(Exception ex, string mediaType = null);
    }
}