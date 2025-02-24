namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Persistence.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class PartsStorageTypeRepository : EntityFrameworkRepository<PartsStorageType, PartsStorageTypeKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartsStorageTypeRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.PartsStorageTypes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<IList<PartsStorageType>> FindAllAsync()
        {
            return await this.serviceDbContext.PartsStorageTypes
                       .Include(pst => pst.Part)
                       .Include(pst => pst.StorageType)
                       .ToListAsync();
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
