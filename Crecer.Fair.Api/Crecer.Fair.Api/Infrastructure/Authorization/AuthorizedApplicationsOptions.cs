using System;
using System.Collections.Generic;

namespace Crecer.Fair.Api.Infrastructure.Authorization
{
    public class AuthorizedApplicationsOptions : List<AutorizedApplicationOptions>
    {
        public const string SectionName = "Ecubytes:AuthorizedApplications";
    }
    public class AutorizedApplicationOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Name { get; set; }
        public List<string> RedirectUris { get; set; }
    }
}
