using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration configuration;
        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
       
        [HttpGet, ActionName("SignOut")]
        public IActionResult LogOut()
        {
            string fairUrl = $"{configuration.GetValue<string>("FairAdminSettings:FairUrl")}/Account/SignOut";
            return Redirect(fairUrl);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            string fairUrl = $"{configuration.GetValue<string>("FairAdminSettings:FairUrl")}/Account/AccessDenied";
            return Redirect(fairUrl);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            string fairUrl = $"{configuration.GetValue<string>("FairAdminSettings:FairUrl")}/Account/Profile";
            return Redirect(fairUrl);
        }
    }
}
