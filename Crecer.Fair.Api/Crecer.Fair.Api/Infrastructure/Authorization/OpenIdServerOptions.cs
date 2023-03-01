using System;

namespace Crecer.Fair.Api.Infrastructure.Authorization
{
    public class OpenIdServerOptions
    {
        public const string SectionName = "Ecubytes:OpenIdServer";
        public string SigningCertificatePassword { get; set; }
        public string SigningCertificatePath { get; set; }
        public bool DisableAccessTokenEncryption { get; set; }
        public string OpenIdConnectionString { get; set; }
    }
}
