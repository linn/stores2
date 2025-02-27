namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class StockTransactionRepository : EntityFrameworkRepository<StockTransaction, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StockTransactionRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StockTransactions)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<IList<StockTransaction>> FilterByAsync(
            Expression<Func<StockTransaction, bool>> filterExpression,
            Expression<Func<StockTransaction, object>> orderByExpression = null)
        {
            return await this.serviceDbContext.StockTransactions.Where(filterExpression)
                       .Include(p => p.Part)
                       .Include(e => e.BookedBy)
                       .ToListAsync();
        }
    }
}
