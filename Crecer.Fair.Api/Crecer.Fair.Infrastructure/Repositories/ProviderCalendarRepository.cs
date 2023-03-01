using System;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class ProviderCalendarRepository : Repository<ProviderCalendar>, IProviderCalendarRepository
    {
        public ProviderCalendarRepository(FairDbContext context) : base(context)
        {
        }        
    }
}
