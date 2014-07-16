namespace Aliencube.GitHub.Cache.Services.Interfaces
{
    public interface IGitHubCacheServiceSettingsProvider
    {
        /// <summary>
        /// Gets the value that specifies whether to use proxy server or not.
        /// </summary>
        bool UseProxy { get; }

        /// <summary>
        /// Gets the proxy server URL.
        /// </summary>
        string ProxyUrl { get; }

        /// <summary>
        /// Gets the value that specifies whether to bypass local connection or not.
        /// </summary>
        bool BypassOnLocal { get; }

        /// <summary>
        /// Gets the authentication type of the GitHub API.
        /// </summary>
        AuthenticationType AuthenticationType { get; }

        /// <summary>
        /// Gets the value that specifies whether to send emails for error log or not.
        /// </summary>
        bool UseErrorLogEmail { get; }

        /// <summary>
        /// Gets the sender's email.
        /// </summary>
        string ErrorLogEmailFrom { get; }

        /// <summary>
        /// Gets the recipient's email.
        /// </summary>
        string ErrorLogEmailTo { get; }
    }
}