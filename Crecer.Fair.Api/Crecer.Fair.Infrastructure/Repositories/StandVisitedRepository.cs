using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.Common;
using Ecubytes.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class StandVisitedRepository : Repository<StandVisited>, IStandVisitedRepository
    {
        public StandVisitedRepository(FairDbContext context) : base(context)
        {
        }

        public Task<long> VisitCountAsync(
            Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            Guid? providerId
            )
        {
            var query = dbSet.Where(p => p.FairStand.FairId == fairId
                && p.DateOfVisit >= dateFrom && p.DateOfVisit <= dateTo);

            if (providerId.HasValue)
                query = query.Where(p => p.ProviderId == providerId);

            return query.LongCountAsync();
        }

        public Task<List<ProviderVisitCount>> GetProviderVisitCountAsync(
            Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            Guid? providerId
        )
        {
            FairDbContext db = (FairDbContext)Context;

            var query = db.ProviderVisitCount.Where(p => p.FairId == fairId
                && p.DateOfVisit >= dateFrom 
                && p.DateOfVisit <= dateTo);

            if (providerId.HasValue)
                query = query.Where(p => p.ProviderId == providerId.Value);

            query = query.GroupBy(p => new { p.ProviderId, p.Provider }).Select(p =>
                new ProviderVisitCount()
                {
                    ProviderId = p.Key.ProviderId,
                    Provider = p.Key.Provider,
                    VisitCount = p.Count()
                });

            return query.ToListAsync();
        }
    }
}
