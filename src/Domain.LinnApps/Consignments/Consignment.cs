namespace Linn.Stores2.Domain.LinnApps.Consignments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Consignment
    {
        public int ConsignmentId { get; set; }

        public int? SalesAccountId { get; set; }

        public int? AddressId { get; set; }

        public DateTime? DateClosed { get; set; }

        public string CustomerName { get; set; }

        public IList<ConsignmentItem> Items { get; set; }

        public Address Address { get; set; }

        public string CarrierCode { get; set; }

        public Carrier Carrier { get; set; }

        public string ShippingMethod { get; set; }

        public string Terms { get; set; }

        public string Status { get; set; }

        public DateTime DateOpened { get; set; }

        public Employee ClosedBy { get; set; }

        public int? ClosedById { get; set; }

        public string DespatchLocationCode { get; set; }

        public string Warehouse { get; set; }

        public int? HubId { get; set; }

        public string CustomsEntryCodePrefix { get; set; }

        public string CustomsEntryCode { get; set; }

        public DateTime? CustomsEntryCodeDate { get; set; }

        public string CarrierRef { get; set; }

        public string MasterCarrierRef { get; set; }

        public string GetCustomerReferences()
        {
            if (this.Items == null || !this.Items.Any())
            {
                return string.Empty;
            }

            var numbers = this.Items
                .Where(a => !string.IsNullOrEmpty(a.SalesOrder?.CustomerOrderNumber))
                .DistinctBy(b => b.SalesOrder?.CustomerOrderNumber)
                .Select(item => item.SalesOrder?.CustomerOrderNumber);

            return string.Join(", ", numbers);
        }

        public string GetContainerDescription(int containerNumber)
        {
            if (this.Items == null || !this.Items.Any())
            {
                return string.Empty;
            }

            var groupedItems = this.Items
                .Where(item => item.ContainerNumber == containerNumber
                    && (item.ItemType == "I" || item.ItemType == "S"))
                .GroupBy(item => new { item.ItemDescription, item.MaybeHalfAPair })
                .Select(group => new
                {
                    TotalQuantity = group.Sum(item => item.Quantity),
                    Description = group.Key.ItemDescription,
                    MaybeHalfAPair = group.Key.MaybeHalfAPair
                })
                .Where(item => !string.IsNullOrEmpty(item.Description));

            var descriptions = new List<string>();

            foreach (var item in groupedItems)
            {
                var totalItems = item.TotalQuantity;

                if (item.MaybeHalfAPair == "Y")
                {
                    totalItems *= 2;
                }

                var formattedQuantity = totalItems.ToString("G29");
                descriptions.Add($"{formattedQuantity} {item.Description}");
            }

            return string.Join(", ", descriptions);
        }
    }
}
