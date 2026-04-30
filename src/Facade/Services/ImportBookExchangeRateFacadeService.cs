namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookExchangeRateFacadeService
        : AsyncFacadeService<ImportBookExchangeRate, ImportBookExchangeRateKey, ImportBookExchangeRateResource, ImportBookExchangeRateResource, ImportBookExchangeRateResource>
    {
        private readonly IQueryRepository<Currency> currencyRepository;

        public ImportBookExchangeRateFacadeService(
            IRepository<ImportBookExchangeRate, ImportBookExchangeRateKey> repository,
            ITransactionManager transactionManager,
            IBuilder<ImportBookExchangeRate> resourceBuilder,
            IQueryRepository<Currency> currencyRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.currencyRepository = currencyRepository;
        }

        protected override async Task<ImportBookExchangeRate> CreateFromResourceAsync(
            ImportBookExchangeRateResource resource,
            IEnumerable<string> privileges = null)
        {
            if (string.IsNullOrEmpty(resource.ExchangeCurrencyCode))
            {
                throw new ImportBookException("Exchange currency code must be provided");
            }

            var exchangeCurrency = await this.currencyRepository.FindByAsync(c => c.Code == resource.ExchangeCurrencyCode);
            if (exchangeCurrency == null)
            {
                throw new ImportBookException($"Exchange currency '{resource.ExchangeCurrencyCode}' not found");
            }

            if (string.IsNullOrEmpty(resource.BaseCurrency))
            {
                throw new ImportBookException("Base currency code must be provided");
            }

            var baseCurrency = await this.currencyRepository.FindByAsync(c => c.Code == resource.BaseCurrency);
            if (baseCurrency == null)
            {
                throw new ImportBookException($"Base currency '{resource.BaseCurrency}' not found");
            }

            return new ImportBookExchangeRate
            {
                PeriodNumber = resource.PeriodNumber,
                ExchangeCurrencyCode = exchangeCurrency.Code,
                ExchangeCurrency = exchangeCurrency,
                BaseCurrency = baseCurrency.Code,
                ExchangeRate = resource.ExchangeRate
            };
        }

        protected override Task UpdateFromResourceAsync(
            ImportBookExchangeRate entity,
            ImportBookExchangeRateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.ExchangeRate = updateResource.ExchangeRate;
            return Task.CompletedTask;
        }

        protected override Expression<Func<ImportBookExchangeRate, bool>> FilterExpression(
            ImportBookExchangeRateResource searchResource)
        {
            return r => r.PeriodNumber == searchResource.PeriodNumber;
        }

        protected override Expression<Func<ImportBookExchangeRate, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            ImportBookExchangeRate entity,
            ImportBookExchangeRateResource resource,
            ImportBookExchangeRateResource updateResource)
        {
            await Task.CompletedTask;
        }

        protected override void DeleteOrObsoleteResource(
            ImportBookExchangeRate entity,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBookExchangeRate, bool>> FindExpression(
            ImportBookExchangeRateResource searchResource)
        {
            return r => r.PeriodNumber == searchResource.PeriodNumber
                        && r.BaseCurrency == searchResource.BaseCurrency
                        && r.ExchangeCurrencyCode == searchResource.ExchangeCurrencyCode;
        }
    }
}
