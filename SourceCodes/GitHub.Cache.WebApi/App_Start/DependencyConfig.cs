using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;

namespace Aliencube.GitHub.Cache.WebApi
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            RegisterServices(builder);
            RegisterControllers(builder);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ValidationService>().As<IValidationService>().PropertiesAutowired();
            builder.RegisterType<WebClientService>().As<IWebClientService>().PropertiesAutowired();
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}