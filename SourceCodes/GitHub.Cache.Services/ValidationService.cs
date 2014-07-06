using Aliencube.GitHub.Cache.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aliencube.GitHub.Cache.Services
{
    /// <summary>
    /// This represents the validation service entity.
    /// </summary>
    public class ValidationService : IValidationService
    {
        /// <summary>
        /// Validates whether all values are required.
        /// </summary>
        /// <param name="values">List of values.</param>
        /// <returns>Returns <c>True</c>, if all values are required; otherwise returns <c>False</c>.</returns>
        public bool ValidateAllValuesRequired(params string[] values)
        {
            return values.All(p => !String.IsNullOrWhiteSpace(p));
        }
    }
}