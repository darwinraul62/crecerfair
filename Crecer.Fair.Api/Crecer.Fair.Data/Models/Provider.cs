using System;
using System.Collections.Generic;
using Crecer.Fair.Data.Models;

namespace Crecer.Fair.Data
{
    public class Provider
    {
        public Guid ProviderId { get; set; }
        public string Identification { get; set; }
        public string Tradename { get; set; }
        public string BusinessName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string FacebookAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string YoutubeAddress { get; set; }
        public string InstagramAddress { get; set; }
        public bool Active { get; set; }
        public Guid? CategoryId { get; set; }
        public ProviderCategory Category { get; set; }
        public List<ProviderResource> Resources { get; set; }
        public List<ProviderUser> Users { get; set; }
        public List<ProviderCalendar> Calendar { get; set; }
    }
}
