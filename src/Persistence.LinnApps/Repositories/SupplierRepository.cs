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
    using Linn.Stores2.Domain.LinnApps.Returns;

    using Microsoft.EntityFrameworkCore;

    public class SupplierRepository : EntityFrameworkQueryRepository<Supplier>
    {
        private readonly ServiceDbContext serviceDbContext;

        public SupplierRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Suppliers)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<Supplier> FindByAsync(Expression<Func<Supplier, bool>> expression)
        {
            return await this.serviceDbContext.Suppliers
                .Include(r => r.Country)
                .Where(expression)
                .SingleOrDefaultAsync();
        }
    }
}
