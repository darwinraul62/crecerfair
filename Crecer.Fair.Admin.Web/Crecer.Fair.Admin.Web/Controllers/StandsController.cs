using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class StandsController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IFairService fairService;
        private IProviderService providerService;
        private FairViewModelDTO fairModel;
        public StandsController(IFairService fairService, IProviderService providerService)
        {
            this.fairService = fairService;
            this.providerService = providerService;
            InitFair();
        }        

        protected FairViewModelDTO Fair
        {
            get
            {
                return fairModel;
            }
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.StandPositionEditor)]
        public async Task<IActionResult> Index()
        {
            StandIndexViewModel model = new StandIndexViewModel()
            {
                FairId = Fair.FairId
            };

            var providers = await providerService.GetAsync(QueryRequest.Builder().
                AddCondition("Active", true));

            model.ProviderSelectList = new SelectList(providers.Data, "ProviderId", "Tradename");

            model.Stands.AddRange(await GetStandsAsync(Fair.FairId));

            return View(model);
        }        

        [HttpGet]
        [Authorize(Roles = SecurityRoles.StandPositionEditor)]
        public async Task<IActionResult> PartialStandMapItems(Guid fairId)
        {
            var model = await GetStandsAsync(fairId);

            return PartialView("_StandMapItems", model);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.StandPositionEditor)]
        public async Task<IActionResult> UpdateProvider(StandProviderViewModel viewModel)
        {
            try
            {
                if(viewModel.ProviderId == Guid.Empty)
                    viewModel.ProviderId = null;
                    
                await fairService.UpdateStandProviderAsync(viewModel.FairId, viewModel.StandId, viewModel.ProviderId);

                return OkModelResult();
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
                return OkModelResult();
            }
        }

        #region methods
        
        private void InitFair()
        {
            fairModel = this.fairService.GetMainAsync().Result;
        }

        private async Task<List<StandCoordViewModel>> GetStandsAsync(Guid fairId)
        {
            var stands = await this.fairService.GetStandsAsync(fairId);

            return stands.Select(p => new StandCoordViewModel()
            {
                Height = p.Height,
                LogoUrl = p.LogoUrl,
                ProviderId = p.ProviderId,
                ProviderName = p.ProviderName,
                StandId = p.StandId,
                Width = p.Width,
                X = p.X,
                Y = p.Y,
                IsEditable = p.IsEditable,
                PositionRef = p.PositionRef                              
            }).ToList();
        }

        #endregion

    }
}
