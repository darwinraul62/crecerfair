using System;
using System.Threading.Tasks;
using Crecer.Fair.Data.Repositories;
using Identity.User.IntegrationEvents;
using MassTransit;

namespace Crecer.Fair.Api.Infrastructure.IntegrationEvents.Handlers
{
    public class UserCreatedIntegrationEventHandler : IConsumer<UserCreatedIntegrationEvent>
    {
        private IFairRepositoryContainer repositoryContainer;
        public UserCreatedIntegrationEventHandler(IFairRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;
        }

        public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
        {
            var partnert = await repositoryContainer.Partnert.GetFirstOrDefaultAsync(p => p.PartnertId == context.Message.UserId);

            if (partnert != null)
            {
                partnert.Email = context.Message.Email;
                partnert.LastNames = context.Message.LastNames;
                partnert.Names = context.Message.Names;
                partnert.LastNames = context.Message.LastNames;
                if (context.Message.Attributtes != null)
                {
                    if (context.Message.Attributtes.ContainsKey("PhoneNumber"))
                        partnert.PhoneNumber = context.Message.Attributtes["PhoneNumber"];
                    if (context.Message.Attributtes.ContainsKey("Address"))
                        partnert.Address = context.Message.Attributtes["Address"];
                    if (context.Message.Attributtes.ContainsKey("Identification"))
                        partnert.Address = context.Message.Attributtes["Identification"];
                }

                partnert.HasUser = true;

                repositoryContainer.Partnert.Update(partnert);
                await repositoryContainer.SaveChangesAsync();
            }
        }
    }
}