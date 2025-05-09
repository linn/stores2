namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using Microsoft.EntityFrameworkCore;

    public class WorkstationRepository : EntityFrameworkRepository<Workstation, string>
    {
        private readonly ServiceDbContext serviceDbContext;

        public WorkstationRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.Workstations)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<Workstation> FilterBy(
            Expression<Func<Workstation, bool>> filterExpression)
        {
            return this.serviceDbContext.Workstations.Where(filterExpression)
                .Include(c => c.Cit)
                .Include(we => we.WorkStationElements)
                .ThenInclude(e => e.CreatedBy);
        }

        public override async Task<Workstation> FindByIdAsync(string key)
        {
            var result = await this.serviceDbContext.Workstations
                             .Include(c => c.Cit)
                             .Include(we => we.WorkStationElements)
                             .ThenInclude(e => e.CreatedBy)
                             .FirstOrDefaultAsync(w => w.WorkstationCode == key);
            return result;
        }
    }
}