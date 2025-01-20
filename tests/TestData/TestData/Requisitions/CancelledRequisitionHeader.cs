namespace TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class CancelledRequisitionHeader : RequisitionHeader
    {
        public CancelledRequisitionHeader(int reqNumber)
            : base(reqNumber, "A cancelled req", new StoresFunctionCode { FunctionCode = "C" })
        {
            this.Cancelled = "Y";
            this.DateCancelled = DateTime.Now;
        }
    }
}
