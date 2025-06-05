namespace Linn.Stores2.TestData.Requisitions
{
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Requisitions;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class LineWithMoves : RequisitionLine
    {
        public LineWithMoves()
        {
            this.Moves = new List<ReqMove> { new ReqMove() };
        }

        public LineWithMoves(
            int reqNumber,
            int lineNumber,
            StoresTransactionDefinition transactionDefinition,
            decimal? quantity = null,
            Part part = null,
            ICollection<StoresBudget> budgets = null)
        {
            this.TransactionDefinition = transactionDefinition;
            this.ReqNumber = reqNumber;
            this.LineNumber = lineNumber;
            this.Part = part ?? new Part();
            this.Qty = quantity ?? 1;
            this.Moves = new List<ReqMove>();
            this.StoresBudgets = budgets;
        }
    }
}
