namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class CancelledRequisitionHeader : RequisitionHeader
    {
        public CancelledRequisitionHeader(int reqNumber)
            : base(
                reqNumber, 
                "A cancelled req", 
                new StoresFunctionCode { FunctionCode = "C" },
                12345678, 
                "TYPE")
        {
            this.Cancelled = "Y";
            this.DateCancelled = DateTime.Now;
        }
    }
}
