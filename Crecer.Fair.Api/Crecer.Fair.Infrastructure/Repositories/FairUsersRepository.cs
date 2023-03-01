using System;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class FairUserRepository : Repository<FairUsers>, IFairUserRepository
    {
        public FairUserRepository(FairDbContext context) : base(context)
        {
        }
    }
}
