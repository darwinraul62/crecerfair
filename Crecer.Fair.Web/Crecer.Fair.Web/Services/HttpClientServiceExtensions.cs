using System;
using Crecer.Fair.Web.Services;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public  static class HttpClientServiceExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultHttpHandlers(configuration);
            
            services.AddHttpClient<IPartnertService,PartnertService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()    
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IFairService,FairService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()                
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            
            services.AddHttpClient<IProviderService,ProviderService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()                    
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddHttpClient<IStandModelService,StandModelService>()
                .AddWaitAndRetryPolicy()
                .AddCircuitBreakerPolicy()
                .AddHttpClientAuthorizationHandler()
                .AddHttpClientDefaultTenantIdHandler()    
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));
            
            // services.AddHttpClient<IUserService,UserService>()
            //     .AddHttpClientAuthorizationHandler()
            //     .AddHttpClientDefaultTenantIdHandler()
            //     .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}
