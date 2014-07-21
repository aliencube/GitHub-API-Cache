using Aliencube.AlienCache.WebApi.Interfaces;
using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Helpers;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Validators;
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

        [Test]
        [TestCase("/v3", "url=repo/abc/def", true, "url", true)]
        [TestCase("/v3", null, true, "url", false)]
        [TestCase("/v3", "url=repo/abc/def", true, "key", false)]
        [TestCase("/v3", "url=repo/abc/def", false, "url", true)]
        public void ValidateRequestUrl_GivenUrl_ReturnResult(string path, string queryString, bool useQueryStringAsKey, string queryStringKey, bool expected)
        {
            var url = String.Format("http://localhost{0}{1}", path, String.IsNullOrWhiteSpace(queryString) ? String.Empty : "?" + queryString);
            var uri = new Uri(url);

            this._request = new HttpRequestMessage(new HttpMethod("GET"), uri);
            this._webApiCacheSettings.UseQueryStringAsKey.Returns(useQueryStringAsKey);
            this._webApiCacheSettings.QueryStringKey.Returns(queryStringKey);
            this._parameterValidator = new ParameterValidator();

            this._helper = new GitHubCacheServiceHelper(this._webApiCacheSettings, this._gitHubCacheSettings, this._parameterValidator);
            this._helper.ValidateRequestUrl(this._request).Should().Be(expected);
        }

        [Test]
        [TestCase("/v3", "url=repo/abc/def", true, "url")]
        [TestCase("/v3", "url=repo/abc/def", false, "url")]
        public void GetRequestUri_GivenRequest_ReturnUrl(string path, string queryString, bool useQueryStringAsKey, string queryStringKey)
        {
            var url = String.Format("http://localhost{0}{1}", path, String.IsNullOrWhiteSpace(queryString) ? String.Empty : "?" + queryString);
            var uri = new Uri(url);

            this._request = new HttpRequestMessage(new HttpMethod("GET"), uri);
            this._webApiCacheSettings.UseQueryStringAsKey.Returns(useQueryStringAsKey);
            this._webApiCacheSettings.QueryStringKey.Returns(queryStringKey);
            this._parameterValidator = new ParameterValidator();

            var expected = String.Format("https://api.github.com{0}",
                                         (!this._webApiCacheSettings.UseQueryStringAsKey
                                              ? this._request.RequestUri.AbsolutePath
                                              : (useQueryStringAsKey
                                                     ? "/" + this._request.RequestUri.ParseQueryString().Get(queryStringKey)
                                                     : String.Empty)));
            this._helper = new GitHubCacheServiceHelper(this._webApiCacheSettings, this._gitHubCacheSettings, this._parameterValidator);
            this._helper.GetRequestUri(this._request).OriginalString.Should().Be(expected);
        }

        [Test]
        [TestCase("/repo?callback=jQuery_123456", true)]
        [TestCase("/repo?key=jQuery_123456", false)]
        [TestCase("/repo", false)]
        public void IsJsonpRequest_GivenRequest_ReturnResult(string url, bool expected)
        {
            var uri = new Uri(String.Format("http://localhost{0}", url));
            this._request = new HttpRequestMessage(new HttpMethod("GET"), uri);

            this._helper = new GitHubCacheServiceHelper(this._webApiCacheSettings, this._gitHubCacheSettings, this._parameterValidator);
            this._helper.IsJsonpRequest(this._request).Should().Be(expected);
        }

        [Test]
        [TestCase("/repo", "jQuery_123456", "{\"key\": \"value\"}")]
        [TestCase("/repo", "jQuery_123456", null)]
        public void WrapJsonpCallback_GivenRequest_ReturnResult(string url, string callback, string value)
        {
            var uri = new Uri(String.Format("http://localhost{0}?callback={1}", url, callback));
            this._request = new HttpRequestMessage(new HttpMethod("GET"), uri);

            var expected = String.Format("{0}({1})", callback, value);
            this._helper = new GitHubCacheServiceHelper(this._webApiCacheSettings, this._gitHubCacheSettings, this._parameterValidator);
            this._helper.WrapJsonpCallback(this._request, value).Should().Be(expected);
        }

        #endregion Tests
    }
}