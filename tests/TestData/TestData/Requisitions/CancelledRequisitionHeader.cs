namespace TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class CancelledRequisitionHeader : RequisitionHeader
    {
        public CancelledRequisitionHeader(int reqNumber)
            : base(reqNumber, "A cancelled req")
        {
            this.Cancelled = "Y";
            this.DateCancelled = DateTime.Now;
        }
    }
}
