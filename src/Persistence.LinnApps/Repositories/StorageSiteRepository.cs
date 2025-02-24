namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class StorageSiteRepository : EntityFrameworkRepository<StorageSite, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StorageSiteRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StorageSites)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<IList<StorageSite>> FindAllAsync()
        {
            var result = await this.serviceDbContext.StorageSites
                .Include(x => x.StorageAreas)
                .ToListAsync();
            return result;
        }

        public override async Task<StorageSite> FindByIdAsync(string key)
        {
            var result = await this.serviceDbContext.StorageSites
                .Include(x => x.StorageAreas)
                .FirstOrDefaultAsync(loc => loc.SiteCode == key);
            return result;
        }
    }
}
