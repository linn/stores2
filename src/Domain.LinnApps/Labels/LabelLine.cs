namespace Linn.Stores2.Domain.LinnApps.Labels
{
    public class LabelLine
    {
        public LabelLine(int qty, int lineNumber)
        {
            this.Qty = qty;
            this.LineNumber = lineNumber;
        }

        public int Qty { get; set; }

        public int LineNumber { get; set; }
    }
}
