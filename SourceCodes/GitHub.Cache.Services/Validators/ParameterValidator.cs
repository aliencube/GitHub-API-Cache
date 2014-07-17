using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Linq;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Validators
{
    /// <summary>
    /// This represents the entity for parameter validator.
    /// </summary>
    public class ParameterValidator : IServiceValidator
    {
        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if validated; otherwise returns <c>False</c>.</returns>
        public bool ValidateAuthentication(HttpRequestMessage request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validates whether all values are required.
        /// </summary>
        /// <param name="values">List of values.</param>
        /// <returns>Returns <c>True</c>, if all values are required; otherwise returns <c>False</c>.</returns>
        public bool ValidateAllValuesRequired(params string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (!values.Any())
            {
                throw new ArgumentNullException("values");
            }

            var validated = values.All(value => !String.IsNullOrWhiteSpace(value));
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