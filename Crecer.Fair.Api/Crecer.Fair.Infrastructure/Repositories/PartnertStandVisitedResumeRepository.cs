using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data;
using Ecubytes.Data.Common;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class PartnertStandVisitedResumeRepository : Repository<PartnertStandVisitedResume>, IPartnertStandVisitedResumeRepository 
    {
        public PartnertStandVisitedResumeRepository(FairDbContext c) 
            :base(c)
        {
        }

        public async Task<QueryResult<PartnertStandVisitedResume>> GetResumeVisitCountAsync(
            Guid fairId,
            DateTime dateFrom,
            DateTime dateTo,
            QueryRequest queryRequest
        )
        {
            QueryResult<PartnertStandVisitedResume> result = new QueryResult<PartnertStandVisitedResume>();

            FairDbContext db = (FairDbContext)Context;

            var query = db.PartnertStandVisitedResume.Where(p => p.FairId == fairId
                && p.DateOfVisit >= dateFrom 
                && p.DateOfVisit <= dateTo);

            query = query.GroupBy(p => 
                new { 
                    p.ProviderId, 
                    p.Provider, 
                    p.PartnertId,
                    p.Partnert,
                    p.PartnertIdentification,
                    p.PartnertEmail,
                    p.PartnertPhoneNumber
                }).Select(p =>
                new PartnertStandVisitedResume()
                {
                    ProviderId = p.Key.ProviderId,
                    Provider = p.Key.Provider,
                    PartnertId = p.Key.PartnertId,
                    Partnert = p.Key.Partnert,
                    PartnertIdentification = p.Key.PartnertIdentification,
                    PartnertEmail = p.Key.PartnertEmail,
                    PartnertPhoneNumber = p.Key.PartnertPhoneNumber,
                    VisitCount = p.Sum(s=>s.VisitCount)
                });

            result = await query.GetQueryResultAsync(queryRequest);

            return result;
        }

    }
}
