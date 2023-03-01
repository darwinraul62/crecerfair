using System;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class FairStandRepository : Repository<FairStand>, IFairStandRepository
    {
        public FairStandRepository(FairDbContext context) : base(context)
        {
        }

        public Task<int> GetCountAsync(Guid fairId)
        {
            return dbSet.Where(p=>p.FairId == fairId && p.ProviderId.HasValue).CountAsync();
        }
    }
}
