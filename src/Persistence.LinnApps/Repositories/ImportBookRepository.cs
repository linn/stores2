namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Imports;

    using Microsoft.EntityFrameworkCore;

    public class ImportBookRepository : EntityFrameworkRepository<ImportBook, int>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ImportBookRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.ImportBooks)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<ImportBook> FindByIdAsync(int key)
        {
            return await this.serviceDbContext
                       .ImportBooks
                       .Include(r => r.OrderDetails).ThenInclude(o => o.ImportBookCpcNumber)
                       .Include(i => i.InvoiceDetails)
                       .Include(i => i.PostEntries)
                       .Include(i => i.Supplier).ThenInclude(s => s.Country)
                       .Include(i => i.CreatedBy)
                       .FirstOrDefaultAsync(r => r.Id == key);
        }
    }
}
