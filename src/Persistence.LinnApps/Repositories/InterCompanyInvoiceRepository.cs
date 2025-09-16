namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stock;

    using Microsoft.EntityFrameworkCore;

    public class InterCompanyInvoiceRepository : EntityFrameworkRepository<InterCompanyInvoice, InterCompanyInvoiceKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public InterCompanyInvoiceRepository(ServiceDbContext serviceDbContext) : base(serviceDbContext.InterCompanyInvoices)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override IQueryable<InterCompanyInvoice> FindAll()
        {
            return this.serviceDbContext.InterCompanyInvoices
                .Include(i => i.Details);
        }

        public override async Task<InterCompanyInvoice> FindByIdAsync(InterCompanyInvoiceKey key)
        {
            var result = await this.serviceDbContext.InterCompanyInvoices
                             .Include(a => a.Details).ThenInclude(d => d.SalesArticle)
                             .Include(a => a.Details).ThenInclude(d => d.Country)
                             .Include(a => a.Details).ThenInclude(d => d.Tariff)
                             .FirstOrDefaultAsync(invoice => invoice.DocumentType == key.DocumentType && invoice.DocumentNumber == key.DocumentNumber);
            return result;
        }

        public override IQueryable<InterCompanyInvoice> FilterBy(
            Expression<Func<InterCompanyInvoice, bool>> filterExpression)
        {
            return this.serviceDbContext.InterCompanyInvoices.Where(filterExpression)
                .Include(a => a.Details).ThenInclude(d => d.SalesArticle)
                .Include(a => a.Details).ThenInclude(d => d.Country)
                .Include(a => a.Details).ThenInclude(d => d.Tariff);
        }
    }
}
