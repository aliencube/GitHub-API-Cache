using Aliencube.GitHub.Cache.Services.Interfaces;
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
                var userAgents = request.Headers.UserAgent.ToList();
                userAgents.Add(new ProductInfoHeaderValue(this.GetType().Assembly.GetName().Name, this.GetType().Assembly.GetName().Version.ToString()));
                client.Headers.Add(HttpRequestHeader.UserAgent, String.Join(" ", userAgents));

                var accepts = request.Headers.Accept.ToList();
                if (!accepts.Any())
                {
                    accepts.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                }
                client.Headers.Add(HttpRequestHeader.Accept, String.Join(",", accepts));

                try
                {
                    var value = client.DownloadString(uri);
                    if (String.IsNullOrWhiteSpace(value))
                    {
                        return request.CreateResponse(HttpStatusCode.InternalServerError);
                    }

                    var content = new StringContent(value, Encoding.UTF8, "application/json");
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
        /// Gets the <c>HttpResponseMessage</c> instance for error.
        /// </summary>
        /// <param name="request"><c>HttpRequestMessage</c> instance.</param>
        /// <param name="ex"><c>Exception</c> instance.</param>
        /// <returns>Returns the <c>HttpResponseMessage</c> instance for error.</returns>
        public HttpResponseMessage GetErrorReponse(HttpRequestMessage request, Exception ex)
        {
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
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
            response = request.CreateResponse(statusCode == null
                                                  ? HttpStatusCode.InternalServerError
                                                  : statusCode);
            return response;
        }
    }
}