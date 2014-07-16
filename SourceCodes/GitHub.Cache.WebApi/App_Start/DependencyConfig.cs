using Aliencube.GitHub.Cache.Services;
using Aliencube.GitHub.Cache.Services.Helpers;
using Aliencube.GitHub.Cache.Services.Interfaces;
using Aliencube.GitHub.Cache.Services.Validators;
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

            RegisterSettings(builder);
            RegisterHelpers(builder);
            RegisterServices(builder);
            RegisterControllers(builder);

            var container = builder.Build();

            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static void RegisterSettings(ContainerBuilder builder)
        {
            builder.RegisterType<GitHubCacheServiceSettingsProvider>().As<IGitHubCacheServiceSettingsProvider>().PropertiesAutowired();
        }

        private static void RegisterHelpers(ContainerBuilder builder)
        {
            builder.RegisterType<EmailHelper>().As<IEmailHelper>().PropertiesAutowired();
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<ParameterValidator>().As<IServiceValidator>().PropertiesAutowired();
            builder.RegisterType<WebClientService>().As<IWebClientService>().PropertiesAutowired();
        }

        private static void RegisterControllers(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
        }
    }
}