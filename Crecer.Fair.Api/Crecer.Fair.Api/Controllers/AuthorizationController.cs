using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Crecer.Fair.Data.Repositories;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Configuration;
using Ecubytes.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Crecer.Fair.Api.Controllers
{
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly ApiProfileManager apiProfileManager;
        private readonly IFairRepositoryContainer repositoryContainer;
        private readonly GlobalOptions globalOptions;
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(
            IFairRepositoryContainer repositoryContainer,
            ApiProfileManager apiProfileManager,
            IOptions<GlobalOptions> globalOptions,
            IOpenIddictApplicationManager applicationManager)
        {
            this.repositoryContainer = repositoryContainer;
            this.apiProfileManager = apiProfileManager;
            this.globalOptions = globalOptions.Value;
            this._applicationManager = applicationManager;
        }

        [HttpPost("~/connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            ClaimsPrincipal claimsPrincipal;

            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var application =
                    await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                    throw new InvalidOperationException("The application cannot be found.");

                // Create a new ClaimsIdentity containing the claims that
                // will be used to create an id_token, a token or a code.
                var identity = new ClaimsIdentity(
                    TokenValidationParameters.DefaultAuthenticationType,
                    Claims.Name, Claims.Role);

                // Use the client_id as the subject identifier.
                identity.AddClaim(Claims.Subject,
                    await _applicationManager.GetClientIdAsync(application),
                    Destinations.AccessToken, Destinations.IdentityToken);

                identity.AddClaim(Claims.Name,
                    await _applicationManager.GetDisplayNameAsync(application),
                    Destinations.AccessToken, Destinations.IdentityToken);

                // Subject (sub) is a required field, we use the client id as the subject identifier here.
                //identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

                // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
                identity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);

                claimsPrincipal = new ClaimsPrincipal(identity);

                claimsPrincipal.SetScopes(request.GetScopes());
            }
            else if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the authorization code
                claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            }
            else if (request.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            }
            else
            {
                throw new InvalidOperationException("The specified grant type is not supported.");
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the user principal stored in the authentication cookie.
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);


            // If the user principal can't be extracted, redirect the user to the login page.
            if (!result.Succeeded)
            {
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                };

                //properties.Items.Add("ecubytesapp", request.GetParameter("ecubytesapp").Value.Value.ToString());

                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: properties);
            }

            Guid userId = Guid.Parse(result.Principal.FindFirstValue("sub"));

            var claims = new List<Claim>
            {
                // 'subject' claim which is required90
                new Claim(OpenIddictConstants.Claims.Subject, userId.ToString())
            };

            //Add ProviderId
            var indetityClient = GetIdentityClientInstance();
            var userModel = await indetityClient.GetAsync(userId);

            claims.Add(new Claim(OpenIddictConstants.Claims.Name, userModel.FullName).SetDestinations(Destinations.IdentityToken));
            claims.Add(new Claim(OpenIddictConstants.Claims.Email, userModel.Email).SetDestinations(Destinations.IdentityToken));
            claims.Add(new Claim("username", userModel.LogonName.ToString()).SetDestinations(Destinations.IdentityToken));            
            claims.Add(new Claim("tenantid", globalOptions.DefaultTenantId.ToString() ).SetDestinations(Destinations.IdentityToken));            

            var providerRel = await repositoryContainer.ProviderUser.GetAsync(p => p.UserId == userModel.UserId, includeProperties: "Provider");
            if (providerRel.Any())
            {
                var provider = providerRel.First().Provider;

                claims.Add(new Claim("provider_id", provider.ProviderId.ToString()).SetDestinations(Destinations.AccessToken));
                claims.Add(new Claim("provider_name", provider.Tradename).SetDestinations(Destinations.AccessToken));

                var resourceLogo = await repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p => p.ProviderId == provider.ProviderId
                    && p.ResourceTypeId == "LOGO");
                
                if (resourceLogo != null)
                    claims.Add(new Claim("picture", resourceLogo.Value).SetDestinations(Destinations.IdentityToken));
            }

            if (request.GetScopes().Contains("roles"))
            {
                var roles = await indetityClient.GetEffectiveRolesAsync(new Ecubytes.Identity.Models.UserRoleRequest()
                {
                    ApplicationId = globalOptions.DefaultApplicationId,
                    UserId = userModel.UserId
                });

                claims.AddRange(roles.Select(p => new Claim("role", p.CodeName).SetDestinations(Destinations.AccessToken)));
            }

            // Create a new claims principal            
            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, "name", "role");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Set requested scopes (this is not done automatically)
            claimsPrincipal.SetScopes(request.GetScopes());
            //claimsPrincipal.SetResources("https://localhost:6001");
            claimsPrincipal.SetResources(request.Resources);

            // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo")]
        public async Task<IActionResult> Userinfo()
        {
            var claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

            return Ok(new
            {
                //Name = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
                sub = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject),
                // role = claimsPrincipal.GetClaims(OpenIddictConstants.Claims.Role),
                //Occupation = "Developer",
                //Age = 43
            });
        }

        //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }

        #region methods

        private IdentityClient GetIdentityClientInstance()
        {
            var profile = apiProfileManager.Get("Identity.User");

            IdentityClient client = new IdentityClient(
                profile.BaseAddress,
                profile.ClientId,
                profile.ClientSecret,
                globalOptions.DefaultTenantId);

            return client;
        }

        #endregion
    }
}
