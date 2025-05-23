namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Linq;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class StoresTransactionDefinition
    {
        public StoresTransactionDefinition()
        {
        }

        public StoresTransactionDefinition(string transactionCode)
        {
            this.TransactionCode = transactionCode;
        }

        public string TransactionCode { get; set; }

        public string Description { get; set; }

        public string QcType { get; set; }

        public string DocType { get; set; }

        public string Doc2Type { get; set; }

        public string StockAllocations { get; set; }

        public string OntoTransactions { get; set; }

        public string DecrementTransaction { get; set; }

        public string TakePriceFrom { get; set; }

        public string RequiresAuth { get; set; }

        public string AuthOpCode { get; set; }

        public string InspectedState { get; set; }

        public ICollection<StoresTransactionPosting> StoresTransactionPostings { get; set; }

        public ICollection<StoresTransactionState> StoresTransactionStates { get; set; }

        public bool RequiresStockAllocations => this.StockAllocations == "Y";

        public bool RequiresOntoTransactions => this.OntoTransactions == "Y";

        public bool IsDecrementTransaction => this.DecrementTransaction == "Y";

        public bool RequiresMoves => this.RequiresStockAllocations || this.RequiresOntoTransactions;

        public bool RequiresAuthorisation => this.RequiresAuth == "Y";

        public bool MaterialVarianceTransaction => this.TakePriceFrom == "M";

        public string FromState { get; set; }

        public string FromCategory { get; set; }

        public string OntoCategory { get; set; }

        public string AuthorisePrivilege()
        {
            if (this.RequiresAuthorisation && !string.IsNullOrEmpty(this.AuthOpCode))
            {
                return $"stores.requisitions.{this.AuthOpCode}";
            }

            return string.Empty;
        }

        public Nominal GetNominal()
        {
            // material variances transactions and SUNWI transactions mess this up so exclude them
            if (this.StoresTransactionPostings != null
                && !this.MaterialVarianceTransaction && !this.TransactionCode.Contains("NW"))
            {
                return this.StoresTransactionPostings.FirstOrDefault(p => p.Nominal != null)?.Nominal;
            }

            return null;
        }

        public IList<string> GetTransactionStates(string fromOrOnto)
        {
            if (this.StoresTransactionStates != null)
            {
                return this.StoresTransactionStates
                    .Where(t => t.TransactionCode == this.TransactionCode && t.FromOrOnto == fromOrOnto)
                    .OrderBy(s => s.State)
                    .Select(s => s.State).ToList();
            }

            return new List<string>();
        }

        public string DefaultFromState()
        {
            if (!string.IsNullOrEmpty(this.FromState))
            {
                return this.FromState;
            }

            if (!string.IsNullOrEmpty(this.InspectedState))
            {
                return this.InspectedState;
            }

            return string.Empty;
        }

        public string DefaultToState()
        {
            if (this.OntoTransactions == "Y" && !string.IsNullOrEmpty(this.InspectedState) &&
                this.GetTransactionStates("O").Any())
            {
                return this.InspectedState;
            }

            return string.Empty;
        }

        public bool HasDefaultFromState() => !string.IsNullOrEmpty(this.DefaultFromState());

        public bool HasDefaultToState() => !string.IsNullOrEmpty(this.DefaultFromState());
    }
}
