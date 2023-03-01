using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Ecubytes.Data;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    public partial class ProviderController
    {
        [HttpGet]
        [Route("{providerId}/users")]
        [ProducesResponseType(typeof(IEnumerable<ProviderUserViewModelDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUsers(
            [FromRoute] Guid providerId)
        {
            var data = await this.repositoryContainer.ProviderUser.GetAsync(p => p.ProviderId == providerId);
            return Ok(data.ToDTO());
        }

        [HttpPost]
        [Route("{providerId}/users")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> AddUser(
            [FromRoute] Guid providerId,
            [FromBody, Required] ProviderAddUserRequestDTO requestDTO)
        {
            if (await this.repositoryContainer.ProviderUser.ExistsAsync(p =>
                 p.ProviderId != providerId && p.UserId == requestDTO.UserId))
                return ConflictModelResult("Usuario ya pertenece a otro proveedor");

            if (!(await this.repositoryContainer.ProviderUser.ExistsAsync(p =>
                 p.ProviderId == providerId && p.UserId == requestDTO.UserId)))
            {
                this.repositoryContainer.ProviderUser.Add(new Data.ProviderUser()
                {
                    ProviderId = providerId,
                    UserId = requestDTO.UserId
                });

                await this.repositoryContainer.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("{providerId}/users/{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ModelResult), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> RemoveUser(
            [FromRoute] Guid providerId,
            [FromRoute] Guid userId)
        {
            if (await this.repositoryContainer.ProviderUser.ExistsAsync(p =>
                 p.ProviderId != providerId && p.UserId == userId))
                return ConflictModelResult("Usuario pertenece a otro proveedor");

            var model = await this.repositoryContainer.ProviderUser.GetFirstOrDefaultAsync(p =>
                p.ProviderId == providerId &&
                p.UserId == userId);

            if (model != null)
            {
                this.repositoryContainer.ProviderUser.Remove(model);
                await this.repositoryContainer.SaveChangesAsync();
            }

            return NoContent();
        }
    }
}