namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class GoodsInLogRepository : EntityFrameworkRepository<GoodsInLogEntry, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public GoodsInLogRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.GoodsInLogEntries)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public async Task<IList<GoodsInLogEntry>> FilterByAsync(
            Expression<Func<GoodsInLogEntry, bool>> filterExpression,
            Expression<Func<GoodsInLogEntry, object>> orderByExpression = null)
        {
            return await this.serviceDbContext.GoodsInLogEntries.Where(filterExpression)
                       .Include(e => e.CreatedBy)
                       .ToListAsync();
        }
    }
}
