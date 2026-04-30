namespace Linn.Stores2.Persistence.LinnApps.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Persistence.EntityFramework;
    using Linn.Stores2.Domain.LinnApps.Imports;

    using Microsoft.EntityFrameworkCore;

    public class ImportBookExchangeRateRepository : EntityFrameworkRepository<ImportBookExchangeRate, ImportBookExchangeRateKey>
    {
        private readonly ServiceDbContext serviceDbContext;

        public ImportBookExchangeRateRepository(ServiceDbContext serviceDbContext)
            : base(serviceDbContext.ImportBookExchangeRates)
        {
            this.serviceDbContext = serviceDbContext;
        }

        public override async Task<ImportBookExchangeRate> FindByIdAsync(ImportBookExchangeRateKey key)
        {
            var result = await this.serviceDbContext.ImportBookExchangeRates
                             .Include(pst => pst.LedgerPeriod)
                             .Include(pst => pst.ExchangeCurrency)
                             .FirstOrDefaultAsync(i =>
                                 i.PeriodNumber == key.PeriodNumber
                                 && i.BaseCurrency == key.BaseCurrency
                                 && i.ExchangeCurrencyCode == key.ExchangeCurrency);
            return result;
        }

        public override IQueryable<ImportBookExchangeRate> FilterBy(Expression<Func<ImportBookExchangeRate, bool>> expression)
        {
            return this.serviceDbContext.ImportBookExchangeRates
                .Where(expression)
                .Include(x => x.LedgerPeriod)
                .Include(x => x.ExchangeCurrency);
        }
    }
}
