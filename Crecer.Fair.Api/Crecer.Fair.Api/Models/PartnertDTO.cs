using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Crecer.Fair.Data;

namespace Crecer.Fair.Api.Models
{
    public class PartnertViewModelDTO
    {
        public string Identification { get; set; }
        public Guid PartnertId { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool HasUser { get; set; }
    }

    public class PartnertRegisterRequestDTO
    {
        [Required]
        public string Identification { get; set; }
        [Required]
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public bool HasUser {get; set; }
    }

    public class PartnertImportRequestDTO
    {
        public string Identification { get; set; }        
        public string Names { get; set; }
        public string LastNames { get; set; }
    }

    public class PartnertRegisterResponseDTO
    {
        public Guid PartnertId { get; set; }
    }

    public static class PartnertModelConverter
    {
        public static List<PartnertViewModelDTO> ToDTO(this IEnumerable<Partnert> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static PartnertViewModelDTO ToDTO(this Partnert model)
        {
            return new PartnertViewModelDTO()
            {
                Identification = model.Identification,
                Address = model.Address,
                Email = model.Email,
                LastNames = model.LastNames,
                Names = model.Names,
                PartnertId = model.PartnertId,
                PhoneNumber = model.PhoneNumber,
                HasUser = model.HasUser
            };
        }
    }
}
