using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Validators
{
    /// <summary>
    /// This represents the basic authentication validator entity.
    /// </summary>
    public class BasicAuthValidator : BaseAuthValidator
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

            var uri = request.RequestUri;
            if (uri == null)
            {
                return false;
            }

            var url = uri.OriginalString;
            var segments = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var validated = segments[1].Contains(":") && segments[1].Contains("@");
            return validated;
        }
    }
}