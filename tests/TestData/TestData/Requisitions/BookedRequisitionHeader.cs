using Linn.Stores2.Domain.LinnApps.Accounts;

namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class BookedRequisitionHeader : RequisitionHeader
    {
        public BookedRequisitionHeader(int reqNumber, int bookedBy, StoresFunctionCode functionCode)
            : base(reqNumber, "A cancelled req", functionCode, 12345678, "TYPE", new Department(), new Nominal(), null)
        {
            this.BookedBy = new Employee { Id = bookedBy };
            this.DateBooked = DateTime.Now;
            this.Lines = new List<RequisitionLine>
                             {
                                 new BookedLine(reqNumber, 1)
                             };
        }
    }
}
