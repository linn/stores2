namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

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

        public override IQueryable<StockTransaction> FilterBy(
            Expression<Func<StockTransaction, bool>> filterExpression)
        {
            return this.serviceDbContext.StockTransactions.Where(filterExpression)
                       .Include(p => p.Part)
                       .Include(e => e.BookedBy);
        }
    }
}
