namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Pcas;

    using Microsoft.EntityFrameworkCore;

    public class PcasStorageTypeRepository : EntityFrameworkRepository<PcasStorageType, PcasStorageTypeKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PcasStorageTypeRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.PcasStorageTypes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PcasStorageType> FindAll()
        {
            return this.serviceDbContext.PcasStorageTypes
                .Include(a => a.PcasBoard)
                .Include(l => l.StorageType);
        }

        public override async Task<PcasStorageType> FindByIdAsync(PcasStorageTypeKey key)
        {
            var result = await this.serviceDbContext.PcasStorageTypes
                             .Include(a => a.PcasBoard)
                             .Include(l => l.StorageType)
                             .FirstOrDefaultAsync(pcasStorageType => pcasStorageType.Key == key);
            return result;
        }
    }
}
