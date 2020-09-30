using Microsoft.Extensions.DependencyInjection;

namespace WebCrawler.Infrastructure.ExtensionMethods
{
    public static class DomainInjectDependence
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddTransient<Domain.ProxyPage.IProxyPageService, Domain.Services.ProxyPageService>();
            services.AddTransient<Domain.ProxyPage.IProxyPageRepository, Data.Repository.ProxyPageRepository>();

            services.AddTransient<Domain.Config.IDbConfig, Data.Config.DbConfig>();
            return services;
        }
    }
}
