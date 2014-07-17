using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Aliencube.GitHub.Cache.WebApi.Tests
{
    [TestFixture]
    public class WebClientServiceTest
    {
        #region SetUp / TearDown

        private IGitHubCacheServiceSettingsProvider _settings;
        private IServiceValidator _parameterValidator;
        private IGitHubCacheServiceHelper _serviceHelper;
        private IEmailHelper _emailHelper;
        private IWebClientService _service;

        [SetUp]
        public void Init()
        {
            this._emailHelper = Substitute.For<IEmailHelper>();
            this._serviceHelper = Substitute.For<IGitHubCacheServiceHelper>();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._service != null)
                this._service.Dispose();

            if (this._parameterValidator != null)
                this._parameterValidator.Dispose();

            if (this._serviceHelper != null)
                this._serviceHelper.Dispose();

            if (this._emailHelper != null)
                this._emailHelper.Dispose();

            if (this._settings != null)
                this._settings.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        public void Test()
        {
            this._settings = Substitute.For<IGitHubCacheServiceSettingsProvider>();

            this._parameterValidator = Substitute.For<IServiceValidator>();

            this._service = new WebClientService(this._settings, this._parameterValidator, this._serviceHelper, this._emailHelper);
        }

        #endregion Tests
    }
}