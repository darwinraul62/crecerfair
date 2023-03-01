using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Infrastructure.IntegrationEvents;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Ecubytes.Integration.Event;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [ApiController]
    [Route("api/partnerts")]
    public class PartnertController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IFairRepositoryContainer repositoryContainer;        
        public PartnertController(IFairRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;            
        }

        [HttpGet]        
        [ProducesResponseType(typeof(QueryResult<PartnertViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] QueryRequest queryRequest)
        {
            var response = await this.repositoryContainer.Partnert.GetAsync(queryRequest);
        
            QueryResult<PartnertViewModelDTO> result = QueryResult.Convert<PartnertViewModelDTO>(response, response.Data.ToDTO());

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(PartnertViewModelDTO), (int)HttpStatusCode.OK)]        
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var partnert = await this.repositoryContainer.Partnert.GetFirstOrDefaultAsync(p => p.PartnertId == id);
          
            return Ok(partnert.ToDTO());
        }

        [HttpGet]
        [Route("byidentification")]        
        [ProducesResponseType(typeof(PartnertViewModelDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdentification([FromQuery] string identification)
        {
            var partnert = await this.repositoryContainer.Partnert.GetFirstOrDefaultAsync(p => p.Identification == identification);
            if(partnert!=null)                
                return Ok(partnert.ToDTO());
            else
                return NotFound();
        }

        [HttpGet]
        [Route("allowregister/{identification}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> IsAuthorizedIdentification([FromRoute] string identification)
        {
            bool exists = await this.repositoryContainer.Partnert.ExistsAsync(p => p.Identification == identification);
            return Ok(exists);
        }

        [HttpPost]        
        [ProducesResponseType(typeof(PartnertRegisterResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Insert([FromBody] PartnertRegisterRequestDTO requestModel)
        {
            if (await (this.repositoryContainer.Partnert.ExistsAsync(p =>
                 p.Email == requestModel.Email)))
                return ConflictModelResult("The Email already exists");

            if (await (this.repositoryContainer.Partnert.ExistsAsync(p =>
                 p.Identification == requestModel.Identification)))
                return ConflictModelResult("The Identification already exists");

            //Verify Exists as partnert
            Partnert model = new Partnert();
            model.PartnertId = Guid.NewGuid();

            model.Address = requestModel.Address;
            model.Email = requestModel.Email;
            model.Identification = requestModel.Identification;
            model.LastNames = requestModel.LastNames;
            model.Names = requestModel.Names;
            model.PhoneNumber = model.PhoneNumber;

            await repositoryContainer.ResilientTransactionAsync(async () =>
            {
                this.repositoryContainer.Partnert.Add(model);
                await this.repositoryContainer.SaveChangesAsync();
                //await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
            });

            PartnertRegisterResponseDTO response = new PartnertRegisterResponseDTO()
            {
                PartnertId = model.PartnertId
            };

            //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);
            return CreatedAtAction(nameof(GetById), new { id = model.PartnertId }, response);
        }

        [HttpPut] 
        [Route("{partnertId}")]       
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update([FromRoute ]Guid partnertId, [FromBody] PartnertRegisterRequestDTO requestModel)
        {
            if (await (this.repositoryContainer.Partnert.ExistsAsync(p =>
                 p.PartnertId != partnertId &&
                 p.Email == requestModel.Email)))
                return ConflictModelResult("The Email already exists");

            if (await (this.repositoryContainer.Partnert.ExistsAsync(p =>
                 p.PartnertId != partnertId &&
                 p.Identification == requestModel.Identification)))
                return ConflictModelResult("The Identification already exists");

            //Verify Exists as partnert
            Partnert model = await this.repositoryContainer.Partnert.GetFirstOrDefaultAsync(p=>p.PartnertId == partnertId);
            
            model.Address = requestModel.Address;
            model.Email = requestModel.Email;
            model.Identification = requestModel.Identification;
            model.LastNames = requestModel.LastNames;
            model.Names = requestModel.Names;
            model.PhoneNumber = requestModel.PhoneNumber;
            model.HasUser = requestModel.HasUser;

            await repositoryContainer.ResilientTransactionAsync(async () =>
            {
                this.repositoryContainer.Partnert.Update(model);
                await this.repositoryContainer.SaveChangesAsync();
                //await this.integrationEventService.SaveEventAsync(applicationUpdateEvent);
            });
           
            //await this.integrationEventService.PublishThroughEventBusAsync(applicationUpdateEvent);
            return NoContent();
        }

        [HttpPost]
        [Route("import")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]        
        public async Task<IActionResult> Import([FromBody] List<PartnertImportRequestDTO> requestModel)
        {            
            List<PartnertImportTemp> dataImport = requestModel.Select(p=> new PartnertImportTemp()
            {
                Id = Guid.NewGuid(),
                Identification = p.Identification,
                LastNames = p.LastNames,
                Names = p.Names 
            }).ToList();

            await repositoryContainer.ResilientTransactionAsync( async ()=>
            {
                repositoryContainer.Partnert.AddImportList(dataImport);
                await repositoryContainer.SaveChangesAsync();
                await repositoryContainer.Partnert.ExecuteImportAsync();                
            });

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromQuery, Required] Guid partnertId)
        {
            Partnert model = await this.repositoryContainer.Partnert.GetFirstOrDefaultAsync(p => p.PartnertId == partnertId);

            if (model == null)
                return this.NotFoundModelResult("El id de socio no existe");

            repositoryContainer.Partnert.Remove(model);
            await repositoryContainer.SaveChangesAsync();

            return NoContent();
        }
    }
}