namespace Linn.Stores2.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using Linn.Stores2.Domain.LinnApps.Returns;
    using Linn.Stores2.Facade.Extensions;
    using Linn.Stores2.Resources;
    using Linn.Stores2.Resources.Imports;

    public class ImportBookFacadeService : AsyncFacadeService<ImportBook, int, ImportBookResource, ImportBookResource, ImportBookSearchResource>, IImportBookFacadeService
    {
        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IDatabaseSequenceService databaseSequenceService;

        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IQueryRepository<Currency> currencyRepository;

        private readonly IQueryRepository<Rsn> rsnRepository;

        private readonly IRepository<Country, string> countryRepository;

        private readonly IRepository<ImportBookCpcNumber, int> importBookCpcRepository;

        private readonly IImportFactory importFactory;

        private readonly IImportCurrencyService importCurrencyService;

        private readonly IAuthorisationService authService;

        private readonly IBuilder<ImportBook> resourceBuilder;

        public ImportBookFacadeService(
            IRepository<ImportBook, int> repository,
            IDatabaseSequenceService databaseSequenceService,
            IRepository<Employee, int> employeeRepository,
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<Currency> currencyRepository,
            IQueryRepository<Rsn> rsnRepository,
            IRepository<Country, string> countryRepository,
            IRepository<ImportBookCpcNumber, int> importBookCpcRepository,
            IImportFactory importFactory,
            IImportCurrencyService importCurrencyService,
            ITransactionManager transactionManager,
            IAuthorisationService authService,
            IBuilder<ImportBook> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.databaseSequenceService = databaseSequenceService;
            this.employeeRepository = employeeRepository;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.rsnRepository = rsnRepository;
            this.countryRepository = countryRepository;
            this.importBookCpcRepository = importBookCpcRepository;
            this.importFactory = importFactory;
            this.importCurrencyService = importCurrencyService;
            this.resourceBuilder = resourceBuilder;
            this.authService = authService;
        }

        public async Task<IResult<ImportBookResource>> InitialiseImportBook(string rsnNumbers, string purchaseOrderNumbers, int? supplierId, int? employeeNumber, IEnumerable<string> privileges)
        {
            if (employeeNumber == null)
            {
                return new BadRequestResult<ImportBookResource>($"Employee not supplied");
            }

            var employee = await this.employeeRepository.FindByIdAsync(employeeNumber.Value);
            if (employee == null)
            {
                return new BadRequestResult<ImportBookResource>($"Employee not found: {employeeNumber.Value}");
            }

            if (!this.authService.HasPermissionFor(AuthorisedActions.ImportBookAdmin, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create import books");
            }

            try
            {
                var candidate = await this.importFactory.CreateImportBook(this.ParseNumbers(rsnNumbers), this.ParseNumbers(purchaseOrderNumbers), supplierId, employee);

                var importBook = new ImportBook(candidate, true);
                return new SuccessResult<ImportBookResource>((ImportBookResource)this.resourceBuilder.Build(importBook, privileges));
            }
            catch (NotFoundException e)
            {
                return new BadRequestResult<ImportBookResource>(e.Message);
            }
            catch (ImportBookException e)
            {
                return new BadRequestResult<ImportBookResource>(e.Message);
            }
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

            var orderDetails = resource.ImportBookOrderDetails != null ? await this.GetOrderDetailCandidates(resource.ImportBookOrderDetails) : null;

            var invoiceDetails = new List<ImportInvoiceDetailCandidate>();
            if (resource.ImportBookInvoiceDetails != null)
            {
                foreach (var invoice in resource.ImportBookInvoiceDetails)
                {
                    invoiceDetails.Add(new ImportInvoiceDetailCandidate(invoice.InvoiceNumber, invoice.InvoiceValue));
                }
            }

            var candidate = new ImportCandidate
            {
                Id = await this.databaseSequenceService.NextImportBookId(),
                CreatedBy = createdBy,
                Supplier = supplier,
                Carrier = carrier,
                Currency = currency,
                BaseCurrency = baseCurrency,
                OrderDetailCandidates = orderDetails,
                InvoiceDetailCandidates = invoiceDetails,
                TransportBillNumber = resource.TransportBillNumber
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

            var currency = string.IsNullOrEmpty(updateResource.Currency)
                ? null
                : await this.currencyRepository.FindByAsync(c => c.Code == updateResource.Currency);

            var orderDetails = updateResource.ImportBookOrderDetails != null ? await this.GetOrderDetailCandidates(updateResource.ImportBookOrderDetails) : null;

            var update = new ImportUpdate
                         {
                             CustomsEntryCode = updateResource.CustomsEntryCode,
                             CustomsEntryCodeDate = string.IsNullOrEmpty(updateResource.CustomsEntryCodeDate) ? null : Convert.ToDateTime(updateResource.CustomsEntryCodeDate),
                             CustomsEntryCodePrefix = updateResource.CustomsEntryCodePrefix,
                             TransportBillNumber = updateResource.TransportBillNumber,
                             Comments = updateResource.Comments,
                             ClearanceComments = updateResource.ClearanceComments,
                             DateInstructionSent = string.IsNullOrEmpty(updateResource.DateInstructionSent) ? null : Convert.ToDateTime(updateResource.DateInstructionSent),
                             DateReceived = string.IsNullOrEmpty(updateResource.DateReceived) ? null : Convert.ToDateTime(updateResource.DateReceived), 
                             Period = entity.Period,
                             Currency = currency,
                             OrderDetailCandidates = orderDetails
                         };

            if (update.Period == null && update.CustomsEntryCodeDate.HasValue)
            {
                // work out period from date
                update.Period = await this.importCurrencyService.GetImportPeriod(update.CustomsEntryCodeDate.Value);

                if (update.Period == null && entity.BaseCurrency != null && currency != null)
                {
                    update.ExchangeRate = await this.importCurrencyService.GetExchangeRate(update.Period, entity.BaseCurrency, currency);
                }
            }

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

        protected override Expression<Func<ImportBook, bool>> FilterExpression(ImportBookSearchResource searchResource)
        {
            return searchResource.ToExpression();
        }

        protected override Expression<Func<ImportBook, bool>> FindExpression(ImportBookSearchResource searchResource)
        {
            return searchResource.ToExpression();
        }

        private IEnumerable<int> ParseNumbers(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
            {
                return new List<int>();
            }

            return numbers.Split(',')
                .Select(n => n.Trim())
                .Where(n => int.TryParse(n, out _))
                .Select(int.Parse);
        }

        private async Task<IList<ImportOrderDetailCandidate>> GetOrderDetailCandidates(IEnumerable<ImportBookOrderDetailResource> candidates)
        {
            var orderDetails = new List<ImportOrderDetailCandidate>();
            foreach (var orderDetailResource in candidates)
            {
                var country = string.IsNullOrEmpty(orderDetailResource.CountryOfOrigin)
                    ? null
                    : await this.countryRepository.FindByIdAsync(orderDetailResource.CountryOfOrigin);

                var cpcNumber = orderDetailResource.CpcNumberId.HasValue
                    ? await this.importBookCpcRepository.FindByIdAsync(orderDetailResource.CpcNumberId.Value)
                    : null;

                var rsn = orderDetailResource.RsnNumber.HasValue
                    ? await this.rsnRepository.FindByAsync(r => r.RsnNumber == orderDetailResource.RsnNumber.Value)
                    : null;

                orderDetails.Add(new ImportOrderDetailCandidate
                {
                    LineType = orderDetailResource.LineType,
                    LineNumber = orderDetailResource.LineNumber,
                    Qty = orderDetailResource.Qty,
                    OrderDescription = orderDetailResource.OrderDescription,
                    TariffCode = orderDetailResource.TariffCode,
                    CountryOfOrigin = country,
                    Rsn = rsn,
                    CpcNumber = cpcNumber,
                    OrderValue = orderDetailResource.OrderValue,
                    DutyValue = orderDetailResource.DutyValue,
                    VatValue = orderDetailResource.VatValue,
                });
            }

            return orderDetails;
        }
    }
}
