using System;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class StandModelResourceRepository : Repository<StandModelResource>, IStandModelResourceRepository
    {
        public StandModelResourceRepository(DbContext context) : base(context)
        {
        }
    }
}
