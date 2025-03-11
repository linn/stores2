namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class StockPoolRepository : EntityFrameworkRepository<StockPool, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StockPoolRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StockPools)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<StockPool> FindAll()
        {
            return this.serviceDbContext.StockPools
                .Include(a => a.AccountingCompany)
                .Include(l => l.StorageLocation);
        }

        public override async Task<StockPool> FindByIdAsync(string key)
        {
            var result = await this.serviceDbContext.StockPools
                             .Include(a => a.AccountingCompany)
                             .Include(l => l.StorageLocation)
                             .FirstOrDefaultAsync(stockPool => stockPool.StockPoolCode == key);
            return result;
        }
    }
}
