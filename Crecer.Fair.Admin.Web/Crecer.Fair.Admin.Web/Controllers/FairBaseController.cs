using System;
using Crecer.Fair.Admin.Web.Services;
using Crecer.Fair.Admin.Web.Services.Models;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class FairBaseController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        protected IFairService fairService;
        protected FairViewModelDTO fairModel;
        public FairBaseController(IFairService fairService)
        {
            this.fairService = fairService;
            InitFair();
        }

        private void InitFair()
        {
            fairModel = this.fairService.GetMainAsync().Result;
        }

        public FairViewModelDTO Fair
        {
            get
            {
                return fairModel;
            }
        }
    }
}
