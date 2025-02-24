namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class StorageLocationRepository : EntityFrameworkRepository<StorageLocation, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public StorageLocationRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.StorageLocations)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<IList<StorageLocation>> FindAllAsync()
        {
            return await this.serviceDbContext.StorageLocations
                       .Where(l => l.DateInvalid == null)
                       .OrderBy(l => l.LocationCode)
                       .ToListAsync();
        }

        public override async Task<IList<StorageLocation>> FilterByAsync(
            Expression<Func<StorageLocation, bool>> filterExpression,
            Expression<Func<StorageLocation, object>> orderByExpression = null)
        {
            return await this.serviceDbContext.StorageLocations
                       .Where(filterExpression)
                       .OrderBy(l => l.LocationCode)
                       .ToListAsync();
        }

        public override async Task<StorageLocation> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.StorageLocations
                .Include(loc => loc.StorageArea)
                .Include(loc => loc.AuditedBy)
                .Include(loc => loc.AuditedByDepartment)
                .FirstOrDefaultAsync(loc => loc.LocationId == key);
            return result;
        }
    }
}
