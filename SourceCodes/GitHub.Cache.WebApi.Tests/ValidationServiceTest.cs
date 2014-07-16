using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Interfaces;
using FluentAssertions;
using NUnit.Framework;

namespace Aliencube.GitHub.Cache.WebApi.Tests
{
    [TestFixture]
    public class ValidationServiceTest
    {
        #region SetUp / TearDown

        private IValidationService _service;

        [SetUp]
        public void Init()
        {
            this._service = new ValidationService();
        }

        [TearDown]
        public void Dispose()
        {
            if (this._service != null)
                this._service.Dispose();
        }

        #endregion SetUp / TearDown

        #region Tests

        [Test]
        [TestCase(true, "abc", "def")]
        [TestCase(false, "abc", null)]
        [TestCase(false, "abc", "")]
        public void ValidateAllValuesRequired_GivenValues_ReturnResult(bool expected, params string[] values)
        {
            this._service.ValidateAllValuesRequired(values).Should().Be(expected);
        }

        #endregion Tests
    }
}