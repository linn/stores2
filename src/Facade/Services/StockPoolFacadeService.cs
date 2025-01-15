namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Facade.Common;
    using Linn.Stores2.Resources;

    public class StockPoolFacadeService : AsyncFacadeService<StockPool, string, StockPoolResource, StockPoolUpdateResource, StockPoolResource>
    {
        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<AccountingCompany, string> accountingCompanyRepository;

        public StockPoolFacadeService(
            IRepository<StockPool, string> repository, 
            ITransactionManager transactionManager, 
            IBuilder<StockPool> resourceBuilder,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<AccountingCompany, string> accountingCompanyRepository)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.storageLocationRepository = storageLocationRepository;
            this.accountingCompanyRepository = accountingCompanyRepository;
        }

        protected override async Task<StockPool> CreateFromResourceAsync(
            StockPoolResource resource, 
            IEnumerable<string> privileges = null)
        {
            var accountingCompany = await this.accountingCompanyRepository.FindByIdAsync(resource.AccountingCompanyCode);
            StorageLocation storageLocation = null;

            if (resource.DefaultLocation.HasValue)
            {
                storageLocation = await this.storageLocationRepository.FindByIdAsync((int)resource.DefaultLocation);
            }

            return new StockPool(
                resource.StockPoolCode,
                resource.StockPoolDescription,
                resource.DateInvalid,
                accountingCompany,
                resource.Sequence,
                resource.StockCategory,
                resource.DefaultLocation,
                storageLocation,
                resource.BridgeId,
                resource.AvailableToMrp);
        }

        protected override async Task UpdateFromResourceAsync(
            StockPool entity,
            StockPoolUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var accountingCompany = await this.accountingCompanyRepository.FindByIdAsync(updateResource.AccountingCompanyCode);
            var storageLocation = await this.storageLocationRepository.FindByIdAsync((int)updateResource.DefaultLocation);
            var dateInvalid = Convert.ToDateTime(updateResource.DateInvalid);

            entity.Update(
                updateResource.StockPoolDescription,
                dateInvalid,
                accountingCompany,
                updateResource.Sequence,
                updateResource.StockCategory,
                updateResource.DefaultLocation,
                storageLocation,
                updateResource.BridgeId,
                updateResource.AvailableToMrp);
        }

        protected override Expression<Func<StockPool, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override async Task SaveToLogTable(
            string actionType,
            int userNumber,
            StockPool entity,
            StockPoolResource resource,
            StockPoolUpdateResource updateResource)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            StockPool entity, 
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StockPool, bool>> FilterExpression(StockPoolResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<StockPool, bool>> FindExpression(StockPoolResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
