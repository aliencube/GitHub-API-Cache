using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Helpers;
using Aliencube.GitHub.Cache.Services.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Aliencube.GitHub.Cache.WebApi.Tests
{
    [TestFixture]
    public class GitHubServiceHelperTest
    {
        #region SetUp / TearDown

        private IGitHubCacheServiceHelper _helper;
        private IGitHubCacheServiceSettingsProvider _settings;

        [SetUp]
        public void Init()
        {
            this._settings = Substitute.For<IGitHubCacheServiceSettingsProvider>();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._helper != null)
                this._helper.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        public void ValidateAllValuesRequired_GivenValues_ReturnResult()
        {
            this._helper = new GitHubCacheServiceHelper(this._settings);
        }

        #endregion Tests
    }
}