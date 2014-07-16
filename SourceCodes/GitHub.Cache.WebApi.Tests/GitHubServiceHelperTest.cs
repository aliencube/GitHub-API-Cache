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
            if (this._request != null)
                this._request.Dispose();

            if (this._helper != null)
                this._helper.Dispose();

            if (this._settings != null)
                this._settings.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase(AuthenticationType.Anonymous, null, true)]
        [TestCase(AuthenticationType.Basic, "username:password", true)]
        [TestCase(AuthenticationType.Basic, null, false)]
        [TestCase(AuthenticationType.AuthenticationKey, "token 1234567890", true)]
        [TestCase(AuthenticationType.AuthenticationKey, null, false)]
        public void ValidateRequest_GivenRequest_ReturnResult(AuthenticationType authType, string authKey, bool expected)
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
            this._settings.AuthenticationType.Returns(authType);

            this._helper = new GitHubCacheServiceHelper(this._settings);
            this._helper.ValidateRequest(this._request).Should().Be(expected);
        }

        #endregion Tests
    }
}