using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Configuration;
using Ecubytes.Data.Common;
using Ecubytes.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Crecer.Fair.Web.ViewComponents
{
    public class ChatViewComponent : ViewComponent
    {
        private readonly IConfiguration configuration;
        private readonly IProviderService providerService;
        private readonly IFairService fairService;
        private readonly ApiProfileManager apiProfileManager;
        private GlobalOptions globalOptions;
        public ChatViewComponent(IConfiguration configuration, 
            IProviderService providerService,
            IFairService fairService, 
            ApiProfileManager apiProfileManager, IOptions<GlobalOptions> globalOptions)
        {
            this.configuration = configuration;
            this.providerService = providerService;
            this.apiProfileManager = apiProfileManager;
            this.globalOptions = globalOptions.Value;
            this.fairService = fairService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? providerId = null, ChatMode chatMode = ChatMode.OnlyAllowedConversations)
        {
            var profile = apiProfileManager.Get("Identity.User"); 
            IdentityClient client;

            string chatServerUrl = configuration.GetValue<string>("FairSettings:ChatServerUrl");
            Guid? meProviderId = HttpContext.User.GetFairProviderId().GetValueOrDefault();

            string providerEmail = null;
            List<string> allowedEmailConversations = null;

            if (providerId.HasValue && 
                meProviderId != providerId.Value)       
            {                
                var users = await providerService.GetUsersAsync(providerId.Value);
                if (users.Any())
                {                    
                    client = new IdentityClient(
                        profile.BaseAddress,
                        profile.ClientId,
                        profile.ClientSecret,
                        globalOptions.DefaultTenantId);

                    var user = await client.GetAsync(users.First().UserId);

                    providerEmail = user.Email;
                }
            }
            
            if (chatMode == ChatMode.OnlyAllowedConversations || chatMode == ChatMode.All)
            {                
                var standUsers = await fairService.GetStandsUsersAsync((await fairService.GetMainAsync()).FairId);
                
                var queryRequest = QueryRequest.Builder();

                queryRequest.AddCondition("UserId", standUsers.Select(p => p.UserId).ToArray());
                
                client = new IdentityClient(
                        profile.BaseAddress,
                        profile.ClientId,
                        profile.ClientSecret,
                        globalOptions.DefaultTenantId);
            
                var users = await client.GetAsync(queryRequest);

                allowedEmailConversations = users.Data.Select(p => p.Email).ToList();
            }

            ChatComponentViewModel model = new ChatComponentViewModel()
            {
                ChatServerUrl = chatServerUrl,
                ProviderId = providerId,
                ProviderEmail = providerEmail,
                AllowedEmailConversations = allowedEmailConversations,
                ChatMode = chatMode
            };

            return View(model);
        }
    }
}
