namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.GoodsIn;

    public class GoodsInLogEntryRepository : EntityFrameworkRepository<GoodsInLogEntry, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public GoodsInLogEntryRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.GoodsInLogEntries)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<GoodsInLogEntry> FindAll()
        {
            return this.serviceDbContext.GoodsInLogEntries.OrderByDescending(g => g.DateCreated);
        }

        public virtual IQueryable<GoodsInLogEntry> FilterBy(Expression<Func<GoodsInLogEntry, bool>> expression)
        {
            return this.serviceDbContext.GoodsInLogEntries.Where(expression).OrderByDescending(g => g.DateCreated);
        }
    }
}
