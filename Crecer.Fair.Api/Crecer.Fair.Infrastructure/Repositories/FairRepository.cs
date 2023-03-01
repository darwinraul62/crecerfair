using System;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class FairRepository : Repository<FairModel>, IFairRepository
    {
        public FairRepository(FairDbContext context) : base(context)
        {
        }
    }
}
