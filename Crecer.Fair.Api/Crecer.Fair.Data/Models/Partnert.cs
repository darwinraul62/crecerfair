using System;

namespace Crecer.Fair.Data
{
    public class Partnert
    {
        public Guid PartnertId { get; set; }
        public string Identification { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }  
        public bool HasUser { get; set; }        
    }
}