using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    public partial class ProviderController
    {
        [HttpGet]
        [Route("{providerId}/resources")]
        [ProducesResponseType(typeof(IEnumerable<ProviderResourceViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListResources(
            [FromRoute] Guid providerId, string resourceTypeId = null)
        {
            var queryFilter = QueryRequest.Builder();

            if (!string.IsNullOrWhiteSpace(resourceTypeId))
                queryFilter.AddCondition("ResourceTypeId", resourceTypeId);

            var queryResult = await this.repositoryContainer.ProviderResource.GetAsync(queryFilter,
                p => p.ProviderId == providerId,
                includeProperties: "ResourceType");

            return Ok(queryResult.Data.ToDTO());
        }

        [HttpGet]
        [Route("{providerId}/resources/{resourceId}")]
        [ProducesResponseType(typeof(ProviderResourceViewModelDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetResource(
            [FromRoute] Guid providerId, [FromRoute] Guid resourceId)
        {
            var model = await this.repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p =>
                p.ResourceId == resourceId, includeProperties: "ResourceType");

            if (model.ProviderId != providerId)
                return NotFound();

            return Ok(model.ToDTO());
        }

        [HttpPost]
        [Route("{providerId}/resources")]
        [ProducesResponseType(typeof(ProviderResourceUpdateResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> InsertResource(
            [FromRoute] Guid providerId,
            [FromBody] ProviderResourceUpdateRequestDTO requestDTO)
        {
            if (!requestDTO.ResourceId.HasValue)
                requestDTO.ResourceId = Guid.NewGuid();

            // if (!ResourceTypes.GeneralResources.Contains(requestDTO.ResourceTypeId))
            //     return BadRequestModelResult("Tipo de Recurso no admitido");

            var model = await this.repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p => p.ResourceId == requestDTO.ResourceId);
            bool created = false;
            if (model == null)
            {
                model = new Data.ProviderResource();
                model.ProviderId = providerId;
                model.ResourceId = requestDTO.ResourceId.Value;
                model.ResourceTypeId = requestDTO.ResourceTypeId;

                repositoryContainer.ProviderResource.Add(model);
                created = true;
            }
            else
            {
                if (model.ProviderId != providerId)
                    return BadRequestModelResult("El Recurso no pertenece al Proveedor");

                if (model.ProviderId != providerId)
                    return BadRequestModelResult("No se puede alterar el Tipo de Recurso");

                repositoryContainer.ProviderResource.Update(model);
            }

            if (created &&
                (await repositoryContainer.ProviderResource.ExistsAsync(p =>
                    p.ProviderId == model.ProviderId &&
                    p.ResourceTypeId == model.ResourceTypeId &&
                    p.ResourceType.UserCanCreate)))
                return ConflictModelResult(GetLocalizableString("Ya existe otro recurso {0} creado", model.ResourceTypeId));

            if (created && requestDTO.Priority == 0)
            {
                if (await repositoryContainer.ProviderResource.ExistsAsync(p => p.ProviderId == providerId &&
                         p.ResourceTypeId == requestDTO.ResourceTypeId))
                    model.Priority = repositoryContainer.ProviderResource.GetQueryable(p => p.ProviderId == providerId &&
                            p.ResourceTypeId == requestDTO.ResourceTypeId).Max(p => p.Priority) + 1;
                else
                    model.Priority = 1;
            }
            else
            {
                model.Priority = requestDTO.Priority;
            }

            model.Name = requestDTO.Name;
            model.Value = requestDTO.Value;
            model.FileName = requestDTO.FileName;
            model.ThumbnailPath = requestDTO.ThumbnailPath;

            await this.repositoryContainer.SaveChangesAsync();

            var modelReturn = new ProviderResourceUpdateResponseDTO()
            {
                ResourceId = model.ResourceId
            };

            if (created)
                return Created(Url.Action(nameof(GetResource),
                    new { providerId = model.ProviderId, resourceId = model.ResourceId }), modelReturn);
            else
                return NoContent();
        }

        [HttpPut]
        [Route("{providerId}/resources/priorities")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateResourcesPriority(
            [FromRoute] Guid providerId,
            [FromQuery, Required] string resourceTypeId,
            [FromBody] IEnumerable<ProviderResourceUpdatePriorityDTO> resources)
        {
            var models = await this.repositoryContainer.ProviderResource.GetAsync(p =>
                p.ProviderId == providerId && p.ResourceTypeId == resourceTypeId);

            foreach (var r in resources)
            {
                var m = models.FirstOrDefault(p => p.ResourceId == r.ResourceId);
                m.Priority = r.Priority;

                repositoryContainer.ProviderResource.Update(m);
            }

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        // [HttpPatch]
        // [Route("{providerId}/resources/{resourceId}")]
        // [ProducesResponseType((int)HttpStatusCode.NoContent)]
        // public async Task<IActionResult> UpdateResource(
        //     [FromRoute] Guid providerId,
        //     [FromRoute] Guid resourceId,
        //     [FromBody] ProviderResourceUpdateRequestDTO resource)
        // {
        //     var model = await this.repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p =>
        //          p.ResourceId == resourceId);

        //     if(model == null || model.ProviderId != providerId)
        //         return NotFound();        

        //     this.repositoryContainer.ProviderResource.Update(model);

        //     await this.repositoryContainer.SaveChangesAsync();

        //     return NoContent();
        // }

        [HttpPatch]
        [Route("{providerId}/resources/{resourceId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateResourcePatch(
            [FromRoute] Guid providerId,
            [FromRoute] Guid resourceId,
            [FromBody] JsonPatchDocument<ProviderResourceUpdateRequestDTO> patchDocument)
        {
            var model = await this.repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p =>
                p.ResourceId == resourceId);

            if (model == null || model.ProviderId != providerId)
                return NotFound();

            var modelApi = mapper.Map<ProviderResourceUpdateRequestDTO>(model);

            patchDocument.ApplyTo(modelApi);

            mapper.Map(modelApi, model);

            this.repositoryContainer.ProviderResource.Update(model);

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("{providerId}/resources/{resourceId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> DeleteResource(
            [FromRoute] Guid providerId, [FromRoute] Guid resourceId)
        {
            var model = await this.repositoryContainer.ProviderResource.GetFirstOrDefaultAsync(p =>
                p.ResourceId == resourceId);

            if (model.ProviderId != providerId)
                return BadRequestModelResult("El Recurso no pertenece al Proveedor");

            repositoryContainer.ProviderResource.Remove(model);

            await this.repositoryContainer.SaveChangesAsync();

            return NoContent();
        }

    }
}
