using System;

namespace Crecer.Fair.Data
{
    public class ProviderUser
    {
        public Guid UserId { get; set; }
        public Guid ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
