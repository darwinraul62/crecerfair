using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crecer.Fair.Admin.Web.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Crecer.Fair.Admin.Web.Services;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration configuration;
        private readonly IProviderService providerService;
        private readonly IFairService fairService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IProviderService providerService, IFairService fairService)
        {
            _logger = logger;
            this.configuration = configuration;
            this.providerService = providerService;
            this.fairService = fairService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            if (!User.GetFairProviderId().HasValue || this.User.HasAccessAll())
                return View("AdminHomeIndex");
            else
                return View("SimpleIndex");
        }

        [HttpGet]
        public IActionResult GoFair()
        {
            string fairUrl = configuration.GetValue<string>("FairAdminSettings:FairHomeUrl");
            return Redirect(fairUrl);
        }

        [HttpGet]
        public async Task<IActionResult> GoFairStand(Guid providerId)
        {
            var fair = await fairService.GetMainAsync();
            var stands = await fairService.GetStandsAsync(fair.FairId);

            Guid? standId = null;

            if (stands.Any())
            {
                var stand = stands.Where(p => p.ProviderId == providerId);
                if (stand.Any())
                {
                    standId = stand.OrderBy(p => p.PositionRef).First().StandId;
                }
            }

            if (standId.HasValue)
            {
                string fairUrl = string.Format(configuration.GetValue<string>("FairAdminSettings:FairStandUrl"), standId);
                return Redirect(fairUrl);
            }
            else
            {
                return View("NoStand");
            }
        }

        [HttpGet]
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
