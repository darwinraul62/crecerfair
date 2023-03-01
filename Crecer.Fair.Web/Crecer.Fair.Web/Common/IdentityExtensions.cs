using System;
using System.Linq;
using System.Security.Claims;

namespace Crecer.Fair.Web
{
    public static class IdentityExtensions
    {
        public static Guid? GetFairProviderId(this ClaimsPrincipal claimPrincipal)
        {
            var claim = claimPrincipal.Claims.FirstOrDefault(p => p.Type == "provider_id");
            if (claim != null)
            {
                Guid providerId = Guid.Empty;
                if(Guid.TryParse(claim.Value, out providerId))
                    return providerId;
            }

            return null;
        }

        public static string GetFairProviderName(this ClaimsPrincipal claimPrincipal)
        {
            var claim = claimPrincipal.Claims.FirstOrDefault(p => p.Type == "provider_name");
            if (claim != null)            
                return claim.Value;            

            return null;
        }
    }
}
