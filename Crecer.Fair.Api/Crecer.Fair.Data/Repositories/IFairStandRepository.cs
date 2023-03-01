using System;
using Ecubytes.Data;
using Crecer.Fair.Data.Models;
using System.Threading.Tasks;

namespace Crecer.Fair.Data.Repositories
{
    public interface IFairStandRepository : IRepository<FairStand>
    {
        Task<int> GetCountAsync(Guid fairId);
    }
}
