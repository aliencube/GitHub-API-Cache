using Aliencube.GitHub.Cache.Services.Exceptions;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Properties;
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

        private readonly IEmailHelper _emailHelper;

        /// <summary>
        /// Initialises a new instance of the WebClientService class.
        /// </summary>
        public WebClientService(IEmailHelper emailHelper)
        {
            this._emailHelper = emailHelper;
            this.Settings = Settings.Default;
        }

        /// <summary>
        /// Gets the configuration settings.
        /// </summary>
        public Settings Settings { get; private set; }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="validationService"><c>ValidationService</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage GetResponseMessage(HttpRequestMessage request, IValidationService validationService)
        {
            var url = request.RequestUri.ParseQueryString().Get("url");
            if (!validationService.ValidateAllValuesRequired(url))
            {
                return request.CreateResponse(HttpStatusCode.NotFound);
            }

            if (url.StartsWith("/"))
            {
                url = url.Substring(1);
            }
            url = String.Join("/", GITHUB_API_URL, url);
            var response = this.GetResponse(request, url);
            return response;
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="url">URL to send the request.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage GetResponse(HttpRequestMessage request, string url)
        {
            var uri = new Uri(url);
            return this.GetResponse(request, uri);
        }

        /// <summary>
        /// Gets the <c>HttpResponseMessage</c> instance.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance.</returns>
        public HttpResponseMessage GetResponse(HttpRequestMessage request, Uri uri)
        {
            HttpResponseMessage response;
            try
            {
                var validated = this.ValidateRequest(request, uri);
                if (!validated)
                {
                    throw new InvalidOperationException("Unauthorised");
                }

                using (var client = new WebClient())
                {
                    if (this.Settings.UseProxy)
                    {
                        var proxy = new WebProxy(this.Settings.ProxyUrl, this.Settings.BypassOnLocal);
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

                    if (this.Settings.AuthenticationType == AuthenticationType.AuthenticationKey)
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

                if (!this.Settings.UseErrorLogEmail)
                {
                    return response;
                }

                var from = this.Settings.ErrorLogEmailFrom;
                var to = this.Settings.ErrorLogEmailTo;
                var subject = String.Format("GitHub API Cache - {0}", ex.Message);
                var body = String.Format("{0}\n{1}", ex.Message, ex.StackTrace);
                this._emailHelper.Send(@from, to, subject, body);
            }
            return response;
        }

        /// <summary>
        /// Validates whether the request comes with proper values or not.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="uri"><c>Uri</c> to send the request.</param>
        /// <returns>Returns <c>True</c>, if the request is valid; otherwise returns <c>False</c>.</returns>
        public bool ValidateRequest(HttpRequestMessage request, Uri uri)
        {
            var validated = true;
            switch (this.Settings.AuthenticationType)
            {
                case AuthenticationType.Basic:
                    validated = this.ValidateBasicAuthentication(uri.OriginalString);
                    break;

                case AuthenticationType.AuthenticationKey:
                    validated = this.ValidateAuthorisationHeader(request.Headers.Authorization);
                    break;
            }

            return validated;
        }

        /// <summary>
        /// Validates whether the username and password are included in the URL segment or not.
        /// </summary>
        /// <param name="url">URL segment.</param>
        /// <returns>Returns <c>True</c>, if the username and password are included in the URL segment; otherwise returns <c>False</c>.</returns>
        public bool ValidateBasicAuthentication(string url)
        {
            var segments = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            var validated = segments[1].Contains(":") && segments[1].Contains("@");
            return validated;
        }

        /// <summary>
        /// Validates whether the authentication key exists in a correct format or not.
        /// </summary>
        /// <param name="header">Authentication header value.</param>
        /// <returns>Returns <c>True</c>, if the authentication key exists in a correct format; otherwise returns <c>False</c>.</returns>
        public bool ValidateAuthorisationHeader(AuthenticationHeaderValue header)
        {
            if (header == null)
            {
                return false;
            }

            var token = header.ToString();
            if (String.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            if (!token.StartsWith("token "))
            {
                return false;
            }

            return true;
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