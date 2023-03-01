using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Web.Services
{
    public interface IFairService
    {
        Task<FairViewModelDTO> GetMainAsync();        
        Task<IEnumerable<FairStandViewModelDTO>> GetStandsAsync(Guid fairId);      
        Task<FairStandViewModelDTO> GetStandByIdAsync(Guid fairId, Guid standId);
        Task RegisterStandVisit(Guid fairId, Guid standId);
        Task<IEnumerable<ProviderUserViewModelDTO>> GetStandsUsersAsync(Guid fairId);        
    }
}
