using System;
using System.Collections.Generic;

namespace Crecer.Fair.Web.Models
{
    public class ChatComponentViewModel
    {        
        public ChatMode ChatMode {get; set;}
        public Guid? ProviderId { get; set; }
        public string ProviderEmail { get; set; }
        public string ChatServerUrl { get; set; }
        public List<string> AllowedEmailConversations { get; set; }         
    }

    public enum ChatMode
    {
        // SpecificConversation,
        OnlyAllowedConversations,
        All
    }
}
