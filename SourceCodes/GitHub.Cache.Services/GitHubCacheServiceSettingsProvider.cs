using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Properties;

namespace Aliencube.GitHub.Cache.Services
{
    /// <summary>
    /// This represents the entity providing configuration settings.
    /// </summary>
    public class GitHubCacheServiceSettingsProvider : IGitHubCacheServiceSettingsProvider
    {
        private readonly Settings _settings;

        /// <summary>
        /// Initialises a new instance of the GitHubCacheServiceSettingsProvider class.
        /// </summary>
        public GitHubCacheServiceSettingsProvider()
        {
            this._settings = Settings.Default;
        }

        /// <summary>
        /// Gets the value that specifies whether to use proxy server or not.
        /// </summary>
        public bool UseProxy
        {
            get { return this._settings.UseProxy; }
        }

        /// <summary>
        /// Gets the proxy server URL.
        /// </summary>
        public string ProxyUrl
        {
            get { return this._settings.ProxyUrl; }
        }

        /// <summary>
        /// Gets the value that specifies whether to bypass local connection or not.
        /// </summary>
        public bool BypassOnLocal
        {
            get { return this._settings.BypassOnLocal; }
        }

        /// <summary>
        /// Gets the authentication type of the GitHub API.
        /// </summary>
        public AuthenticationType AuthenticationType
        {
            get { return this._settings.AuthenticationType; }
        }

        /// <summary>
        /// Gets the value that specifies whether to send emails for error log or not.
        /// </summary>
        public bool UseErrorLogEmail
        {
            get { return this._settings.UseErrorLogEmail; }
        }

        /// <summary>
        /// Gets the sender's email.
        /// </summary>
        public string ErrorLogEmailFrom
        {
            get { return this._settings.ErrorLogEmailFrom; }
        }

        /// <summary>
        /// Gets the recipient's email.
        /// </summary>
        public string ErrorLogEmailTo
        {
            get { return this._settings.ErrorLogEmailTo; }
        }
    }
}