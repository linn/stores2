namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Persistence.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class StockLocatorRepository : EntityFrameworkRepository<StockLocator, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StockLocatorRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StockLocators)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<StockLocator> FilterBy(Expression<Func<StockLocator, bool>> expression)
        {
            return this.serviceDbContext.StockLocators.Where(expression)
                .Include(l => l.StorageLocation)
                .Include(l => l.Part);
        }

        public override async Task<IList<StockLocator>> FilterByAsync(
            Expression<Func<StockLocator, bool>> filterByExpression,
            Expression<Func<StockLocator, object>> orderByExpression = null)
        {
            var query = this.serviceDbContext.StockLocators.Where(filterByExpression);

            var results = await query
                              .Include(l => l.StorageLocation)
                              .Include(l => l.Part)
                              .ToListAsync();

            return results;
        }
    }
}
