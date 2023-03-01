using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Ecubytes.Data;
using Ecubytes.Data.Common;

namespace Crecer.Fair.Data.Repositories
{
    public interface IStandVisitedRepository : IRepository<StandVisited>
    {
        Task<long> VisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId);        

        Task<List<ProviderVisitCount>> GetProviderVisitCountAsync(Guid fairId, DateTime dateFrom, DateTime dateTo, Guid? providerId);
    }
}
