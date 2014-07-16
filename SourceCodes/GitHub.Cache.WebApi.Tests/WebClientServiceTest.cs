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
        private IEmailHelper _helper;
        private IWebClientService _service;

        [SetUp]
        public void Init()
        {
            this._helper = Substitute.For<IEmailHelper>();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._service != null)
                this._service.Dispose();

            if (this._helper != null)
                this._helper.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        public void Test()
        {
            this._settings = Substitute.For<IGitHubCacheServiceSettingsProvider>();
            this._service = new WebClientService(this._settings, this._helper);
        }

        #endregion Tests
    }
}