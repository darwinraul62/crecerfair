using System;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class ProviderResourceRepository : Repository<ProviderResource>, IProviderResourceRepository
    {
        public ProviderResourceRepository(FairDbContext context) : base(context)
        {
        }
    }
}
