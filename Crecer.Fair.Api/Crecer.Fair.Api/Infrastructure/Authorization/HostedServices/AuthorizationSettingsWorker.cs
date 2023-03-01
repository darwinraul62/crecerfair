using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ecubytes.Authorization.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Linq;
using System.Collections.Generic;
using Crecer.Fair.Api.Infrastructure.Authorization;

namespace Crecer.Fair.Api.Authorization.HostedServices
{
    public class AuthorizationSettingsWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthorizationSettingsWorker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            AuthorizedApplicationsOptions configureListOptions =
                configuration.GetSection(AuthorizedApplicationsOptions.SectionName).Get<AuthorizedApplicationsOptions>();

            var context = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager =
                scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            //var tokenManager = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();

            foreach (var configureOptions in configureListOptions)
            {
                // if (await manager.FindByClientIdAsync(configureOptions.ClientId) is not null)
                // {
                //     await manager.DeleteAsync(await manager.FindByClientIdAsync(configureOptions.ClientId));
                // }
                
                if (await manager.FindByClientIdAsync(configureOptions.ClientId) is null)
                {
                    List<string> permissions = new List<string>();

                    OpenIddictApplicationDescriptor app = new OpenIddictApplicationDescriptor();

                    app.ClientId = configureOptions.ClientId;
                    app.ClientSecret = configureOptions.ClientSecret;
                    app.DisplayName = configureOptions.Name;
                    app.RedirectUris.UnionWith(configureOptions.RedirectUris.Select(p => new Uri(p)));

                    permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);

                    // permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                    // permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);

                    permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                    permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                    permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);

                    permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);

                    permissions.Add(OpenIddictConstants.Permissions.Scopes.Profile);
                    permissions.Add(OpenIddictConstants.Permissions.Scopes.Roles);

                    // permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + "openid");
                    // permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + "roles");

                    app.Permissions.UnionWith(permissions);

                    await manager.CreateAsync(app);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
