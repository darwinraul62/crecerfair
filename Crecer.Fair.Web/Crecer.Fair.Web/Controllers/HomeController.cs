using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services;
using Microsoft.Extensions.Configuration;

namespace Crecer.Fair.Web.Controllers
{
    public class HomeController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFairService fairService;
        private readonly IProviderService providerService;        
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, 
            IConfiguration configuration,
            IFairService fairService, 
            IProviderService providerService)
        {
            _logger = logger;
            this.configuration = configuration;
            this.fairService = fairService;
            this.providerService = providerService;
        }

        public async Task<IActionResult> Index()
        {
            string backOfficeUrl = configuration.GetValue<string>("FairSettings:BackOfficeUrl");

            HomeIndexViewModel model = new HomeIndexViewModel()
            {
                Fair = (await fairService.GetMainAsync()).ToViewModel()
            };

            var resources = await providerService.GetResourcesAsync(model.Fair.HostProviderId, ResourceTypes.FairBanner);

            model.BannerItems = resources.Select(p=> new FairBannerUrl()
            {
                Url = $"{backOfficeUrl}/{p.Value}"                
            }).ToList();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
