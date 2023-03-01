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
    public class ProviderRepository : Repository<Provider>, IProviderRepository
    {
        public ProviderRepository(FairDbContext context) : base(context)
        {
        }

        // public async Task<QueryResult<IEnumerable<Provider>>> GetAsync(QueryRequest request)
        // {
        //     QueryResult<IEnumerable<Provider>> response = new QueryResult<IEnumerable<Provider>>();

        //     int page = request.Page;
        //     int pageSize = request.PageSize;

        //     var query = this.dbSet.AsQueryable();

        //     if (request.Fields != null && request.Fields.Any())
        //     {
        //         foreach (var field in request.Fields.Where(p => p.SortOrientation.HasValue))
        //         {
        //             string propertyName = this.dbSet.EntityType.ClrType.GetPropertyCaseInsensitive(field.Name).Name;

        //             if (field.SortOrientation == SortOrientation.Ascendent)
        //                 query = query.OrderBy(p => EF.Property<object>(p, propertyName));
        //             else
        //                 query = query.OrderByDescending(p => EF.Property<object>(p, propertyName));
        //         }
        //     }
        //     else
        //     {
        //         query = query.OrderBy(p => p.Tradename);
        //     }

        //     var result = await query.
        //         Skip(pageSize * (page - 1)).Take(pageSize).ToListAsync();

        //     response.Page = request.Page;
        //     response.Data = result;

        //     response.TotalCount = await this.dbSet.CountAsync();

        //     return response;
        // }
    }
}
