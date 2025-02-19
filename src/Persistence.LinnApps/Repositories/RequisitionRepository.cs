namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using Microsoft.EntityFrameworkCore;

    public class RequisitionRepository : EntityFrameworkRepository<RequisitionHeader, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public RequisitionRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.RequisitionHeaders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<RequisitionHeader> FindByIdAsync(int key)
        {
            return await this.serviceDbContext
                .RequisitionHeaders
                .Include(r => r.StoresFunction)
                .Include(r => r.Lines).ThenInclude(l => l.Part)
                .Include(r => r.Lines).ThenInclude(l => l.TransactionDefinition)
                .Include(r => r.Lines).ThenInclude(l => l.NominalAccountPostings).ThenInclude(p => p.NominalAccount)
                .ThenInclude(a => a.Nominal)
                .Include(r => r.Lines).ThenInclude(l => l.NominalAccountPostings).ThenInclude(p => p.NominalAccount)
                .ThenInclude(a => a.Department)
                .Include(r => r.Lines).ThenInclude(l => l.StoresBudgets)
                .Include(r => r.CancelledBy)
                .Include(r => r.CreatedBy)
                .Include(r => r.BookedBy)
                .Include(r => r.Department)
                .Include(r => r.Nominal)
                .Include(r => r.ToLocation)
                .Include(r => r.FromLocation)
                .Include(r => r.Lines).ThenInclude(r => r.Moves).ThenInclude(m => m.Location)
                .Include(r => r.Lines).ThenInclude(r => r.Moves).ThenInclude(m => m.StockLocator)
                .ThenInclude(l => l.StorageLocation)
                .FirstOrDefaultAsync(r => r.ReqNumber == key);
        }

        public override IQueryable<RequisitionHeader> FilterBy(Expression<Func<RequisitionHeader, bool>> expression)
        {
            return this.serviceDbContext.RequisitionHeaders.Where(expression)
                .Include(r => r.Lines).ThenInclude(l => l.TransactionDefinition)
                .Include(r => r.Lines).ThenInclude(l => l.Part);
        }

        public override async Task AddAsync(RequisitionHeader entity)
        {
            await base.AddAsync(entity);

            // always want to save after adding a req, since subsequent stored procedure calls
            // will need to the req to exist in the db
            await this.serviceDbContext.SaveChangesAsync();
        }
    }
}
