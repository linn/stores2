namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class StoresPalletRepository : EntityFrameworkRepository<StoresPallet, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StoresPalletRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StoresPallets)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<StoresPallet> FindAll()
        {
            return this.serviceDbContext.StoresPallets
                .Include(a => a.DefaultStockPool)
                .Include(a => a.LocationType)
                .Include(l => l.StorageLocation);
        }

        public override async Task<StoresPallet> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.StoresPallets
                             .Include(a => a.DefaultStockPool)
                             .Include(a => a.LocationType)
                             .Include(l => l.StorageLocation)
                             .FirstOrDefaultAsync(pallet => pallet.PalletNumber == key);
            return result;
        }

        public override IQueryable<StoresPallet> FilterBy(Expression<Func<StoresPallet, bool>> filterExpression)
        {
            return this.serviceDbContext.StoresPallets.Where(filterExpression)
                .Include(a => a.DefaultStockPool)
                .Include(a => a.LocationType)
                .Include(l => l.StorageLocation);
        }
    }
}
