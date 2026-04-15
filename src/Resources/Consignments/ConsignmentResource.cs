namespace Linn.Stores2.Resources.Consignments
{
    using Linn.Common.Resources;

    public class ConsignmentResource : HypermediaResource
    {
        public int ConsignmentId { get; set; }

        public int? SalesAccountId { get; set; }

        public int? AddressId { get; set; }

        public string DateOpened { get; set; }

        public string DateClosed { get; set; }

        public string CustomerName { get; set; }

        public string CarrierCode { get; set; }

        public string ShippingMethod { get; set; }

        public string Terms { get; set; }

        public string Status { get; set; }

        public int? ClosedById { get; set; }

        public string DespatchLocationCode { get; set; }

        public string Warehouse { get; set; }

        public int? HubId { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public string CustomsEntryCode { get; set; }

        public string CustomsEntryCodeDate { get; set; }

        public string CarrierRef { get; set; }

        public string MasterCarrierRef { get; set; }
    }
}
