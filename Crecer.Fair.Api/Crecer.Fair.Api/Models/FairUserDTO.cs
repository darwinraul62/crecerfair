using System;

namespace Crecer.Fair.Api.Models
{
    public class FairUserDTO
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsProvider { get; set; }
        public bool IsPartnert { get; set; }
    }
}
