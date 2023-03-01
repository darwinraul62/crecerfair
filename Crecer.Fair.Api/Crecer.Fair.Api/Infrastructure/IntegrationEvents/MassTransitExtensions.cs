using System;
using Crecer.Fair.Api.Infrastructure.IntegrationEvents;
using Crecer.Fair.Api.Infrastructure.IntegrationEvents.Handlers;
using Identity.User.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitEventBus(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<UserCreatedIntegrationEventHandler>();            

                x.UsingAmazonSqs((context, cfg) =>
                {
                    //Set Default OPTIONS From Configuration
                    cfg.AddHost("AWS", configuration);

                    cfg.UseDefaultKillSwitch();
                   
                    cfg.ReceiveEndpoint("AWS","UserCreated", configuration, e =>
                     {
                         e.ConfigureConsumeTopology = false;

                         e.Subscribe("AWS","UserCreated", configuration, c =>
                         {
                             c.TopicSubscriptionAttributes["FilterPolicy"] = $"{{\"IsPartnert\": [\"true\"]}}";                             
                         });
                    
                         e.Consumer<UserCreatedIntegrationEventHandler>(context);
                     });
                });
            });

            services.AddMassTransitHostedService(true);

            return services;
        }
    }
}
