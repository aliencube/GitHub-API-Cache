namespace Aliencube.GitHub.Cache.Services
{
    /// <summary>
    /// This specifies the authentication type.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Identifies the authentiction type is not determined or no authentication is required.
        /// </summary>
        Anonymous = 0,

        /// <summary>
        /// Identifies basic username and password are used for authentication. Strongly discouraged to use.
        /// </summary>
        Basic = 1,

        /// <summary>
        /// Identifies authentication key is used for authentication.
        /// </summary>
        AuthenticationKey = 2,
    }
}