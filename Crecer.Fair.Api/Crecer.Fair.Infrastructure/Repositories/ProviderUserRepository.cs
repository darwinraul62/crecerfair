using System;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class ProviderUserRepository : Repository<ProviderUser>, IProviderUserRepository
    {
        public ProviderUserRepository(FairDbContext context) : base(context)
        {
        }
    }
}
