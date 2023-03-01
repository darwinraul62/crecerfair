using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
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
        [Required]
        public string Identification { get; set; }
        [Required]
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

    public class ProviderRelationViewModelDTO
    {
        public Guid ProviderId { get; set; }
    }

    public class ProviderUpdateRequestDTO
    {
        [Required]
        public Guid? ProviderId { get; set; }
        [Required]
        public string Identification { get; set; }
        [Required]
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

    public static class ProviderModelConverter
    {
        public static IEnumerable<ProviderViewModelDTO> ToDTO(this IEnumerable<Provider> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static ProviderViewModelDTO ToDTO(this Provider model)
        {
            return new ProviderViewModelDTO()
            {
                Active = model.Active,
                Address = model.Address,
                BusinessName = model.BusinessName,
                Email = model.Email,
                Identification = model.Identification,
                PhoneNumber1 = model.PhoneNumber1,
                PhoneNumber2 = model.PhoneNumber2,
                ProviderId = model.ProviderId,
                Tradename = model.Tradename,
                Website = model.Website,
                FacebookAddress = model.FacebookAddress,
                InstagramAddress = model.InstagramAddress,
                TwitterAddress = model.TwitterAddress,
                YoutubeAddress = model.YoutubeAddress,
                CategoryId = model.CategoryId
            };
        }
    }
}
