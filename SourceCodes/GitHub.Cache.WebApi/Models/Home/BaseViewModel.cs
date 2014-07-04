using Aliencube.GitHub.Cache.WebApi.Properties;

namespace Aliencube.GitHub.Cache.WebApi.Models.Home
{
    /// <summary>
    /// This represents the base view model class. This must be inherited.
    /// </summary>
    public abstract class BaseViewModel
    {
        protected BaseViewModel()
        {
            this.Settings = Settings.Default;
        }

        /// <summary>
        /// Gets the configuration settings instance.
        /// </summary>
        protected Settings Settings { get; private set; }

        /// <summary>
        /// Gets the Google Analytics code from the configuration settings.
        /// </summary>
        public string GoogleAnalyticsCode
        {
            get { return this.Settings.GoogleAnalyticsCode; }
        }

        /// <summary>
        /// Gets the base URL of the application.
        /// </summary>
        public string BaseUrl
        {
            get { return this.Settings.BaseUrl; }
        }
    }
}