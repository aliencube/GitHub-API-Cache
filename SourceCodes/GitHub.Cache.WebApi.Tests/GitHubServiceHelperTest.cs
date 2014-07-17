using Aliencube.AlienCache.WebApi.Interfaces;
using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Helpers;
using Aliencube.GitHub.Cache.Services.Interfaces;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.WebApi.Tests
{
    [TestFixture]
    public class GitHubServiceHelperTest
    {
        #region SetUp / TearDown

        private HttpRequestMessage _request;
        private IWebApiCacheConfigurationSettingsProvider _webApiCacheSettings;
        private IGitHubCacheServiceSettingsProvider _gitHubCacheSettings;
        private IServiceValidator _parameterValidator;
        private IGitHubCacheServiceHelper _helper;

        [SetUp]
        public void Init()
        {
            this._webApiCacheSettings = Substitute.For<IWebApiCacheConfigurationSettingsProvider>();
            this._gitHubCacheSettings = Substitute.For<IGitHubCacheServiceSettingsProvider>();
            this._parameterValidator = Substitute.For<IServiceValidator>();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._helper != null)
                this._helper.Dispose();

            if (this._parameterValidator != null)
                this._parameterValidator.Dispose();

            if (this._webApiCacheSettings != null)
                this._webApiCacheSettings.Dispose();

            if (this._gitHubCacheSettings != null)
                this._gitHubCacheSettings.Dispose();

            if (this._request != null)
                this._request.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase(AuthenticationType.Anonymous, null, true)]
        [TestCase(AuthenticationType.Basic, "username:password", true)]
        [TestCase(AuthenticationType.Basic, null, false)]
        [TestCase(AuthenticationType.AuthenticationKey, "token 1234567890", true)]
        [TestCase(AuthenticationType.AuthenticationKey, null, false)]
        public void ValidateAuthentication_GivenRequest_ReturnResult(AuthenticationType authType, string authKey, bool expected)
        {
            var url = "http://localhost";
            if (authType == AuthenticationType.Basic)
            {
                url = String.Format("http://{0}@localhost", authKey);
            }
            var uri = new Uri(url);

            this._request = new HttpRequestMessage(new HttpMethod("GET"), uri);
            if (authType == AuthenticationType.AuthenticationKey)
            {
                if (!String.IsNullOrWhiteSpace(authKey))
                    this._request.Headers.Add("Authorization", authKey);
            }
            this._gitHubCacheSettings.AuthenticationType.Returns(authType);

            this._helper = new GitHubCacheServiceHelper(this._webApiCacheSettings, this._gitHubCacheSettings, this._parameterValidator);
            this._helper.ValidateAuthentication(this._request).Should().Be(expected);
        }

        #endregion Tests
    }
}