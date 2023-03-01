using System;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Data.Repositories
{
    public interface IPartnertStandVisitedResumeRepository : IRepository<PartnertStandVisitedResume>
    {
        Task<QueryResult<PartnertStandVisitedResume>> GetResumeVisitCountAsync( Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            QueryRequest queryRequest);        
    }
}
