namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System.Collections.Generic;
    using System.Linq;
    using Linn.Stores2.Domain.LinnApps.Accounts;

    public class StoresFunction
    {
        public StoresFunction()
        {
        }

        public StoresFunction(string functionCode)
        {
            this.FunctionCode = functionCode;
        }

        public string FunctionCode { get; set; }
        
        public string Description { get; set; }

        public string FunctionType { get; set; }

        public string CancelFunction { get; set; }

        public string DepartmentNominalRequired { get; set; }

        public string ManualPickRequired { get; set; }

        public string FromStateRequired { get; set; }

        public string FromLocationRequired { get; set; }

        public string FromStockPoolRequired { get; set; }

        public string QuantityRequired { get; set; }

        public string Category { get; set; }

        public string ToStateRequired { get; set; }
        
        public string ToStockPoolRequired { get; set; }

        public string ToLocationRequired { get; set; }

        // Y - Yes, O, X or N - No
        public string Document1RequiredFlag { get; set; }

        public string Document1LineRequiredFlag { get; set; }

        public string Document1Text { get; set; }

        // Y - Yes, O, X or N - No
        public string Document2RequiredFlag { get; set; }

        public string Document2Text { get; set; }

        public string PartSource { get; set; }

        public string BatchRequired { get; set; }

        public string BatchDateRequired { get; set; }

        public string FunctionAvailableFlag { get; set; }

        public string ToStockPool { get; set; }

        public string CanBeReversed { get; set; }
        
        public string CanBeCancelled { get; set; }

        public int ProcessStage { get; set; }

        public string ReceiptDateRequired { get; set; }

        public string AuditLocationRequired { get; set; }

        public string UpdateSodQtyOS { get; set; }

        public ICollection<StoresFunctionTransaction> TransactionsTypes { get; set; }

        public string LinesRequired { get; set;  }

        public bool AuditFunction() => this.FunctionCode == "AUDIT" || this.FunctionCode == "KOUNT";

        public bool Document1Required() => this.Document1RequiredFlag is "Y" or "O" or "X";

        public bool Document1Entered() => this.Document1RequiredFlag is "Y" or "O";

        public bool Document2Required() => this.Document2RequiredFlag is "Y" or "O" or "X";

        public bool Document2Entered() => this.Document2RequiredFlag is "Y" or "O";

        public bool PartNumberRequired() => this.PartSource != "N";

        public bool NewPartNumberRequired() => this.FunctionCode == "PARTNO CH";

        public bool AutomaticFunctionType() => this.FunctionType == "A";

        public bool FunctionAvailable() => this.FunctionAvailableFlag == "Y";

        public Nominal GetNominal()
        {
            // for some reason PARTNO CH gets this wrong wanting to default nom to Stock Adjustments
            if (this.TransactionsTypes != null && this.FunctionCode != "PARTNO CH")
            {
                return this.TransactionsTypes.FirstOrDefault(t => t.TransactionDefinition?.GetNominal() != null)
                    ?.TransactionDefinition.GetNominal();
            }
            return null;
        }

        public IList<string> GetTransactionStates(string fromOrOnto)
        {
            if (this.TransactionsTypes != null)
            {
                return this.TransactionsTypes.SelectMany(t => t.TransactionDefinition?.GetTransactionStates(fromOrOnto) ?? Enumerable.Empty<string>())
                    .Distinct().OrderBy(s => s).ToList();

            }
            return new List<string>();
        }

        public string Document1Name()
        {
            if (this.TransactionsTypes != null)
            {
                // see REQ_UT FUNCTION_CODE_WVI cursor C 
                return this.TransactionsTypes.FirstOrDefault()?.TransactionDefinition?.DocType;
            }
            return string.Empty;
        }

        public string Document2Name()
        {
            if (this.TransactionsTypes != null)
            {
                // see REQ_UT FUNCTION_CODE_WVI cursor C 
                return this.TransactionsTypes.FirstOrDefault()?.TransactionDefinition?.Doc2Type;
            }
            return string.Empty;
        }

        public string DefaultFromState()
        {
            if ((this.FromStateRequired == "Y" || this.FromStateRequired == "O") && this.TransactionsTypes != null)
            {
                var states = this.TransactionsTypes
                    .Where(t => t.TransactionDefinition.HasDefaultFromState())
                    .Select(t => t.TransactionDefinition?.DefaultFromState()).ToList();

                if (states.Contains("STORES"))
                {
                    return "STORES";
                }

                return states.FirstOrDefault();
            }

            return string.Empty;
        }

        public string DefaultToState()
        {
            if (this.TransactionsTypes != null)
            {
                if (this.ToStateRequired == "Y" || this.ToStateRequired == "O" || this.FunctionCode == "SUKIT" ||
                     this.TransactionsTypes.All(t => t.TransactionDefinition.HasDefaultToState()))
                {
                    var states = this.TransactionsTypes
                        .Where(t => t.TransactionDefinition.HasDefaultToState())
                        .Select(t => t.TransactionDefinition.DefaultToState()).ToList();

                    if (states.Contains("STORES"))
                    {
                        return "STORES";
                    }

                    return states.FirstOrDefault();
                }
            }

            return string.Empty;
        }
        
        public bool IsQuantityRequiredOrOptional() =>
            this.QuantityRequired is "Y" or "O";
        
        public bool ToLocationRequiredOrOptional() =>
            this.ToLocationRequired is "Y" or "O";
        
        public bool ToLocationIsRequired() =>
            this.ToLocationRequired is "Y";
        
        public bool ToStateRequiredOrOptional() =>
            this.ToStateRequired is "Y" or "O";
        
        public bool FromStateRequiredOrOptional() =>
            this.FromStateRequired is "Y" or "O";
        
        public bool FromStockPoolRequiredOrOptional() =>
            this.FromStockPoolRequired is "Y" or "O";
        
        public bool FromLocationRequiredOrOptional() =>
            this.FromLocationRequired is "Y" or "O";
    }
}
