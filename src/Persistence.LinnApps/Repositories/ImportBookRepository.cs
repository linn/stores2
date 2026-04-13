namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

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
                       .Include(r => r.OrderDetails).ThenInclude(o => o.Rsn).ThenInclude(r => r.AllegedReason)
                       .Include(i => i.InvoiceDetails)
                       .Include(i => i.PostEntries)
                       .Include(i => i.Supplier).ThenInclude(s => s.Country)
                       .Include(i => i.Carrier)
                       .Include(i => i.CreatedBy)
                       .Include(i => i.ContactEmployee)
                       .Include(i => i.Currency)
                       .Include(i => i.BaseCurrency)
                       .Include(i => i.ExchangeCurrency)
                       .Include(i => i.Period)
                       .FirstOrDefaultAsync(r => r.Id == key);
        }

        public override IQueryable<ImportBook> FilterBy(
            Expression<Func<ImportBook, bool>> filterExpression)
        {
            return this.serviceDbContext.ImportBooks.Where(filterExpression)
                .Include(i => i.InvoiceDetails)
                .Include(r => r.OrderDetails).ThenInclude(o => o.ImportBookCpcNumber)
                .Include(i => i.Supplier).ThenInclude(s => s.Country)
                .Include(i => i.CreatedBy);
        }
    }
}
