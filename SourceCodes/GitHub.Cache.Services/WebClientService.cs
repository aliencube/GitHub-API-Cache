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
        private readonly Settings _settings;

        /// <summary>
        /// Initialises a new instance of the WebClientService class.
        /// </summary>
        public WebClientService()
        {
            this._settings = Settings.Default;
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
            using (var client = new WebClient())
            {
                if (this._settings.UseProxy)
                {
                    var proxy = new WebProxy(this._settings.ProxyUrl, this._settings.BypassOnLocal);
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

                switch (this._settings.AuthenticationType)
                {
                    case AuthenticationType.Basic:
                        uri = this.GetBasicAuthenticationUri(uri.OriginalString);
                        break;

                    case AuthenticationType.AuthenticationKey:
                        client.Headers.Add(HttpRequestHeader.Authorization, String.Format("token {0}", this._settings.AutenticationKey));
                        break;
                }

                try
                {
                    var value = client.DownloadString(uri);
                    if (String.IsNullOrWhiteSpace(value))
                    {
                        return request.CreateResponse(HttpStatusCode.InternalServerError);
                    }

                    var content = this.GetStringContent(value);
                    response = request.CreateResponse(HttpStatusCode.OK);
                    response.Content = content;
                }
                catch (Exception ex)
                {
                    response = this.GetErrorReponse(request, ex);
                }
            }
            return response;
        }

        /// <summary>
        /// Gets the basic authentication URL with username and password.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>Returns the basic authentication URL with username and password.</returns>
        public Uri GetBasicAuthenticationUri(string url)
        {
            var segments = url.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            segments[1] = String.Format("{0}:{1}@{2}", this._settings.Username, this._settings.Password, segments[1]);
            var uri = new Uri(String.Join("//", segments));
            return uri;
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
        /// Gets the string content.
        /// </summary>
        /// <param name="value">Content value.</param>
        /// <returns>Returns the string content.</returns>
        public StringContent GetStringContent(string value)
        {
            var content = new StringContent(value, Encoding.UTF8, "application/json");
            return content;
        }

        /// <summary>
        /// Gets the string content.
        /// </summary>
        /// <param name="ex">Exception instance.</param>
        /// <returns>Returns the string content.</returns>
        public StringContent GetStringContent(Exception ex)
        {
            var json = new JObject
            {
                {"message", ex.Message},
                {"moreInfo", "https://github.com/aliencube/GitHub-API-Cache"}
            };
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            return content;
        }
    }
}