using System;

namespace Crecer.Fair.Web.Models
{
    public class FooterSectionModel
    {
        public FairContact ContactData { get; set; }
    }

    public class FairContact
    {
        public string Host { get; set; }
        public string Address { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string FacebookAddress { get; set; }
        public string InstagramAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string YoutubeAddress { get; set; }
    }
}
