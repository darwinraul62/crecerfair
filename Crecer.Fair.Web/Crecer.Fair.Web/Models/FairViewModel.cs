using System;
using System.Collections.Generic;
using Crecer.Fair.Web.Services.Models;

namespace Crecer.Fair.Web.Models
{
    public class FairViewModel
    {
        public string Title { get; set; }
        public string WelcomeText { get; set; }
        public Guid HostProviderId { get; set; }
        public string HostProviderName { get; set; }        
    }   

    public static class FairViewModelConverter
    {
        public static FairViewModel ToViewModel(this FairViewModelDTO model)
        {
            return new FairViewModel()
            {
                HostProviderId = model.HostProviderId,
                HostProviderName = model.HostProviderName,
                Title = model.Title,
                WelcomeText = model.WelcomeText
            };
        }
    }
}
