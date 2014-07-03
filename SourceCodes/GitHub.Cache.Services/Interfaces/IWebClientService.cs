using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
        /// Validates whether the request comes with proper values or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        bool ValidateRequest(HttpRequestMessage request, Uri uri);

        /// <summary>
        /// Validates whether the username and password are included in the URL segment or not.
        /// </summary>
        /// <param name="url">URL segment.</param>
        /// <returns>Returns <c>True</c>, if the username and password are included in the URL segment; otherwise returns <c>False</c>.</returns>
        bool ValidateBasicAuthentication(string url);

        /// <summary>
        /// Validates whether the authentication key exists in a correct format or not.
        /// </summary>
        /// <param name="header">Authentication header value.</param>
        /// <returns>Returns <c>True</c>, if the authentication key exists in a correct format; otherwise returns <c>False</c>.</returns>
        bool ValidateAuthorisationHeader(AuthenticationHeaderValue header);

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