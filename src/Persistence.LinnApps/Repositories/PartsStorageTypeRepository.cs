namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Persistence.LinnApps;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class PartsStorageTypeRepository : EntityFrameworkRepository<PartsStorageType, int>
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

        public override async Task<PartsStorageType> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.PartsStorageTypes
                             .Include(pst => pst.Part)
                             .Include(pst => pst.StorageType)
                             .FirstOrDefaultAsync(pst => pst.BridgeId == key);
            return result;
        }

        public override IQueryable<PartsStorageType> FilterBy(Expression<Func<PartsStorageType, bool>> expression)
        {
            return this.serviceDbContext.PartsStorageTypes.Where(expression)
                .Include(x => x.Part)
                .Include(x => x.StorageType);
        }
    }
}
