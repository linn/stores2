namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Persistence.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class PartsStorageTypeRepository : EntityFrameworkRepository<PartsStorageType, PartsStorageTypeKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartsStorageTypeRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.PartsStorageTypes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PartsStorageType> FindAll()
        {
            return this.serviceDbContext.PartsStorageTypes
                .Include(pst => pst.Part)
                .Include(pst => pst.StorageType);
        }

        public override async Task<PartsStorageType> FindByIdAsync(PartsStorageTypeKey key)
        {
            var result = await this.serviceDbContext.PartsStorageTypes
                             .Include(pst => pst.Part)
                             .Include(pst => pst.StorageType)
                             .FirstOrDefaultAsync(pst => pst.PartNumber == key.PartNumber && pst.StorageTypeCode == key.StorageTypeCode);
            return result;
        }
    }
}
