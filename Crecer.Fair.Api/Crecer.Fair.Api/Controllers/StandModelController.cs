using System;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [ApiController]
    [Route("api/standmodels")]
    public class StandModelController :  Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IStandModelRepository standModelRepository;
        public StandModelController(
            IStandModelRepository standModelRepository)
        {            
            this.standModelRepository = standModelRepository;
        }

        [HttpGet]
        [Route("{modelcode}")]
        [ProducesResponseType(typeof(StandModelViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById([FromRoute] string modelCode)
        {
            var model = await standModelRepository.GetFirstOrDefaultAsync(p=>p.ModelCode == modelCode, includeProperties: "Resources");
                        
            return Ok(model.ToDTO());
        }
    }
}
