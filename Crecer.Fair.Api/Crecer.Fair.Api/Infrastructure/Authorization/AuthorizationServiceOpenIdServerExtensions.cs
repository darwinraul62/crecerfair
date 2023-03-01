using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Crecer.Fair.Api.Infrastructure.Authorization;
using Ecubytes.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationServiceOpenIdServerExtensions
    {
        public static IServiceCollection AddAuthorizationServer(this IServiceCollection services,
            IConfiguration configuration)
        {
            var openIdServerOptions = configuration.GetSection(OpenIdServerOptions.SectionName).
                Get<OpenIdServerOptions>();

            services.AddDbContext<AuthorizationDbContext>(options =>
               {
                   options.UseNpgsql(openIdServerOptions.OpenIdConnectionString);
                   // Register the entity sets needed by OpenIddict.
                   // Note: use the generic overload if you need
                   // to replace the default OpenIddict entities.
                   options.UseOpenIddict();
               }
           );

            services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<AuthorizationDbContext>();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options
                .SetAuthorizationEndpointUris("/connect/authorize")
                .SetTokenEndpointUris("/connect/token")
                .SetUserinfoEndpointUris("/connect/userinfo")
                .SetLogoutEndpointUris("/connect/logout");


                // Enable the client credentials flow.
                options.AllowClientCredentialsFlow();
                options.AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange();
                options.AllowRefreshTokenFlow();

                // Register the signing and encryption credentials.
                // options.AddDevelopmentEncryptionCertificate()
                // .AddDevelopmentSigningCertificate();

                var certificate = new X509Certificate2(
                    openIdServerOptions.SigningCertificatePath,
                    openIdServerOptions.SigningCertificatePassword
                );

                // options
                // .AddEphemeralEncryptionKey()
                // .AddEphemeralSigningKey()
                // .DisableAccessTokenEncryption();                                               
                options
                //.AddEphemeralEncryptionKey()
                //.AddEncryptionCertificate(openIdServerOptions.EncryptionCertificateThumprint)
                //.AddSigningCertificate(openIdServerOptions.SigningCertificateThumprint);
                .AddEncryptionCertificate(certificate)
                .AddSigningCertificate(certificate);



                if (openIdServerOptions.DisableAccessTokenEncryption)
                    options.DisableAccessTokenEncryption();

                //options.RegisterScopes("grapeti.category.write","grapeti.category.read","profile","openid");                

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                .EnableTokenEndpointPassthrough()
                .EnableAuthorizationEndpointPassthrough()
                .EnableUserinfoEndpointPassthrough()
                .EnableLogoutEndpointPassthrough();

                options.RegisterScopes("openid", "profile", "roles");
                options.DisableScopeValidation();
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();
                //options.AddAudiences("grapeti-catalogs");
                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

            return services;
        }
    }
}
