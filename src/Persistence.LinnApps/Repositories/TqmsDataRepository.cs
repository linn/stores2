namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Microsoft.EntityFrameworkCore;

    public class TqmsDataRepository : EntityFrameworkQueryRepository<TqmsData>
    {
        private readonly ServiceDbContext serviceDbContext;

        public TqmsDataRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.TqmsData)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<TqmsData> FilterBy(Expression<Func<TqmsData, bool>> filterExpression)
        {
            return this.serviceDbContext.TqmsData
                .Where(filterExpression).AsNoTracking()
                .Include(x => x.Part).ThenInclude(p => p.Bom);
        }
    }
}
