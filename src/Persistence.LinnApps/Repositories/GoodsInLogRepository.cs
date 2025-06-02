namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;

    using Microsoft.EntityFrameworkCore;

    public class GoodsInLogRepository : EntityFrameworkRepository<GoodsInLogEntry, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public GoodsInLogRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.GoodsInLogEntries)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<GoodsInLogEntry> FilterBy(
            Expression<Func<GoodsInLogEntry, bool>> filterExpression)
        {
            return this.serviceDbContext.GoodsInLogEntries.Where(filterExpression)
                       .Include(e => e.CreatedBy);
        }
    }
}
