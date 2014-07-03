using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "RefApi",
                routeTemplate: "api/{controller}/{user}/{repo}/{branch}",
                defaults: new { user = RouteParameter.Optional, repo = RouteParameter.Optional, branch = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}