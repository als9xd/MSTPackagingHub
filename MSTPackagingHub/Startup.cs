using Microsoft.Owin;
using Owin;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MSTPackagingHub.Services;
using MSTPackagingHub.Interfaces;

using System.Web.Mvc;

using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(MSTPackagingHub.Startup))]
namespace MSTPackagingHub
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
           IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }

    public class DefaultDependencyResolver : 
        System.Web.Http.Dependencies.IDependencyResolver,
        System.Web.Mvc.IDependencyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {

        }
    }

    public partial class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            PackageScraperService pScraper = new PackageScraperService();
            pScraper.SetDirectories(new[] {
                "\\\\minerfiles.mst.edu\\dfs\\software\\itwindist\\win10"
            });
            pScraper.LoadScripts();
            services.AddSingleton<IPackageScraper>(pScraper);
            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition).Where(t => typeof(IController).IsAssignableFrom(t) || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));
        }

        public void Configuration(IAppBuilder app)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            ConfigureAuth(app);
            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

        }
    }

}
