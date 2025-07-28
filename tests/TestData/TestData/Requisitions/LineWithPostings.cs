namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class LineWithPostings : RequisitionLine
    {
        public LineWithPostings(
            int reqNumber,
            int lineNumber,
            StoresTransactionDefinition transactionDefinition,
            decimal? quantity = null,
            Part part = null,
            ICollection<RequisitionLinePosting> postings = null)
        {
            this.TransactionDefinition = transactionDefinition;
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Part = part ?? new Part();
            this.Qty = quantity ?? 1;
            this.Moves = new List<ReqMove>();
            this.NominalAccountPostings = postings;
        }
    }
}