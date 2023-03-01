using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Data;
using Crecer.Fair.Data.Models;
using Crecer.Fair.Data.Repositories;
using Ecubytes.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Crecer.Fair.Infrastructure.Repositories
{
    public class PartnertRepository : Repository<Partnert>, IPartnertRepository
    {
        public PartnertRepository(FairDbContext context) 
            : base(context)
        {
        }
        
        public void AddImportList(List<PartnertImportTemp> data)
        {
            var c = (FairDbContext)Context;            
            c.AddRange(data);
        }

        public Task ExecuteImportAsync()
        {
            return Context.Database.ExecuteSqlRawAsync("call createpartnertfromimport()");
        }
    }
}
