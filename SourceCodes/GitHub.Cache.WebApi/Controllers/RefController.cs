using Aliencube.AlienCache.WebApi;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    public class RefController : ApiController
    {
        private const string REF_URL = "https://api.github.com/repos/{0}/{1}/git/refs/heads/{2}";

        [WebApiCache()]
        public HttpResponseMessage Get(string user, string repo, string branch)
        {
            if (String.IsNullOrWhiteSpace(user))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            if (String.IsNullOrWhiteSpace(repo))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            if (String.IsNullOrWhiteSpace(branch))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            HttpResponseMessage response;
            using (var client = new WebClient())
            {
                var userAgents = Request.Headers.UserAgent.ToList();
                userAgents.Add(new ProductInfoHeaderValue(this.GetType().Assembly.GetName().Name, this.GetType().Assembly.GetName().Version.ToString()));
                client.Headers.Add(HttpRequestHeader.UserAgent, String.Join(" ", userAgents));

                var accepts = Request.Headers.Accept.ToList();
                if (!accepts.Any())
                {
                    accepts.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                }
                client.Headers.Add(HttpRequestHeader.Accept, String.Join(",", accepts));

                try
                {
                    var url = String.Format(REF_URL, user, repo, branch);
                    var value = client.DownloadString(url);
                    if (String.IsNullOrWhiteSpace(value))
                    {
                        return Request.CreateResponse(HttpStatusCode.InternalServerError);
                    }

                    var content = new StringContent(value, Encoding.UTF8, "application/json");
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = content;
                }
                catch (Exception ex)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                    if (ex.GetType() != typeof (WebException))
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
                    response = Request.CreateResponse(statusCode == null
                                                          ? HttpStatusCode.InternalServerError
                                                          : statusCode);
                }
            }
            return response;
        }
    }
}