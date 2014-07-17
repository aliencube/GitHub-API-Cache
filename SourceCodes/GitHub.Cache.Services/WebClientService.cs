using Aliencube.AlienCache.WebApi.Interfaces;
using Aliencube.GitHub.Cache.Services.Exceptions;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Aliencube.GitHub.Cache.Services
{
    /// <summary>
    /// This represents the web client service entity.
    /// </summary>
    public class WebClientService : IWebClientService
    {
        private const string GITHUB_API_URL = "https://api.github.com";

        private readonly IWebApiCacheConfigurationSettingsProvider _webApiCacheSettings;
        private readonly IGitHubCacheServiceSettingsProvider _gitHubCacheSettings;
        private readonly IServiceValidator _parameterValidator;
        private readonly IGitHubCacheServiceHelper _gitHubCacheServiceHelper;
        private readonly IEmailHelper _emailHelper;

        /// <summary>
        /// Initialises a new instance of the WebClientService class.
        /// </summary>
        /// <param name="webApiCacheSettings"><c>WebApiCacheConfigurationSettingsProvider</c> instance.</param>
        /// <param name="gitHubCacheSettings"><c>GitHubCacheServiceSettingsProvider</c> instance.</param>
        /// <param name="parameterValidator"><c>ServiceValidator</c> instance.</param>
        /// <param name="gitHubCacheServiceHelper"><c>GitHubCacheServiceHelper</c> instance.</param>
        /// <param name="emailHelper"><c>EmailHelper</c> instance.</param>
        public WebClientService(IWebApiCacheConfigurationSettingsProvider webApiCacheSettings,
                                IGitHubCacheServiceSettingsProvider gitHubCacheSettings,
                                IServiceValidator parameterValidator,
                                IGitHubCacheServiceHelper gitHubCacheServiceHelper,
                                IEmailHelper emailHelper)
        {
            if (webApiCacheSettings == null)
            {
                throw new ArgumentNullException("webApiCacheSettings");
            }
            this._webApiCacheSettings = webApiCacheSettings;

            if (gitHubCacheSettings == null)
            {
                throw new ArgumentNullException("gitHubCacheSettings");
            }
            this._gitHubCacheSettings = gitHubCacheSettings;

            if (parameterValidator == null)
            {
                throw new ArgumentNullException("parameterValidator");
            }
            this._parameterValidator = parameterValidator;

            if (gitHubCacheServiceHelper == null)
            {
                throw new ArgumentNullException("gitHubCacheServiceHelper");
            }
            this._gitHubCacheServiceHelper = gitHubCacheServiceHelper;

            if (emailHelper == null)
            {
                throw new ArgumentNullException("emailHelper");
            }
            this._emailHelper = emailHelper;
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage GetResponseMessage(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpResponseMessage response;
            try
            {
                var validated = this._gitHubCacheServiceHelper.ValidateAuthentication(request);
                if (!validated)
                {
                    throw new InvalidOperationException("Unauthorised");
                }

                validated = this._gitHubCacheServiceHelper.ValidateRequestUrl(request);
                if (!validated)
                {
                    response = request.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }

                using (var client = new WebClient())
                {
                    if (this._gitHubCacheSettings.UseProxy)
                    {
                        var proxy = new WebProxy(this._gitHubCacheSettings.ProxyUrl, this._gitHubCacheSettings.BypassOnLocal);
                        client.Proxy = proxy;
                    }

                    var userAgents = request.Headers.UserAgent.ToList();
                    userAgents.Add(new ProductInfoHeaderValue(this.GetType().Assembly.GetName().Name, this.GetType().Assembly.GetName().Version.ToString()));
                    client.Headers.Add(HttpRequestHeader.UserAgent, String.Join(" ", userAgents));

                    var accepts = request.Headers.Accept.ToList();
                    if (!accepts.Any())
                    {
                        accepts.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                    }
                    client.Headers.Add(HttpRequestHeader.Accept, String.Join(",", accepts));

                    if (this._gitHubCacheSettings.AuthenticationType == AuthenticationType.AuthenticationKey)
                    {
                        client.Headers.Add(HttpRequestHeader.Authorization, request.Headers.Authorization.ToString());
                    }

                    var value = client.DownloadString(uri);
                    if (String.IsNullOrWhiteSpace(value))
                    {
                        throw new GitHubResponseNullException("GitHub response value is null", WebExceptionStatus.ReceiveFailure);
                    }

                    var mediaType = "application/json";
                    response = request.CreateResponse(HttpStatusCode.OK);
                    if (this.IsJsonpRequest(request))
                    {
                        value = this.WrapJsonpCallback(request, value);
                        mediaType = "text/javascript";
                    }

                    var content = this.GetStringContent(value, mediaType);
                    response.Content = content;
                }
            }
            catch (Exception ex)
            {
                response = this.GetErrorReponse(request, ex);

                if (!this._gitHubCacheSettings.UseErrorLogEmail)
                {
                    return response;
                }

                var from = this._gitHubCacheSettings.ErrorLogEmailFrom;
                var to = this._gitHubCacheSettings.ErrorLogEmailTo;
                var subject = String.Format("GitHub API Cache - {0}", ex.Message);
                var body = String.Format("{0}\n{1}", ex.Message, ex.StackTrace);
                this._emailHelper.Send(@from, to, subject, body);
            }
            return response;
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance for error.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="ex"><c>Exception</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance for error.</returns>
        public HttpResponseMessage GetErrorReponse(HttpRequestMessage request, Exception ex)
        {
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            response.Content = this.GetStringContent(ex);
            if (ex.GetType() != typeof(WebException))
            {
                return response;
            }

            var webException = ex as WebException;
            if (webException == null)
            {
                return response;
            }

            var webResponse = webException.Response as HttpWebResponse;
            if (webResponse == null)
            {
                return response;
            }

            var statusCode = webResponse.StatusCode;
            if (statusCode != null)
            {
                response.StatusCode = statusCode;
            }

            return response;
        }

        /// <summary>
        /// Checks whether the request contains JSONP callback or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <returns>Returns <c>True</c>, if the request contains JSONP callback; otherwise returns <c>False</c>.</returns>
        public bool IsJsonpRequest(HttpRequestMessage request)
        {
            var qs = request.RequestUri.ParseQueryString();
            var callback = qs.Get("callback");
            return !String.IsNullOrWhiteSpace(callback);
        }

        /// <summary>
        /// Wraps the response message with the callback function specified.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="value">Content value.</param>
        /// <returns>Returns the response message with the callback function specified.</returns>
        public string WrapJsonpCallback(HttpRequestMessage request, string value)
        {
            var qs = request.RequestUri.ParseQueryString();
            var callback = qs.Get("callback");
            value = String.Format("{0}({1})", callback, value);
            return value;
        }

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="value">Content value.</param>
        /// <param name="mediaType">Media type.</param>
        /// <returns>Returns the string content.</returns>
        public StringContent GetStringContent(string value, string mediaType = null)
        {
            if (String.IsNullOrWhiteSpace(mediaType))
            {
                mediaType = "application/json";
            }
            var content = new StringContent(value, Encoding.UTF8, mediaType);
            return content;
        }

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="ex">Exception instance.</param>
        /// <param name="mediaType">Media type.</param>
        /// <returns>Returns the string content.</returns>
        public StringContent GetStringContent(Exception ex, string mediaType = null)
        {
            if (String.IsNullOrWhiteSpace(mediaType))
            {
                mediaType = "application/json";
            }
            var json = new JObject
            {
                {"message", ex.Message},
                {"moreInfo", "https://github.com/aliencube/GitHub-API-Cache"}
            };
            var content = new StringContent(json.ToString(), Encoding.UTF8, mediaType);
            return content;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}