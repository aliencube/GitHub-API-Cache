using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace Aliencube.GitHub.Cache.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(config =>
                                          {
                                              config.MapHttpAttributeRoutes();
                                              var jsonFormatter = config.Formatters.JsonFormatter;
                                              jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                                              config.AddJsonpFormatter();
                                          });

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyConfig.RegisterDependencies();
        }
    }
}