namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Persistence.LinnApps;
    using Microsoft.EntityFrameworkCore;

    public class PartStorageTypeRepository : EntityFrameworkRepository<PartStorageType, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PartStorageTypeRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.PartsStorageTypes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<PartStorageType> FindAll()
        {
            return this.serviceDbContext.PartsStorageTypes
                       .Include(pst => pst.Part)
                       .Include(pst => pst.StorageType);
        }

        public override async Task<PartStorageType> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.PartsStorageTypes
                             .Include(pst => pst.Part)
                             .Include(pst => pst.StorageType)
                             .FirstOrDefaultAsync(pst => pst.BridgeId == key);
            return result;
        }

        public override IQueryable<PartStorageType> FilterBy(Expression<Func<PartStorageType, bool>> expression)
        {
            return this.serviceDbContext.PartsStorageTypes.Where(expression)
                .Include(x => x.Part)
                .Include(x => x.StorageType);
        }
    }
}
