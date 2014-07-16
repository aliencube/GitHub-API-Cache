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

        private IEmailHelper _helper;
        private IWebClientService _service;

        [SetUp]
        public void Init()
        {
            this._helper = Substitute.For<IEmailHelper>();
            this._service = new WebClientService(this._helper);
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
        }

        #endregion Tests
    }
}