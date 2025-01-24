namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class ReqWithLines : RequisitionHeader
    {
        public ReqWithLines(int reqNumber, StoresFunctionCode functionCode)
            : base(reqNumber, "A cancelled req", functionCode, 12345678, "TYPE")
        {
            this.CreatedBy = new Employee { Id = 100 };
            this.Lines = new List<RequisitionLine>
                             {
                                 new LineWithMoves(reqNumber, 1)
                             };
        }
    }
}
