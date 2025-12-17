namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;

    using Microsoft.EntityFrameworkCore;

    public class ExportBookRepository : EntityFrameworkRepository<ExportBook, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ExportBookRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.Expbooks)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<ExportBook> FindByIdAsync(int key)
        {
            return await this.serviceDbContext.Expbooks
                       .Include((ExportBook e) => e.Address)
                       .FirstOrDefaultAsync((ExportBook e) => e.Id == key);
        }
    }
}
