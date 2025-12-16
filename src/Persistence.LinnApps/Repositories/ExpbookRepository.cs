namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class ExpbookRepository : EntityFrameworkRepository<Expbook, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ExpbookRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Expbooks)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<Expbook> FindByIdAsync(int key)
        {
            return await this.serviceDbContext.Expbooks
                       .Include((Expbook e) => e.Address)
                       .FirstOrDefaultAsync((Expbook e) => e.Id == key);
        }
    }
}
