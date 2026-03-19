namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportFactory : IImportFactory
    {
        private readonly IQueryRepository<Supplier> supplierRepository;

        private readonly IQueryRepository<Currency> currencyRepository;

        private readonly IQueryRepository<Rsn> rsnRepository;

        private ImportCandidate candidate;

        public ImportFactory(
            IQueryRepository<Supplier> supplierRepository,
            IQueryRepository<Currency> currencyRepository,
            IQueryRepository<Rsn> rsnRepository)
        {
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.rsnRepository = rsnRepository;
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
            return this.candidate;
        }

        private async Task<ImportCandidate> CreateImportBookFromSupplierId(int supplierId)
        {
            return this.candidate;
        }
    }
}
