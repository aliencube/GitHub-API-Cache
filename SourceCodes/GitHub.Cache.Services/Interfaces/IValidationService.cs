using System.Collections.Generic;

namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    /// <summary>
    /// This provides interfaces to the ValidationService class.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validates whether all values are required.
        /// </summary>
        /// <param name="values">List of values.</param>
        /// <returns>Returns <c>True</c>, if all values are required; otherwise returns <c>False</c>.</returns>
        bool ValidateAllValuesRequired(params string[] values);
    }
}