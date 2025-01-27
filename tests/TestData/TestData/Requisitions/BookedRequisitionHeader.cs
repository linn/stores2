namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class BookedRequisitionHeader : RequisitionHeader
    {
        public BookedRequisitionHeader(int reqNumber, int bookedBy, StoresFunctionCode functionCode)
            : base(reqNumber, "A cancelled req", functionCode, 12345678, "TYPE")
        {
            this.BookedBy = new Employee { Id = bookedBy };
            this.DateBooked = DateTime.Now;
        }
    }
}
