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
            var result = this.serviceDbContext.PcasStorageTypes
                .Include(a => a.PcasBoard)
                .Include(l => l.StorageType);

            return result;
        }

        public override async Task<PcasStorageType> FindByIdAsync(PcasStorageTypeKey id)
        {
            var result = await this.serviceDbContext.PcasStorageTypes
                             .Include(a => a.PcasBoard)
                             .Include(l => l.StorageType)
                             .FirstOrDefaultAsync(pcasStorageType =>
                                 pcasStorageType.BoardCode == id.BoardCode
                                 && pcasStorageType.StorageTypeCode == id.StorageTypeCode);
            return result;
        }
    }
}
