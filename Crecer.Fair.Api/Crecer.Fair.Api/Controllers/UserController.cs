using System;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Api.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace Crecer.Fair.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private IFairRepositoryContainer repositoryContainer;
        public UserController(IFairRepositoryContainer repositoryContainer)
        {
            this.repositoryContainer = repositoryContainer;
        }

        [Route("{userId}/providersrelation")]
        [HttpGet]
        public async Task<IActionResult> GetProviders([FromRoute] Guid userId)
        {
            var data = await this.repositoryContainer.ProviderUser.GetAsync(p => p.UserId == userId);
            return Ok(data.Select(p => new ProviderRelationViewModelDTO()
            {
                ProviderId = p.ProviderId
            }));
        }

        [HttpGet]
        public async Task<IActionResult> Get(QueryRequest request)
        {
            var data = await this.repositoryContainer.FairUser.GetAsync(request);

            QueryResult<FairUserDTO> dataResponse = QueryResult.Convert<FairUserDTO>(data,
                data.Data.Select(p => new FairUserDTO()
                {
                    Email = p.Email,
                    FullName = p.FullName,
                    Identification = p.Identification,
                    PhoneNumber = p.PhoneNumber,
                    UserId = p.UserId,
                    IsPartnert = p.IsPartnert,
                    IsProvider = p.IsProvider
                }));

            return Ok(dataResponse);
        }
    }
}
