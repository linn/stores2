namespace Linn.Stores2.Domain.LinnApps
{
    using System;

    public class AccountingCompany
    {
        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public DateTime? DateInvalid { get; protected set; }

        public string DefaultSaDiscountFamily { get; protected set; }

        public string WritebackToVax { get; protected set; }

        public int? Sequence { get; protected set; }

        public int? LatestSosJobId { get; protected set; }

        public DateTime? DateLatestSosJobId { get; protected set; }

        public int LedgerId { get; protected set; }

        public DateTime? LatestSalesAllocationDate { get; protected set; }

        public int? LatestSalesAllocationJobId { get; protected set; }

        public int? BridgeId { get; protected set; }
    }
}
