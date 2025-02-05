namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using Microsoft.EntityFrameworkCore;

    public class StoresFunctionCodeRepository : EntityFrameworkRepository<StoresFunction, string>
    {
        private readonly ServiceDbContext serviceDbContext;
        
        public StoresFunctionCodeRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.StoresFunctionCodes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<StoresFunction> FindAll()
        {
            return this.serviceDbContext.StoresFunctionCodes
                             .Include(x => x.TransactionsTypes).ThenInclude(d => d.TransactionDefinition);
        }
    }
}
