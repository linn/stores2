namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Imports;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookFacadeService : AsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookResource>
    {
        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IDatabaseSequenceService databaseSequenceService;

        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IQueryRepository<Currency> currencyRepository;

        private readonly IAuthorisationService authService;

        public ImportBookFacadeService(
            IRepository<ImportBook, int> repository,
            IDatabaseSequenceService databaseSequenceService,
            IRepository<Employee, int> employeeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<Currency> currencyRepository,
            ITransactionManager transactionManager,
            IAuthorisationService authService,
            IBuilder<ImportBook> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.databaseSequenceService = databaseSequenceService;
            this.employeeRepository = employeeRepository;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.authService = authService;
        }

        protected override async Task<ImportBook> CreateFromResourceAsync(
            ImportBookResource resource,
            IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.ImportBookAdmin, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create import books");
            }

            var createdBy = resource.CreatedById.HasValue
                                ? await this.employeeRepository.FindByIdAsync(resource.CreatedById.Value)
                                : null;

            var supplier = resource.SupplierId > 0
                                ? await this.supplierRepository.FindByAsync(s => s.Id == resource.SupplierId)
                                : null;

            var carrier = resource.CarrierId > 0
                               ? await this.supplierRepository.FindByAsync(s => s.Id == resource.CarrierId)
                               : null;

            var currency = string.IsNullOrEmpty(resource.Currency)
                               ? null
                               : await this.currencyRepository.FindByAsync(c => c.Code == resource.Currency);

            var baseCurrency = await this.currencyRepository.FindByAsync(c => c.Code == "GBP");

            var candidate = new ImportCandidate
            {
                Id = await this.databaseSequenceService.NextImportBookId(),
                CreatedBy = createdBy,
                Supplier = supplier,
                Carrier = carrier,
                Currency = currency,
                BaseCurrency = baseCurrency
            };

            return new ImportBook(candidate);
        }

        protected override async Task UpdateFromResourceAsync(
            ImportBook entity,
            ImportBookResource updateResource,
            IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedActions.ImportBookAdmin, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update import books");
            }

            var update = new ImportUpdate
                         {
                             CustomsEntryCode = updateResource.CustomsEntryCode,
                             CustomsEntryCodeDate = string.IsNullOrEmpty(updateResource.CustomsEntryCodeDate) ? null : Convert.ToDateTime(updateResource.CustomsEntryCodeDate),
                             CustomsEntryCodePrefix = updateResource.CustomsEntryCodePrefix,
                             LinnDuty = updateResource.LinnDuty,
                             LinnVat = updateResource.LinnVat
                         };
            entity.Update(update);
        }

        protected override Expression<Func<ImportBook, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Task SaveToLogTable(
            string actionType,
            int userNumber,
            ImportBook entity,
            ImportBookResource resource,
            ImportBookResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(ImportBook entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBook, bool>> FilterExpression(ImportBookResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ImportBook, bool>> FindExpression(ImportBookResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
