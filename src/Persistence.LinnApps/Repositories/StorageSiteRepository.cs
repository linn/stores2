namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class StorageSiteRepository : EntityFrameworkRepository<StorageSite, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StorageSiteRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StorageSites)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<StorageSite> FindAll()
        {
            var result = this.serviceDbContext.StorageSites
                .Include(x => x.StorageAreas);
            return result;
        }
    }
}
