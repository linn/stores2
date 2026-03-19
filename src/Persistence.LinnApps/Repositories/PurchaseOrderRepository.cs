namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;

    using Microsoft.EntityFrameworkCore;

    public class PurchaseOrderRepository : EntityFrameworkRepository<PurchaseOrder, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public PurchaseOrderRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.PurchaseOrders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<PurchaseOrder> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.PurchaseOrders
                .Include(x => x.Details).ThenInclude(d => d.SalesArticle)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.OrderNumber == key);
            return result;
        }
    }
}
