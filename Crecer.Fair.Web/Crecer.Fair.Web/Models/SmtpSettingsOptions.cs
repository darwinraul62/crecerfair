using System;

namespace Crecer.Fair.Web.Models
{
    public class SmtpSettingsOptions
    {
        public const string SectionName = "SmtpSettings";
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
