namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using Microsoft.EntityFrameworkCore;

    public class WorkStationRepository : EntityFrameworkRepository<WorkStation, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public WorkStationRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.WorkStations)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<WorkStation> FilterBy(
            Expression<Func<WorkStation, bool>> filterExpression)
        {
            return this.serviceDbContext.WorkStations.Where(filterExpression)
                .Include(c => c.Cit)
                .Include(we => we.WorkStationElements)
                .ThenInclude(e => e.CreatedBy)
                .Include(we => we.WorkStationElements)
                .ThenInclude(p => p.Pallet)
                .Include(we => we.WorkStationElements)
                .ThenInclude(s => s.StorageLocation);
        }

        public override async Task<WorkStation> FindByIdAsync(string key)
        {
            var result = await this.serviceDbContext.WorkStations
                             .Include(c => c.Cit)
                             .Include(we => we.WorkStationElements)
                             .ThenInclude(e => e.CreatedBy)
                             .Include(we => we.WorkStationElements)
                             .ThenInclude(p => p.Pallet)
                             .Include(we => we.WorkStationElements)
                             .ThenInclude(s => s.StorageLocation)
                             .FirstOrDefaultAsync(w => w.WorkStationCode == key);
            return result;
        }
    }
}