using System;
using Crecer.Fair.Admin.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientServiceExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultHttpHandlers(configuration);

            services.AddHttpClient<IProviderService, ProviderService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IPartnertService, PartnertService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IResourceTypeService, ResourceTypeService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IProviderCategoryService, ProviderCategoryService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IFairService, FairService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}
