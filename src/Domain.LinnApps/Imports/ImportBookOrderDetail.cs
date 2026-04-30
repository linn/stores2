namespace Linn.Stores2.Domain.LinnApps.Imports
{
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps.Exceptions;

    using Linn.Stores2.Domain.LinnApps.Imports.Models;
    using Linn.Stores2.Domain.LinnApps.Returns;

    public class ImportBookOrderDetail
    {
        public ImportBookOrderDetail()
        {
            // for ef
        }

        public ImportBookOrderDetail(ImportOrderDetailCandidate candidate)
        {
            this.ImportBookId = candidate.ImportBookId;

            if (candidate.LineType == "RSN")
            {
                if (candidate.Rsn == null)
                {
                    throw new ImportBookException("RSN order detail has no RSN supplied");
                }

                if (!candidate.Rsn.ExportRsn)
                {
                    throw new ImportBookException("RSN order detail is not for an export RSN");
                }

                if (candidate.Rsn.ImportBookOrderDetails.Any(d => d.ImportBookId != this.ImportBookId))
                {
                    throw new ImportBookException("RSN order detail is already associated with a different import book");
                }

                this.Rsn = candidate.Rsn;
                this.RsnNumber = candidate.Rsn?.RsnNumber;
            }
            else if (candidate.LineType == "PO")
            {
                if (candidate.PurchaseOrder == null)
                {
                    throw new ImportBookException("PO order detail has no Purchase Order supplied");
                }

                this.OrderNumber = candidate.PurchaseOrder?.OrderNumber;
            }

            this.LineType = candidate.LineType;
            this.Qty = candidate.Qty;
            this.OrderDescription = candidate.OrderDescription;
            this.TariffCode = candidate.TariffCode;
            this.CountryOfOrigin = candidate.CountryOfOrigin?.CountryCode;
            this.ImportBookCpcNumber = candidate.CpcNumber;
            this.CpcNumberId = candidate.CpcNumber?.CpcNumber;
            this.DutyValue = candidate.DutyValue;
            this.VatValue = candidate.VatValue;
            this.CurrencyOrderValue = candidate.CurrencyOrderValue;

            this.OrderValue = candidate.IsBaseCurrency ? candidate.CurrencyOrderValue : candidate.OrderValue;
        }

        public int? CpcNumberId { get; set; }

        public int ImportBookId { get; set; }

        public int? InsNumber { get; set; }

        public int LineNumber { get; set; }

        public string LineType { get; set; }

        public int? LoanNumber { get; set; }

        public string OrderDescription { get; set; }

        public int? OrderNumber { get; set; }

        public int POLineNumber { get; set; }

        public int Qty { get; set; }

        public int? RsnNumber { get; set; }

        public Rsn Rsn { get; set; }

        public string TariffCode { get; set; }

        public string PostDuty { get; set; }

        public string CountryOfOrigin { get; set; }

        public decimal Weight { get; set; }

        public decimal CurrencyOrderValue { get; set; }

        // important values on order details are in base currency
        public decimal? OrderValue { get; set; }

        public decimal DutyValue { get; set; }

        public decimal FreightValue { get; set; }

        public int? VatRate { get; set; }

        public decimal VatValue { get; set; }

        public ImportBookCpcNumber ImportBookCpcNumber { get; set; }

        public bool IsIPR => this.ImportBookCpcNumber != null && this.ImportBookCpcNumber.IsIPR;

        public int? LineDocument()
        {
            if (this.LineType == "RSN")
            {
                return this.RsnNumber;
            }

            if (this.LineType == "PO" || this.LineType == "RO")
            {
                return this.OrderNumber;
            }

            if (this.LineType == "LOAN")
            {
                return this.LoanNumber;
            }

            return null;
        }
    }
}
