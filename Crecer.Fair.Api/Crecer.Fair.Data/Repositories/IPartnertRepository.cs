using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crecer.Fair.Data.Models;
using Ecubytes.Data;

namespace Crecer.Fair.Data.Repositories
{
    public interface IPartnertRepository : IRepository<Partnert>
    {
        void AddImportList(List<PartnertImportTemp> data);
        Task ExecuteImportAsync();
    }
}
