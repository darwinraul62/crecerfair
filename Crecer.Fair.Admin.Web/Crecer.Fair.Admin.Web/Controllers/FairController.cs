using System;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class FairController : FairBaseController
    {
        public FairController(IFairService fairService)
            : base(fairService)
        {
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.FairGeneralEditor)]
        public IActionResult Edit()
        {
            FairEditViewModel viewModel = new FairEditViewModel()
            {
                Title = Fair.Title,
                WelcomeText = Fair.WelcomeText,
                HostProviderId = Fair.HostProviderId,
                HostProviderName = Fair.HostProviderName
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize(Roles = SecurityRoles.FairGeneralEditor)]
        public async Task<IActionResult> Edit(FairEditViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await fairService.UpdateAsync(Fair.FairId, new Services.Models.FairUpdateRequestDTO()
                    {
                        Title = viewModel.Title,
                        WelcomeText = viewModel.WelcomeText,
                        HostProviderId = Fair.HostProviderId                                                
                    });

                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return View(viewModel);
        }
        
    }
}
