using System;
using System.Collections.Generic;

namespace Identity.User.IntegrationEvents
{
    public interface UserCreatedIntegrationEvent
    {
        Guid UserId { get; }
        string LogonName { get; }
        string Names { get; }
        string LastNames { get; }
        string Email { get; }
        bool BlockLogin { get; }
        Guid TenantId { get;}
        Dictionary<string, string> Attributtes { get; }
    }
}
