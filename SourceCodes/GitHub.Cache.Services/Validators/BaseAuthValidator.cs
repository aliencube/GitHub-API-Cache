using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.Services.Validators
{
    /// <summary>
    /// This represents the validator entity for authentication. This must be inherited.
    /// </summary>
    public abstract class BaseAuthValidator : IServiceValidator
    {
        /// <summary>
        /// Validates the authentication request.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if validated; otherwise returns <c>False</c>.</returns>
        public virtual bool ValidateAuthentication(HttpRequestMessage request)
        {
            return true;
        }

        /// <summary>
        /// Validates whether all values are required.
        /// </summary>
        /// <param name="values">List of values.</param>
        /// <returns>Returns <c>True</c>, if all values are required; otherwise returns <c>False</c>.</returns>
        public bool ValidateAllValuesRequired(params string[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the BaseAuthValidator instance based on the AuthenticationType value.
        /// </summary>
        /// <param name="authType"><c>AuthenticationType</c> value.</param>
        /// <returns>Returns the BaseAuthValidator instance.</returns>
        public static BaseAuthValidator CreateInstance(AuthenticationType authType)
        {
            switch (authType)
            {
                case AuthenticationType.Anonymous:
                    return new AnonymousValidator();

                case AuthenticationType.Basic:
                    return new BasicAuthValidator();

                case AuthenticationType.AuthenticationKey:
                    return new AuthKeyValidator();

                default:
                    throw new InvalidOperationException("Invalid AuthenticationType");
            }
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