namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class LineWithMoves : RequisitionLine
    {
        public LineWithMoves()
        {
            this.Moves = new List<ReqMove> { new ReqMove() };
        }

        public LineWithMoves(int reqNumber, int lineNumber)
        {
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Part = new Part();
            this.Qty = 1;
            this.Moves = new List<ReqMove>();
        }
    }
}
