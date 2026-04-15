namespace Linn.Stores2.Domain.LinnApps.Consignments
{
    public class SalesOrder
    {
        public int OrderNumber { get; set; }

        public int AccountId { get; set; }

        public int OutletNumber { get; set; }

        public string CustomerOrderNumber { get; set; }
    }
}
