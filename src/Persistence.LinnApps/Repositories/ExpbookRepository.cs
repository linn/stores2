
    namespace Linn.Stores2.Persistence.LinnApps.Repositories
    {
        using System.Threading.Tasks;

        using Linn.Common.Persistence.EntityFramework;
        using Linn.Stores2.Domain.LinnApps;
        using Linn.Stores2.Persistence.LinnApps;

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
                    .Include(e => e.Address)
                    .FirstOrDefaultAsync(e => e.Id == key);
            }
        }
    }