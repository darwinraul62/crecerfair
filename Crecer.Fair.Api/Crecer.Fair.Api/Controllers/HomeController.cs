using System;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    public class HomeController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        } 
    }
}
