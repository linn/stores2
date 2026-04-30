namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.PurchaseOrders;
    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportFactory : IImportFactory
    {
        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IQueryRepository<Currency> currencyRepository;

        private readonly IQueryRepository<Rsn> rsnRepository;

        private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        private readonly IRepository<ImportBookCpcNumber, int> importBookCpcRepository;

        private readonly IRepository<ImportBook, int> importBookRepository;

        private ImportCandidate candidate;

        public ImportFactory(
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<Currency> currencyRepository,
            IQueryRepository<Rsn> rsnRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository,
            IRepository<ImportBookCpcNumber, int> importBookCpcRepository,
            IRepository<ImportBook, int> importBookRepository)
        {
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.rsnRepository = rsnRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
            this.importBookCpcRepository = importBookCpcRepository;
            this.importBookRepository = importBookRepository;
        }

        public async Task<ImportCandidate> CreateImportBook(
            IEnumerable<int> rsnNumbers,
            IEnumerable<int> poNumbers,
            int? supplierId,
            Employee createdEmployee)
        {
            this.candidate = new ImportCandidate
            {
                CreatedBy = createdEmployee, // default some fields
                BaseCurrency = await this.currencyRepository.FindByAsync(c => c.Code == "GBP")
            };

            if (rsnNumbers != null && rsnNumbers.Any())
            {
                return await this.CreateImportBookFromRsnNumbers(rsnNumbers);
            }

            if (poNumbers != null && poNumbers.Any())
            {
                return await this.CreateImportBookFromPoNumbers(poNumbers);
            }

            if (supplierId.HasValue)
            {
                return await this.CreateImportBookFromSupplierId(supplierId.Value);
            }

            return this.candidate;
        }

        private async Task<ImportCandidate> CreateImportBookFromRsnNumbers(IEnumerable<int> rsnNumbers)
        {
            var cpcNumbers = await this.importBookCpcRepository.FilterByAsync(i => i.DateInvalid == null);
            var isFirstRsn = true;

            foreach (var rsnNumber in rsnNumbers)
            {
                var rsn = await this.rsnRepository.FindByAsync(r => r.RsnNumber == rsnNumber);
                if (rsn == null)
                {
                    throw new NotFoundException($"RSN {rsnNumber} not found");
                }

                if (isFirstRsn)
                {
                    await this.SetSupplierAndCarrierFromPreviousImport(rsn.AccountId, rsn.OutletNumber);
                    isFirstRsn = false;
                }

                var cpc = cpcNumbers.FirstOrDefault(c => c.ReasonForImport == rsn.ImportScheme());

                this.candidate.OrderDetailCandidates.Add(new ImportOrderDetailCandidate(rsn, cpc));
                this.candidate.InvoiceDetailCandidates.Add(new ImportInvoiceDetailCandidate(rsn));
            }

            return this.candidate;
        }

        private async Task SetSupplierAndCarrierFromPreviousImport(int accountId, int? outletNumber)
        {
            var candidates = await this.rsnRepository.FilterByAsync(
                r => r.AccountId == accountId
                     && r.OutletNumber == outletNumber
                     && r.ImportBookOrderDetails.Any());

            var mostRecentRsnNumber = candidates
                .OrderByDescending(r => r.RsnNumber)
                .FirstOrDefault()?.RsnNumber;

            if (mostRecentRsnNumber == null)
            {
                return;
            }

            var previousRsn = await this.rsnRepository.FindByAsync(r => r.RsnNumber == mostRecentRsnNumber);
            if (previousRsn == null)
            {
                return;
            }

            var importBookId = previousRsn.ImportBookOrderDetails.First().ImportBookId;
            var importBook = await this.importBookRepository.FindByIdAsync(importBookId);
            if (importBook != null)
            {
                this.candidate.Supplier = importBook.Supplier;
                this.candidate.Carrier = importBook.Carrier;
            }
        }

        private async Task<ImportCandidate> CreateImportBookFromPoNumbers(IEnumerable<int> poNumbers)
        {
            var cpcNumber =
                await this.importBookCpcRepository.FindByAsync(i =>
                    i.DateInvalid == null && i.ReasonForImport == "Material");

            foreach (var poNumber in poNumbers)
            {
                var purchaseOrder = await this.purchaseOrderRepository.FindByIdAsync(poNumber);
                if (purchaseOrder == null)
                {
                    throw new NotFoundException($"Purchase order {poNumber} not found");
                }

                if (this.candidate.Supplier == null)
                {
                    this.candidate.Supplier = purchaseOrder.Supplier;
                }
                else if (this.candidate.Supplier.Id != purchaseOrder.Supplier.Id)
                {
                    throw new ImportBookException($"Cannot mix suppliers");
                }

                this.candidate.OrderDetailCandidates.Add(new ImportOrderDetailCandidate(purchaseOrder, cpcNumber));

                // do not setup invoiceDetails as cannot easily be derived
            }

            return this.candidate;
        }

        private async Task<ImportCandidate> CreateImportBookFromSupplierId(int supplierId)
        {
            var supplier = await this.supplierRepository.FindByAsync(s => s.Id == supplierId);
            if (supplier == null)
            {
               throw new NotFoundException($"Supplier {supplierId} not found");
            }

            this.candidate.Supplier = supplier;

            return this.candidate;
        }
    }
}
