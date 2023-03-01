using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Admin.Web.Services
{
    public interface IFairService
    {
        Task<FairViewModelDTO> GetMainAsync();
        Task UpdateAsync(Guid fairId, FairUpdateRequestDTO requestDTO);
        Task<IEnumerable<FairStandViewModelDTO>> GetStandsAsync(Guid fairId);
        Task UpdateStandProviderAsync(Guid fairId, Guid standId, Guid? providerId);
        Task<int> GetStandsCountAsync(Guid fairId);
        Task<long> GetStandsVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId);
        Task<IEnumerable<ProviderVisitCountViewModelDTO>> GetProviderVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId);
        Task<QueryResult<PartnertStandVisitedResumeDTO>> GetPartnertStandVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, QueryRequest queryRequest);
        Task<QueryResult<FairUsersDTO>> GetFairsUsersAsync(QueryRequest request);
    }
}
