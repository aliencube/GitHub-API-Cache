using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Validators
{
    /// <summary>
    /// This represents the basic authentication validator entity.
    /// </summary>
    public class AnonymousValidator : BaseAuthenticationValidator
    {
        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns <c>True</c>, if validated; otherwise returns <c>False</c>.</returns>
        public override bool ValidateAuthentication(HttpRequestMessage request, Uri uri)
        {
            return true;
        }
    }
}