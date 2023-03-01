using System;

namespace Crecer.Fair.Web.Services.Models
{
    public class PartnertRegisterRequestDTO
    {
        public string Identification { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool HasUser { get; set; }
    }
   
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

    public class PartnertRegisterResponseDTO
    {
        public Guid PartnertId { get; set; }
    }
}
