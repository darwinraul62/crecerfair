using System;
using Crecer.Fair.Infrastructure;
using Ecubytes.Integration.Event.Repository.EntityFramework;
using Ecubytes.Integration.Event.Repository.EntityFramework.Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FairDbContextExtensions
    {
        public static IServiceCollection AddFairDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("FairDbContext");

            services.AddDbContext<FairDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddLoggingDbContext(options=>            
                options.UseNpgsql(connectionString)
            );

            services.AddIntegrationEventDbContext<FairDbContext>();

            return services;
        }
    }
}
