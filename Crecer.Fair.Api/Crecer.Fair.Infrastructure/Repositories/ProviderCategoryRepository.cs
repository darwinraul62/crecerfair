using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.Common;
using Ecubytes.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class ProviderCategoryRepository : Repository<ProviderCategory>, IProviderCategoryRepository
    {
        public ProviderCategoryRepository(FairDbContext context) : base(context)
        {
        }
    }
}
