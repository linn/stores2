namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Persistence.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class StockPoolRepository : EntityFrameworkRepository<StockPool, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StockPoolRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StockPools)
        {
            this.serviceDbContext = serviceDbContext;
        }
        
        public override IQueryable<StockPool> FilterBy(Expression<Func<StockPool, bool>> expression)
        {
            return this.serviceDbContext.StockPools.Where(expression)
                .Include(l => l.StorageLocation);
        }
    }
}
