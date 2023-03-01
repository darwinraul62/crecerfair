using System;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class StandModelRepository : Repository<StandModel>, IStandModelRepository
    {
        public StandModelRepository(FairDbContext context) : base(context)
        {
        }
    }
}
