using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Validators;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Aliencube.GitHub.Cache.WebApi.Tests
{
    [TestFixture]
    public class ServiceValidatorTest
    {
        #region SetUp / TearDown

        private HttpRequestMessage _request;
        private IServiceValidator _validator;

        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Dispose()
        {
            if (this._validator != null)
                this._validator.Dispose();

            if (this._request != null)
                this._request.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase(true, "abc", "def")]
        [TestCase(false, "abc", null)]
        [TestCase(false, "abc", "")]
        public void ValidateAllValuesRequired_GivenValues_ReturnResult(bool expected, params string[] values)
        {
            this._validator = new ParameterValidator();
            this._validator.ValidateAllValuesRequired(values).Should().Be(expected);
        }

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

            this._validator = BaseAuthenticationValidator.CreateInstance(authType);
            this._validator.ValidateAuthentication(this._request).Should().Be(expected);
        }

        #endregion Tests
    }
}