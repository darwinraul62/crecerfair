using System;

namespace Crecer.Fair.Web.Services.Models
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
    public class ProviderResourceViewModelDTO
    {
        public Guid ResourceId { get; set; }
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Value { get; set; }
        public string ResourceTypeName { get; set; }
        public string ThumbnailPath { get; set; }
        public int Priority { get; set; }
    }
    public class ProviderUserViewModelDTO
    {
        public Guid UserId { get; set; }
    }
}
