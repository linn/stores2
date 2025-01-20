namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    using Microsoft.EntityFrameworkCore;

    public class RequisitionRepository : EntityFrameworkRepository<RequisitionHeader, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public RequisitionRepository(
            ServiceDbContext serviceDbContext)
            : base(serviceDbContext.RequisitionHeaders)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<RequisitionHeader> FindByIdAsync(int key)
        {
            return await this.serviceDbContext
                       .RequisitionHeaders
                       .Include(r => r.FunctionCode)
                       .Include(r => r.Lines).ThenInclude(l => l.Part)
                       .Include(r => r.Lines).ThenInclude(l => l.TransactionDefinition)
                       .Include(r => r.CancelledBy)
                       .Include(r => r.CreatedBy)
                       .Include(r => r.BookedBy)
                       .FirstOrDefaultAsync(r => r.ReqNumber == key);
        }
    }
}
