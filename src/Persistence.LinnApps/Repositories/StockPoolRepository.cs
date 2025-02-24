namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

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

        public override async Task<IList<StockPool>> FindAllAsync()
        {
            return await this.serviceDbContext.StockPools
                .Include(a => a.AccountingCompany)
                .Include(l => l.StorageLocation)
                .ToListAsync();
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
