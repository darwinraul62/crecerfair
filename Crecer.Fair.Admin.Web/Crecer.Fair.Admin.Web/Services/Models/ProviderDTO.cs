using System;

namespace Crecer.Fair.Admin.Web.Services.Models
{
    public class ProviderViewModelDTO
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
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Active { get; set; }
    }

    public class ProviderInsertRequestDTO
    {        
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
        public Guid? CategoryId { get; set; }
        public bool Active { get; set; }
    }

    public class ProviderInsertResponseDTO
    {
        public Guid ProviderId { get; set; }
    }
    public class ProviderUpdateRequestDTO
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
        public Guid? CategoryId { get; set; }
        public bool Active { get; set; }
    }
    public class ProviderRelationViewModelDTO
    {
        public Guid ProviderId { get; set; }
    }
}
