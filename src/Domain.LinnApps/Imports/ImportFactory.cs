namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
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

        private ImportCandidate candidate;

        public ImportFactory(
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<Currency> currencyRepository,
            IQueryRepository<Rsn> rsnRepository,
            IRepository<PurchaseOrder, int> purchaseOrderRepository)
        {
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.rsnRepository = rsnRepository;
            this.purchaseOrderRepository = purchaseOrderRepository;
        }

        public async Task<ImportCandidate> CreateImportBook(IEnumerable<int> rsnNumbers,
            IEnumerable<int> poNumbers,
            int? supplierId,
            Employee createdEmployee)
        {
            this.candidate = new ImportCandidate();
            this.candidate.CreatedBy = createdEmployee;

            // default some fields
            this.candidate.BaseCurrency = await this.currencyRepository.FindByAsync(c => c.Code == "GBP");

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
            foreach (var rsnNumber in rsnNumbers)
            {
                var rsn = await this.rsnRepository.FindByAsync(r => r.RsnNumber == rsnNumber);
                if (rsn == null)
                {
                    throw new NotFoundException($"RSN {rsnNumber} not found");
                }

                this.candidate.OrderDetailCandidates.Add(new ImportOrderDetailCandidate(rsn));
                this.candidate.InvoiceDetailCandidates.Add(new ImportInvoiceDetailCandidate(rsn));
            }

            return this.candidate;
        }

        private async Task<ImportCandidate> CreateImportBookFromPoNumbers(IEnumerable<int> poNumbers)
        {
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

                this.candidate.OrderDetailCandidates.Add(new ImportOrderDetailCandidate(purchaseOrder));

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
