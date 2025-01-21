namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using Microsoft.EntityFrameworkCore;

    public class StoresBudgetRepository : EntityFrameworkRepository<StoresBudget, int>
    {
        private readonly ServiceDbContext serviceDbContext;
        
        public StoresBudgetRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.StoresBudgets)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<StoresBudget> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.StoresBudgets
                             .Include(x => x.Part)
                             .Include(x => x.StoresBudgetPostings)
                             .Include(x => x.Requisition)
                             .FirstOrDefaultAsync(storesBudget => storesBudget.BudgetId == key);
            return result;
        }
    }
}
