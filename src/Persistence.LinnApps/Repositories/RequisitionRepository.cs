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
                       .FirstOrDefaultAsync(r => r.ReqNumber == key);
        }
    }
}
