using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the entities inheriting the BaseAuthenticationValidator class.
    /// </summary>
    public interface IServiceValidator : IDisposable
    {
        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns <c>True</c>, if validated; otherwise returns <c>False</c>.</returns>
        bool ValidateAuthentication(HttpRequestMessage request, Uri uri);

        /// <summary>
        /// Validates whether all values are required.
        /// </summary>
        /// <param name="values">List of values.</param>
        /// <returns>Returns <c>True</c>, if all values are required; otherwise returns <c>False</c>.</returns>
        bool ValidateAllValuesRequired(params string[] values);
    }
}