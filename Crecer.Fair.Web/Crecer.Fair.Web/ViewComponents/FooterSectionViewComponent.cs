using System;
using System.Threading.Tasks;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Web.ViewComponents
{
    public class FooterSectionViewComponent : ViewComponent
    {
        private readonly IProviderService providerService;
        private readonly IFairService fairService;
        public FooterSectionViewComponent(IFairService fairService, IProviderService providerService)
        {
            this.providerService = providerService;
            this.fairService = fairService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        //int maxPriority, bool isDone)//example params
        {
            var fair = await fairService.GetMainAsync();
            var provider = await providerService.GetAsync(fair.HostProviderId);

            FooterSectionModel model = new FooterSectionModel();
            model.ContactData = new FairContact();
            model.ContactData.Address = provider.Address;
            model.ContactData.Email = provider.Email;
            model.ContactData.FacebookAddress = provider.FacebookAddress;
            model.ContactData.Host = provider.Tradename;
            model.ContactData.InstagramAddress = provider.InstagramAddress;
            model.ContactData.PhoneNumber1 = provider.PhoneNumber1;
            model.ContactData.PhoneNumber2 = provider.PhoneNumber2;
            model.ContactData.TwitterAddress = provider.TwitterAddress;
            model.ContactData.WebSite = provider.Website;
            model.ContactData.YoutubeAddress = provider.YoutubeAddress;

            return View(model);
        }
    }
}
