using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Validators
{
    /// <summary>
    /// This represents the basic authentication validator entity.
    /// </summary>
    public class AuthKeyValidator : BaseAuthValidator
    {
        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if validated; otherwise returns <c>False</c>.</returns>
        public override bool ValidateAuthentication(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var headers = request.Headers;
            if (headers == null)
            {
                return false;
            }

            var header = headers.Authorization;
            if (header == null)
            {
                return false;
            }

            var token = header.ToString();
            if (String.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            if (!token.StartsWith("token "))
            {
                return false;
            }

            return true;
        }
    }
}