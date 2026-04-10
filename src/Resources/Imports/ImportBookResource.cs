namespace Linn.Stores2.Resources.Imports
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class ImportBookResource : HypermediaResource
    {
        public int Id { get; set; }

        public string DateCreated { get; set; }

        public int? CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public string DateReceived { get; set; }

        public string DateInstructionSent { get; set; }

        public int? ContactEmployeeId { get; set; }

        public string ContactEmployeeName { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int CarrierId { get; set; }

        public string CarrierName { get; set; }

        public CountryResource SupplierCountry { get; set; }

        public string ArrivalDate { get; set; }

        public string ArrivalPort { get; set; }

        public int? CancelledBy { get; set; }

        public string CancelledReason { get; set; }

        public string Comments { get; set; }

        public string Currency { get; set; }

        public string BaseCurrency { get; set; }

        public string ExchangeCurrency { get; set; }

        public string CustomsEntryCode { get; set; }

        public string CustomsEntryCodeDate { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public string DateCancelled { get; set; }

        public string DeliveryTermCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public string ForeignCurrency { get; set; }

        public IEnumerable<ImportBookInvoiceDetailResource> ImportBookInvoiceDetails { get; set; }

        public IEnumerable<ImportBookOrderDetailResource> ImportBookOrderDetails { get; set; }

        public IEnumerable<ImportBookPostEntryResource> ImportBookPostEntries { get; set; }

        public decimal? LinnDuty { get; set; }

        public decimal? LinnVat { get; set; }

        public int? NumCartons { get; set; }

        public int? NumPallets { get; set; }

        public int? ParcelNumber { get; set; }

        public string Pva { get; set; }

        public decimal? TotalImportValue { get; set; }

        public string TransportBillNumber { get; set; }

        public decimal? Weight { get; set; }
    }
}
