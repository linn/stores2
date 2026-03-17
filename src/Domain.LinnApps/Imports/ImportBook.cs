namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Imports.Models;

    public class ImportBook
    {
        public ImportBook()
        {
            // ef
        }

        public ImportBook(
            ImportCandidate candidate)
        {
            if (candidate.CreatedBy == null)
            {
                throw new ImportBookException("Created by employee not supplied");
            }

            if (candidate.Supplier == null)
            {
                throw new ImportBookException("Supplier not supplied");
            }

            if (candidate.Carrier == null)
            {
                throw new ImportBookException("Carrier not supplied");
            }

            if (candidate.Currency == null)
            {
                throw new ImportBookException("Currency not supplied");
            }

            if (candidate.BaseCurrency == null)
            {
                throw new ImportBookException("Base Currency not supplied");
            }

            this.Id = candidate.Id;
            this.DateCreated = DateTime.UtcNow;
            this.CreatedBy = candidate.CreatedBy;
            this.CreatedById = candidate.CreatedBy.Id;
            this.SupplierId = candidate.Supplier.Id;
            this.Supplier = candidate.Supplier;
            this.CarrierId = candidate.Carrier.Id;
            this.Carrier = candidate.Carrier;
            this.Currency = candidate.Currency;
            this.CurrencyCode = candidate.Currency?.Code;
            this.BaseCurrency = candidate.BaseCurrency;
            this.ExchangeCurrency = candidate.ExchangeCurrency;

            this.UpdateCustomsEntry(candidate.CustomsEntryCodePrefix, candidate.CustomsEntryCode, candidate.CustomsEntryCodeDate);

            this.LinnDuty = candidate.LinnDuty;
            this.LinnVat = candidate.LinnVat;

            this.OrderDetails = new List<ImportBookOrderDetail>();
            this.InvoiceDetails = new List<ImportBookInvoiceDetail>();
            this.PostEntries = new List<ImportBookPostEntry>();

            foreach (var orderDetailCandidate in candidate.OrderDetailCandidates)
            {
                this.AddOrderDetail(new ImportBookOrderDetail(orderDetailCandidate));
            }
        }

        public ImportBook(ImportSetup setup)
        {
            this.DateCreated = DateTime.UtcNow;
            this.CreatedBy = setup.CreatedBy;
            this.ContactEmployee = setup.CreatedBy;
            this.OrderDetails = new List<ImportBookOrderDetail>();
            this.InvoiceDetails = new List<ImportBookInvoiceDetail>();
            this.PostEntries = new List<ImportBookPostEntry>();

            foreach (var candidate in setup.OrderDetailCandidates())
            {
                this.AddOrderDetail(new ImportBookOrderDetail(candidate));
            }
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        public int? CreatedById { get; set; }

        public Employee CreatedBy { get; set; }

        public DateTime? DateReceived { get; set; }

        public DateTime? DateInstructionSent { get; set; }

        public Employee ContactEmployee { get; set; }

        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; }

        public Supplier Carrier { get; set; }

        public int CarrierId { get; set; }

        public string CurrencyCode { get; set; }

        public Currency Currency { get; set; }

        public Currency BaseCurrency { get; set; }

        public Currency ExchangeCurrency { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public string ArrivalPort { get; set; }

        public int? CancelledBy { get; set; }

        public string CancelledReason { get; set; }

        public string Comments { get; set; }

        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public DateTime? DateCancelled { get; set; }

        public string DeliveryTermCode { get; set; }

        public decimal? ExchangeRate { get; set; }

        public string ForeignCurrency { get; set; }

        public IList<ImportBookInvoiceDetail> InvoiceDetails { get; set; }

        public decimal? LinnDuty { get; set; }

        public decimal? LinnVat { get; set; }

        public int? NumCartons { get; set; }

        public int? NumPallets { get; set; }

        public IList<ImportBookOrderDetail> OrderDetails { get; set; }

        public int? ParcelNumber { get; set; }

        public int? PeriodNumber { get; set; }

        public IList<ImportBookPostEntry> PostEntries { get; set; }

        public string Pva { get; set; }

        public decimal TotalImportValue { get; set; }

        public int TransactionId { get; set; }

        public string TransportBillNumber { get; set; }

        public int TransportId { get; set; }

        public decimal? Weight { get; set; }

        public void Update(ImportUpdate update)
        {
            this.UpdateCustomsEntry(update.CustomsEntryCodePrefix, update.CustomsEntryCode, update.CustomsEntryCodeDate);

            this.LinnDuty = update.LinnDuty;
            this.LinnVat = update.LinnVat;
        }

        public void UpdateCustomsEntry(string prefix, string entryCode, DateTime? entryDate)
        {
            if (!string.IsNullOrEmpty(prefix) && prefix.Length > 3)
            {
                throw new ImportBookException("Customs Entry Code Prefix must be 3 characters or less");
            }

            this.CustomsEntryCode = entryCode;
            this.CustomsEntryCodeDate = entryDate;
            this.CustomsEntryCodePrefix = prefix;
        }

        public void AddOrderDetail(ImportBookOrderDetail orderDetail)
        {
            orderDetail.ImportBookId = this.Id;
            orderDetail.LineNumber = this.OrderDetails.Count + 1;
            this.OrderDetails.Add(orderDetail);
        }
    }
}
