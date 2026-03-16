namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class RsnRepository : EntityFrameworkQueryRepository<Rsn>
    {
        private readonly ServiceDbContext serviceDbContext;

        public RsnRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Rsns)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<Rsn> FindByAsync(Expression<Func<Rsn, bool>> expression)
        {
            return await this.serviceDbContext.Rsns
                .Include(r => r.SalesOutlet).ThenInclude(o => o.OutletAddress).ThenInclude(a => a.Country)
                .Include(r => r.SalesArticle).ThenInclude(a => a.Tariff)
                .Include(r => r.ImportBookOrderDetails)
                .Where(expression)
                .SingleOrDefaultAsync();
        }
    }
}
