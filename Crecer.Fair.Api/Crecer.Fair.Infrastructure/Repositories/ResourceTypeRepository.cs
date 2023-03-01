using System;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class ResourceTypeRepository : Repository<ResourceType>, IResourceTypeRepository
    {
        public ResourceTypeRepository(FairDbContext context) : base(context)
        {
        }
    }
}
