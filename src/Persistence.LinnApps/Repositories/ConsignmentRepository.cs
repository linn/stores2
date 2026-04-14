namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Consignments;

    using Microsoft.EntityFrameworkCore;

    public class ConsignmentRepository : EntityFrameworkRepository<Consignment, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ConsignmentRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Consignments)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<Consignment> FindByIdAsync(int key)
        {
            var result = await this.serviceDbContext.Consignments
                             .Include(x => x.Items)
                             .ThenInclude(b => b.SalesOrder)
                             .Include(x => x.Address)
                             .Include(x => x.Carrier)
                             .FirstOrDefaultAsync(c => c.ConsignmentId == key);

            return result;
        }
    }
}
