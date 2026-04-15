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
            this.OrderDetails = new List<ImportBookOrderDetail>();
            this.InvoiceDetails = new List<ImportBookInvoiceDetail>();
        }

        public ImportBook(
            ImportCandidate candidate, bool initialise = false)
        {
            if (candidate.CreatedBy == null)
            {
                throw new ImportBookException("Created by employee not supplied");
            }

            if (candidate.BaseCurrency == null)
            {
                throw new ImportBookException("Base Currency not supplied");
            }

            if (candidate.Supplier == null && !initialise)
            {
                throw new ImportBookException("Supplier not supplied");
            }

            if (candidate.Carrier == null && !initialise)
            {
                throw new ImportBookException("Carrier not supplied");
            }

            if (candidate.Currency == null && !initialise)
            {
                throw new ImportBookException("Currency not supplied");
            }

            this.Id = candidate.Id;
            this.DateCreated = DateTime.UtcNow;
            this.CreatedBy = candidate.CreatedBy;
            this.CreatedById = candidate.CreatedBy.Id;
            this.BaseCurrency = candidate.BaseCurrency;
            this.ExchangeCurrency = candidate.ExchangeCurrency;
            this.TransportBillNumber = candidate.TransportBillNumber;

            if (candidate.Supplier != null)
            {
                this.SupplierId = candidate.Supplier.Id;
                this.Supplier = candidate.Supplier;
            }

            if (candidate.Carrier != null)
            {
                this.CarrierId = candidate.Carrier.Id;
                this.Carrier = candidate.Carrier;
            }

            if (candidate.Currency != null)
            {
                this.Currency = candidate.Currency;
                this.CurrencyCode = candidate.Currency.Code;
            }

            this.UpdateCustomsEntry(candidate.CustomsEntryCodePrefix, candidate.CustomsEntryCode, candidate.CustomsEntryCodeDate);

            this.LinnDuty = candidate.LinnDuty;
            this.LinnVat = candidate.LinnVat;

            this.OrderDetails = new List<ImportBookOrderDetail>();
            this.InvoiceDetails = new List<ImportBookInvoiceDetail>();
            this.PostEntries = new List<ImportBookPostEntry>();

            this.ForeignCurrency = this.CurrencyCode != this.BaseCurrency.Code ? "Y" : "N";
            this.Pva = "Y";

            foreach (var orderDetailCandidate in candidate.OrderDetailCandidates)
            {
                this.AddOrderDetail(new ImportBookOrderDetail(orderDetailCandidate));
            }

            foreach (var invoiceDetailCandidate in candidate.InvoiceDetailCandidates)
            {
                this.AddInvoiceDetail(invoiceDetailCandidate);
            }

            if (candidate.Period != null)
            {
                this.PeriodNumber = candidate.Period.PeriodNumber;
                this.Period = candidate.Period;
            }

            if (candidate.ExchangeRate != null)
            {
                this.ApplyExchangeRate(candidate.ExchangeRate);
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

        public LedgerPeriod Period { get; set; }

        public IList<ImportBookPostEntry> PostEntries { get; set; }

        public string Pva { get; set; }

        // in Base Currency cannot calculate until have exchange rate
        public decimal? TotalImportValue { get; set; }

        public int? TransactionId { get; set; }

        public string TransportBillNumber { get; set; }

        public int? TransportId { get; set; }

        public decimal? Weight { get; set; }

        public void Update(ImportUpdate update)
        {
            this.UpdateCustomsEntry(update.CustomsEntryCodePrefix, update.CustomsEntryCode, update.CustomsEntryCodeDate);

            this.TransportBillNumber = update.TransportBillNumber;
            this.DateReceived = update.DateReceived;
            this.DateInstructionSent = update.DateInstructionSent;

            if (update.Period != null && this.Period == null)
            {
                this.PeriodNumber = update.Period.PeriodNumber;
                this.Period = update.Period;
            }

            if (update.ExchangeRate != null && this.ExchangeRate == null)
            {
                this.ApplyExchangeRate(update.ExchangeRate);
            }

            if (update.OrderDetailCandidates != null)
            {
                foreach (var orderDetailCandidate in update.OrderDetailCandidates)
                {
                    var orderDetail = this.OrderDetails.FirstOrDefault(d => d.LineNumber == orderDetailCandidate.LineNumber);
                    if (orderDetail == null)
                    {
                        this.AddOrderDetail(new ImportBookOrderDetail(orderDetailCandidate));
                    }
                    else
                    {
                        orderDetail.DutyValue = orderDetailCandidate.DutyValue;
                        orderDetail.VatValue = orderDetailCandidate.VatValue;
                        orderDetail.OrderValue = orderDetailCandidate.OrderValue;
                        orderDetail.OrderDescription = orderDetailCandidate.OrderDescription;
                    }
                }

                var dutyTotal = this.OrderDetails.Sum(d => d.DutyValue);
                var vatTotal = this.OrderDetails.Sum(d => d.VatValue);
                this.LinnDuty = dutyTotal;
                this.LinnVat = vatTotal;
            }
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

        public void ApplyExchangeRate(ImportBookExchangeRate exchangeRate)
        {
            if (exchangeRate != null)
            {
                // check if we have already applied this exchange rate
                if (this.ExchangeCurrency != null || this.ExchangeRate != exchangeRate.ExchangeRate)
                {
                    this.ExchangeRate = exchangeRate.ExchangeRate;
                    this.ExchangeCurrency = exchangeRate.ExchangeCurrency;
                }

                var currencyTotal = this.InvoiceDetails.Sum(d => d.InvoiceValue);
                this.TotalImportValue = exchangeRate.ConvertToBaseValue(currencyTotal);
            }
        }

        public void AddOrderDetail(ImportBookOrderDetail orderDetail)
        {
            orderDetail.ImportBookId = this.Id;
            orderDetail.LineNumber = this.OrderDetails.Count + 1;
            this.OrderDetails.Add(orderDetail);
        }

        public void AddInvoiceDetail(ImportInvoiceDetailCandidate candidate)
        {
            var invoiceDetail = new ImportBookInvoiceDetail
            {
                ImportBookId = this.Id,
                LineNumber = this.InvoiceDetails.Count + 1,
                InvoiceNumber = candidate.InvoiceNumber,
                InvoiceValue = candidate.InvoiceValue
            };
            this.InvoiceDetails.Add(invoiceDetail);

            if (candidate.Currency != null)
            {
                if (this.Currency == null)
                {
                    this.Currency = candidate.Currency;
                    this.CurrencyCode = candidate.Currency.Code;
                }
                else if (this.Currency.Code != candidate.Currency.Code)
                {
                    throw new ImportBookException($"Invoice detail currency {candidate.Currency.Code} does not match import book currency {this.Currency.Code}");
                }
            }
        }

        public string Status()
        {
            if (this.DateCancelled.HasValue)
            {
                return "Cancelled";
            }
            else if (this.DateReceived.HasValue)
            {
                return "Received";
            }
            else if (this.CustomsEntryCodeDate.HasValue)
            {
                return "Cleared";
            }
            else if (this.DateInstructionSent.HasValue)
            {
                return "Instruction Sent";
            }

            return "Raised";
        }
    }
}
