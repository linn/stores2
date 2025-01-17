namespace TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class BookedRequisitionHeader : RequisitionHeader
    {
        public BookedRequisitionHeader(int reqNumber, int bookedBy)
            : base(reqNumber, "A cancelled req")
        {
            this.BookedBy = new Employee { Id = bookedBy };
            this.DateBooked = DateTime.Now;
        }
    }
}
