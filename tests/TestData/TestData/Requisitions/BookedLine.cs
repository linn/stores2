namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Requisitions;

    public class BookedLine : LineWithMoves
    {
        public BookedLine()
        {
            this.Moves = new List<ReqMove> { new ReqMove() };
        }

        public BookedLine(int reqNumber, int lineNumber) : base(reqNumber, lineNumber) 
        {
           this.DateBooked = DateTime.Now;
        }
    }
}
