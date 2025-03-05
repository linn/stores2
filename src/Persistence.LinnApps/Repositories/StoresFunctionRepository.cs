namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using Microsoft.EntityFrameworkCore;

    public class StoresFunctionRepository : EntityFrameworkRepository<StoresFunction, string>
    {
        private readonly ServiceDbContext serviceDbContext;
        
        public StoresFunctionRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.StoresFunctionCodes)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<StoresFunction> FindByIdAsync(string key)
        {
            return await this.serviceDbContext.StoresFunctionCodes
                       .Include(a => a.TransactionsTypes)
                       .ThenInclude(d => d.TransactionDefinition)
                       .ThenInclude(d => d.StoresTransactionPostings)
                       .ThenInclude(p => p.Nominal)
                       .Include(a => a.TransactionsTypes)
                       .ThenInclude(d => d.TransactionDefinition)
                       .ThenInclude(d => d.StoresTransactionStates)
                       .FirstOrDefaultAsync(a => a.FunctionCode == key);
        }

        public override IQueryable<StoresFunction> FindAll()
        {
            return this.serviceDbContext.StoresFunctionCodes
                .Include(x => x.TransactionsTypes)
                .ThenInclude(d => d.TransactionDefinition)
                .ThenInclude(d => d.StoresTransactionPostings)
                .ThenInclude(p => p.Nominal)
                .Include(a => a.TransactionsTypes)
                .ThenInclude(d => d.TransactionDefinition)
                .ThenInclude(d => d.StoresTransactionStates);
        }
    }
}
